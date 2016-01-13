using VisualEditor.Logic.Controls.Docking;

namespace VisualEditor.Logic.Commands.FloatingWindows
{
    internal class Concepts : AbstractCommand
    {
        public Concepts()
        {
            name = CommandNames.Concepts;
            text = CommandTexts.Concepts;
            image = Properties.Resources.ConceptWindow;
        }

        public override void Execute(object @object)
        {
            if (!Enabled)
            {
                return;
            }

            DockContainer.Instance.ConceptWindow.Show();
        }
    }
}