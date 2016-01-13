using System;
using System.Drawing;
using System.Windows.Forms;

namespace VisualEditor.Utils.Controls.Ribbon
{
    public class RibbonCanvasEventArgs : EventArgs
    {
        #region ctor

        public RibbonCanvasEventArgs(Controls.Ribbon.Ribbon owner, Graphics g, Rectangle bounds, Control canvas, object relatedObject)
        {
            Owner = owner;
            Graphics = g;
            Bounds = bounds;
            Canvas = canvas;
            RelatedObject = relatedObject;
        }

        #endregion

        #region Props

        public object RelatedObject { get; set; }


        /// <summary>
        /// Gets or sets the Ribbon that raised the event
        /// </summary>
        public Controls.Ribbon.Ribbon Owner { get; set; }

        /// <summary>
        /// Gets or sets the graphics to paint
        /// </summary>
        public Graphics Graphics { get; set; }

        /// <summary>
        /// Gets or sets the bounds that should be painted
        /// </summary>
        public Rectangle Bounds { get; set; }

        /// <summary>
        /// Gets or sets the control where to be painted
        /// </summary>
        public Control Canvas { get; set; }

        #endregion
    }
}