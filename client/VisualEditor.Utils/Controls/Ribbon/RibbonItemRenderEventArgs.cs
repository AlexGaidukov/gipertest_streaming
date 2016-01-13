using System.Drawing;

namespace VisualEditor.Utils.Controls.Ribbon
{
    public class RibbonItemRenderEventArgs : RibbonRenderEventArgs
    {
        private RibbonItem _item;

        public RibbonItemRenderEventArgs(Controls.Ribbon.Ribbon owner, Graphics g, Rectangle clip, RibbonItem item)
            : base(owner, g, clip)
        {
            Item = item;
        }

        public RibbonItem Item
        {
            get
            {
                return _item;
            }
            set
            {
                _item = value;
            }
        }
    }
}