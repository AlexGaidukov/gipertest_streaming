namespace VisualEditor.Utils.Controls.Ribbon
{
    internal class RibbonButtonListDesigner : RibbonElementWithItemCollectionDesigner
    {
        public override Controls.Ribbon.Ribbon Ribbon
        {
            get
            {
                if (Component is RibbonButtonList)
                {
                    return (Component as RibbonButtonList).Owner;
                }
                return null;
            }
        }

        public override RibbonItemCollection Collection
        {
            get
            {
                if (Component is RibbonButtonList)
                {
                    return (Component as RibbonButtonList).Buttons;
                }
                return null;
            }
        }
    }
}