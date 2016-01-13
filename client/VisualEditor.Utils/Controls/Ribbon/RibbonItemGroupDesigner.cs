namespace VisualEditor.Utils.Controls.Ribbon
{
    internal class RibbonItemGroupDesigner : RibbonElementWithItemCollectionDesigner
    {
        public override Controls.Ribbon.Ribbon Ribbon
        {
            get
            {
                if (Component is RibbonItemGroup)
                {
                    return (Component as RibbonItemGroup).Owner;
                }
                return null;
            }
        }

        public override RibbonItemCollection Collection
        {
            get 
            {
                if (Component is RibbonItemGroup)
                {
                    return (Component as RibbonItemGroup).Items;
                }
                return null;
            }
        }
    }
}