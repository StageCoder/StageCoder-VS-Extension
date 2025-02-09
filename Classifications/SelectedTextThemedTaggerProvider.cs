using Microsoft.VisualStudio.Text;
using Microsoft.VisualStudio.Text.Classification;
using Microsoft.VisualStudio.Text.Editor;
using Microsoft.VisualStudio.Text.Tagging;
using Microsoft.VisualStudio.Utilities;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;

[Export(typeof(IViewTaggerProvider))]
[ContentType("text")]
[TagType(typeof(ClassificationTag))]
public class SelectedTextThemedTaggerProvider : IViewTaggerProvider
{
    [Import]
    internal IClassificationTypeRegistryService ClassificationTypeRegistry { get; set; }

    public ITagger<T> CreateTagger<T>(ITextView textView, ITextBuffer buffer) where T : ITag
    {
        // Ensure the tagger is only applied to the primary view
        if (textView.TextBuffer != buffer)
            return null;

        return new SelectedTextThemedTagger((IWpfTextView)textView, ClassificationTypeRegistry.GetClassificationType("ThemedSelectedTextStyle")) as ITagger<T>;
    }
}

internal class SelectedTextThemedTagger : ITagger<ClassificationTag>
{
    private readonly IWpfTextView _view;
    private readonly IClassificationType _classificationType;

    public SelectedTextThemedTagger(IWpfTextView view, IClassificationType classificationType)
    {
        _view = view;
        _classificationType = classificationType;

        // Hook into selection change events
        _view.Selection.SelectionChanged += (s, e) => TagsChanged?.Invoke(this, new SnapshotSpanEventArgs(new SnapshotSpan(_view.TextSnapshot, 0, _view.TextSnapshot.Length)));
    }

    public IEnumerable<ITagSpan<ClassificationTag>> GetTags(NormalizedSnapshotSpanCollection spans)
    {
        if (_view.Selection.IsEmpty)
            yield break;

        foreach (var span in _view.Selection.SelectedSpans)
        {
            yield return new TagSpan<ClassificationTag>(span, new ClassificationTag(_classificationType));
        }
    }

    public event EventHandler<SnapshotSpanEventArgs> TagsChanged;
}
