using VisualEditor.Logic.Warehouse;

namespace VisualEditor.Logic.Commands.Document
{
    internal class NavigateBackward : AbstractCommand
    {
        public NavigateBackward()
        {
            name = CommandNames.NavigateBackward;
            text = CommandTexts.NavigateBackward;
            image = Properties.Resources.NavigateBackward;
        }

        public override void Execute(object @object)
        {
            if (!Enabled)
            {
                return;
            }
            
            PreviewObserver.PreviousDocument().Show();
        }
    }
}