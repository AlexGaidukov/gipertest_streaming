using System;
using System.Drawing;
using System.Drawing.Drawing2D;

namespace VisualEditor.Utils.Controls.Ribbon
{
    public class RibbonProfessionalRenderer : RibbonRenderer
    {
        #region Types

        public enum Corners
        {
            None = 0,
            NorthWest = 2,
            NorthEast = 4,
            SouthEast = 8,
            SouthWest = 16,
            All = NorthWest | NorthEast | SouthEast | SouthWest,
            North = NorthWest | NorthEast,
            South = SouthEast | SouthWest,
            East = NorthEast | SouthEast,
            West = NorthWest | SouthWest
        }

        

        #endregion

        #region Fields

        private Size arrowSize = new Size(5, 3);
        private Size moreSize = new Size(7, 7);

        #endregion

        #region Ctor

        public RibbonProfessionalRenderer()
        {
            ColorTable = new RibbonProfesionalRendererColorTable();
        }

        #endregion

        #region Props

        public RibbonProfesionalRendererColorTable ColorTable { get; set; }

        #endregion

        #region Methods

        #region Util

        public Color GetTextColor(bool enabled)
        {
            return GetTextColor(enabled, ColorTable.Text);
        }

        public Color GetTextColor(bool enabled, Color alternative)
        {
            if (enabled)
            {
                return alternative;
            }

            return ColorTable.ArrowDisabled;
        }

        /// <summary>
        /// Creates a rectangle with rounded corners
        /// </summary>
        /// <param name="r"></param>
        /// <param name="radius"></param>
        /// <returns></returns>
        public static GraphicsPath RoundRectangle(Rectangle r, int radius)
        {
            return RoundRectangle(r, radius, Corners.All);
        }

        /// <summary>
        /// Creates a rectangle with the specified corners rounded
        /// </summary>
        /// <param name="r"></param>
        /// <param name="radius"></param>
        /// <param name="corners"></param>
        /// <returns></returns>
        public static GraphicsPath RoundRectangle(Rectangle r, int radius, Corners corners)
        {
            var path = new GraphicsPath();
            radius -= 2;
            int d = radius * 2;

            int nw = (corners & Corners.NorthWest) == Corners.NorthWest ? d : 0;
            int ne = (corners & Corners.NorthEast) == Corners.NorthEast ? d : 0;
            int se = (corners & Corners.SouthEast) == Corners.SouthEast ? d : 0;
            int sw = (corners & Corners.SouthWest) == Corners.SouthWest ? d : 0;

            path.AddLine(r.Left + nw, r.Top, r.Right - ne, r.Top);

            if (ne > 0)
            {
                path.AddArc(Rectangle.FromLTRB(r.Right - ne, r.Top, r.Right, r.Top + ne),
                            -90, 90);
            }

            path.AddLine(r.Right, r.Top + ne, r.Right, r.Bottom - se);

            if (se > 0)
            {
                path.AddArc(Rectangle.FromLTRB(r.Right - se, r.Bottom - se, r.Right, r.Bottom),
                            0, 90);
            }

            path.AddLine(r.Right - se, r.Bottom, r.Left + sw, r.Bottom);

            if (sw > 0)
            {
                path.AddArc(Rectangle.FromLTRB(r.Left, r.Bottom - sw, r.Left + sw, r.Bottom),
                            90, 90);
            }

            path.AddLine(r.Left, r.Bottom - sw, r.Left, r.Top + nw);

            if (nw > 0)
            {
                path.AddArc(Rectangle.FromLTRB(r.Left, r.Top, r.Left + nw, r.Top + nw),
                            180, 90);
            }

            path.CloseFigure();

            return path;
        }

        /// <summary>
        /// Draws a rectangle with a vertical gradient
        /// </summary>
        /// <param name="g"></param>
        /// <param name="r"></param>
        /// <param name="northColor"></param>
        /// <param name="southColor"></param>
        private void GradientRect(Graphics g, Rectangle r, Color northColor, Color southColor)
        {
            using (Brush b = new LinearGradientBrush(
                new Point(r.X, r.Y - 1), new Point(r.Left, r.Bottom), northColor, southColor))
            {
                g.FillRectangle(b, r);
            }
        }

        /// <summary>
        /// Draws a shadow that indicates that the element is pressed
        /// </summary>
        /// <param name="g"></param>
        /// <param name="r"></param>
        public void DrawPressedShadow(Graphics g, Rectangle r)
        {
            var shadow = Rectangle.FromLTRB(r.Left, r.Top, r.Right, r.Top + 4);

            using (var path = RoundRectangle(shadow, 3, Corners.NorthEast | Corners.NorthWest))
            {
                using (var b = new LinearGradientBrush(shadow,
                                                       Color.FromArgb(50, Color.Black),
                                                       Color.FromArgb(0, Color.Black),
                                                       90))
                {
                    b.WrapMode = WrapMode.TileFlipXY;
                    g.FillPath(b, path);

                }
            }
        }

        /// <summary>
        /// Draws an arrow on the specified bounds
        /// </summary>
        /// <param name="g"></param>
        /// <param name="c"></param>
        public void DrawArrow(Graphics g, Rectangle b, Color c, RibbonArrowDirection d)
        {
            var path = new GraphicsPath();
            var bounds = b;

            if(b.Width % 2 != 0 && (d == RibbonArrowDirection.Up ))
                bounds = new Rectangle(new Point(b.Left - 1, b.Top -1), new Size(b.Width + 1, b.Height + 1));

            if (d == RibbonArrowDirection.Up)
            {
                path.AddLine(bounds.Left, bounds.Bottom, bounds.Right, bounds.Bottom);
                path.AddLine(bounds.Right, bounds.Bottom, bounds.Left + bounds.Width / 2, bounds.Top);
            }
            else if(d == RibbonArrowDirection.Down)
            {
                path.AddLine(bounds.Left, bounds.Top, bounds.Right , bounds.Top);
                path.AddLine(bounds.Right, bounds.Top, bounds.Left + bounds.Width / 2, bounds.Bottom);
            }
            else if (d == RibbonArrowDirection.Left)
            {
                path.AddLine(bounds.Left, bounds.Top, bounds.Right, bounds.Top + bounds.Height / 2);
                path.AddLine(bounds.Right, bounds.Top + bounds.Height / 2, bounds.Left, bounds.Bottom);
            }
            else
            {
                path.AddLine(bounds.Right, bounds.Top, bounds.Left, bounds.Top + bounds.Height / 2);
                path.AddLine(bounds.Left, bounds.Top + bounds.Height / 2, bounds.Right, bounds.Bottom);
            }

            path.CloseFigure();

            using (var bb = new SolidBrush(c))
            {
                var sm = g.SmoothingMode;
                g.SmoothingMode = SmoothingMode.None;
                g.FillPath(bb, path);
                g.SmoothingMode = sm;
            }

            path.Dispose();
        }

        /// <summary>
        /// Draws the pair of arrows that make a shadded arrow, centered on the specified bounds
        /// </summary>
        /// <param name="g"></param>
        /// <param name="b"></param>
        /// <param name="d"></param>
        /// <param name="enabled"></param>
        public void DrawArrowShaded(Graphics g, Rectangle b, RibbonArrowDirection d, bool enabled)
        {
            var arrSize = arrowSize;

            if (d == RibbonArrowDirection.Left || d == RibbonArrowDirection.Right)
            {
                //Invert size
                arrSize = new Size(arrowSize.Height, arrowSize.Width);
            }

            var arrowP = new Point(
                b.Left + (b.Width - arrSize.Width) / 2,
                b.Top + (b.Height - arrSize.Height) / 2
                );

            var bounds = new Rectangle(arrowP, arrSize);
            var boundsLight = bounds; boundsLight.Offset(0, 1);

            var lt = ColorTable.ArrowLight;
            var dk = ColorTable.Arrow;

            if (!enabled)
            {
                lt = Color.Transparent;
                dk = ColorTable.ArrowDisabled;
            }

            DrawArrow(g, boundsLight, lt, d);
            DrawArrow(g, bounds, dk, d);
        }

        /// <summary>
        /// Centers the specified rectangle on the specified container
        /// </summary>
        /// <param name="container"></param>
        /// <param name="r"></param>
        /// <returns></returns>
        public Rectangle CenterOn(Rectangle container, Rectangle r)
        {
            var result = new Rectangle(
                container.Left + ((container.Width - r.Width) / 2),
                container.Top + ((container.Height - r.Height) / 2),
                r.Width, r.Height);

            return result;
        }

        /// <summary>
        /// Draws a dot of the grip
        /// </summary>
        /// <param name="g"></param>
        /// <param name="location"></param>
        public void DrawGripDot(Graphics g, Point location)
        {
            var lt = new Rectangle(location.X - 1, location.Y + 1, 2, 2);
            var dk = new Rectangle(location, new Size(2, 2));

            using (var b = new SolidBrush(ColorTable.DropDownGripLight))
            {
                g.FillRectangle(b, lt);
            }

            using (var b = new SolidBrush(ColorTable.DropDownGripDark))
            {
                g.FillRectangle(b, dk);
            }
        }

        #endregion

        #region Tab
        /// <summary>
        /// Creates the path of the tab and its contents
        /// </summary>
        /// <returns></returns>
        public GraphicsPath CreateCompleteTabPath(RibbonTab t)
        {
            var path = new GraphicsPath();
            const int corner = 6;

            path.AddLine(t.TabBounds.Left + corner, t.TabBounds.Top,
                         t.TabBounds.Right - corner, t.TabBounds.Top);
            path.AddArc(
                Rectangle.FromLTRB(t.TabBounds.Right - corner, t.TabBounds.Top, t.TabBounds.Right, t.TabBounds.Top + corner),
                -90, 90);
            path.AddLine(t.TabBounds.Right, t.TabBounds.Top + corner,
                         t.TabBounds.Right, t.TabBounds.Bottom - corner);
            path.AddArc(Rectangle.FromLTRB(
                            t.TabBounds.Right, t.TabBounds.Bottom - corner, t.TabBounds.Right + corner, t.TabBounds.Bottom),
                        -180, -90);
            path.AddLine(t.TabBounds.Right + corner, t.TabBounds.Bottom, t.TabContentBounds.Right - corner, t.TabBounds.Bottom);
            path.AddArc(Rectangle.FromLTRB(
                            t.TabContentBounds.Right - corner, t.TabBounds.Bottom, t.TabContentBounds.Right, t.TabBounds.Bottom + corner),
                        -90, 90);
            path.AddLine(t.TabContentBounds.Right, t.TabContentBounds.Top + corner, t.TabContentBounds.Right, t.TabContentBounds.Bottom - corner);
            path.AddArc(Rectangle.FromLTRB(
                            t.TabContentBounds.Right - corner, t.TabContentBounds.Bottom - corner, t.TabContentBounds.Right, t.TabContentBounds.Bottom),
                        0, 90);
            path.AddLine(t.TabContentBounds.Right - corner, t.TabContentBounds.Bottom, t.TabContentBounds.Left + corner, t.TabContentBounds.Bottom);
            path.AddArc(Rectangle.FromLTRB(
                            t.TabContentBounds.Left, t.TabContentBounds.Bottom - corner, t.TabContentBounds.Left + corner, t.TabContentBounds.Bottom),
                        90, 90);
            path.AddLine(t.TabContentBounds.Left, t.TabContentBounds.Bottom - corner, t.TabContentBounds.Left, t.TabBounds.Bottom + corner);
            path.AddArc(Rectangle.FromLTRB(
                            t.TabContentBounds.Left, t.TabBounds.Bottom, t.TabContentBounds.Left + corner, t.TabBounds.Bottom + corner),
                        180, 90);
            path.AddLine(t.TabContentBounds.Left + corner, t.TabContentBounds.Top, t.TabBounds.Left - corner, t.TabBounds.Bottom);
            path.AddArc(Rectangle.FromLTRB(
                            t.TabBounds.Left - corner, t.TabBounds.Bottom - corner, t.TabBounds.Left, t.TabBounds.Bottom),
                        90, -90);
            path.AddLine(t.TabBounds.Left, t.TabBounds.Bottom - corner, t.TabBounds.Left, t.TabBounds.Top + corner);
            path.AddArc(Rectangle.FromLTRB(
                            t.TabBounds.Left, t.TabBounds.Top, t.TabBounds.Left + corner, t.TabBounds.Top + corner),
                        180, 90);
            path.CloseFigure();

            return path;
        }

        /// <summary>
        /// Creates the path of the tab and its contents
        /// </summary>
        /// <returns></returns>
        public GraphicsPath CreateTabPath(RibbonTab t)
        {
            var path = new GraphicsPath();
            const int corner = 6;
            const int rightOffset = 1;

            path.AddLine(
                t.TabBounds.Left, t.TabBounds.Bottom,
                t.TabBounds.Left, t.TabBounds.Top + corner);
            path.AddArc(
                new Rectangle(
                    t.TabBounds.Left, t.TabBounds.Top,
                    corner, corner),
                180, 90);
            path.AddLine(
                t.TabBounds.Left + corner, t.TabBounds.Top,
                t.TabBounds.Right - corner - rightOffset, t.TabBounds.Top);
            path.AddArc(
                new Rectangle(
                    t.TabBounds.Right - corner - rightOffset, t.TabBounds.Top,
                    corner, corner),
                -90, 90);
            path.AddLine(
                t.TabBounds.Right - rightOffset, t.TabBounds.Top + corner,
                t.TabBounds.Right - rightOffset, t.TabBounds.Bottom);
            

            return path;
        }

        /// <summary>
        /// Draws a complete tab
        /// </summary>
        /// <param name="e"></param>
        public void DrawCompleteTab(RibbonTabRenderEventArgs e)
        {
            DrawTabActive(e);

            //Background gradient
            using (var path = RoundRectangle(e.Tab.TabContentBounds, 4))
            {
                var north = ColorTable.TabContentNorth;
                var south = ColorTable.TabContentSouth;

                if (e.Tab.Contextual)
                {
                    north = ColorTable.DropDownBg;
                    south = north;
                }

                using (var b = new LinearGradientBrush(
                    new Point(0, e.Tab.TabContentBounds.Top + 30), 
                    new Point(0, e.Tab.TabContentBounds.Bottom - 10), north, south))
                {
                    b.WrapMode = WrapMode.TileFlipXY;
                    e.Graphics.FillPath(b, path);
                }
            }

            //Glossy effect
            var glossy = Rectangle.FromLTRB(e.Tab.TabContentBounds.Left, e.Tab.TabContentBounds.Top + 0, e.Tab.TabContentBounds.Right, e.Tab.TabContentBounds.Top + 18);
            using (var path = RoundRectangle(glossy, 6, Corners.NorthWest | Corners.NorthEast))
            {
                using (var b = new SolidBrush(Color.FromArgb(30, Color.White)))
                {
                    e.Graphics.FillPath(b, path);
                }
            }

            //Tab border
            using (var path = CreateCompleteTabPath(e.Tab))
            {
                using (var p = new Pen(ColorTable.TabBorder))
                {
                    e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
                    e.Graphics.DrawPath(p, path);
                }
            }

            if (e.Tab.Selected)
            {
                //Selected glow
                using (var path = CreateTabPath(e.Tab))
                {
                    var p = new Pen(Color.FromArgb(150, Color.Gold)) {Width = 2};

                    e.Graphics.DrawPath(p, path);

                    p.Dispose();
                }
            }

            
        }

        /// <summary>
        /// Draws a complete tab
        /// </summary>
        /// <param name="e"></param>
        public void DrawTabNormal(RibbonTabRenderEventArgs e)
        {
            var lastClip = e.Graphics.ClipBounds;

            var clip = Rectangle.FromLTRB(
                e.Tab.TabBounds.Left,
                e.Tab.TabBounds.Top,
                e.Tab.TabBounds.Right,
                e.Tab.TabBounds.Bottom);

            var r = Rectangle.FromLTRB(
                e.Tab.TabBounds.Left - 1,
                e.Tab.TabBounds.Top - 1,
                e.Tab.TabBounds.Right,
                e.Tab.TabBounds.Bottom);

            e.Graphics.SetClip(clip);

            using (Brush b = new SolidBrush(ColorTable.RibbonBackground))
            {
                e.Graphics.FillRectangle(b, r);
            }

            e.Graphics.SetClip(lastClip);
        }

        /// <summary>
        /// Draws a selected tab
        /// </summary>
        /// <param name="e"></param>
        public void DrawTabSelected(RibbonTabRenderEventArgs e)
        {
            var outerR = Rectangle.FromLTRB(
                e.Tab.TabBounds.Left,
                e.Tab.TabBounds.Top,
                e.Tab.TabBounds.Right - 1,
                e.Tab.TabBounds.Bottom);
            var innerR = Rectangle.FromLTRB(
                outerR.Left + 1,
                outerR.Top + 1,
                outerR.Right - 1,
                outerR.Bottom);

            var glossyR = Rectangle.FromLTRB(
                innerR.Left + 1,
                innerR.Top + 1,
                innerR.Right - 1,
                innerR.Top + e.Tab.TabBounds.Height / 2);

            var outer = RoundRectangle(outerR, 3, Corners.NorthEast | Corners.NorthWest);
            var inner = RoundRectangle(innerR, 3, Corners.NorthEast | Corners.NorthWest);
            var glossy = RoundRectangle(glossyR, 3, Corners.NorthEast | Corners.NorthWest);

            using (var p = new Pen(ColorTable.TabBorder))
            {
                e.Graphics.DrawPath(p, outer);
            }

            using (var p = new Pen(Color.FromArgb(200, Color.White)))
            {
                e.Graphics.DrawPath(p, inner);
            }

            using (var radialPath = new GraphicsPath())
            {
                radialPath.AddRectangle(innerR);
                radialPath.CloseFigure();

                var gr = new PathGradientBrush(radialPath)
                             {
                                 CenterPoint = new PointF(
                                     Convert.ToSingle(innerR.Left + innerR.Width/2),
                                     Convert.ToSingle(innerR.Top - 5)),
                                 CenterColor = Color.Transparent,
                                 SurroundColors = new[] {ColorTable.TabSelectedGlow}
                             };
                var blend = new Blend(3)
                                {
                                    Factors = new[] {0.0f, 0.9f, 0.0f},
                                    Positions = new[] {0.0f, 0.8f, 1.0f}
                                };

                gr.Blend = blend;

                e.Graphics.FillPath(gr, radialPath);

                gr.Dispose();
            }
            using (var b = new SolidBrush(Color.FromArgb(100, Color.White)))
            {
                e.Graphics.FillPath(b, glossy);
            }

            outer.Dispose();
            inner.Dispose();
            glossy.Dispose();

        }

        /// <summary>
        /// Draws a pressed tab
        /// </summary>
        /// <param name="e"></param>
        public void DrawTabPressed(RibbonTabRenderEventArgs e)
        {

        }

        /// <summary>
        /// Draws an active tab
        /// </summary>
        /// <param name="e"></param>
        public void DrawTabActive(RibbonTabRenderEventArgs e)
        {
            DrawTabNormal(e);

            var glossy = new Rectangle(e.Tab.TabBounds.Left, e.Tab.TabBounds.Top, e.Tab.TabBounds.Width, 4);
            var shadow = e.Tab.TabBounds; 
            shadow.Offset(2, 1);
            var tab = e.Tab.TabBounds;

            using (var path = RoundRectangle(shadow, 6, Corners.NorthWest | Corners.NorthEast))
            {
                using (var b = new PathGradientBrush(path))
                {
                    b.WrapMode = WrapMode.Clamp;

                    var cb = new ColorBlend(3);
                    cb.Colors = new []{Color.Transparent, 
                                       Color.FromArgb(50, Color.Black), 
                                       Color.FromArgb(100, Color.Black)};
                    cb.Positions = new [] { 0f, .1f, 1f };

                    b.InterpolationColors = cb;

                    e.Graphics.FillPath(b, path);
                }
            }

            using (var path = RoundRectangle(tab, 6, Corners.North))
            {
                using (var p = new Pen(ColorTable.TabNorth, 1.6f))
                {
                    e.Graphics.DrawPath(p, path);
                }

                using (var b = new LinearGradientBrush(
                    e.Tab.TabBounds, ColorTable.TabNorth, ColorTable.TabSouth, 90))
                {
                    e.Graphics.FillPath(b, path);
                }
            }

            using (var path = RoundRectangle(glossy, 6, Corners.North))
            {
                using (var b = new SolidBrush(Color.FromArgb(180, Color.White)))
                {
                    e.Graphics.FillPath(b, path);
                }
            }
        } 
        #endregion

        #region Panel
        /// <summary>
        /// Draws a panel in normal state (unselected)
        /// </summary>
        /// <param name="e"></param>
        public void DrawPanelNormal(RibbonPanelRenderEventArgs e)
        {
            var darkBorder = Rectangle.FromLTRB(
                e.Panel.Bounds.Left,
                e.Panel.Bounds.Top,
                e.Panel.Bounds.Right,
                e.Panel.Bounds.Bottom);

            var lightBorder = Rectangle.FromLTRB(
                e.Panel.Bounds.Left + 1,
                e.Panel.Bounds.Top + 1,
                e.Panel.Bounds.Right + 1,
                e.Panel.Bounds.Bottom);

            var textArea =
                Rectangle.FromLTRB(
                    e.Panel.Bounds.Left + 1,
                    e.Panel.ContentBounds.Bottom,
                    e.Panel.Bounds.Right - 1,
                    e.Panel.Bounds.Bottom - 1);

            var dark = RoundRectangle(darkBorder, 3);
            var light = RoundRectangle(lightBorder, 3);
            var txt = RoundRectangle(textArea, 3, Corners.SouthEast | Corners.SouthWest);

            using (var p = new Pen(ColorTable.PanelLightBorder))
            {
                e.Graphics.DrawPath(p, light);
            }

            using (var p = new Pen(ColorTable.PanelDarkBorder))
            {
                e.Graphics.DrawPath(p, dark);
            }

            using (var b = new SolidBrush(ColorTable.PanelTextBackground))
            {
                e.Graphics.FillPath(b, txt);
            }

            if (e.Panel.ButtonMoreVisible)
            {
                DrawButtonMoreGlyph(e.Graphics, e.Panel.ButtonMoreBounds, e.Panel.ButtonMoreEnabled && e.Panel.Enabled);
            }

            txt.Dispose();
            dark.Dispose();
            light.Dispose();
        }

        /// <summary>
        /// Draws a panel in selected state
        /// </summary>
        /// <param name="e"></param>
        public void DrawPanelSelected(RibbonPanelRenderEventArgs e)
        {
            var darkBorder = Rectangle.FromLTRB(
                e.Panel.Bounds.Left,
                e.Panel.Bounds.Top,
                e.Panel.Bounds.Right,
                e.Panel.Bounds.Bottom);

            var lightBorder = Rectangle.FromLTRB(
                e.Panel.Bounds.Left + 1,
                e.Panel.Bounds.Top + 1,
                e.Panel.Bounds.Right - 1,
                e.Panel.Bounds.Bottom - 1);

            var textArea =
                Rectangle.FromLTRB(
                    e.Panel.Bounds.Left + 1,
                    e.Panel.ContentBounds.Bottom,
                    e.Panel.Bounds.Right - 1,
                    e.Panel.Bounds.Bottom - 1);

            var dark = RoundRectangle(darkBorder, 3);
            var light = RoundRectangle(lightBorder, 3);
            var txt = RoundRectangle(textArea, 3, Corners.SouthEast | Corners.SouthWest);

            using (var p = new Pen(ColorTable.PanelLightBorder))
            {
                e.Graphics.DrawPath(p, light);
            }

            using (var p = new Pen(ColorTable.PanelDarkBorder))
            {
                e.Graphics.DrawPath(p, dark);
            }

            using (var b = new SolidBrush(ColorTable.PanelBackgroundSelected))
            {
                e.Graphics.FillPath(b, light);
            }

            using (var b = new SolidBrush(ColorTable.PanelTextBackgroundSelected))
            {
                e.Graphics.FillPath(b, txt);
            }

            if (e.Panel.ButtonMoreVisible)
            {
                if (e.Panel.ButtonMorePressed)
                {
                    DrawButtonPressed(e.Graphics, e.Panel.ButtonMoreBounds, Corners.SouthEast);
                }
                else if(e.Panel.ButtonMoreSelected)
                {
                    DrawButtonSelected(e.Graphics, e.Panel.ButtonMoreBounds, Corners.SouthEast);
                }

                DrawButtonMoreGlyph(e.Graphics, e.Panel.ButtonMoreBounds, e.Panel.ButtonMoreEnabled && e.Panel.Enabled);
            }

            txt.Dispose();
            dark.Dispose();
            light.Dispose();
        }

        public void DrawButtonMoreGlyph(Graphics g, Rectangle b, bool enabled)
        {
            var dark = enabled ? ColorTable.Arrow : ColorTable.ArrowDisabled;
            var light = ColorTable.ArrowLight;

            var bounds = CenterOn(b, new Rectangle(Point.Empty, moreSize));
            var boundsLight = bounds; boundsLight.Offset(1, 1);

            DrawButtonMoreGlyph(g, boundsLight.Location, light);
            DrawButtonMoreGlyph(g, bounds.Location, dark);
        }

        public void DrawButtonMoreGlyph(Graphics gr, Point p, Color color)
        {
            var a = p;
            var b = new Point(p.X + moreSize.Width - 1, p.Y);
            var c = new Point(p.X, p.Y + moreSize.Height - 1);
            var f = new Point(p.X + moreSize.Width, p.Y + moreSize.Height);
            var d = new Point(f.X, f.Y - 3);
            var e = new Point(f.X - 3, f.Y);
            var g = new Point(f.X - 3, f.Y - 3);

            var lastMode = gr.SmoothingMode;

            gr.SmoothingMode = SmoothingMode.None;

            using (var pen = new Pen(color))
            {
                gr.DrawLine(pen, a, b);
                gr.DrawLine(pen, a, c);
                gr.DrawLine(pen, e, f);
                gr.DrawLine(pen, d, f);
                gr.DrawLine(pen, e, d);
                gr.DrawLine(pen, g, f);
            }

            gr.SmoothingMode = lastMode;
        }

        /// <summary>
        /// Draws an overflown panel in normal state
        /// </summary>
        /// <param name="e"></param>
        public void DrawPanelOverflowNormal(RibbonPanelRenderEventArgs e)
        {
            var darkBorder = Rectangle.FromLTRB(
                e.Panel.Bounds.Left,
                e.Panel.Bounds.Top,
                e.Panel.Bounds.Right,
                e.Panel.Bounds.Bottom);

            var lightBorder = Rectangle.FromLTRB(
                e.Panel.Bounds.Left + 1,
                e.Panel.Bounds.Top + 1,
                e.Panel.Bounds.Right - 1,
                e.Panel.Bounds.Bottom - 1);


            var dark = RoundRectangle(darkBorder, 3);
            var light = RoundRectangle(lightBorder, 3);

            using (var p = new Pen(ColorTable.PanelLightBorder))
            {
                e.Graphics.DrawPath(p, light);
            }

            using (var p = new Pen(ColorTable.PanelDarkBorder))
            {
                e.Graphics.DrawPath(p, dark);
            }

            DrawPanelOverflowImage(e);

            dark.Dispose();
            light.Dispose();
        }

        /// <summary>
        /// Draws an overflown panel in selected state
        /// </summary>
        /// <param name="e"></param>
        public void DrawPannelOveflowSelected(RibbonPanelRenderEventArgs e)
        {
            var darkBorder = Rectangle.FromLTRB(
                e.Panel.Bounds.Left,
                e.Panel.Bounds.Top,
                e.Panel.Bounds.Right,
                e.Panel.Bounds.Bottom);

            var lightBorder = Rectangle.FromLTRB(
                e.Panel.Bounds.Left + 1,
                e.Panel.Bounds.Top + 1,
                e.Panel.Bounds.Right - 1,
                e.Panel.Bounds.Bottom - 1);


            var dark = RoundRectangle(darkBorder, 3);
            var light = RoundRectangle(lightBorder, 3);

            using (var p = new Pen(ColorTable.PanelLightBorder))
            {
                e.Graphics.DrawPath(p, light);
            }

            using (var p = new Pen(ColorTable.PanelDarkBorder))
            {
                e.Graphics.DrawPath(p, dark);
            }

            using (var b = new LinearGradientBrush(
                lightBorder, ColorTable.PanelOverflowBackgroundSelectedNorth, Color.Transparent, 90))
            {
                e.Graphics.FillPath(b, light);
            }

            DrawPanelOverflowImage(e);

            dark.Dispose();
            light.Dispose();

        }

        /// <summary>
        /// Draws an overflown panel in pressed state
        /// </summary>
        /// <param name="e"></param>
        public void DrawPanelOverflowPressed(RibbonPanelRenderEventArgs e)
        {
            var darkBorder = Rectangle.FromLTRB(
                e.Panel.Bounds.Left,
                e.Panel.Bounds.Top,
                e.Panel.Bounds.Right,
                e.Panel.Bounds.Bottom);

            var lightBorder = Rectangle.FromLTRB(
                e.Panel.Bounds.Left + 1,
                e.Panel.Bounds.Top + 1,
                e.Panel.Bounds.Right - 1,
                e.Panel.Bounds.Bottom - 1);

            var glossy = Rectangle.FromLTRB(
                e.Panel.Bounds.Left,
                e.Panel.Bounds.Top,
                e.Panel.Bounds.Right,
                e.Panel.Bounds.Top + 17);


            var dark = RoundRectangle(darkBorder, 3);
            var light = RoundRectangle(lightBorder, 3);



            using (var b = new LinearGradientBrush(lightBorder,
                                                   ColorTable.PanelOverflowBackgroundPressed,
                                                   ColorTable.PanelOverflowBackgroundSelectedSouth, 90))
            {
                b.WrapMode = WrapMode.TileFlipXY;
                e.Graphics.FillPath(b, dark);
            }

            using (var path = RoundRectangle(glossy, 3, Corners.NorthEast | Corners.NorthWest))
            {
                using (var b = new LinearGradientBrush(
                    glossy,
                    Color.FromArgb(150, Color.White),
                    Color.FromArgb(50, Color.White), 90
                    ))
                {
                    b.WrapMode = WrapMode.TileFlipXY;
                    e.Graphics.FillPath(b, path);
                }
            }

            using (var p = new Pen(Color.FromArgb(40, Color.White)))
            {
                e.Graphics.DrawPath(p, light);
            }

            using (var p = new Pen(ColorTable.PanelDarkBorder))
            {
                e.Graphics.DrawPath(p, dark);
            }

            DrawPanelOverflowImage(e);

            DrawPressedShadow(e.Graphics, glossy);

            dark.Dispose();
            light.Dispose();
        }

        /// <summary>
        /// Draws the image of the panel when collapsed
        /// </summary>
        /// <param name="e"></param>
        public void DrawPanelOverflowImage(RibbonPanelRenderEventArgs e)
        {
            const int margin = 3;
            var imgSquareSize = new Size(32, 32);
            var imgSquareR = new Rectangle(new Point(
                                               e.Panel.Bounds.Left + (e.Panel.Bounds.Width - imgSquareSize.Width) / 2,
                                               e.Panel.Bounds.Top+ 5), imgSquareSize);

            var imgSquareBottomR = Rectangle.FromLTRB(
                imgSquareR.Left, imgSquareR.Bottom - 10, imgSquareR.Right, imgSquareR.Bottom);

            var textR = Rectangle.FromLTRB(
                e.Panel.Bounds.Left + margin,
                imgSquareR.Bottom + margin,
                e.Panel.Bounds.Right - margin,
                e.Panel.Bounds.Bottom - margin);

            var imgSq = RoundRectangle(imgSquareR, 5);
            var imgSqB = RoundRectangle(imgSquareBottomR, 5, Corners.South);

            using (var b = new LinearGradientBrush(
                imgSquareR, ColorTable.TabContentNorth, ColorTable.TabContentSouth, 90
                ))
            {
                e.Graphics.FillPath(b, imgSq);
            }

            using (var b = new SolidBrush(ColorTable.PanelTextBackground))
            {
                e.Graphics.FillPath(b, imgSqB);
            }

            using (var p = new Pen(ColorTable.PanelDarkBorder))
            {
                e.Graphics.DrawPath(p, imgSq);
            }

            if (e.Panel.Image != null)
            {
                e.Graphics.DrawImage(e.Panel.Image,
                                     new Point(
                                         imgSquareR.Left + (imgSquareR.Width - e.Panel.Image.Width) / 2,
                                         imgSquareR.Top + ((imgSquareR.Height - imgSquareBottomR.Height) - e.Panel.Image.Height) / 2));

            }

            using (var b = new SolidBrush(GetTextColor(e.Panel.Enabled)))
            {
                var sf = new StringFormat
                             {
                                 Alignment = StringAlignment.Center,
                                 LineAlignment = StringAlignment.Near,
                                 Trimming = StringTrimming.Character
                             };

                e.Graphics.DrawString(e.Panel.Text, e.Ribbon.Font, b, textR, sf);
            }

            var bounds = LargeButtonDropDownArrowBounds(e.Graphics, e.Panel.Owner.Font, e.Panel.Text, textR);

            if (bounds.Right < e.Panel.Bounds.Right)
            {


                var boundsLight = bounds; boundsLight.Offset(0, 1);

                var lt = ColorTable.ArrowLight;
                var dk = ColorTable.Arrow;


                DrawArrow(e.Graphics, boundsLight, lt, RibbonArrowDirection.Down);
                DrawArrow(e.Graphics, bounds, dk, RibbonArrowDirection.Down);

            }
            imgSq.Dispose();
            imgSqB.Dispose();
        }

        #endregion

        #region Button

        /// <summary>
        /// Gets the corners to round on the specified button
        /// </summary>
        /// <param name="button"></param>
        /// <returns></returns>
        private Corners ButtonCorners(RibbonButton button)
        {
            if (!(button.OwnerItem is RibbonItemGroup))
            {
                return Corners.All;
            }

            var g = button.OwnerItem as RibbonItemGroup;
            var c = Corners.None;
            if (button == g.FirstItem)
            {
                c |= Corners.West;
            }
                
            if (button == g.LastItem)
            {
                c |= Corners.East;
            }

            return c;
        }

        /// <summary>
        /// Determines buttonface corners
        /// </summary>
        /// <param name="button"></param>
        /// <returns></returns>
        private Corners ButtonFaceRounding(RibbonButton button)
        {
            if (!(button.OwnerItem is RibbonItemGroup))
            {
                return button.SizeMode == RibbonElementSizeMode.Large ? Corners.North : Corners.West;
            }

            var c = Corners.None;
            var g = button.OwnerItem as RibbonItemGroup;
            if (button == g.FirstItem)
            {
                c |= Corners.West;
            }

            return c;
        }

        /// <summary>
        /// Determines button's dropDown corners
        /// </summary>
        /// <param name="button"></param>
        /// <returns></returns>
        private Corners ButtonDdRounding(RibbonButton button)
        {
            if (!(button.OwnerItem is RibbonItemGroup))
            {
                return button.SizeMode == RibbonElementSizeMode.Large ? Corners.South : Corners.East;
            }

            var c = Corners.None;
            var g = button.OwnerItem as RibbonItemGroup;
            if (button == g.LastItem)
            {
                c |= Corners.East;
            }

            return c;
        }

        /// <summary>
        /// Draws the orb's option buttons background
        /// </summary>
        /// <param name="g"></param>
        /// <param name="bounds"></param>
        public void DrawOrbOptionButton(Graphics g, Rectangle bounds)
        {
            bounds.Width -= 1; bounds.Height -= 1;

            using (var p = RoundRectangle(bounds, 3))
            {
                using (var b = new SolidBrush(ColorTable.OrbOptionBackground))
                {
                    g.FillPath(b, p);
                }

                GradientRect(g, Rectangle.FromLTRB(bounds.Left, bounds.Top + bounds.Height / 2, bounds.Right, bounds.Bottom - 2),
                             ColorTable.OrbOptionShine, ColorTable.OrbOptionBackground);

                using (var pen = new Pen(ColorTable.OrbOptionBorder))
                {
                    g.DrawPath(pen, p);
                }
            }
        }

        /// <summary>
        /// Draws a regular button in normal state 
        /// </summary>
        public void DrawButton(Graphics g, Rectangle bounds, Corners corners)
        {
            var outerR = Rectangle.FromLTRB(
                bounds.Left,
                bounds.Top,
                bounds.Right - 1,
                bounds.Bottom - 1);

            var innerR = Rectangle.FromLTRB(
                bounds.Left + 1,
                bounds.Top + 1,
                bounds.Right - 2,
                bounds.Bottom - 2);

            var glossyR = Rectangle.FromLTRB(
                bounds.Left + 1,
                bounds.Top + 1,
                bounds.Right - 2,
                bounds.Top + Convert.ToInt32((double)bounds.Height * .36));

            using (var boundsPath = RoundRectangle(outerR, 3, corners))
            {
                using (var brus = new SolidBrush(ColorTable.ButtonBgOut))
                {
                    g.FillPath(brus, boundsPath);
                }

                #region Main Bg
                using (var path = new GraphicsPath())
                {
                    path.AddEllipse(new Rectangle(bounds.Left, bounds.Top, bounds.Width, bounds.Height * 2));
                    path.CloseFigure();
                    using (var gradient = new PathGradientBrush(path))
                    {
                        gradient.WrapMode = WrapMode.Clamp;
                        gradient.CenterPoint = new PointF(
                            Convert.ToSingle(bounds.Left + bounds.Width / 2),
                            Convert.ToSingle(bounds.Bottom));
                        gradient.CenterColor = ColorTable.ButtonBgCenter;
                        gradient.SurroundColors = new [] { ColorTable.ButtonBgOut };

                        var lastClip = g.Clip;
                        var newClip = new Region(boundsPath);
                        newClip.Intersect(lastClip);
                        g.SetClip(newClip.GetBounds(g));
                        g.FillPath(gradient, path);
                        g.Clip = lastClip;
                    }
                }
                #endregion

                //Border
                using (var p = new Pen(ColorTable.ButtonBorderOut))
                {
                    g.DrawPath(p, boundsPath);
                }

                //Inner border
                using (var path = RoundRectangle(innerR, 3, corners))
                {
                    using (var p = new Pen(ColorTable.ButtonBorderIn))
                    {
                        g.DrawPath(p, path);
                    }
                }

                //Glossy effect
                using (var path = RoundRectangle(glossyR, 3, (corners & Corners.NorthWest) | (corners & Corners.NorthEast)))
                {
                    using (var b = new LinearGradientBrush(
                        glossyR, ColorTable.ButtonGlossyNorth, ColorTable.ButtonGlossySouth, 90))
                    {
                        b.WrapMode = WrapMode.TileFlipXY;
                        g.FillPath(b, path);
                    }
                }
            }
        }

        public Rectangle LargeButtonDropDownArrowBounds(Graphics g, Font font, string text, Rectangle textLayout)
        {
            bool moreWords = text.Contains(" ");
            var sf = new StringFormat
                         {
                             Alignment = StringAlignment.Center,
                             LineAlignment = (moreWords ? StringAlignment.Center : StringAlignment.Near),
                             Trimming = StringTrimming.EllipsisCharacter
                         };

            sf.SetMeasurableCharacterRanges(new [] { new CharacterRange(0, text.Length) });
            var regions = g.MeasureCharacterRanges(text, font, textLayout, sf);

            var lastCharBounds = Rectangle.Round(regions[regions.Length - 1].GetBounds(g));

            if (moreWords)
            {
                return new Rectangle(lastCharBounds.Right + 3,
                                     lastCharBounds.Top + (lastCharBounds.Height - arrowSize.Height) / 2, arrowSize.Width, arrowSize.Height);
            }

            return new Rectangle(
                textLayout.Left + (textLayout.Width - arrowSize.Width) / 2,
                lastCharBounds.Bottom + ((textLayout.Bottom - lastCharBounds.Bottom) - arrowSize.Height) / 2, arrowSize.Width, arrowSize.Height);
        }

        /// <summary>
        /// Draws the arrow of buttons
        /// </summary>
        /// <param name="g"></param>
        /// <param name="button"></param>
        public void DrawButtonDropDownArrow(Graphics g, RibbonButton button, Rectangle textLayout)
        {
            var bounds = Rectangle.Empty;

            if (button.SizeMode == RibbonElementSizeMode.Large || button.SizeMode == RibbonElementSizeMode.Overflow)
            {

                bounds = LargeButtonDropDownArrowBounds(g, button.Owner.Font, button.Text, textLayout);

            }
            else
            {
                bounds = textLayout;
            }

            DrawArrowShaded(g, bounds, button.DropDownArrowDirection, button.Enabled);
        }

        /// <summary>
        /// Draws a regular button in disabled state 
        /// </summary>
        public void DrawButtonDisabled(Graphics g, Rectangle bounds, Corners corners)
        {
            var outerR = Rectangle.FromLTRB(
                bounds.Left,
                bounds.Top,
                bounds.Right - 1,
                bounds.Bottom - 1);

            var innerR = Rectangle.FromLTRB(
                bounds.Left + 1,
                bounds.Top + 1,
                bounds.Right - 2,
                bounds.Bottom - 2);

            var glossyR = Rectangle.FromLTRB(
                bounds.Left + 1,
                bounds.Top + 1,
                bounds.Right - 2,
                bounds.Top + Convert.ToInt32((double)bounds.Height * .36));

            using (var boundsPath = RoundRectangle(outerR, 3, corners))
            {
                using (var brus = new SolidBrush(ColorTable.ButtonDisabledBgOut))
                {
                    g.FillPath(brus, boundsPath);
                }

                #region Main Bg
                using (var path = new GraphicsPath())
                {
                    path.AddEllipse(new Rectangle(bounds.Left, bounds.Top, bounds.Width, bounds.Height * 2));
                    path.CloseFigure();
                    using (var gradient = new PathGradientBrush(path))
                    {
                        gradient.WrapMode = WrapMode.Clamp;
                        gradient.CenterPoint = new PointF(
                            Convert.ToSingle(bounds.Left + bounds.Width / 2),
                            Convert.ToSingle(bounds.Bottom));
                        gradient.CenterColor = ColorTable.ButtonDisabledBgCenter;
                        gradient.SurroundColors = new [] { ColorTable.ButtonDisabledBgOut };

                        var lastClip = g.Clip;
                        var newClip = new Region(boundsPath);
                        newClip.Intersect(lastClip);
                        g.SetClip(newClip.GetBounds(g));
                        g.FillPath(gradient, path);
                        g.Clip = lastClip;
                    }
                }
                #endregion

                //Border
                using (var p = new Pen(ColorTable.ButtonDisabledBorderOut))
                {
                    g.DrawPath(p, boundsPath);
                }

                //Inner border
                using (var path = RoundRectangle(innerR, 3, corners))
                {
                    using (var p = new Pen(ColorTable.ButtonDisabledBorderIn))
                    {
                        g.DrawPath(p, path);
                    }
                }

                //Glossy effect
                using (var path = RoundRectangle(glossyR, 3, (corners & Corners.NorthWest) | (corners & Corners.NorthEast)))
                {
                    using (var b = new LinearGradientBrush(
                        glossyR, ColorTable.ButtonDisabledGlossyNorth, ColorTable.ButtonDisabledGlossySouth, 90))
                    {
                        b.WrapMode = WrapMode.TileFlipXY;
                        g.FillPath(b, path);
                    }
                }
            }
        }

        /// <summary>
        /// Draws a regular button in pressed state
        /// </summary>
        public void DrawButtonPressed(Graphics g, Rectangle bounds, Corners corners)
        {
            var outerR = Rectangle.FromLTRB(
                bounds.Left,
                bounds.Top,
                bounds.Right - 1,
                bounds.Bottom - 1);

            var innerR = Rectangle.FromLTRB(
                bounds.Left + 1,
                bounds.Top + 1,
                bounds.Right - 2,
                bounds.Bottom - 2);

            var glossyR = Rectangle.FromLTRB(
                bounds.Left + 1,
                bounds.Top + 1,
                bounds.Right - 2,
                bounds.Top + Convert.ToInt32((double)bounds.Height * .36));

            using (var boundsPath = RoundRectangle(outerR, 3, corners))
            {
                using (var brus = new SolidBrush(ColorTable.ButtonPressedBgOut))
                {
                    g.FillPath(brus, boundsPath);
                }

                #region Main Bg
                using (var path = new GraphicsPath())
                {
                    path.AddEllipse(new Rectangle(bounds.Left, bounds.Top, bounds.Width, bounds.Height * 2));
                    path.CloseFigure();
                    using (var gradient = new PathGradientBrush(path))
                    {
                        gradient.WrapMode = WrapMode.Clamp;
                        gradient.CenterPoint = new PointF(
                            Convert.ToSingle(bounds.Left + bounds.Width / 2),
                            Convert.ToSingle(bounds.Bottom));
                        gradient.CenterColor = ColorTable.ButtonPressedBgCenter;
                        gradient.SurroundColors = new [] { ColorTable.ButtonPressedBgOut };

                        var lastClip = g.Clip;
                        var newClip = new Region(boundsPath);
                        newClip.Intersect(lastClip);
                        g.SetClip(newClip.GetBounds(g));
                        g.FillPath(gradient, path);
                        g.Clip = lastClip;
                    }
                }
                #endregion

                //Border
                using (var p = new Pen(ColorTable.ButtonPressedBorderOut))
                {
                    g.DrawPath(p, boundsPath);
                }

                //Inner border
                using (var path = RoundRectangle(innerR, 3, corners))
                {
                    using (var p = new Pen(ColorTable.ButtonPressedBorderIn))
                    {
                        g.DrawPath(p, path);
                    }
                }

                //Glossy effect
                using (var path = RoundRectangle(glossyR, 3, (corners & Corners.NorthWest) | (corners & Corners.NorthEast)))
                {
                    using (var b = new LinearGradientBrush(
                        glossyR, ColorTable.ButtonPressedGlossyNorth, ColorTable.ButtonPressedGlossySouth, 90))
                    {
                        b.WrapMode = WrapMode.TileFlipXY;
                        g.FillPath(b, path);
                    }
                }
            }

            DrawPressedShadow(g, outerR);
        }

        /// <summary>
        /// Draws a regular buttton in selected state
        /// </summary>
        public void DrawButtonSelected(Graphics g, Rectangle bounds, Corners corners)
        {
            var outerR = Rectangle.FromLTRB(
                bounds.Left,
                bounds.Top,
                bounds.Right - 1,
                bounds.Bottom - 1);

            var innerR = Rectangle.FromLTRB(
                bounds.Left + 1,
                bounds.Top + 1,
                bounds.Right - 2,
                bounds.Bottom - 2);

            var glossyR = Rectangle.FromLTRB(
                bounds.Left + 1,
                bounds.Top + 1,
                bounds.Right - 2,
                bounds.Top + Convert.ToInt32((double)bounds.Height * .36));

            using (var boundsPath = RoundRectangle(outerR, 3, corners))
            {
                using (var brus = new SolidBrush(ColorTable.ButtonSelectedBgOut))
                {
                    g.FillPath(brus, boundsPath);
                }

                #region Main Bg
                using (var path = new GraphicsPath())
                {
                    path.AddEllipse(new Rectangle(bounds.Left, bounds.Top, bounds.Width, bounds.Height * 2));
                    path.CloseFigure();
                    using (var gradient = new PathGradientBrush(path))
                    {
                        gradient.WrapMode = WrapMode.Clamp;
                        gradient.CenterPoint = new PointF(
                            Convert.ToSingle(bounds.Left + bounds.Width / 2),
                            Convert.ToSingle(bounds.Bottom));
                        gradient.CenterColor = ColorTable.ButtonSelectedBgCenter;
                        gradient.SurroundColors = new [] { ColorTable.ButtonSelectedBgOut };

                        var lastClip = g.Clip;
                        var newClip = new Region(boundsPath);
                        newClip.Intersect(lastClip);
                        g.SetClip(newClip.GetBounds(g));
                        g.FillPath(gradient, path);
                        g.Clip = lastClip;
                    }
                }
                #endregion

                //Border
                using (var p = new Pen(ColorTable.ButtonSelectedBorderOut))
                {
                    g.DrawPath(p, boundsPath);
                }

                //Inner border
                using (var path = RoundRectangle(innerR, 3, corners))
                {
                    using (var p = new Pen(ColorTable.ButtonSelectedBorderIn))
                    {
                        g.DrawPath(p, path);
                    }
                }

                //Glossy effect
                using (var path = RoundRectangle(glossyR, 3, (corners & Corners.NorthWest) | (corners & Corners.NorthEast)))
                {
                    using (var b = new LinearGradientBrush(
                        glossyR, ColorTable.ButtonSelectedGlossyNorth, ColorTable.ButtonSelectedGlossySouth, 90))
                    {
                        b.WrapMode = WrapMode.TileFlipXY;
                        g.FillPath(b, path);
                    }
                }
            }
        }

        /// <summary>
        /// Draws the button as pressed
        /// </summary>
        /// <param name="g"></param>
        /// <param name="button"></param>
        public void DrawButtonPressed(Graphics g, RibbonButton button)
        {
            DrawButtonPressed(g, button.Bounds, ButtonCorners(button));
        }

        /// <summary>
        /// Draws the button as Checked
        /// </summary>
        /// <param name="g"></param>
        /// <param name="button"></param>
        public void DrawButtonChecked(Graphics g, RibbonButton button)
        {
            DrawButtonChecked(g, button.Bounds, ButtonCorners(button));
        }

        /// <summary>
        /// Draws the button as checked
        /// </summary>
        /// <param name="g"></param>
        public void DrawButtonChecked(Graphics g, Rectangle bounds, Corners corners)
        {
            var outerR = Rectangle.FromLTRB(
                bounds.Left,
                bounds.Top,
                bounds.Right - 1,
                bounds.Bottom - 1);

            var innerR = Rectangle.FromLTRB(
                bounds.Left + 1,
                bounds.Top + 1,
                bounds.Right - 2,
                bounds.Bottom - 2);

            var glossyR = Rectangle.FromLTRB(
                bounds.Left + 1,
                bounds.Top + 1,
                bounds.Right - 2,
                bounds.Top + Convert.ToInt32((double)bounds.Height * .36));

            using (var boundsPath = RoundRectangle(outerR, 3, corners))
            {
                using (var brus = new SolidBrush(ColorTable.ButtonCheckedBgOut))
                {
                    g.FillPath(brus, boundsPath);
                }

                #region Main Bg
                using (var path = new GraphicsPath())
                {
                    path.AddEllipse(new Rectangle(bounds.Left, bounds.Top, bounds.Width, bounds.Height * 2));
                    path.CloseFigure();
                    using (var gradient = new PathGradientBrush(path))
                    {
                        gradient.WrapMode = WrapMode.Clamp;
                        gradient.CenterPoint = new PointF(
                            Convert.ToSingle(bounds.Left + bounds.Width / 2),
                            Convert.ToSingle(bounds.Bottom));
                        gradient.CenterColor = ColorTable.ButtonCheckedBgCenter;
                        gradient.SurroundColors = new [] { ColorTable.ButtonCheckedBgOut };

                        var lastClip = g.Clip;
                        var newClip = new Region(boundsPath);
                        newClip.Intersect(lastClip);
                        g.SetClip(newClip.GetBounds(g));
                        g.FillPath(gradient, path);
                        g.Clip = lastClip;
                    }
                }
                #endregion

                //Border
                using (var p = new Pen(ColorTable.ButtonCheckedBorderOut))
                {
                    g.DrawPath(p, boundsPath);
                }

                //Inner border
                using (var path = RoundRectangle(innerR, 3, corners))
                {
                    using (var p = new Pen(ColorTable.ButtonCheckedBorderIn))
                    {
                        g.DrawPath(p, path);
                    }
                }

                //Glossy effect
                using (var path = RoundRectangle(glossyR, 3, (corners & Corners.NorthWest) | (corners & Corners.NorthEast)))
                {
                    using (var b = new LinearGradientBrush(
                        glossyR, ColorTable.ButtonCheckedGlossyNorth, ColorTable.ButtonCheckedGlossySouth, 90))
                    {
                        b.WrapMode = WrapMode.TileFlipXY;
                        g.FillPath(b, path);
                    }
                }
            }

            DrawPressedShadow(g, outerR);
        }

        /// <summary>
        /// Draws the button as a selected button
        /// </summary>
        /// <param name="g"></param>
        /// <param name="button"></param>
        public void DrawButtonSelected(Graphics g, RibbonButton button)
        {
            DrawButtonSelected(g, button.Bounds, ButtonCorners(button));
        }       

        /// <summary>
        /// Draws a SplitDropDown button in selected state
        /// </summary>
        /// <param name="e"></param>
        /// <param name="button"></param>
        public void DrawSplitButtonSelected(RibbonItemRenderEventArgs e, RibbonButton button)
        {
            var outerR = Rectangle.FromLTRB(
                button.DropDownBounds.Left,
                button.DropDownBounds.Top,
                button.DropDownBounds.Right - 1,
                button.DropDownBounds.Bottom - 1);

            var innerR = Rectangle.FromLTRB(
                outerR.Left + 1,
                outerR.Top + 1,
                outerR.Right - 1,
                outerR.Bottom - 1);

            var faceOuterR = Rectangle.FromLTRB(
                button.ButtonFaceBounds.Left,
                button.ButtonFaceBounds.Top,
                button.ButtonFaceBounds.Right - 1,
                button.ButtonFaceBounds.Bottom - 1);

            var faceInnerR = Rectangle.FromLTRB(
                faceOuterR.Left + 1,
                faceOuterR.Top + 1,
                faceOuterR.Right + (button.SizeMode == RibbonElementSizeMode.Large ? -1 : 0),
                faceOuterR.Bottom + (button.SizeMode == RibbonElementSizeMode.Large ? 0 : -1));

            var faceCorners = ButtonFaceRounding(button);
            var ddCorners = ButtonDdRounding(button);

            var outer = RoundRectangle(outerR, 3, ddCorners);
            var inner = RoundRectangle(innerR, 2, ddCorners);
            var faceOuter = RoundRectangle(faceOuterR, 3, faceCorners);
            var faceInner = RoundRectangle(faceInnerR, 2, faceCorners);

            using (var b = new SolidBrush(Color.FromArgb(150, Color.White)))
            {
                e.Graphics.FillPath(b, inner);
            }


            using (var p = new Pen(button.Pressed && button.SizeMode != RibbonElementSizeMode.DropDown ? ColorTable.ButtonPressedBorderOut : ColorTable.ButtonSelectedBorderOut))
            {
                e.Graphics.DrawPath(p, outer);
            }

            using (var p = new Pen(button.Pressed && button.SizeMode != RibbonElementSizeMode.DropDown ? ColorTable.ButtonPressedBorderIn : ColorTable.ButtonSelectedBorderIn))
            {
                e.Graphics.DrawPath(p, faceInner);
            }

            
            outer.Dispose(); inner.Dispose(); faceOuter.Dispose(); faceInner.Dispose();
        }

        /// <summary>
        /// Draws a SplitDropDown button with the dropdown area pressed
        /// </summary>
        /// <param name="e"></param>
        /// <param name="button"></param>
        public void DrawSplitButtonDropDownPressed(RibbonItemRenderEventArgs e, RibbonButton button)
        {
        }

        /// <summary>
        /// Draws a SplitDropDown button with the dropdown area selected
        /// </summary>
        /// <param name="e"></param>
        /// <param name="button"></param>
        public void DrawSplitButtonDropDownSelected(RibbonItemRenderEventArgs e, RibbonButton button)
        {
            var outerR = Rectangle.FromLTRB(
                button.DropDownBounds.Left,
                button.DropDownBounds.Top,
                button.DropDownBounds.Right - 1,
                button.DropDownBounds.Bottom - 1);

            var innerR = Rectangle.FromLTRB(
                outerR.Left + 1,
                outerR.Top + (button.SizeMode == RibbonElementSizeMode.Large ? 1 : 0),
                outerR.Right - 1,
                outerR.Bottom - 1);

            var faceOuterR = Rectangle.FromLTRB(
                button.ButtonFaceBounds.Left,
                button.ButtonFaceBounds.Top,
                button.ButtonFaceBounds.Right - 1,
                button.ButtonFaceBounds.Bottom - 1);

            var faceInnerR = Rectangle.FromLTRB(
                faceOuterR.Left + 1,
                faceOuterR.Top + 1,
                faceOuterR.Right + (button.SizeMode == RibbonElementSizeMode.Large ? -1 : 0),
                faceOuterR.Bottom + (button.SizeMode == RibbonElementSizeMode.Large ? 0 : -1));

            var faceCorners = ButtonFaceRounding(button);
            var ddCorners = ButtonDdRounding(button);

            var outer = RoundRectangle(outerR, 3, ddCorners);
            var inner = RoundRectangle(innerR, 2, ddCorners);
            var faceOuter = RoundRectangle(faceOuterR, 3, faceCorners);
            var faceInner = RoundRectangle(faceInnerR, 2, faceCorners);

            using (var b = new SolidBrush(Color.FromArgb(150, Color.White)))
            {
                e.Graphics.FillPath(b, faceInner);
            }

            using (var p = new Pen(button.Pressed && button.SizeMode != RibbonElementSizeMode.DropDown ? ColorTable.ButtonPressedBorderIn : ColorTable.ButtonSelectedBorderIn))
            {
                e.Graphics.DrawPath(p, faceInner);
            }

            using (var p = new Pen(button.Pressed && button.SizeMode != RibbonElementSizeMode.DropDown ? ColorTable.ButtonPressedBorderOut : ColorTable.ButtonSelectedBorderOut))
            {
                e.Graphics.DrawPath(p, faceOuter);
            }

            outer.Dispose(); inner.Dispose(); faceOuter.Dispose(); faceInner.Dispose();
        } 
        #endregion

        #region Group
        /// <summary>
        /// Draws the background of the specified  RibbonItemGroup
        /// </summary>
        /// <param name="e"></param>
        /// <param name="?"></param>
        public void DrawItemGroup(RibbonItemRenderEventArgs e, RibbonItemGroup grp)
        {
            var outerR = Rectangle.FromLTRB(
                grp.Bounds.Left,
                grp.Bounds.Top,
                grp.Bounds.Right - 1,
                grp.Bounds.Bottom - 1);

            var innerR = Rectangle.FromLTRB(
                outerR.Left + 1,
                outerR.Top + 1,
                outerR.Right - 1,
                outerR.Bottom - 1);

            var glossyR = Rectangle.FromLTRB(
                outerR.Left + 1,
                outerR.Top + outerR.Height / 2 + 1,
                outerR.Right - 1,
                outerR.Bottom - 1);

            var outer = RoundRectangle(outerR, 2);
            var inner = RoundRectangle(innerR, 2);
            var glossy = RoundRectangle(glossyR, 2);

            using (var b = new LinearGradientBrush(
                innerR, ColorTable.ItemGroupBgNorth, ColorTable.ItemGroupBgSouth, 90))
            {
                e.Graphics.FillPath(b, inner);
            }

            using (var b = new LinearGradientBrush(
                glossyR, ColorTable.ItemGroupBgGlossy, Color.Transparent, 90))
            {
                e.Graphics.FillPath(b, glossy);
            }

            outer.Dispose();
            inner.Dispose();
        }

        /// <summary>
        /// Draws the background of the specified  RibbonItemGroup
        /// </summary>
        /// <param name="e"></param>
        /// <param name="?"></param>
        public void DrawItemGroupBorder(RibbonItemRenderEventArgs e, RibbonItemGroup grp)
        {
            var outerR = Rectangle.FromLTRB(
                grp.Bounds.Left,
                grp.Bounds.Top,
                grp.Bounds.Right - 1,
                grp.Bounds.Bottom - 1);

            var innerR = Rectangle.FromLTRB(
                outerR.Left + 1,
                outerR.Top + 1,
                outerR.Right - 1,
                outerR.Bottom - 1);

            var outer = RoundRectangle(outerR, 2);
            var inner = RoundRectangle(innerR, 2);

            using (var dark = new Pen(ColorTable.ItemGroupSeparatorDark))
            {
                using (var light = new Pen(ColorTable.ItemGroupSeparatorLight))
                {
                    foreach (var item in grp.Items)
                    {
                        if (item == grp.LastItem) break;

                        e.Graphics.DrawLine(dark,
                                            new Point(item.Bounds.Right, item.Bounds.Top),
                                            new Point(item.Bounds.Right, item.Bounds.Bottom));

                        e.Graphics.DrawLine(light,
                                            new Point(item.Bounds.Right + 1, item.Bounds.Top),
                                            new Point(item.Bounds.Right + 1, item.Bounds.Bottom));
                    }
                }
            }

            using (var p = new Pen(ColorTable.ItemGroupOuterBorder))
            {
                e.Graphics.DrawPath(p, outer);
            }

            using (var p = new Pen(ColorTable.ItemGroupInnerBorder))
            {
                e.Graphics.DrawPath(p, inner);
            }

            outer.Dispose();
            inner.Dispose();
        } 

        #endregion

        #region ButtonList

        public void DrawButtonList(Graphics g, RibbonButtonList list)
        {
            var outerR = Rectangle.FromLTRB(
                list.Bounds.Left,
                list.Bounds.Top,
                list.Bounds.Right - 1,
                list.Bounds.Bottom);

            using (var path = RoundRectangle(outerR, 3, Corners.East))
            {
                var bgcolor = list.Selected ? ColorTable.ButtonListBgSelected : ColorTable.ButtonListBg;

                if (list.Canvas is RibbonDropDown) bgcolor = ColorTable.DropDownBg;

                using (var b = new SolidBrush(bgcolor))
                {
                    g.FillPath(b, path);
                }

                using (var p = new Pen(ColorTable.ButtonListBorder))
                {
                    g.DrawPath(p, path);
                } 
            }

            #region Control Buttons

            if (!list.ButtonDownEnabled)
            {
                DrawButtonDisabled(g, list.ButtonDownBounds,  list.ButtonDropDownPresent ? Corners.None : Corners.SouthEast);
            }
            else if (list.ButtonDownPressed)
            {
                DrawButtonPressed(g, list.ButtonDownBounds, list.ButtonDropDownPresent ? Corners.None : Corners.SouthEast);
            }
            else if (list.ButtonDownSelected)
            {
                DrawButtonSelected(g, list.ButtonDownBounds, list.ButtonDropDownPresent ? Corners.None : Corners.SouthEast);
            }
            else
            {
                DrawButton(g, list.ButtonDownBounds, Corners.None);
            }

            if (!list.ButtonUpEnabled)
            {
                DrawButtonDisabled(g, list.ButtonUpBounds, Corners.NorthEast);
            }
            else if (list.ButtonUpPressed)
            {
                DrawButtonPressed(g, list.ButtonUpBounds, Corners.NorthEast);
            }
            else if (list.ButtonUpSelected)
            {
                DrawButtonSelected(g, list.ButtonUpBounds, Corners.NorthEast);
            }
            else
            {
                DrawButton(g, list.ButtonUpBounds, Corners.NorthEast);
            }

            if (list.ButtonDropDownPresent)
            {
                if (list.ButtonDropDownPressed)
                {
                    DrawButtonPressed(g, list.ButtonDropDownBounds, Corners.SouthEast);
                }
                else if (list.ButtonDropDownSelected)
                {
                    DrawButtonSelected(g, list.ButtonDropDownBounds, Corners.SouthEast);
                }
                else
                {
                    DrawButton(g, list.ButtonDropDownBounds, Corners.SouthEast);
                } 
            }

            var dk = ColorTable.Arrow;
            var lt = ColorTable.ArrowLight;
            var ds = ColorTable.ArrowDisabled;

            var arrUp = CenterOn(list.ButtonUpBounds, new Rectangle(Point.Empty, arrowSize)); arrUp.Offset(0, 1);
            var arrD = CenterOn(list.ButtonDownBounds, new Rectangle(Point.Empty, arrowSize)); arrD.Offset(0, 1);
            var arrdd = CenterOn(list.ButtonDropDownBounds, new Rectangle(Point.Empty, arrowSize)); arrdd.Offset(0, 3);

            DrawArrow(g, arrUp, list.ButtonUpEnabled ? lt : Color.Transparent, RibbonArrowDirection.Up); arrUp.Offset(0, -1);
            DrawArrow(g, arrUp, list.ButtonUpEnabled ? dk : ds, RibbonArrowDirection.Up);

            DrawArrow(g, arrD, list.ButtonDownEnabled ? lt : Color.Transparent, RibbonArrowDirection.Down); arrD.Offset(0, -1);
            DrawArrow(g, arrD, list.ButtonDownEnabled ? dk : ds, RibbonArrowDirection.Down);

            if (list.ButtonDropDownPresent)
            {


                using (var b = new SolidBrush(ColorTable.Arrow))
                {
                    var sm = g.SmoothingMode;
                    g.SmoothingMode = SmoothingMode.None;
                    g.FillRectangle(b, new Rectangle(new Point(arrdd.Left - 1, arrdd.Top - 4), new Size(arrowSize.Width + 2, 1)));
                    g.SmoothingMode = sm;
                }

                DrawArrow(g, arrdd, lt, RibbonArrowDirection.Down); arrdd.Offset(0, -1);
                DrawArrow(g, arrdd, dk, RibbonArrowDirection.Down);
            }
            #endregion
        }

        public void DrawButtonListButtonUp(Graphics g, RibbonButtonList list)
        {
            
        }

        public void DrawButtonListButtonDown(Graphics g, RibbonButtonList list)
        {

        }

        public void DrawButtonListButtonDropDown(Graphics g, RibbonButtonList list)
        {

        }

        #endregion

        #region Separator

        public void DrawSeparator(Graphics g, RibbonSeparator separator)
        {
            if (separator.SizeMode == RibbonElementSizeMode.DropDown)
            {
                if (!string.IsNullOrEmpty(separator.Text))
                {
                    using (var b = new SolidBrush(ColorTable.SeparatorBg))
                    {
                        g.FillRectangle(b, separator.Bounds);
                    }

                    using (var p = new Pen(ColorTable.SeparatorLine))
                    {
                        g.DrawLine(p,
                                   new Point(separator.Bounds.Left, separator.Bounds.Bottom),
                                   new Point(separator.Bounds.Right, separator.Bounds.Bottom));
                    }
                }
                else
                {
                    using (var p = new Pen(ColorTable.DropDownImageSeparator))
                    {
                        g.DrawLine(p,
                                   new Point(separator.Bounds.Left + 30, separator.Bounds.Top + 1),
                                   new Point(separator.Bounds.Right, separator.Bounds.Top + 1));
                    }
                }
            }
            else
            {
                using (var p = new Pen(ColorTable.SeparatorDark))
                {
                    g.DrawLine(p,
                               new Point(separator.Bounds.Left, separator.Bounds.Top),
                               new Point(separator.Bounds.Left, separator.Bounds.Bottom));
                }

                using (var p = new Pen(ColorTable.SeparatorLight))
                {
                    g.DrawLine(p,
                               new Point(separator.Bounds.Left + 1, separator.Bounds.Top),
                               new Point(separator.Bounds.Left + 1, separator.Bounds.Bottom));
                }
            }
        }

        public void DrawSeparatorText(RibbonItemBoundsEventArgs e, RibbonSeparator sep)
        {
            using (Brush b = new SolidBrush(GetTextColor(sep.Enabled)))
            {
                e.Graphics.DrawString(sep.Text, new Font(sep.Owner.Font, FontStyle.Bold), b, e.Bounds); 
            }
        }

        #endregion

        #region TextBox

        /// <summary>
        /// Draws a disabled textbox
        /// </summary>
        /// <param name="g"></param>
        /// <param name="bounds"></param>
        public void DrawTextBoxDisabled(Graphics g, Rectangle bounds)
        {

            using (var b = new SolidBrush(SystemColors.Control))
            {
                g.FillRectangle(b, bounds);
            }

            using (var p = new Pen(ColorTable.TextBoxBorder))
            {
                g.DrawRectangle(p, bounds);
            }

        }

        /// <summary>
        /// Draws an unselected textbox
        /// </summary>
        /// <param name="g"></param>
        /// <param name="bounds"></param>
        public void DrawTextBoxUnselected(Graphics g, Rectangle bounds)
        {
            
            using (var b = new SolidBrush(ColorTable.TextBoxUnselectedBg))
            {
                g.FillRectangle(b, bounds);
            }

            using (var p = new Pen(ColorTable.TextBoxBorder))
            {
                g.DrawRectangle(p, bounds);
            }
            
        }

        /// <summary>
        /// Draws an unselected textbox
        /// </summary>
        /// <param name="g"></param>
        /// <param name="bounds"></param>
        public void DrawTextBoxSelected(Graphics g, Rectangle bounds)
        {
            using (var b = new SolidBrush(SystemColors.Window))
            {
                g.FillRectangle(b, bounds);
            }

            using (var p = new Pen(ColorTable.TextBoxBorder))
            {
                g.DrawRectangle(p, bounds);
            }
        }

        /// <summary>
        /// Draws the text of a RibbonTextbox
        /// </summary>
        /// <param name="g"></param>
        /// <param name="e"></param>
        public void DrawTextBoxText(Graphics g, RibbonItemBoundsEventArgs e)
        {
            var t = e.Item as RibbonTextBox;

            var f = new StringFormat()
                        {
                            Alignment = StringAlignment.Near,
                            LineAlignment = StringAlignment.Center,
                            Trimming = StringTrimming.None,
                        };
            f.FormatFlags |= StringFormatFlags.NoWrap;

            e.Graphics.DrawString(t.TextBoxText, e.Ribbon.Font, 
                                  t.Enabled ? SystemBrushes.ControlText : SystemBrushes.GrayText, t.TextBoxTextBounds, f);

            if (t.LabelVisible)
            {
                using (Brush b = new SolidBrush(GetTextColor(e.Item.Enabled)))
                {
                    e.Graphics.DrawString(t.Text, e.Ribbon.Font, b, t.LabelBounds, f);
                } 
            }
        }

        #endregion

        #region ComboBox

        public void DrawComboxDropDown(Graphics g, RibbonComboBox b)
        {
            if (b.DropDownButtonPressed)
            {
                DrawButtonPressed(g, b.DropDownButtonBounds, Corners.None);
            }
            else if (b.DropDownButtonSelected)
            {
                DrawButtonSelected(g, b.DropDownButtonBounds, Corners.None);
            }
            else if(b.Selected)
            {
                DrawButton(g, b.DropDownButtonBounds, Corners.None);
            }

            DrawArrowShaded(g, b.DropDownButtonBounds, RibbonArrowDirection.Down, true);//b.Enabled);
        }

        #endregion

        #region Quick Access and Caption Bar

        public void DrawCaptionBarBackground(Rectangle r, Graphics g)
        {
            var smbuff = g.SmoothingMode;
            var r1 = new Rectangle(0, 0, r.Width, 4);
            var r2 = new Rectangle(0, r1.Bottom, r.Width, 4);
            var r3 = new Rectangle(0, r2.Bottom, r.Width, r.Height - 8);
            var r4 = new Rectangle(0, r3.Bottom, r.Width, 1);

            var rects = new [] { r1, r2, r3, r4 };
            var colors = new [,] { 
                                     { ColorTable.Caption1, ColorTable.Caption2 },
                                     { ColorTable.Caption3, ColorTable.Caption4 },
                                     { ColorTable.Caption5, ColorTable.Caption6 },
                                     { ColorTable.Caption7, ColorTable.Caption7 }
                                 };

            g.SmoothingMode = SmoothingMode.None;

            for (int i = 0; i < rects.Length; i++)
            {
                var grect = rects[i]; grect.Height += 2; grect.Y -= 1;
                using (var b = new LinearGradientBrush(grect, colors[i, 0], colors[i, 1], 90))
                {
                    g.FillRectangle(b, rects[i]);
                }
            }

            g.SmoothingMode = smbuff;
        }

        public override void OnRenderRibbonCaptionBar(RibbonRenderEventArgs e)
        {
            DrawCaptionBarBackground(new Rectangle(0, 0, e.Ribbon.Width, e.Ribbon.CaptionBarSize), e.Graphics);
        }

        public override void OnRenderRibbonOrb(RibbonRenderEventArgs e)
        {
            if (e.Ribbon.OrbVisible)
                DrawOrb(e.Graphics, e.Ribbon.OrbBounds, e.Ribbon.OrbImage, e.Ribbon.OrbSelected, e.Ribbon.OrbPressed);
        }

        public override void OnRenderRibbonQuickAccessToolbarBackground(RibbonRenderEventArgs e)
        {
            var bounds = e.Ribbon.QuickAcessToolbar.Bounds;
            var padding = e.Ribbon.QuickAcessToolbar.Padding;
            var margin = e.Ribbon.QuickAcessToolbar.Margin;
            var a = new Point(bounds.Left - (e.Ribbon.OrbVisible ? margin.Left : 0), bounds.Top);
            var b = new Point(bounds.Right + padding.Right, bounds.Top);
            var c = new Point(bounds.Left, bounds.Bottom);
            var d = new Point(b.X, c.Y);
            var z = new Point(c.X - 2, a.Y + bounds.Height / 2 - 1);

            using (var p = new Pen(ColorTable.QuickAccessBorderLight, 3))
            {
                using (var path = CreateQuickAccessPath(a, b, c, d, z, bounds, 0, 0, e.Ribbon))
                {
                    e.Graphics.DrawPath(p, path);
                }
            }

            using (var path = CreateQuickAccessPath(a, b, c, d, z, bounds, 0, 0, e.Ribbon))
            {
                using (var p = new Pen(ColorTable.QuickAccessBorderDark))
                {
                    e.Graphics.DrawPath(p, path);
                }

                using (var br = new LinearGradientBrush(
                    b, d, Color.FromArgb(150, ColorTable.QuickAccessUpper),Color.FromArgb(150, ColorTable.QuickAccessLower)
                    ))
                {
                    e.Graphics.FillPath(br, path);
                }
            }
        }

        private GraphicsPath CreateQuickAccessPath(Point a, Point b, Point c, Point d, Point e, Rectangle bounds, int offsetx, int offsety, Controls.Ribbon.Ribbon ribbon)
        {
            a.Offset(offsetx, offsety); b.Offset(offsetx, offsety); c.Offset(offsetx, offsety); 
            d.Offset(offsetx, offsety); e.Offset(offsetx, offsety);

            var path = new GraphicsPath();

            path.AddLine(a, b);
            path.AddArc(new Rectangle(b.X - bounds.Height / 2, b.Y, bounds.Height, bounds.Height), -90, 180);
            path.AddLine(d, c);
            if (ribbon.OrbVisible)
            {
                path.AddCurve(new [] { c, e, a });
            }
            else
            {
                path.AddArc(new Rectangle(a.X - bounds.Height / 2, a.Y, bounds.Height, bounds.Height), 90, 180);
            }
            

            return path;
        }

        /// <summary>
        /// Draws the orb on the specified state
        /// </summary>
        /// <param name="g">Device to draw</param>
        /// <param name="r">Layout rectangle for the orb</param>
        /// <param name="selected">Specifies if the orb should be drawn as selected</param>
        /// <param name="pressed">Specifies if the orb should be drawn as pressed</param>
        public void DrawOrb(Graphics g, Rectangle r, Image image, bool selected, bool pressed)
        {
            int sweep, start;
            Point p1, p2, p3;
            Color bgdark, bgmed, bglight, light;
            var rinner = r; 
            rinner.Inflate(-1, -1);
            var shadow = r; 
            shadow.Offset(1, 1); shadow.Inflate(2, 2);

            #region Color Selection

            if (pressed)
            {
                bgdark = ColorTable.OrbPressedBackgroundDark;
                bgmed = ColorTable.OrbPressedBackgroundMedium;
                bglight = ColorTable.OrbPressedBackgroundLight;
                light = ColorTable.OrbPressedLight;
            }
            else if (selected)
            {
                bgdark = ColorTable.OrbSelectedBackgroundDark;
                bgmed = ColorTable.OrbSelectedBackgroundDark;
                bglight = ColorTable.OrbSelectedBackgroundLight;
                light = ColorTable.OrbSelectedLight;
            }
            else
            {
                bgdark = ColorTable.OrbBackgroundDark;
                bgmed = ColorTable.OrbBackgroundMedium;
                bglight = ColorTable.OrbBackgroundLight;
                light = ColorTable.OrbLight;
            }

            #endregion

            #region Shadow

            using (var p = new GraphicsPath())
            {
                p.AddEllipse(shadow);

                using (var gradient = new PathGradientBrush(p))
                {
                    gradient.WrapMode = WrapMode.Clamp;
                    gradient.CenterPoint = new PointF(shadow.Left + shadow.Width / 2, shadow.Top + shadow.Height / 2);
                    gradient.CenterColor = Color.FromArgb(180, Color.Black);
                    gradient.SurroundColors = new [] { Color.Transparent };

                    var blend = new Blend(3)
                                    {
                                        Factors = new[] {0f, 1f, 1f},
                                        Positions = new[] {0, 0.2f, 1f}
                                    };
                    gradient.Blend = blend;

                    g.FillPath(gradient, p);
                }

            }

            

            #endregion

            #region Orb Background

            using (var p = new Pen(bgdark, 1))
            {
                g.DrawEllipse(p, r);
            }

            using (var p = new GraphicsPath())
            {
                p.AddEllipse(r);
                using (var gradient = new PathGradientBrush(p))
                {
                    gradient.WrapMode = WrapMode.Clamp;
                    gradient.CenterPoint = new PointF(Convert.ToSingle(r.Left + r.Width / 2), Convert.ToSingle(r.Bottom));
                    gradient.CenterColor = bglight;
                    gradient.SurroundColors = new [] { bgmed };

                    var blend = new Blend(3)
                                    {
                                        Factors = new[] {0f, .8f, 1f},
                                        Positions = new[] {0, 0.50f, 1f}
                                    };
                    gradient.Blend = blend;
                    
                    g.FillPath(gradient, p);
                } 
            }

            #endregion

            #region Bottom round shine

            var bshine = new Rectangle(0, 0, r.Width / 2, r.Height / 2);
            bshine.X = r.X + (r.Width - bshine.Width) / 2;
            bshine.Y = r.Y + r.Height / 2;



            using (var p = new GraphicsPath())
            {
                p.AddEllipse(bshine);

                using (var gradient = new PathGradientBrush(p))
                {
                    gradient.WrapMode = WrapMode.Clamp;
                    gradient.CenterPoint = new PointF(Convert.ToSingle(r.Left + r.Width / 2), Convert.ToSingle(r.Bottom));
                    gradient.CenterColor = Color.White;
                    gradient.SurroundColors = new Color[] { Color.Transparent };

                    g.FillPath(gradient, p);
                }
            }

            

            #endregion

            #region Upper Glossy
            using (var p = new GraphicsPath())
            {
                sweep = 160;
                start = 180 + (180 - sweep) / 2;
                p.AddArc(rinner, start, sweep);

                p1 = Point.Round(p.PathData.Points[0]);
                p2 = Point.Round(p.PathData.Points[p.PathData.Points.Length - 1]);
                p3 = new Point(rinner.Left + rinner.Width / 2, p2.Y - 3);
                p.AddCurve(new [] { p2, p3, p1 });

                using (var gradient = new PathGradientBrush(p))
                {
                    gradient.WrapMode = WrapMode.Clamp;
                    gradient.CenterPoint = p3;
                    gradient.CenterColor = Color.Transparent;
                    gradient.SurroundColors = new [] { light };

                    var blend = new Blend(3) {Factors = new[] {.3f, .8f, 1f}, Positions = new[] {0, 0.50f, 1f}};
                    gradient.Blend = blend;

                    g.FillPath(gradient, p);
                }

                using (var b = new LinearGradientBrush(new Point(r.Left, r.Top), new Point(r.Left, p1.Y), Color.White, Color.Transparent))
                {
                    var blend = new Blend(4)
                                    {
                                        Factors = new[] {0f, .4f, .8f, 1f},
                                        Positions = new[] {0f, .3f, .4f, 1f}
                                    };
                    b.Blend = blend;
                    g.FillPath(b, p);
                }
            } 
            #endregion

            #region Upper Shine
            ///Upper shine
            using (var p = new GraphicsPath())
            {
                sweep = 160;
                start = 180 + (180 - sweep) / 2;
                p.AddArc(rinner, start, sweep);

                using (var pen = new Pen(Color.White))
                {
                    g.DrawPath(pen, p); 
                }
            } 
            #endregion

            #region Lower Shine
            using (var p = new GraphicsPath())
            {
                sweep = 160;
                start = (180 - sweep) / 2;
                p.AddArc(rinner, start, sweep);
                var pt = Point.Round(p.PathData.Points[0]);

                var rrinner = rinner; rrinner.Inflate(-1, -1);
                sweep = 160;
                start = (180 - sweep) / 2;
                p.AddArc(rrinner, start, sweep);

                using (var b = new LinearGradientBrush(
                    new Point(rinner.Left, rinner.Bottom),
                    new Point(rinner.Left, pt.Y - 1),
                    light, Color.FromArgb(50, light)))
                {
                    g.FillPath(b, p);
                }
            }

            #endregion

            #region Orb Icon

            if (image != null)
            {
                var irect = new Rectangle(Point.Empty, image.Size);
                irect.X = r.X + (r.Width - irect.Width) / 2;
                irect.Y = r.Y + (r.Height - irect.Height) / 2;
                g.DrawImage(image, irect);
            }

            #endregion


        }

        #endregion

        #region Ribbon Orb DropDown

        public override void OnRenderOrbDropDownBackground(RibbonOrbDropDownEventArgs e)
        {
            int Width = e.RibbonOrbDropDown.Width;
            int Height = e.RibbonOrbDropDown.Height;

            var OrbDDContent = e.RibbonOrbDropDown.ContentBounds;
            var Bcontent = e.RibbonOrbDropDown.ContentButtonsBounds;
            var OuterRect = new Rectangle(0, 0, Width - 1, Height - 1);
            var InnerRect = new Rectangle(1, 1, Width - 3, Height - 3);
            var NorthNorthRect = new Rectangle(1, 1, Width - 3, OrbDDContent.Top / 2);
            var northSouthRect = new Rectangle(1, NorthNorthRect.Bottom, NorthNorthRect.Width, OrbDDContent.Top / 2);
            var southSouthRect = Rectangle.FromLTRB(1,
                                                    (Height - OrbDDContent.Bottom) / 2 + OrbDDContent.Bottom, Width - 1, Height - 1);

            var OrbDropDownDarkBorder = ColorTable.OrbDropDownDarkBorder;
            var OrbDropDownLightBorder = ColorTable.OrbDropDownLightBorder;
            var OrbDropDownBack = ColorTable.OrbDropDownBack;
            var OrbDropDownNorthA = ColorTable.OrbDropDownNorthA;
            var OrbDropDownNorthB = ColorTable.OrbDropDownNorthB;
            var OrbDropDownNorthC = ColorTable.OrbDropDownNorthC;
            var OrbDropDownNorthD = ColorTable.OrbDropDownNorthD;
            var OrbDropDownSouthC = ColorTable.OrbDropDownSouthC;
            var OrbDropDownSouthD = ColorTable.OrbDropDownSouthD;
            var OrbDropDownContentbg = ColorTable.OrbDropDownContentbg;
            var OrbDropDownContentbglight = ColorTable.OrbDropDownContentbglight;
            var OrbDropDownSeparatorlight = ColorTable.OrbDropDownSeparatorlight;
            var OrbDropDownSeparatordark = ColorTable.OrbDropDownSeparatordark;

            var innerPath = RoundRectangle(InnerRect, 6);
            var outerPath = RoundRectangle(OuterRect, 6);

            e.Graphics.SmoothingMode = SmoothingMode.None;

            using (Brush b = new SolidBrush(Color.FromArgb(0x8e, 0x8e, 0x8e)))
            {
                e.Graphics.FillRectangle(b, new Rectangle(Width - 10, Height - 10, 10, 10));
            }

            using (Brush b = new SolidBrush(OrbDropDownBack))
            {
                e.Graphics.FillPath(b, outerPath);
            }

            GradientRect(e.Graphics, NorthNorthRect, OrbDropDownNorthA, OrbDropDownNorthB);
            GradientRect(e.Graphics, northSouthRect, OrbDropDownNorthC, OrbDropDownNorthD);
            GradientRect(e.Graphics, southSouthRect, OrbDropDownSouthC, OrbDropDownSouthD);

            using (var p = new Pen(OrbDropDownDarkBorder))
            {
                e.Graphics.DrawPath(p, outerPath);
            }

            e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;

            using (var p = new Pen(OrbDropDownLightBorder))
            {
                e.Graphics.DrawPath(p, innerPath);
            }

            innerPath.Dispose();
            outerPath.Dispose();

            #region Content
            InnerRect = OrbDDContent; InnerRect.Inflate(0, 0);
            OuterRect = OrbDDContent; OuterRect.Inflate(1, 1);

            using (var b = new SolidBrush(OrbDropDownContentbg))
            {
                e.Graphics.FillRectangle(b, OrbDDContent);
            }

            using (var b = new SolidBrush(OrbDropDownContentbglight))
            {
                e.Graphics.FillRectangle(b, Bcontent);
            }

            using (var p = new Pen(OrbDropDownSeparatorlight))
            {
                e.Graphics.DrawLine(p, Bcontent.Right, Bcontent.Top, Bcontent.Right, Bcontent.Bottom);
            }

            using (var p = new Pen(OrbDropDownSeparatordark))
            {
                e.Graphics.DrawLine(p, Bcontent.Right - 1, Bcontent.Top, Bcontent.Right - 1, Bcontent.Bottom);
            }

            using (var p = new Pen(OrbDropDownLightBorder))
            {
                e.Graphics.DrawRectangle(p, OuterRect);
            }

            using (var p = new Pen(OrbDropDownDarkBorder))
            {
                e.Graphics.DrawRectangle(p, InnerRect);
            } 
            #endregion

            #region Orb
            var orbb = e.Ribbon.RectangleToScreen(e.Ribbon.OrbBounds);
            orbb = e.RibbonOrbDropDown.RectangleToClient(orbb);
            DrawOrb(e.Graphics, orbb, e.Ribbon.OrbImage, e.Ribbon.OrbSelected, e.Ribbon.OrbPressed); 
            #endregion
        }

        #endregion

        #endregion

        #region Overrides
        public override void OnRenderRibbonBackground(RibbonRenderEventArgs e)
        {
            e.Graphics.Clear(ColorTable.RibbonBackground);
        }

        public override void OnRenderRibbonTab(RibbonTabRenderEventArgs e)
        {
            if (e.Tab.Active)
            {
                DrawCompleteTab(e);
            }
            else if (e.Tab.Pressed)
            {
                DrawTabPressed(e);
            }
            else if (e.Tab.Selected)
            {
                DrawTabSelected(e);
            }
            else
            {
                DrawTabNormal(e);
            }
        }

        public override void OnRenderRibbonTabText(RibbonTabRenderEventArgs e)
        {

            var sf = new StringFormat();

            sf.Alignment = StringAlignment.Center;
            sf.Trimming = StringTrimming.EllipsisCharacter;
            sf.LineAlignment = StringAlignment.Center;
            sf.FormatFlags |= StringFormatFlags.NoWrap;

            var r = Rectangle.FromLTRB(
                e.Tab.TabBounds.Left + e.Ribbon.TabTextMargin.Left,
                e.Tab.TabBounds.Top + e.Ribbon.TabTextMargin.Top,
                e.Tab.TabBounds.Right - e.Ribbon.TabTextMargin.Right,
                e.Tab.TabBounds.Bottom - e.Ribbon.TabTextMargin.Bottom);

            using (Brush b = new SolidBrush(GetTextColor(true, e.Tab.Active ? ColorTable.TabActiveText : ColorTable.TabText )))
            {
                e.Graphics.TextRenderingHint = System.Drawing.Text.TextRenderingHint.ClearTypeGridFit;
                e.Graphics.DrawString(e.Tab.Text, e.Ribbon.Font, b, r, sf);
            }

        }

        public override void OnRenderRibbonPanelBackground(RibbonPanelRenderEventArgs e)
        {
            if (e.Panel.OverflowMode && !(e.Canvas is RibbonPanelPopup))
            {
                if (e.Panel.Pressed)
                {
                    DrawPanelOverflowPressed(e);
                }
                else if (e.Panel.Selected)
                {
                    DrawPannelOveflowSelected(e);
                }
                else
                {
                    DrawPanelOverflowNormal(e);
                }
            }
            else
            {
                if (e.Panel.Selected)
                {
                    DrawPanelSelected(e);
                }
                else
                {
                    DrawPanelNormal(e);
                }
            }
        }

        public override void OnRenderRibbonPanelText(RibbonPanelRenderEventArgs e)
        {
            if (e.Panel.OverflowMode && !(e.Canvas is RibbonPanelPopup))
            {
                return;
            }

            var textArea =
                Rectangle.FromLTRB(
                    e.Panel.Bounds.Left + 1,
                    e.Panel.ContentBounds.Bottom,
                    e.Panel.Bounds.Right - 1,
                    e.Panel.Bounds.Bottom - 1);

            var sf = new StringFormat {Alignment = StringAlignment.Center, LineAlignment = StringAlignment.Center};

            using (Brush b = new SolidBrush(GetTextColor(e.Panel.Enabled, ColorTable.PanelText)))
            {
                e.Graphics.DrawString(e.Panel.Text, e.Ribbon.Font, b, textArea, sf);
            }

        }

        public override void OnRenderRibbonItem(RibbonItemRenderEventArgs e)
        {
            if (e.Item is RibbonButton)
            {
                #region Button
                var b = e.Item as RibbonButton;

                if (b.Enabled)
                {
                    if (b.Style == RibbonButtonStyle.Normal)
                    {
                        if (b.Pressed && b.SizeMode != RibbonElementSizeMode.DropDown)
                        {
                            DrawButtonPressed(e.Graphics, b);
                        }
                        else if (b.Selected)
                        {
                            if (b.Checked)
                            {
                                DrawButtonPressed(e.Graphics, b);
                            }
                            else
                            {
                                DrawButtonSelected(e.Graphics, b);
                            }
                        }
                        else if (b.Checked)
                        {
                            DrawButtonChecked(e.Graphics, b);
                        }
                        else if (b is RibbonOrbOptionButton)
                        {
                            DrawOrbOptionButton(e.Graphics, b.Bounds);
                        }
                    }
                    else
                    {

                        if (b.DropDownPressed && b.SizeMode != RibbonElementSizeMode.DropDown)
                        {
                            DrawButtonPressed(e.Graphics, b);
                            DrawSplitButtonDropDownSelected(e, b);
                        }
                        else if (b.Pressed && b.SizeMode != RibbonElementSizeMode.DropDown)
                        {
                            DrawButtonPressed(e.Graphics, b);
                            DrawSplitButtonSelected(e, b);
                        }

                        else if (b.DropDownSelected)
                        {
                            DrawButtonSelected(e.Graphics, b);
                            DrawSplitButtonDropDownSelected(e, b);
                        }
                        else if (b.Selected)
                        {
                            DrawButtonSelected(e.Graphics, b);
                            DrawSplitButtonSelected(e, b);
                        }
                        else if (b.Checked)
                        {
                            DrawButtonChecked(e.Graphics, b);
                        }
                    } 
                }

                if (b.Style != RibbonButtonStyle.Normal && !(b.Style == RibbonButtonStyle.DropDown && b.SizeMode == RibbonElementSizeMode.Large))
                {
                    if (b.Style == RibbonButtonStyle.DropDown)
                    {
                        DrawButtonDropDownArrow(e.Graphics, b, b.OnGetDropDownBounds(b.SizeMode, b.Bounds));
                    }
                    else
                    {
                        DrawButtonDropDownArrow(e.Graphics, b, b.DropDownBounds);
                    }
                }
                
                #endregion
            }
            else if (e.Item is RibbonItemGroup)
            {
                #region Group
                DrawItemGroup(e, e.Item as RibbonItemGroup);
                #endregion
            }
            else if (e.Item is RibbonButtonList)
            {
                #region ButtonList
                DrawButtonList(e.Graphics, e.Item as RibbonButtonList);
                #endregion
            }
            else if (e.Item is RibbonSeparator)
            {
                #region Separator
                DrawSeparator(e.Graphics, e.Item as RibbonSeparator);
                #endregion
            }
            else if (e.Item is RibbonTextBox)
            {
                #region TextBox

                RibbonTextBox t = e.Item as RibbonTextBox;

                if (t.Enabled)
                {
                    if (t != null && (t.Selected || (t.Editing)))
                    {
                        DrawTextBoxSelected(e.Graphics, t.TextBoxBounds);
                    }
                    else
                    {
                        DrawTextBoxUnselected(e.Graphics, t.TextBoxBounds);
                    }

                }
                else
                {
                    DrawTextBoxDisabled(e.Graphics, t.TextBoxBounds);
                }

                if (t is RibbonComboBox)
                {
                    DrawComboxDropDown(e.Graphics, t as RibbonComboBox);
                }

                #endregion
            }
        }

        public override void OnRenderRibbonItemBorder(RibbonItemRenderEventArgs e)
        {
            if (e.Item is RibbonItemGroup)
            {
                DrawItemGroupBorder(e, e.Item as RibbonItemGroup);
            }
        }

        public override void OnRenderRibbonItemText(RibbonItemBoundsEventArgs e)
        {
            if (e.Item is RibbonButton)
            {
                #region Button
                var b = e.Item as RibbonButton;
                var sf = new StringFormat
                             {
                                 LineAlignment = StringAlignment.Center,
                                 Alignment = StringAlignment.Near
                             };

                if (e.Item.SizeMode == RibbonElementSizeMode.Large)
                {
                    sf.Alignment = StringAlignment.Center;

                    if (!string.IsNullOrEmpty(e.Item.Text) && !e.Item.Text.Contains(" "))
                    {
                        sf.LineAlignment = StringAlignment.Near;
                    }
                }

                using (var brush = new SolidBrush(GetTextColor(e.Item.Enabled)))
                {
                    e.Graphics.DrawString(e.Item.Text, e.Ribbon.Font, brush, e.Bounds, sf);
                }

                if (b.Style == RibbonButtonStyle.DropDown && b.SizeMode == RibbonElementSizeMode.Large)
                {
                    DrawButtonDropDownArrow(e.Graphics, b, e.Bounds);
                } 
                #endregion
            }
            else if (e.Item is RibbonSeparator)
            {
                #region Separator
                DrawSeparatorText(e, e.Item as RibbonSeparator); 
                #endregion
            }
            else if (e.Item is RibbonTextBox)
            {
                #region Textbox
                DrawTextBoxText(e.Graphics, e);
                #endregion
            }
            else
            {
                #region Generic
                e.Graphics.DrawString(e.Item.Text, e.Ribbon.Font, SystemBrushes.ControlText, e.Bounds); 
                #endregion
            }
        }

        public override void OnRenderRibbonItemImage(RibbonItemBoundsEventArgs e)
        {
            var img = e.Item.Image;

            if (e.Item is RibbonButton)
            {
                if (!(e.Item.SizeMode == RibbonElementSizeMode.Large || e.Item.SizeMode == RibbonElementSizeMode.Overflow))
                {
                    img = (e.Item as RibbonButton).SmallImage;
                }
            }

            if (img != null)
            {

                if (!e.Item.Enabled)
                    img = CreateDisabledImage(img);

                e.Graphics.DrawImage(img, e.Bounds);
            }
            
        }

        public override void OnRenderPanelPopupBackground(RibbonCanvasEventArgs e)
        {
            var pnl = e.RelatedObject as RibbonPanel;

            if (pnl == null) return;

            var darkBorder = Rectangle.FromLTRB(
                e.Bounds.Left,
                e.Bounds.Top,
                e.Bounds.Right,
                e.Bounds.Bottom);

            var lightBorder = Rectangle.FromLTRB(
                e.Bounds.Left + 1,
                e.Bounds.Top + 1,
                e.Bounds.Right - 1,
                e.Bounds.Bottom - 1);

            var textArea =
                Rectangle.FromLTRB(
                    e.Bounds.Left + 1,
                    pnl.ContentBounds.Bottom,
                    e.Bounds.Right - 1,
                    e.Bounds.Bottom - 1);

            var dark = RoundRectangle(darkBorder, 3);
            var light = RoundRectangle(lightBorder, 3);
            var txt = RoundRectangle(textArea, 3, Corners.SouthEast | Corners.SouthWest);

            using (var p = new Pen(ColorTable.PanelLightBorder))
            {
                e.Graphics.DrawPath(p, light);
            }

            using (var p = new Pen(ColorTable.PanelDarkBorder))
            {
                e.Graphics.DrawPath(p, dark);
            }

            using (var b = new SolidBrush(ColorTable.PanelBackgroundSelected))
            {
                e.Graphics.FillPath(b, light);
            }

            using (var b = new SolidBrush(ColorTable.PanelTextBackground))
            {
                e.Graphics.FillPath(b, txt);
            }

            txt.Dispose();
            dark.Dispose();
            light.Dispose();
        }

        public override void OnRenderDropDownBackground(RibbonCanvasEventArgs e)
        {
            var outerR = new Rectangle(0, 0, e.Bounds.Width - 1, e.Bounds.Height - 1);
            var imgsR = new Rectangle(0, 0, 26, e.Bounds.Height);
            var dd = e.Canvas as RibbonDropDown;

            using (var b = new SolidBrush(ColorTable.DropDownBg))
            {
                e.Graphics.FillRectangle(b, outerR);
            }

            using (var b = new SolidBrush(ColorTable.DropDownImageBg))
            {
                e.Graphics.FillRectangle(b, imgsR);
            }

            using (var p = new Pen(ColorTable.DropDownImageSeparator))
            {
                e.Graphics.DrawLine(p,
                                    new Point(imgsR.Right, imgsR.Top),
                                    new Point(imgsR.Right, imgsR.Bottom));
            }

            using (var p = new Pen(ColorTable.DropDownBorder))
            {
                e.Graphics.DrawRectangle(p, outerR);
            }

            if (dd.ShowSizingGrip)
            {
                var gripArea = Rectangle.FromLTRB(
                    e.Bounds.Left + 1,
                    e.Bounds.Bottom - dd.SizingGripHeight,
                    e.Bounds.Right - 1,
                    e.Bounds.Bottom - 1);

                if (gripArea.Width == 0)
                {
                    return;
                }
                
                using (var b = new LinearGradientBrush(
                    gripArea, ColorTable.DropDownGripNorth, ColorTable.DropDownGripSouth, 90))
                {
                    e.Graphics.FillRectangle(b, gripArea);
                }

                using (var p = new Pen(ColorTable.DropDownGripBorder))
                {
                    e.Graphics.DrawLine(p,
                                        gripArea.Location,
                                        new Point(gripArea.Right - 1, gripArea.Top));
                }

                DrawGripDot(e.Graphics, new Point(gripArea.Right - 7, gripArea.Bottom - 3));
                DrawGripDot(e.Graphics, new Point(gripArea.Right - 3, gripArea.Bottom - 7));
                DrawGripDot(e.Graphics, new Point(gripArea.Right - 3, gripArea.Bottom - 3));
            }
        }

        public override void OnRenderTabScrollButtons(RibbonTabRenderEventArgs e)
        {
            if (e.Tab.ScrollLeftVisible)
            {
                if (e.Tab.ScrollLeftSelected)
                {
                    DrawButtonSelected(e.Graphics, e.Tab.ScrollLeftBounds, Corners.West);
                }
                else
                {
                    DrawButton(e.Graphics, e.Tab.ScrollLeftBounds, Corners.West);
                }

                DrawArrowShaded(e.Graphics, e.Tab.ScrollLeftBounds, RibbonArrowDirection.Right, true);
                
            }

            if (e.Tab.ScrollRightVisible)
            {
                if (e.Tab.ScrollRightSelected)
                {
                    DrawButtonSelected(e.Graphics, e.Tab.ScrollRightBounds, Corners.East);
                }
                else
                {
                    DrawButton(e.Graphics, e.Tab.ScrollRightBounds, Corners.East);
                }

                DrawArrowShaded(e.Graphics, e.Tab.ScrollRightBounds, RibbonArrowDirection.Left, true);
            }
        }

        #endregion
    }
}