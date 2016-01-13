using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace VisualEditor.Utils.Controls.Ribbon
{
    /// <summary>
    /// Represents a collection of RibbonPanel objects
    /// </summary>
    public sealed class RibbonPanelCollection : List<RibbonPanel>
    {
        private RibbonTab _ownerTab;

        public RibbonPanelCollection(RibbonTab ownerTab)
        {
            _ownerTab = ownerTab;
        }

        /// <summary>
        /// Creates a new RibbonPanelCollection
        /// </summary>
        /// <param name="owner">Ribbon that contains the tab of this panel collection</param>
        /// <param name="ownerTab">RibbonTab that contains this panel collection</param>
        /// <exception cref="ArgumentNullException">owner is null</exception>
        /// <exception cref="ArgumentNullException">ownerTab is null</exception>
        internal RibbonPanelCollection(Controls.Ribbon.Ribbon owner, RibbonTab ownerTab)
        {
            if (ownerTab == null) throw new ArgumentNullException("ownerTab");

            _ownerTab = ownerTab;
        }

        /// <summary>
        /// Gets the Ribbon that contains this panel collection
        /// </summary>
        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public Controls.Ribbon.Ribbon Owner
        {
            get
            {
                return _ownerTab.Owner;
            }
        }

        /// <summary>
        /// Gets the RibbonTab that contains this panel collection
        /// </summary>
        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public RibbonTab OwnerTab
        {
            get
            {
                return _ownerTab;
            }
        }

        /// <summary>
        /// Adds the specified item to the collection
        /// </summary>
        public new void Add(RibbonPanel item)
        {
            item.SetOwner(Owner);
            item.SetOwnerTab(OwnerTab);
            base.Add(item);
        }

        /// <summary>
        /// Adds a range of panels to the collection
        /// </summary>
        /// <param name="items">Panels to add</param>
        public new void AddRange(IEnumerable<RibbonPanel> items)
        {
            foreach (RibbonPanel p in items)
            {
                p.SetOwner(Owner);
                p.SetOwnerTab(OwnerTab);
            }

            base.AddRange(items);
        }

        /// <summary>
        /// Inserts the specified panel at the desired index
        /// </summary>
        /// <param name="index">Desired index to insert the panel</param>
        /// <param name="item">Panel to insert</param>
        public new void Insert(int index, RibbonPanel item)
        {
            item.SetOwner(Owner);
            item.SetOwnerTab(OwnerTab);
            base.Insert(index, item);
        }

        /// <summary>
        /// Sets the value of the Owner Property
        /// </summary>
        internal void SetOwner(Controls.Ribbon.Ribbon owner)
        {
            foreach (RibbonPanel panel in this)
            {
                panel.SetOwner(owner);
            }
        }

        /// <summary>
        /// Sets the value of the OwnerTab Property
        /// </summary>
        internal void SetOwnerTab(RibbonTab ownerTab)
        {
            _ownerTab = ownerTab;

            foreach (RibbonPanel panel in this)
            {
                panel.SetOwnerTab(OwnerTab);
            }
        }
    }
}