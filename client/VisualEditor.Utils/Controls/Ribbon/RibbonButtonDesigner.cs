namespace VisualEditor.Utils.Controls.Ribbon
{
    internal class RibbonButtonDesigner : RibbonElementWithItemCollectionDesigner
    {

        public override Controls.Ribbon.Ribbon Ribbon
        {
            get
            {
                if (Component is RibbonButton)
                {
                    return (Component as RibbonButton).Owner;
                }
                return null;
            }
        }

        public override RibbonItemCollection Collection
        {
            get
            {
                if (Component is RibbonButton)
                {
                    return (Component as RibbonButton).DropDownItems;
                }
                return null;
            }
        }
    }
}