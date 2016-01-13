namespace VisualEditor.Logic.Commands.Embedding
{
    internal class Picture : AbstractCommand
    {
        public Picture()
        {
            name = CommandNames.Picture;
            text = CommandTexts.Picture;
            image = Properties.Resources.Picture;
        }

        public override void Execute(object @object)
        {
            if (!Enabled)
            {
                return;
            }

            CommandManager.Instance.GetCommand(CommandNames.PictureSmall).Execute(null);
        }
    }
}