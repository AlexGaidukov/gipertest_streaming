using VisualEditor.Logic.Controls.Docking;

namespace VisualEditor.Logic.Commands.FloatingWindows
{
    internal class Course : AbstractCommand
    {
        public Course()
        {
            name = CommandNames.Course;
            text = CommandTexts.Course;
            image = Properties.Resources.CourseWindow;
        }

        public override void Execute(object @object)
        {
            if (!Enabled)
            {
                return;
            }

            DockContainer.Instance.CourseWindow.Show();
        }
    }
}