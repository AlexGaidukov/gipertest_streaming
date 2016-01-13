using VisualEditor.Logic.Warehouse;

namespace VisualEditor.Logic.Commands.Document
{
    internal class NavigateForward : AbstractCommand
    {
        public NavigateForward()
        {
            name = CommandNames.NavigateForward;
            text = CommandTexts.NavigateForward;
            image = Properties.Resources.NavigateForward;
        }

        public override void Execute(object @object)
        {
            if (!Enabled)
            {
                return;
            }

            PreviewObserver.NextDocument().Show();
        }
    }
}