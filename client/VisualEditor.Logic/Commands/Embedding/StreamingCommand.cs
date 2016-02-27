namespace VisualEditor.Logic.Commands.Embedding
{
    using System;
    using System.IO;
    using System.Windows.Forms;

    using VisualEditor.Logic.Dialogs;
    using VisualEditor.Logic.Helpers;
    using VisualEditor.Logic.Warehouse;
    using VisualEditor.Utils.Controls.HtmlEditing;
    using VisualEditor.Utils.ExceptionHandling;

    internal class StreamingCommand : AbstractCommand
    {
        private const string operationCantBePerformedMessage = "Невозможно вставить видео в редактор.";

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

                if (streamingVideoDialog.ShowDialog(EditorObserver.DialogOwner) == DialogResult.OK)
                {
                    var rawHtml = EditorObserver.ActiveEditor.Document.CreateElement(TagNames.ImageTagName);

                    rawHtml.SetAttribute("sdocument", "0");

                    #region Источник

                    var sourceImage = Path.Combine(Warehouse.RelativeImagesDirectory, "Vid.png");
                    rawHtml.SetAttribute("src", sourceImage);

                    var sourceLink = dataTransferUnit.GetNodeValue("Source");
                    rawHtml.SetAttribute("src_", sourceLink);

                    #endregion

                    try
                    {
                        HtmlEditingToolHelper.InsertHtml(EditorObserver.ActiveEditor, rawHtml);
                    }
                    catch (Exception exception)
                    {
                        ExceptionManager.Instance.LogException(exception);
                        UIHelper.ShowMessage(operationCantBePerformedMessage, MessageBoxButtons.OK,
                            MessageBoxIcon.Error);
                        return;
                    }
                }
            }
        }
    }
}
