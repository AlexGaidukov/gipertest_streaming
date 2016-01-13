namespace VisualEditor.Utils.Controls.Ribbon
{
    internal class RibbonPanelDesigner : RibbonElementWithItemCollectionDesigner
    {

        public override Controls.Ribbon.Ribbon Ribbon
        {
            get
            {
                if (Component is RibbonPanel)
                {
                    return (Component as RibbonPanel).Owner;
                }
                return null;
            }
        }

        public override RibbonItemCollection Collection
        {
            get
            {
                if (Component is RibbonPanel)
                {
                    return (Component as RibbonPanel).Items;
                }
                return null;
            }
        }
    }
}