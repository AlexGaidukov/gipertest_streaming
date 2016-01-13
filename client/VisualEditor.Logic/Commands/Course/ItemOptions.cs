namespace VisualEditor.Logic.Commands.Course
{
    internal class ItemOptions : AbstractCommand
    {
        public ItemOptions()
        {
            name = CommandNames.ItemOptions;
            text = CommandTexts.ItemOptions;
            image = Properties.Resources.ItemOptions;
        }

        public override void Execute(object @object)
        {
            if (!Enabled)
            {
                return;
            }

            CommandManager.Instance.GetCommand(CommandNames.ItemOptionsSmall).Execute(null);
        }
    }
}