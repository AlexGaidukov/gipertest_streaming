using System.Drawing;

namespace VisualEditor.Utils.Controls.Ribbon
{
    public sealed class RibbonTabRenderEventArgs : RibbonRenderEventArgs
    {
        public RibbonTabRenderEventArgs(Controls.Ribbon.Ribbon owner, Graphics g, Rectangle clip, RibbonTab tab)
            : base(owner, g, clip)
        {
            Tab = tab;
        }

        /// <summary>
        /// Gets or sets the RibbonTab related to the evennt
        /// </summary>
        public RibbonTab Tab { get; set; }
    }
}