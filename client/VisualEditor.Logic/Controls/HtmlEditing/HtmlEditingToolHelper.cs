using System.Collections.Generic;
using System.Windows.Forms;
using VisualEditor.Logic.Controls.Docking.Documents;
using VisualEditor.Utils.Controls.HtmlEditing;

namespace VisualEditor.Logic.Controls.HtmlEditing
{
    internal static class HtmlEditingToolHelper
    {
        public static DocumentBase GetParentDocument(HtmlEditingTool htmlEditingTool)
        {
          return htmlEditingTool.Parent as DocumentBase;
        }

      public static void SetDefaultStyle(HtmlEditingTool htmlEditingTool, bool outer = false)
        {
          Dictionary<string, string> d = new Dictionary<string, string>
                        {
                            {"href", string.Concat((outer) ? Warehouse.Warehouse.OuterProjectEditorLocation : Warehouse.Warehouse.ProjectEditorLocation, "\\")}
                        };
          htmlEditingTool.SetBaseTag(d);

          d = new Dictionary<string, string>
                        {
                            {"href", string.Concat(Application.StartupPath, "\\css\\system.css")},
                            {"rel", "stylesheet"},
                            {"type", "text/css"}
                        };
          htmlEditingTool.SetLinkTag(d);
        }

      public static void SetDefaultDocumentHtml(HtmlEditingTool htmlEditingTool, bool outer = false)
        {
          SetDefaultStyle(htmlEditingTool, outer);

          List<HtmlElement> ms = htmlEditingTool.GetElementsByTagName("meta");
            foreach (HtmlElement m in ms)
            {
                m.OuterHtml = string.Empty;
            }

            htmlEditingTool.BodyInnerHtml = "<p></p>";
        }
    }
}