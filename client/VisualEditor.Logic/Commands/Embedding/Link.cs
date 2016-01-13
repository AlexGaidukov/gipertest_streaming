namespace VisualEditor.Logic.Commands.Embedding
{
    internal class Link : AbstractCommand
    {
        public Link()
        {
            name = CommandNames.Link;
            text = CommandTexts.Link;
            image = Properties.Resources.Link;
        }

        public override void Execute(object @object)
        {
            if (!Enabled)
            {
                return;
            }

            CommandManager.Instance.GetCommand(CommandNames.LinkSmall).Execute(null);
        }
    }
}