using System;
using System.IO;
using System.Windows.Forms;
using VisualEditor.Logic.Dialogs;
using VisualEditor.Logic.Helpers;
using VisualEditor.Logic.Warehouse;
using VisualEditor.Utils.Controls.HtmlEditing;
using VisualEditor.Utils.ExceptionHandling;

namespace VisualEditor.Logic.Commands.Embedding
{
    internal class VideoSmall : AbstractCommand
    {
        private const string fileIsAlreadyUsedMessage = "Файл с данным именем уже используется в проекте.\nЗаменить?";
        private const string operationCantBePerformedMessage = "Невозможно вставить видео в редактор.";

        public VideoSmall()
        {
            name = CommandNames.VideoSmall;
            text = CommandTexts.VideoSmall;
            image = Logic.Properties.Resources.VideoSmall;
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

            using (var vd = new VideoDialog())
            {
                var dtu = vd.DataTransferUnit;

                if (vd.ShowDialog(EditorObserver.DialogOwner) == DialogResult.OK)
                {
                    var source = dtu.GetNodeValue("Source");
                    var videoName = Guid.NewGuid().ToString();
                    var destPath = Path.Combine(Warehouse.Warehouse.AbsoluteEditorVideosDirectory, videoName);
                    destPath += Path.GetExtension(source);

                    // POSTPONE: Реализовать проверку размера файла.
                    if (!File.Exists(destPath))
                    {
                        try
                        {
                            File.Copy(source, destPath);
                        }
                        catch (Exception exception)
                        {
                            ExceptionManager.Instance.LogException(exception);
                            UIHelper.ShowMessage(operationCantBePerformedMessage, MessageBoxButtons.OK,
                                MessageBoxIcon.Error);
                            return;
                        }
                    }
                    else
                    {
                        var dr = MessageBox.Show(fileIsAlreadyUsedMessage,
                            System.Windows.Forms.Application.ProductName,
                            MessageBoxButtons.OKCancel, MessageBoxIcon.Question);
                        if (dr.Equals(DialogResult.OK))
                        {
                            try
                            {
                                File.Copy(source, destPath, true);
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

                    var videoNameWithoutExtension = Path.GetFileNameWithoutExtension(source);
                    source = source.Replace(videoNameWithoutExtension, videoName);

                    var i = EditorObserver.ActiveEditor.Document.CreateElement(TagNames.ImageTagName);
                    var lt = dtu.GetNodeValue("LinkText");

                    if (string.IsNullOrEmpty(lt))
                    {
                        #region Видео

                        i.SetAttribute("sdocument", "0");

                        #region Источник

                        var s = Path.Combine(Warehouse.Warehouse.RelativeImagesDirectory, "Vid.png");
                        i.SetAttribute("src", s);

                        var s_ = Path.Combine(Warehouse.Warehouse.RelativeVideosDirectory, Path.GetFileName(source));
                        i.SetAttribute("src_", s_);

                        #endregion

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

	                    #endregion                    
                    }
                    else
                    {
                        #region Ссылка на видео

                        i.SetAttribute("sdocument", "1");

                        #region Источник

                        var s = Path.Combine(Warehouse.Warehouse.RelativeImagesDirectory, "Vid.png");
                        i.SetAttribute("src", s);

                        var s_ = Path.Combine(Warehouse.Warehouse.RelativeVideosDirectory, Path.GetFileName(source));
                        i.SetAttribute("src_", s_);

                        #endregion

                        #region Текст ссылки

                        i.SetAttribute("alt", lt);

                        #endregion

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

                        #endregion
                    }
                }
            }
        }
    }
}