namespace VisualEditor.Utils.Controls.Ribbon
{
    internal class RibbonComboBoxDesigner : RibbonElementWithItemCollectionDesigner
    {
        public override Controls.Ribbon.Ribbon Ribbon
        {
            get
            {
                if (Component is RibbonComboBox)
                {
                    return (Component as RibbonComboBox).Owner;
                }
                return null; 
            }
        }

        public override RibbonItemCollection Collection
        {
            get
            {
                if (Component is RibbonComboBox)
                {
                    return (Component as RibbonComboBox).DropDownItems;
                }
                return null; 
            }
        }
    }
}