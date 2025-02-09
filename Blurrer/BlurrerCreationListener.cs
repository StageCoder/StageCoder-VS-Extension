using Microsoft.VisualStudio.Text.Classification;
using Microsoft.VisualStudio.Text.Editor;
using Microsoft.VisualStudio.Utilities;
using System.ComponentModel.Composition;
using System.Windows.Media;
using System.Windows;

namespace StageCoder.Blurrer;

/// <summary>
/// Creates the listener for the text view creation event.
/// The export attribute is used to export the BlurrerCreationListener to the MEF composition container, which will make it available to the editor.
/// ContentType and TextViewRole work as filters for when to apply the listener.
/// </summary>
/// <remarks>For this to be called and used, the VSIX Manifest MUST have the MEFComponent Asset registered.</remarks>
[Export(typeof(IWpfTextViewCreationListener))]
[ContentType(StandardContentTypeNames.Any)]
[TextViewRole(PredefinedTextViewRoles.Interactive)]
internal sealed class BlurrerCreationListener : IWpfTextViewCreationListener
{
    // We need an object with the AdornmentLayerDefinition attributes to define the layer, which we'll get later in the BlurrerAdorner class
    // However, we don't access it directly, so we suppress the warning. To retrieve it from the Adorner, we use the textView.GetAdornmentLayer("CodeBlurrer") method.
    [Export(typeof(AdornmentLayerDefinition))]
    [Name("CodeBlurrer")]
    [Order(After = PredefinedAdornmentLayers.Text)]
    [TextViewRole(PredefinedTextViewRoles.Structured)]
#pragma warning disable IDE0051 // Remove unused private members
    private readonly AdornmentLayerDefinition _codeBlurrerLayer;
#pragma warning restore IDE0051 // Remove unused private members


    [Import]
    internal IEditorFormatMapService FormatMapService = null;

    /// <summary>
    /// Called when a text view having matching roles is created over a text data model having a matching content type.
    /// Instantiates a BlurrerAdorner when the textView is created.
    /// </summary>
    /// <param name="textView">The <see cref="IWpfTextView"/> upon which the adornment should be placed</param>
    public void TextViewCreated(IWpfTextView textView)
    {
    


        // The adornment will listen to any event that changes the layout (text changes, scrolling, etc). This needs to be instantiated here, but we can throw it away
        // as the MEF composition container will keep it alive and track it.
        _ = new BlurrerAdorner(textView, FormatMapService);
    }
}
