namespace VisualEditor.Logic.Commands.Embedding
{
    internal class Bookmark : AbstractCommand
    {
        public Bookmark()
        {
            name = CommandNames.Bookmark;
            text = CommandTexts.Bookmark;
            image = Properties.Resources.Bookmark;
        }

        public override void Execute(object @object)
        {
            if (!Enabled)
            {
                return;
            }

            CommandManager.Instance.GetCommand(CommandNames.BookmarkSmall).Execute(null);
        }
    }
}