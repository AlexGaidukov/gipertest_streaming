using System.Windows.Forms;

namespace VisualEditor.Utils.Controls.Ribbon
{
    public class RibbonContextMenu : ContextMenuStrip
    {
        public RibbonContextMenu()
        {
            Renderer = new Office2007Renderer();
        }
    }
}