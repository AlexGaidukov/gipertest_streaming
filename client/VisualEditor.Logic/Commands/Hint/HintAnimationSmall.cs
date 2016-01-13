using System;
using System.IO;
using System.Windows.Forms;
using VisualEditor.Logic.Dialogs;
using VisualEditor.Logic.Helpers;
using VisualEditor.Logic.Warehouse;
using VisualEditor.Utils.Controls.HtmlEditing;
using VisualEditor.Utils.ExceptionHandling;

namespace VisualEditor.Logic.Commands.Hint
{
    internal class HintAnimationSmall : AbstractCommand
    {
        private const string fileIsAlreadyUsedMessage = "Файл с данным именем уже используется в проекте.\nЗаменить?";
        private const string operationCantBePerformedMessage = "Невозможно вставить анимацию в редактор.";

        public HintAnimationSmall()
        {
            name = CommandNames.HintAnimationSmall;
            text = CommandTexts.HintAnimationSmall;
            image = Properties.Resources.AnimationSmall;
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

            using (var ad = new AnimationDialog())
            {
                var dtu = ad.DataTransferUnit;

                if (ad.ShowDialog(EditorObserver.DialogOwner) == DialogResult.OK)
                {
                    var source = dtu.GetNodeValue("Source");
                    var destPath = Path.Combine(Warehouse.Warehouse.AbsoluteEditorFlashesDirectory, Path.GetFileName(source));

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

                    var i = EditorObserver.ActiveEditor.Document.CreateElement(TagNames.ImageTagName);
                    var lt = dtu.GetNodeValue("LinkText");

                    if (string.IsNullOrEmpty(lt))
                    {
                        #region Анимация

                        i.SetAttribute("sdocument", "0");

                        #region Источник

                        var s = Path.Combine(Warehouse.Warehouse.RelativeImagesDirectory, "Anim.png");
                        i.SetAttribute("src", s);

                        var s_ = Path.Combine(Warehouse.Warehouse.RelativeFlashesDirectory, Path.GetFileName(source));
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
                        #region Ссылка на анимацию

                        i.SetAttribute("sdocument", "1");

                        #region Источник

                        var s = Path.Combine(Warehouse.Warehouse.RelativeImagesDirectory, "Anim.png");
                        i.SetAttribute("src", s);

                        var s_ = Path.Combine(Warehouse.Warehouse.RelativeFlashesDirectory, Path.GetFileName(source));
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