using EnvDTE;
using Microsoft.VisualStudio.Shell;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace StageCoder.Data
{

    public class Code
    {
        public string SnippetName { get; set; }
        public string OriginalSelection { get; set; }
        public string SnippetCode { get; set; }
    }
    public class CodeHandler
    {
        public static Code GetCode(DTE dte,TextSelection selection, string commandarg)
        {
            var text = commandarg;
            var originalselection = "";
            if (string.IsNullOrEmpty(text))
            {
                if (string.IsNullOrEmpty(selection.Text))
                {
                    selection.SelectLine();
                    text = selection.Text;
                    for (int a = 0; a < text.Length; a++)
                    {
                        if (char.IsWhiteSpace(text[a]))
                        {
                            selection.Insert(text[a].ToString());
                        }
                        else
                        {
                            text = text.Substring(a);
                            break;
                        }
                    }
                    selection.NewLine();
                    selection.LineUp();
                    originalselection = text = text.TrimSuffix("\r\n");
                }
                else
                {
                    originalselection = text = selection.Text;
                }

                if (text.StartsWith("//"))
                {
                    originalselection = text;
                    int lastposition = text.LastIndexOf('/');
                    text = text.Substring(lastposition + 1);
                }

                if (text.StartsWith("/*"))
                {
                    text = text.Replace("/*", "");
                    text = text.Replace("*/", "");
                    text = text.Trim();
                }

                if (text.StartsWith("@*"))
                {
                    text = text.Replace("@*", "");
                    text = text.Replace("*@", "");
                    text = text.Trim();
                }

                if (text.StartsWith("<!--"))
                {
                    text = text.Replace("<!--", "");
                    text = text.Replace("-->", "");
                    text = text.Trim();
                }

                if (text.StartsWith("todo:"))
                {
                    text = text.Replace("todo:", "");
                }
            }

            var snippetcode = "";
            if (text.ToLower() == "[clipboard]")
            {
                snippetcode = Clipboard.GetText();
            }
            else
            {
                var snippet = SnippetRepository.Instance.Snippets.FirstOrDefault(s => s.CodeSnippet.Header.Shortcut.ToLower() == text.ToLower());
                if (snippet != null)
                {
                    snippetcode = snippet.CodeSnippet.Snippet.Code.Value.Trim();
                }
            }

            return new Code() 
            { 
                SnippetName = text, 
                OriginalSelection = originalselection,
                SnippetCode=snippetcode
            };
        }
    }
}
