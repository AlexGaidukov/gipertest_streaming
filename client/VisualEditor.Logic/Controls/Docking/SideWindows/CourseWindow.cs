using System.Drawing;
using System.Windows.Forms;
using VisualEditor.Logic.Controls.Trees;
using VisualEditor.Utils.Controls.Docking;

namespace VisualEditor.Logic.Controls.Docking.SideWindows
{
    internal class CourseWindow : SideWindowBase
    {
        private const string text = "Учебный курс";

        public CourseWindow()
        {
            InitializeWindow();
        }

        public CourseTree CourseTree { get; private set; }

        private void InitializeWindow()
        {
            Text = text;
            ShowHint = DockState.DockLeft;
            Icon = Icon.FromHandle(Properties.Resources.CourseWindow.GetHicon());

            CourseTree = new CourseTree
                             {
                                 BorderStyle = BorderStyle.Fixed3D,
                                 Dock = DockStyle.Fill
                             };
            Controls.Add(CourseTree);
        }
    }
}