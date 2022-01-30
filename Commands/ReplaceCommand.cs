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

namespace StageCoder
{

   
    /// <summary>
    /// Command handler
    /// </summary>
    internal sealed class ReplaceCommand
    {

        /// <summary>
        /// Command ID.
        /// </summary>
        public const int CommandId = 0x0200;

        /// <summary>
        /// Command menu group (command set GUID).
        /// </summary>
        public static readonly Guid CommandSet = new Guid("49ba9bf5-beb2-4eb0-a635-4890158aa62a");

        /// <summary>
        /// VS Package that provides this command, not null.
        /// </summary>
        private readonly AsyncPackage package;

        /// <summary>
        /// Initializes a new instance of the <see cref="TypeCommand"/> class.
        /// Adds our command handlers for menu (commands must exist in the command table file)
        /// </summary>
        /// <param name="package">Owner package, not null.</param>
        /// <param name="commandService">Command service to add command to, not null.</param>
        private ReplaceCommand(AsyncPackage package, OleMenuCommandService commandService)
        {
            this.package = package ?? throw new ArgumentNullException(nameof(package));
            commandService = commandService ?? throw new ArgumentNullException(nameof(commandService));

            var menuCommandID = new CommandID(CommandSet, CommandId);
            var menuItem = new OleMenuCommand(this.Execute, menuCommandID);
            menuItem.ParametersDescription = "SnippetName";
            commandService.AddCommand(menuItem);
        }

        /// <summary>
        /// Gets the instance of the command.
        /// </summary>
        public static ReplaceCommand Instance
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets the service provider from the owner package.
        /// </summary>
        private Microsoft.VisualStudio.Shell.IAsyncServiceProvider ServiceProvider
        {
            get
            {
                return this.package;
            }
        }

        /// <summary>
        /// Initializes the singleton instance of the command.
        /// </summary>
        /// <param name="package">Owner package, not null.</param>
        public static async Task InitializeAsync(AsyncPackage package)
        {
            await SnippetRepository.Instance.LoadSnippetsAsync();
            // Switch to the main thread - the call to AddCommand in TypeCommand's constructor requires
            // the UI thread.
            await ThreadHelper.JoinableTaskFactory.SwitchToMainThreadAsync(package.DisposalToken);

            OleMenuCommandService commandService = await package.GetServiceAsync(typeof(IMenuCommandService)) as OleMenuCommandService;
            Instance = new ReplaceCommand(package, commandService);
        }
        private EnvDTE80.TextDocumentKeyPressEvents textDocKeyEvents;
        string snippetcode = null;
        TextSelection selection;
        /// <summary>
        /// This function is the callback used to execute the command when the menu item is clicked.
        /// See the constructor to see how the menu item is associated with this function using
        /// OleMenuCommandService service and MenuCommand class.
        /// </summary>
        /// <param name="sender">Event sender.</param>
        /// <param name="e">Event args.</param>
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
            dte.Events.TextEditorEvents.LineChanged += TextEditorEvents_LineChanged;

            Assumes.Present(dte);
            if (dte.ActiveDocument != null)
            {
                selection = (TextSelection)dte.ActiveDocument.Selection;
                var code = CodeHandler.GetCode(dte,selection, text);
                text = code.SnippetName;
                originalselection = code.OriginalSelection;
                snippetcode = code.SnippetCode;

                // Replace the selection with the modified text.
                if (!string.IsNullOrEmpty(snippetcode))
                {
                    selection.Insert(snippetcode);
                }
                else 
                {
                    selection.Insert(originalselection);
                }
            }
        }


        private void TextEditorEvents_LineChanged(TextPoint StartPoint, TextPoint EndPoint, int Hint)
        {
           
        }
    }
}
