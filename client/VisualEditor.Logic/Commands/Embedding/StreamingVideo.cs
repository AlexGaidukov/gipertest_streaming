namespace VisualEditor.Logic.Commands.Embedding
{
    internal class StreamingVideo : AbstractCommand
    {
        public StreamingVideo()
        {
            this.name = CommandNames.StreamingVideo;
            this.text = CommandTexts.StreamingVideo;
            this.image = Properties.Resources.Video;
        }

        public override void Execute(object @object)
        {
            if (!this.Enabled)
            {
                return;
            }

            CommandManager.Instance.GetCommand(CommandNames.StreamingVideo).Execute(null);
        }
    }
}
