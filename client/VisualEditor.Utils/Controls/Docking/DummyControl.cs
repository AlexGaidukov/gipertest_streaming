using System.Windows.Forms;

namespace VisualEditor.Utils.Controls.Docking
{
    internal class DummyControl : Control
    {
        public DummyControl()
        {
            SetStyle(ControlStyles.Selectable, false);
        }
    }
}