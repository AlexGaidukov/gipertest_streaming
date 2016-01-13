using System;
using System.ComponentModel.Design;

namespace VisualEditor.Utils.Controls.Ribbon
{
    public class RibbonItemCollectionEditor : CollectionEditor
    {
        public RibbonItemCollectionEditor()
            : base(typeof(RibbonItemCollection))
        {

        }

        protected override Type CreateCollectionItemType()
        {
            return typeof(RibbonButton);
        }

        protected override Type[] CreateNewItemTypes()
        {
            return new [] {
                              typeof(RibbonButton),
                              typeof(RibbonButtonList),
                              typeof(RibbonItemGroup),
                              typeof(RibbonSeparator)};
        }
    }
}