using System.Windows.Forms;

namespace VisualEditor.Utils.Controls.Ribbon
{
    public class RibbonStatusStrip : StatusStrip
    {
        public RibbonStatusStrip()
        {
            Renderer = new Office2007Renderer();
        }
    }
}