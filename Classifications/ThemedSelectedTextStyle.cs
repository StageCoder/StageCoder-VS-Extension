using System.ComponentModel.Composition;
using Microsoft.VisualStudio.Text.Classification;
using Microsoft.VisualStudio.Utilities;

namespace StageCoder.Classifications
{
    [Export(typeof(ClassificationTypeDefinition))]
    [Name("ThemedSelectedTextStyle")]
    internal static class ThemedSelectedTextStyle
    {
    }
}
