using System.Windows.Forms;
using VisualEditor.Logic.Controls;
using VisualEditor.Logic.Dialogs;
using VisualEditor.Utils.Controls.HtmlEditing;

namespace VisualEditor.Logic.Warehouse
{
    internal static class EditorObserver
    {
        public static Utils.Controls.HtmlEditing.Enums.HtmlEditingToolMode HostEditorMode { get; set; }
        public static Enums.RenderingStyle RenderingStyle { get; set; }
        public static Form DialogOwner
        {
            get
            {
                if (!HintDialog.Instance.Visible)
                {
                    return MainForm.Instance;
                }

                return HintDialog.Instance;
            }
        }
        public static HtmlEditingTool ActiveEditor { get; set; }
    }
}