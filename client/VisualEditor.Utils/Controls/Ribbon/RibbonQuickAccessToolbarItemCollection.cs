using System.Collections.Generic;

namespace VisualEditor.Utils.Controls.Ribbon
{
    public class RibbonQuickAccessToolbarItemCollection : RibbonItemCollection
    {
        #region Fields

        #endregion

        /// <summary>
        /// Creates a new collection
        /// </summary>
        internal RibbonQuickAccessToolbarItemCollection(RibbonQuickAccessToolbar toolbar)
        {
            OwnerToolbar = toolbar;
            SetOwner(toolbar.Owner);
        }

        /// <summary>
        /// Gets the group that owns this item collection
        /// </summary>
        public RibbonQuickAccessToolbar OwnerToolbar { get; private set; }

        /// <summary>
        /// Adds the specified item to the collection
        /// </summary>
        public new void Add(RibbonItem item)
        {
            item.MaxSizeMode = RibbonElementSizeMode.Compact;
            base.Add(item);
        }

        /// <summary>
        /// Adds the specified range of items
        /// </summary>
        /// <param name="items">Items to add</param>
        public new void AddRange(IEnumerable<RibbonItem> items)
        {
            foreach (RibbonItem item in items)
            {
                item.MaxSizeMode = RibbonElementSizeMode.Compact;
            }
            base.AddRange(items);
        }

        /// <summary>
        /// Inserts the specified item at the desired index
        /// </summary>
        /// <param name="index">Desired index of the item</param>
        /// <param name="item">Item to insert</param>
        public new void Insert(int index, RibbonItem item)
        {
            item.MaxSizeMode = RibbonElementSizeMode.Compact;
            base.Insert(index, item);
        }
    }
}