using EnvDTE;
using Microsoft;
using Microsoft.VisualStudio.Settings;
using Microsoft.VisualStudio.Shell;
using Microsoft.VisualStudio.Shell.Interop;
using Microsoft.VisualStudio.Shell.Settings;
using Microsoft.VisualStudio.Threading;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;

namespace StageCoder.Data
{
    class SnippetRepository
    {
        public List<CodeSnippets> Snippets { get; set; } = new List<CodeSnippets>();
        
        private static readonly Lazy<SnippetRepository> lazy =new Lazy<SnippetRepository>(() => new SnippetRepository());

        public static SnippetRepository Instance { get { return lazy.Value; } }

        private SnippetRepository()
        {
            
        }


        private static async System.Threading.Tasks.Task<ShellSettingsManager> GetSettingsManagerAsync()
        {
#pragma warning disable VSTHRD010
            // False-positive in Threading Analyzers. Bug tracked here https://github.com/Microsoft/vs-threading/issues/230
            var svc = await AsyncServiceProvider.GlobalProvider.GetServiceAsync(typeof(SVsSettingsManager)) as IVsSettingsManager;
#pragma warning restore VSTHRD010

            Assumes.Present(svc);

            return new ShellSettingsManager(svc);
        }

        public static SettingsStore Settings { get; private set; }
        private static ShellSettingsManager SettingsManagerInstance { get; set; }
        private static readonly AsyncLazy<ShellSettingsManager> SettingsManager = new AsyncLazy<ShellSettingsManager>(GetSettingsManagerAsync, ThreadHelper.JoinableTaskFactory);
        public async Task LoadSnippets()
        {

            SettingsManagerInstance = await SettingsManager.GetValueAsync();
            Settings = SettingsManagerInstance.GetReadOnlySettingsStore(SettingsScope.Configuration);
            var userSettings = SettingsManagerInstance.GetReadOnlySettingsStore(SettingsScope.UserSettings);
            var mydocspath = userSettings.GetString(@"\", "VisualStudioLocation");

            List<string> snippetPaths = new List<string>();
            var settingpaths = Settings.GetSubCollectionNames(@"Languages\CodeExpansions\");
            foreach (var spath in settingpaths)
            {
                var Propnamesandvalues=Settings.GetPropertyNamesAndValues(@"Languages\CodeExpansions\" + spath + @"\Paths");
                foreach (var prop in Propnamesandvalues)
                {
                    var paths = prop.Value.ToString().Split(';');
                    foreach (var p in paths)
                    {
                        if (p.Contains("%LCID%") || p.Contains("%InstallRoot%"))
                        {
                            //Ignore theses paths for now
                        }
                        else
                        {
                            var pathtoadd = p;
                            pathtoadd = pathtoadd.Replace("%MyDocs%", mydocspath);
                            snippetPaths.Add(pathtoadd);
                        }
                    }
                }
            }


            //Add Solution folder
            var dte = GetCurrentDTE();
            if (dte != null && dte.Solution!=null && dte.Solution.FullName!=null)
            {
                var solutionpath=System.IO.Path.GetDirectoryName(dte.Solution.FullName) + @"\Snippets";
                if (Directory.Exists(solutionpath))
                {
                    snippetPaths.Add(solutionpath);
                }
            }
            //Get all Snippets
            Snippets.Clear();

            


            foreach (var path in snippetPaths)
            {
                
                var snippetsfiles = Directory.GetFiles(path, "*.snippet");
                foreach (var snippetfile in snippetsfiles)
                {
                    try
                    {
                        XmlSerializer serializer = new XmlSerializer(typeof(CodeSnippets));
                        CodeSnippets cs = (CodeSnippets)serializer.Deserialize(new XmlTextReader(snippetfile));
                        Snippets.Add(cs);
                    }
                    catch { }
                }
            }
        }

        public static DTE GetCurrentDTE(IServiceProvider provider)
        {
            ThreadHelper.ThrowIfNotOnUIThread();
            DTE vs = (DTE)provider.GetService(typeof(DTE));
            if (vs == null) throw new InvalidOperationException("DTE not found.");
            return vs;
        }

        public static DTE GetCurrentDTE()
        {
            return GetCurrentDTE(/* Microsoft.VisualStudio.Shell. */ServiceProvider.GlobalProvider);
        }

    }
}
