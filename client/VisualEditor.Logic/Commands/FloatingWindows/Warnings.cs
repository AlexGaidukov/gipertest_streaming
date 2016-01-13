using VisualEditor.Logic.Controls.Docking;

namespace VisualEditor.Logic.Commands.FloatingWindows
{
    internal class Warnings : AbstractCommand
    {
        public Warnings()
        {
            name = CommandNames.Warnings;
            text = CommandTexts.Warnings;
            image = Properties.Resources.WarningWindow;
        }

        public override void Execute(object @object)
        {
            if (!Enabled)
            {
                return;
            }

            DockContainer.Instance.WarningWindow.Show();
        }
    }
}