namespace VisualEditor.Logic.Commands.Embedding
{
    internal class Style : AbstractCommand
    {
        public Style()
        {
            name = CommandNames.Style;
            text = CommandTexts.Style;
            image = Properties.Resources.Style;
        }

        public override void Execute(object @object)
        {
            if (!Enabled)
            {
                return;
            }

            CommandManager.Instance.GetCommand(CommandNames.StyleSmall).Execute(null);
        }
    }
}