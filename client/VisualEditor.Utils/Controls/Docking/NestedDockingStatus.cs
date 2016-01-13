using System.Drawing;

namespace VisualEditor.Utils.Controls.Docking
{
    public sealed class NestedDockingStatus
    {
        internal NestedDockingStatus(DockPane pane)
        {
            PreviousPane = null;
            NestedPanes = null;
            IsDisplaying = false;
            DisplayingPreviousPane = null;
            DockPane = pane;
        }

        public DockPane DockPane { get; private set; }

        public NestedPaneCollection NestedPanes { get; private set; }

        public DockPane PreviousPane { get; private set; }

        private DockAlignment m_alignment = DockAlignment.Left;
        public DockAlignment Alignment
        {
            get	{	return m_alignment;	}
        }

        private double m_proportion = 0.5;
        public double Proportion
        {
            get	{	return m_proportion;	}
        }

        public bool IsDisplaying { get; private set; }

        public DockPane DisplayingPreviousPane { get; private set; }

        private DockAlignment m_displayingAlignment = DockAlignment.Left;
        public DockAlignment DisplayingAlignment
        {
            get	{	return m_displayingAlignment;	}
        }

        private double m_displayingProportion = 0.5;
        public double DisplayingProportion
        {
            get	{	return m_displayingProportion;	}
        }

        private Rectangle m_logicalBounds = Rectangle.Empty; 
        public Rectangle LogicalBounds
        {
            get	{	return m_logicalBounds;	}
        }

        private Rectangle m_paneBounds = Rectangle.Empty;
        public Rectangle PaneBounds
        {
            get	{	return m_paneBounds;	}
        }

        private Rectangle m_splitterBounds = Rectangle.Empty;
        public Rectangle SplitterBounds
        {
            get	{	return m_splitterBounds;	}
        }

        internal void SetStatus(NestedPaneCollection nestedPanes, DockPane previousPane, DockAlignment alignment, double proportion)
        {
            NestedPanes = nestedPanes;
            PreviousPane = previousPane;
            m_alignment = alignment;
            m_proportion = proportion;
        }

        internal void SetDisplayingStatus(bool isDisplaying, DockPane displayingPreviousPane, DockAlignment displayingAlignment, double displayingProportion)
        {
            IsDisplaying = isDisplaying;
            DisplayingPreviousPane = displayingPreviousPane;
            m_displayingAlignment = displayingAlignment;
            m_displayingProportion = displayingProportion;
        }

        internal void SetDisplayingBounds(Rectangle logicalBounds, Rectangle paneBounds, Rectangle splitterBounds)
        {
            m_logicalBounds = logicalBounds;
            m_paneBounds = paneBounds;
            m_splitterBounds = splitterBounds;
        }
    }
}