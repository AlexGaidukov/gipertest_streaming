using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Windows.Forms;
using VisualEditor.Logic.Helpers;
using VisualEditor.Logic.Warehouse;
using VisualEditor.Utils.Controls.HtmlEditing;
using VisualEditor.Utils.ExceptionHandling;

namespace VisualEditor.Logic.Commands.Hint
{
    internal class HintPaste : AbstractCommand
    {
        private const string operationCantBePerformedMessage = "Невозможно выполнить операцию. Попробуйте повтротить снова.";

        public HintPaste()
        {
            name = CommandNames.HintPaste;
            text = CommandTexts.HintPaste;
            image = Properties.Resources.Paste;
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

            var data = Clipboard.GetDataObject();

            // Выполняется при копировании рисунка извне.
            if (data.GetDataPresent(DataFormats.Bitmap))
            {
                var b = (Bitmap)data.GetData(DataFormats.Bitmap);
                var path = Path.Combine(Warehouse.Warehouse.AbsoluteEditorImagesDirectory, string.Concat(Guid.NewGuid(), ".jpeg"));
                
                try
                {
                    b.Save(path, ImageFormat.Jpeg);
                }
                catch (Exception exception)
                {
                    ExceptionManager.Instance.LogException(exception);
                    UIHelper.ShowMessage(operationCantBePerformedMessage,
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                var imageDimension = Image.FromFile(path).PhysicalDimension;
                path = Path.Combine(Warehouse.Warehouse.RelativeImagesDirectory, Path.GetFileName(path));

                var i = EditorObserver.ActiveEditor.Document.CreateElement(TagNames.ImageTagName);
                i.SetAttribute("sdocument", "0");
                i.SetAttribute("src", path);
                i.SetAttribute("align", "left");
                i.SetAttribute("height", imageDimension.Height.ToString());
                i.SetAttribute("width", imageDimension.Width.ToString());

                try
                {
                    HtmlEditingToolHelper.InsertHtml(EditorObserver.ActiveEditor, i);
                }
                catch (Exception exception)
                {
                    ExceptionManager.Instance.LogException(exception);
                    UIHelper.ShowMessage(operationCantBePerformedMessage, MessageBoxButtons.OK,
                        MessageBoxIcon.Error);
                    return;
                }
            }
            // Выполняется при копировании различного контента и рисунка в пределах редактора.
            else
            {
                EditorObserver.ActiveEditor.Paste();
            }
        }
    }
}