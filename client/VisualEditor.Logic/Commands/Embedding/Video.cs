namespace VisualEditor.Logic.Commands.Embedding
{
    internal class Video : AbstractCommand
    {
        public Video()
        {
            name = CommandNames.Video;
            text = CommandTexts.Video;
            image = Logic.Properties.Resources.Video;
        }

        public override void Execute(object @object)
        {
            if (!Enabled)
            {
                return;
            }

            CommandManager.Instance.GetCommand(CommandNames.VideoSmall).Execute(null);
        }
    }
}