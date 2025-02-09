using Microsoft.VisualStudio.Text.Classification;
using Microsoft.VisualStudio.Utilities;
using System.ComponentModel.Composition;
using System.Windows.Media;

[Export(typeof(EditorFormatDefinition))]
[ClassificationType(ClassificationTypeNames = "ThemedSelectedTextStyle")]
[Name("ThemedSelectedTextStyle")]
[UserVisible(true)] // Allows users to see and customize the style in Tools > Options
[Order(Before = Priority.Default)]
internal sealed class ThemedSelectedTextStyleFormat : ClassificationFormatDefinition
{
    public ThemedSelectedTextStyleFormat()
    {
        DisplayName = "Themed Selected Text Style";

        // Use Visual Studio theme colors
        ForegroundColor = System.Windows.Media.Colors.Black;
        BackgroundColor = System.Windows.Media.Colors.Transparent; // Theme-neutral background
        FontRenderingSize = 14; // Font size slightly larger than default
    }
}
