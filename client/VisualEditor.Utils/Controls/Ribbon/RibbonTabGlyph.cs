using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;
using System.Windows.Forms.Design.Behavior;

namespace VisualEditor.Utils.Controls.Ribbon
{
    public class RibbonTabGlyph : Glyph
    {
        BehaviorService _behaviorService;
        Controls.Ribbon.Ribbon _ribbon;
        Size size;

        public RibbonTabGlyph(BehaviorService behaviorService, RibbonDesigner designer, Controls.Ribbon.Ribbon ribbon)
            : base(new RibbonTabGlyphBehavior(designer, ribbon))
        {
            _behaviorService = behaviorService;
            _ribbon = ribbon;
            size = new Size(60, 16);
        }

        public override Rectangle Bounds
        {
            get
            {
                Point edge = _behaviorService.ControlToAdornerWindow(_ribbon);
                Point tab = new Point(5,_ribbon.OrbBounds.Bottom + 5 );

                //If has tabs
                if (_ribbon.Tabs.Count > 0)
                {
                    //Place glyph next to the last tab
                    RibbonTab t = _ribbon.Tabs[_ribbon.Tabs.Count - 1];
                    tab.X = t.Bounds.Right + 5;
                    tab.Y = t.Bounds.Top + 2;
                }

                return new Rectangle(
                    edge.X + tab.X,
                    edge.Y + tab.Y, 
                    size.Width , size.Height);
            }
        }

        public override Cursor GetHitTest(Point p)
        {
            if (Bounds.Contains(p))
            {
                return Cursors.Hand;
            }

            return null;
        }


        public override void Paint(PaintEventArgs pe)
        {
            var smbuff = pe.Graphics.SmoothingMode;
            pe.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
            using (var p = RibbonProfessionalRenderer.RoundRectangle(Bounds, 2))
            {
                using (var b = new SolidBrush(Color.FromArgb(50, Color.Blue)))
                {
                    pe.Graphics.FillPath(b, p);
                } 
            }
            var sf = new StringFormat {Alignment = StringAlignment.Center, LineAlignment = StringAlignment.Center};
            pe.Graphics.DrawString("Add Tab", SystemFonts.DefaultFont, Brushes.White, Bounds, sf);
            pe.Graphics.SmoothingMode = smbuff;
        }
    }

    public class RibbonTabGlyphBehavior : Behavior
    {
        RibbonDesigner _designer;

        public RibbonTabGlyphBehavior(RibbonDesigner designer, Controls.Ribbon.Ribbon ribbon)
        {
            _designer = designer;
        }



        public override bool OnMouseUp(Glyph g, MouseButtons button)
        {
            _designer.AddTabVerb(this, EventArgs.Empty);
            return base.OnMouseUp(g, button);
        }
    }
}