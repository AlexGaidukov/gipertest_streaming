namespace VisualEditor.Logic.Commands.Embedding
{
    using System.Windows.Forms;

    using VisualEditor.Logic.Dialogs;
    using VisualEditor.Logic.Warehouse;

    internal class StreamingCommand : AbstractCommand
    {
        public StreamingCommand()
        {
            this.name = CommandNames.StreamingCommand;
            this.text = CommandTexts.StreamingCommand;
            this.image = Properties.Resources.VideoSmall;
        }

        public override void Execute(object @object)
        {
            if (!this.Enabled)
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

            using (var streamingVideoDialog = new StreamingVideoDialog())
            {
                var dataTransferUnit = streamingVideoDialog.DataTransferUnit;

                streamingVideoDialog.ShowDialog(EditorObserver.DialogOwner);

                if (streamingVideoDialog.ShowDialog(EditorObserver.DialogOwner) == DialogResult.OK)
                {
                    
                }
            }
        }
    }
}
