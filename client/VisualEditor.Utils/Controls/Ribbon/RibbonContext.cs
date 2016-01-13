using System.ComponentModel;

namespace VisualEditor.Utils.Controls.Ribbon
{
    /// <summary>
    /// Represents a context on the Ribbon
    /// </summary>
    /// <remarks>Contexts are useful when some tabs are volatile, depending on some selection. A RibbonTabContext can be added to the ribbon by calling Ribbon.Contexts.Add</remarks>
    [ToolboxItem(false)]
    public class RibbonContext : Component
    {
        private Controls.Ribbon.Ribbon _owner;
        private RibbonTabCollection _tabs;

        /// <summary>
        /// Creates a new RibbonTabContext
        /// </summary>

        public RibbonContext(Controls.Ribbon.Ribbon owner)
        {
            _tabs = new RibbonTabCollection(owner);
        }

        /// <summary>
        /// Gets or sets the text of the Context
        /// </summary>
        public string Text { get; set; }

        /// <summary>
        /// Gets or sets the color of the glow that indicates a context
        /// </summary>
        public System.Drawing.Color GlowColor { get; set; }

        /// <summary>
        /// Gets the Ribbon that owns this context
        /// </summary>
        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public Controls.Ribbon.Ribbon Owner
        {
            get
            {
                return _owner;
            }
        }

        public RibbonTabCollection Tabs
        {
            get
            {
                return _tabs;
            }
        }

        /// <summary>
        /// Sets the value of the Owner Property
        /// </summary>
        internal void SetOwner(Controls.Ribbon.Ribbon owner)
        {
            _owner = owner;
            _tabs.SetOwner(owner);
        }
    }
}