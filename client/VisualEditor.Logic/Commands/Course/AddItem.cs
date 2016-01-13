namespace VisualEditor.Logic.Commands.Course
{
    internal class AddItem : AbstractCommand
    {
        public AddItem()
        {
            name = CommandNames.AddItem;
            text = CommandTexts.AddItem;
            image = Properties.Resources.AddItem;
        }

        public override void Execute(object @object)
        {
            if (!Enabled)
            {
                return;
            }
        }
    }
}