namespace VisualEditor.Logic.Commands.Embedding
{
    using VisualEditor.Logic.Warehouse;

    internal class Streaming : AbstractCommand
    {
        public Streaming()
        {
            this.name = CommandNames.Streaming;
            this.text = CommandTexts.Streaming;
            this.image = Properties.Resources.VideoSmall;
        }

        public override void Execute(object @object)
        {
            if (!Enabled)
            {
                return;
            }

            if (EditorObserver.ActiveEditor == null)
            {
                return;
            }

            if (EditorObserver.ActiveEditor.IsSelection)
            {
                return;
            }
        }
    }
}
