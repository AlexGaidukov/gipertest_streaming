using System.Drawing;
using System.Windows.Forms;
using VisualEditor.Logic.Controls.Trees;
using VisualEditor.Utils.Controls.Docking;

namespace VisualEditor.Logic.Controls.Docking.SideWindows
{
    internal class WarningWindow : SideWindowBase
    {
        private const string text = "Предупреждения ";

        public WarningWindow()
        {
            InitializeWindow();
        }

        public WarningTree WarningTree { get; private set; }

        private void InitializeWindow()
        {
            Text = text;
            ShowHint = DockState.DockBottom;
            Icon = Icon.FromHandle(Properties.Resources.WarningWindow.GetHicon());

            WarningTree = new WarningTree
                              {
                                  BorderStyle = BorderStyle.Fixed3D,
                                  Dock = DockStyle.Fill
                              };
            Controls.Add(WarningTree);
        }
    }
}