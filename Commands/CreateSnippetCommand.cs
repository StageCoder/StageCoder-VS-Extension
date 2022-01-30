using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;
using StageCoder.Data;
using EnvDTE;
using Microsoft;
using Microsoft.VisualStudio.Shell;
using Microsoft.VisualStudio.Shell.Interop;
using Task = System.Threading.Tasks.Task;
using System.Text;
using System.Windows.Forms;
using System.Windows.Interop;

namespace StageCoder
{

    internal sealed class CreateSnippetCommand
    {
        public const int CommandId = 0x0190;       
        public static readonly Guid CommandSet = new Guid("0c1acc31-15ac-417c-86b2-eefdc669e8bf");
        private readonly AsyncPackage package;

        private CreateSnippetCommand(AsyncPackage package, OleMenuCommandService commandService)
        {
            this.package = package ?? throw new ArgumentNullException(nameof(package));
            commandService = commandService ?? throw new ArgumentNullException(nameof(commandService));

            var menuCommandID = new CommandID(CommandSet, CommandId);
            var menuItem = new OleMenuCommand(this.Execute, menuCommandID);
            menuItem.ParametersDescription = "SnippetName";
            commandService.AddCommand(menuItem);
        }

        public static CreateSnippetCommand Instance
        {
            get;
            private set;
        }

        private Microsoft.VisualStudio.Shell.IAsyncServiceProvider ServiceProvider
        {
            get
            {
                return this.package;
            }
        }

        public static async Task InitializeAsync(AsyncPackage package)
        {
            await ThreadHelper.JoinableTaskFactory.SwitchToMainThreadAsync(package.DisposalToken);

            OleMenuCommandService commandService = await package.GetServiceAsync(typeof(IMenuCommandService)) as OleMenuCommandService;
            Instance = new CreateSnippetCommand(package, commandService);
        }

        private async void Execute(object sender, EventArgs e)
        {
            var args = e as Microsoft.VisualStudio.Shell.OleMenuCmdEventArgs;
            var text = "";
            var originalselection = "";
            if (args != null && args.InValue!=null)
            {
                text = args.InValue.ToString();
            }

            await ThreadHelper.JoinableTaskFactory.SwitchToMainThreadAsync();
            DTE dte = (DTE)await package.GetServiceAsync(typeof(DTE));
            var selection = (TextSelection)dte.ActiveDocument.Selection;
            if (!selection.IsEmpty)
            {
                var snippetsPath = System.IO.Path.GetDirectoryName(dte.Solution.FullName) + @"\Snippets";
                if (!Directory.Exists(snippetsPath))
                {
                    Directory.CreateDirectory(snippetsPath);
                }

                string name = "";
                System.Windows.Window window = (System.Windows.Window)HwndSource.FromHwnd(dte.MainWindow.HWnd).RootVisual;
                FileNameDialog dialog = new FileNameDialog()
                {

                    Owner = window
                };

                bool? result = dialog.ShowDialog();
                name= (result.HasValue && result.Value) ? dialog.Input : string.Empty;

                if (!string.IsNullOrEmpty(name))
                {
                    StringBuilder content = new StringBuilder();
                    content.AppendLine("<?xml version=\"1.0\" encoding=\"utf-8\" ?>");
                    content.AppendLine("<CodeSnippets xmlns=\"http://schemas.microsoft.com/VisualStudio/2005/CodeSnippet\">");
                    content.AppendLine("<CodeSnippet Format=\"1.0.0\">");
                    content.AppendLine("<Header>");
                    content.AppendLine("<SnippetTypes>");
                    content.AppendLine("<SnippetType>Expansion</SnippetType>");
                    content.AppendLine("</SnippetTypes>");
                    content.AppendLine($"<Title>{name}</Title>");
                    content.AppendLine("<Author>CodePresenterGenerator</Author>");
                    content.AppendLine("<Description></Description><HelpUrl></HelpUrl>");
                    content.AppendLine($"<Shortcut>{name}</Shortcut>");
                    content.AppendLine("</Header>");
                    content.AppendLine("<Snippet>");
                    //TODO: Language =\"csharp\" add this back
                    content.AppendLine($"<Code Delimiter = \"$\"><![CDATA[{selection.Text}]]></Code>");
                    content.AppendLine("</Snippet>");
                    content.AppendLine("</CodeSnippet>");
                    content.AppendLine("</CodeSnippets>");
                    File.WriteAllText(Path.Combine(snippetsPath, name + ".snippet"), content.ToString());
                    await SnippetRepository.Instance.LoadSnippetsAsync();
                }
            }
        }
    }
}
