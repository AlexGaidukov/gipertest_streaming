using System.Drawing;

namespace VisualEditor.Utils.Controls.Ribbon
{
    public sealed class RibbonSeparator : RibbonItem
    {
        public RibbonSeparator()
        {

        }

        public RibbonSeparator(string text)
        {
            Text = text;
        }

        public override void OnPaint(object sender, RibbonElementPaintEventArgs e)
        {
            Owner.Renderer.OnRenderRibbonItem(new RibbonItemRenderEventArgs(
                                                  Owner, e.Graphics, e.Clip, this));

            if (!string.IsNullOrEmpty(Text))
            {
                Owner.Renderer.OnRenderRibbonItemText(new RibbonItemBoundsEventArgs(
                                                          Owner, e.Graphics, e.Clip, this,
                                                          Rectangle.FromLTRB(
                                                              Bounds.Left + Owner.ItemMargin.Left,
                                                              Bounds.Top + Owner.ItemMargin.Top,
                                                              Bounds.Right - Owner.ItemMargin.Right,
                                                              Bounds.Bottom - Owner.ItemMargin.Bottom))); 
            }
        }

        public override Size MeasureSize(object sender, RibbonElementMeasureSizeEventArgs e)
        {

            if (e.SizeMode == RibbonElementSizeMode.DropDown)
            {
                if (string.IsNullOrEmpty(Text))
                {
                    return new Size(1, 1);
                }

                var sz = e.Graphics.MeasureString(Text, new Font(Owner.Font, FontStyle.Bold)).ToSize();
                SetLastMeasuredSize(new Size(sz.Width + Owner.ItemMargin.Horizontal,
                                             sz.Height + Owner.ItemMargin.Vertical));
            }
            else
            {
                SetLastMeasuredSize(new Size(2,
                                             OwnerPanel.ContentBounds.Height - Owner.ItemPadding.Vertical -
                                             Owner.ItemMargin.Vertical));
            }

            return LastMeasuredSize;
        }
    }
}