using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace VisualEditor.Utils.Controls.Ribbon
{
    [ToolboxItem(false)]
    public class RibbonPanelPopup : RibbonPopup
    {
        #region Fields

        private bool _ignoreNext;

        #endregion

        #region Ctor
        internal RibbonPanelPopup(RibbonPanel panel)
        {
            DoubleBuffered = true;

            Sensor = new RibbonSensor(this, panel.Owner, panel.OwnerTab) {PanelLimit = panel};
            Panel = panel;
            Panel.PopUp = this;

            using (var g = CreateGraphics())
            {
                var s = panel.MeasureSize(this, new RibbonElementMeasureSizeEventArgs(g,
                                                                                      GetSizeMode(panel)));
                Size = s;
                panel.SetBounds(new Rectangle(0, 0, Size.Width, Size.Height));
                panel.UpdateItemsRegions(g, GetSizeMode(panel));
            }

            foreach (var item in panel.Items)
            {
                item.SetCanvas(this);
            }
        } 
        #endregion
        
        #region Props

        public RibbonSensor Sensor { get; private set; }

        /// <summary>
        /// Gets the panel related to the form
        /// </summary>
        public RibbonPanel Panel { get; private set; }

        #endregion

        #region Methods

        public RibbonElementSizeMode GetSizeMode(RibbonPanel pnl)
        {
            if (pnl.FlowsTo == RibbonPanelFlowDirection.Right)
            {
                return RibbonElementSizeMode.Medium;
            }

            return RibbonElementSizeMode.Large;
        }

        /// <summary>
        /// Prevents the form from being hidden the next time the mouse clicks on the form.
        /// It is useful for reacting to clicks of items inside items.
        /// </summary>
        public void IgnoreNextClickDeactivation()
        {
            _ignoreNext = true;
        }

        #endregion

        #region Overrides
        protected override void OnMouseUp(MouseEventArgs e)
        {
            base.OnMouseUp(e);

            if (_ignoreNext)
            {
                _ignoreNext = false;
                return;
            }

            Close();
        }
        

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);

            Panel.avoidPaintBg = true;
            Panel.Owner.Renderer.OnRenderPanelPopupBackground(
                new RibbonCanvasEventArgs(Panel.Owner, e.Graphics, new Rectangle(Point.Empty, ClientSize), this, Panel));

            //Panel.OnPaint(this, new RibbonElementPaintEventArgs(
            //    new Rectangle(Point.Empty, Panel.Bounds.Size), e.Graphics, GetSizeMode(Panel)));
            foreach (RibbonItem item in Panel.Items)
            {
                item.OnPaint(this, new RibbonElementPaintEventArgs(e.ClipRectangle, e.Graphics, RibbonElementSizeMode.Large));
            }

            Panel.Owner.Renderer.OnRenderRibbonPanelText(new RibbonPanelRenderEventArgs(Panel.Owner, e.Graphics, e.ClipRectangle, Panel, this));

            Panel.avoidPaintBg = false;
        }

        protected override void OnClosed(EventArgs e)
        {
            foreach (RibbonItem item in Panel.Items)
            {
                item.SetCanvas(null);
            }
            
            Panel.Owner.UpdateRegions();
            Panel.Owner.Refresh();
            Panel.PopUp = null;
            Panel.Owner.ResumeSensor();
        } 

        #endregion

        #region Shadow

        // Define the CS_DROPSHADOW constant
        private const int CS_DROPSHADOW = 0x00020000;

        // Override the CreateParams property
        protected override CreateParams CreateParams
        {
            get
            {
                CreateParams cp = base.CreateParams;
                cp.ClassStyle |= CS_DROPSHADOW;
                return cp;
            }
        }


        #endregion
    }
}