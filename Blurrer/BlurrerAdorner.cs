using EnvDTE;
using EnvDTE80;
using Microsoft.VisualStudio.Debugger.Interop;
using Microsoft.VisualStudio.Imaging.Interop;
using Microsoft.VisualStudio.PlatformUI;
using Microsoft.VisualStudio.Shell;
using Microsoft.VisualStudio.Shell.Interop;
using Microsoft.VisualStudio.Text;
using Microsoft.VisualStudio.Text.Classification;
using Microsoft.VisualStudio.Text.Editor;
using Microsoft.VisualStudio.Text.Tagging;
using Microsoft.VisualStudio.Threading;
using StageCoder.Settings;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Effects;

namespace StageCoder.Blurrer;

internal class BlurrerAdorner:IDisposable
{

    private readonly IWpfTextView _textView;
    private readonly IAdornmentLayer _codeBlurrerLayer;
    private readonly IEditorFormatMapService _formatMapService;
    private readonly Brush _blurBrush;

    public BlurrerAdorner(IWpfTextView textView,IEditorFormatMapService formatMapService)
    {
            _textView = textView;
            _formatMapService= formatMapService;
     

        // Get blur layer defined in the <ref>BlurrerCreationListener</ref> class
        _codeBlurrerLayer = textView.GetAdornmentLayer("CodeBlurrer");

        var b = new BlurEffect();
        // Get current background brush for the text view. We'll use it to create the blur effect.
        // NOTE: This works poorly if the background is not a solid color.

        _blurBrush = _textView.Background.Clone();
        _blurBrush.Opacity = 0.8; //Setting?

        // Register for the events when we need to update the overlay adorner. For this example,
        // we want to update when the user scrolls or resizes the window, or when a text selection is made/modified.
        _textView.LayoutChanged += OnLayoutChanged;
        _textView.Selection.SelectionChanged += OnSelectionChanged;
        General.Instance.EnableHighlighterChanged += OnEnableHighlighterChangedAsync;
        if (General.Instance.EnableHighlighter)
        {
            SetOpacity(0x0, 0);
        }
        else
        {
            SetOpacity(0xFF, 1.0);
            _codeBlurrerLayer.RemoveAllAdornments();
        }
    }

    private async void OnEnableHighlighterChangedAsync(object sender, PropertyChangedEventArgs e)
    {
        try
        {
            if (e.PropertyName == nameof(General.Instance.EnableHighlighter))
            {
                if (General.Instance.EnableHighlighter)
                {
                    SetOpacity(0x0, 0);
                }
                else
                {
                    SetOpacity(0xFF, 1.0);
                    _codeBlurrerLayer.RemoveAllAdornments();
                }
            }
        }
        catch (Exception ex)
        {
        }

        var dte = await ServiceProvider.GetGlobalServiceAsync(typeof(SDTE)) as DTE2;

        if (dte?.ActiveDocument != null)
        {
            // Store current selection
            var selection = _textView.Selection;
            var selectedSpans = selection.SelectedSpans.ToList();

            await ThreadHelper.JoinableTaskFactory.SwitchToMainThreadAsync();
            // Store document path and position
            string documentPath = dte.ActiveDocument.FullName;
            var currentPoint = selection.Start.Position;

            // Close the document
            dte.ActiveDocument.Close(vsSaveChanges.vsSaveChangesYes);

            // Reopen the document
            dte.ItemOperations.OpenFile(documentPath);
        }
    }

    /// <summary>
    /// The event called when the layout of the text view changes.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    void OnLayoutChanged(object sender, TextViewLayoutChangedEventArgs e)
    {
        ExecuteBlurrer();
    }

    /// <summary>
    /// The event called when the selection in the text view changes.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    void OnSelectionChanged(object sender, EventArgs e)
    {
        ExecuteBlurrer();
    }

    private void ExecuteBlurrer()
    {
        if (!General.Instance.EnableHighlighter)
        {
            _codeBlurrerLayer.RemoveAllAdornments();
        }
        else
        {
            try
            {
                BlurText();
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.Message);
    
            }
        }
    }


    const string SelectedText = "Selected Text";
    const string InactiveSelectedText="Inactive Selected Text";
    private void SetOpacity(byte alpha, double opacity)
    {
        try
        {
            IEditorFormatMap formatMap = _formatMapService.GetEditorFormatMap(_textView);

            ResourceDictionary selectedText = formatMap.GetProperties(SelectedText);
            ResourceDictionary inactiveSelectedText = formatMap.GetProperties(InactiveSelectedText);

            System.Windows.Media.Color SelectedTextBackgroundColor = (System.Windows.Media.Color)selectedText[EditorFormatDefinition.BackgroundColorId];
            System.Windows.Media.Color InactiveSelectedTextBackgroundColor = (System.Windows.Media.Color)inactiveSelectedText[EditorFormatDefinition.BackgroundColorId];

            //Check the colors
            formatMap.BeginBatchUpdate();
            SelectedTextBackgroundColor.A = alpha;
            InactiveSelectedTextBackgroundColor.A = alpha;

            selectedText[EditorFormatDefinition.BackgroundColorId] = SelectedTextBackgroundColor;
            inactiveSelectedText[EditorFormatDefinition.BackgroundColorId] = InactiveSelectedTextBackgroundColor;

            var selectedTextBackgroundBrush = new SolidColorBrush(SelectedTextBackgroundColor);
            var InactiveSelectedTextBackgroundBrush = new SolidColorBrush(InactiveSelectedTextBackgroundColor);
            selectedTextBackgroundBrush.Opacity = opacity;
            InactiveSelectedTextBackgroundBrush.Opacity = opacity;

            selectedText[EditorFormatDefinition.BackgroundBrushId] = selectedTextBackgroundBrush;
            inactiveSelectedText[EditorFormatDefinition.BackgroundBrushId] = InactiveSelectedTextBackgroundBrush;
            formatMap.SetProperties(SelectedText, selectedText);
            formatMap.SetProperties(InactiveSelectedText, inactiveSelectedText);
            formatMap.EndBatchUpdate();

            _textView.VisualElement.InvalidateVisual();
            _textView.VisualElement.UpdateLayout();
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine(ex.Message);
        }
        
    }

    /// <summary>
    /// Blurs the text that is not selected in the text view.
    /// </summary>
    private void BlurText()
    {
        try
        {
            // Clear the blur layer
            _codeBlurrerLayer.RemoveAllAdornments();

            // Exit out if nothing is selected
            if (_textView.Selection.IsEmpty)
            {
                return;
            }

            var selectedSpans = _textView.Selection.SelectedSpans.ToList();

            // Invert selected spans
            var notSelectedSpans = new List<SnapshotSpan>();

            int topEnd = 0;
            foreach (var selectedSpan in selectedSpans)
            {
                if (selectedSpan.Start.Position == selectedSpan.End.Position)
                {
                    continue;
                }

                var start = selectedSpan.Start.Position;

                if (start > topEnd)
                {
                    var newSpan = new SnapshotSpan(_textView.TextSnapshot, Span.FromBounds(topEnd, start));
                    notSelectedSpans.Add(newSpan);
                }
                topEnd = selectedSpan.End.Position;
            }

            // Add the last not selected span
            var lastSpan = new SnapshotSpan(_textView.TextSnapshot, Span.FromBounds(topEnd, _textView.TextSnapshot.Length));
            notSelectedSpans.Add(lastSpan);

            // Blur the not selected spans
            foreach (var span in notSelectedSpans)
            {
                BlurSpan(span);
            }
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine(ex.Message);
        }
    }


    /// <summary>
    /// Blurs the text within the specified span.
    /// </summary>
    /// <param name="span">The span of the text to blur.</param>
    /// <remarks>This does the actual "work" of putting an overlay above the text</remarks>
    private void BlurSpan(SnapshotSpan span)
    {
        try
        {
            // Get the "document coordinates" of the text in the span
            var geometry = _textView.TextViewLines.GetMarkerGeometry(span);

            if (geometry != null)
            {
                // Create a drawing for the blur effect based on the geometry of the text
                var drawing = new GeometryDrawing(_blurBrush, null, geometry);
                drawing.Freeze();

                var drawingImage = new DrawingImage(drawing);
                drawingImage.Freeze();

                var image = new Image
                {
                    Source = drawingImage,
                    Effect = new BlurEffect() { Radius = 20, KernelType = KernelType.Gaussian }
                };

                // Align the image with the top of the bounds of the text geometry
                Canvas.SetLeft(image, geometry.Bounds.Left);
                Canvas.SetTop(image, geometry.Bounds.Top);

                // Add the image to the layer in the view
                _codeBlurrerLayer.AddAdornment(AdornmentPositioningBehavior.TextRelative, span, null, image, null);
            }
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine(ex.Message);
        }
    }

    public void Dispose()
    {
        General.Instance.EnableHighlighterChanged -= OnEnableHighlighterChangedAsync;
        _textView.LayoutChanged -= OnLayoutChanged;
        _textView.Selection.SelectionChanged -= OnSelectionChanged;
    }
}
