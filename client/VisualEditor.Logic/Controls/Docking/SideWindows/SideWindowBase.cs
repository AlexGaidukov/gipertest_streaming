using VisualEditor.Utils.Controls.Docking;

namespace VisualEditor.Logic.Controls.Docking.SideWindows
{
    internal abstract class SideWindowBase : DockContent
    {
        protected SideWindowBase()
        {
            DockAreas = DockAreas.Float | DockAreas.DockLeft | DockAreas.DockRight | 
                        DockAreas.DockTop | DockAreas.DockBottom;
            HideOnClose = true;
        }

        public new virtual void Show()
        {
            Show(DockContainer.Instance);
        }
    }
}