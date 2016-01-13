using System.Drawing;
using System.Windows.Forms;
using VisualEditor.Logic.Controls.Trees;
using VisualEditor.Utils.Controls.Docking;

namespace VisualEditor.Logic.Controls.Docking.SideWindows
{
    internal class ConceptWindow : SideWindowBase
    {
        private const string text = "Компетенции";

        public ConceptWindow()
        {
            InitializeWindow();
        }

        public ConceptTree ConceptTree { get; private set; }

        private void InitializeWindow()
        {
            Text = text;
            ShowHint = DockState.DockRight;
            Icon = Icon.FromHandle(Properties.Resources.ConceptWindow.GetHicon());

            ConceptTree = new ConceptTree
                               {
                                   BorderStyle = BorderStyle.Fixed3D,
                                   Dock = DockStyle.Fill
                               };
            Controls.Add(ConceptTree);
        }
    }
}