using System.Drawing;

namespace VisualEditor.Utils.Controls.Ribbon
{
    public class RibbonItemBoundsEventArgs : RibbonItemRenderEventArgs
    {
        public RibbonItemBoundsEventArgs(Controls.Ribbon.Ribbon owner, Graphics g, Rectangle clip, RibbonItem item, Rectangle bounds)
            : base(owner, g, clip, item)
        {
            Bounds = bounds;
        }

        #region Properties

        /// <summary>
        /// Gets or sets the suggested bounds
        /// </summary>
        public Rectangle Bounds { get; set; }

        #endregion
    }
}