using System;
using System.Drawing;

namespace VisualEditor.Utils.Controls.Ribbon
{
    /// <remarks>Ribbon rendering event data</remarks>
    public class RibbonRenderEventArgs : EventArgs
    {
        public RibbonRenderEventArgs(Controls.Ribbon.Ribbon owner, Graphics g, Rectangle clip)
        {
            Ribbon = owner;
            Graphics = g;
            ClipRectangle = clip;
        }

        /// <summary>
        /// Gets the Ribbon related to the render
        /// </summary>
        public Controls.Ribbon.Ribbon Ribbon { get; set; }

        /// <summary>
        /// Gets the Device to draw into
        /// </summary>
        public Graphics Graphics { get; set; }

        /// <summary>
        /// Gets the Rectangle area where to draw into
        /// </summary>
        public Rectangle ClipRectangle { get; set; }
    }
}