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
    internal class PictureSmall : AbstractCommand
    {
        private const string fileIsAlreadyUsedMessage = "Файл с данным именем уже используется в проекте.\nЗаменить?";
        private const string operationCantBePerformedMessage = "Невозможно вставить рисунок в редактор.";

        public PictureSmall()
        {
            name = CommandNames.PictureSmall;
            text = CommandTexts.PictureSmall;
            image = Properties.Resources.PictureSmall;
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

            using (var pd = new PictureDialog())
            {
                var dtu = pd.DataTransferUnit;

                if (pd.ShowDialog(EditorObserver.DialogOwner) == DialogResult.OK)
                {
                    var source = dtu.GetNodeValue("Source");
                    var imageName = Guid.NewGuid().ToString();
                    var destPath = Path.Combine(Warehouse.Warehouse.AbsoluteEditorImagesDirectory, imageName);
                    destPath += Path.GetExtension(source);

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

                            // POSTPONE: Реализовать обновление изображения в редакторе.
                            //VisualHtmlEditor.ActiveEditor.Refresh();
                            //VisualHtmlEditor.ActiveEditor.Update();
                        }
                    }

                    var imageNameWithoutExtension = Path.GetFileNameWithoutExtension(source);
                    source = source.Replace(imageNameWithoutExtension, imageName);

                    var i = EditorObserver.ActiveEditor.Document.CreateElement(TagNames.ImageTagName);
                    var lt = dtu.GetNodeValue("LinkText");

                    if (string.IsNullOrEmpty(lt))
                    {
                        #region Рисунок

                        i.SetAttribute("sdocument", "0");

                        #region Источник

                        var s = Path.Combine(Warehouse.Warehouse.RelativeImagesDirectory, Path.GetFileName(source));
                        i.SetAttribute("src", s);

                        #endregion

                        #region Всплывающая подсказка

                        var t = dtu.GetNodeValue("Title");
                        if (!string.IsNullOrEmpty(t))
                        {
                            i.SetAttribute("title", t);
                        }

                        #endregion

                        #region Высота рисунка

                        var h = dtu.GetNodeValue("Height");
                        if (dtu.GetNodeValue("HeightUnit").Equals("% от размера окна"))
                        {
                            h += "%";
                        }
                        i.SetAttribute("height", h);

                        #endregion

                        #region Ширина рисунка

                        var w = dtu.GetNodeValue("Width");
                        if (dtu.GetNodeValue("WidthUnit").Equals("% от размера окна"))
                        {
                            w += "%";
                        }
                        i.SetAttribute("width", w);

                        #endregion

                        #region Выравнивание рисунка

                        var j = dtu.GetNodeValue("Justify");
                        if (j.Equals("по верхнему краю"))
                        {
                            i.SetAttribute("align", "top");
                        }
                        if (j.Equals("по центру"))
                        {
                            i.SetAttribute("align", "middle");
                        }
                        if (j.Equals("по нижнему краю"))
                        {
                            i.SetAttribute("align", "bottom");
                        }
                        if (j.Equals("по левому краю"))
                        {
                            i.SetAttribute("align", "left");
                        }
                        if (j.Equals("по правому краю"))
                        {
                            i.SetAttribute("align", "right");
                        }

                        #endregion

                        #region Толщина рамки

                        var pixels = Convert.ToInt32(dtu.GetNodeValue("BorderPixels"));
                        if (pixels > 0)
                        {
                            i.SetAttribute("border", pixels.ToString());
                        }

                        #endregion

                        #region Поля слева/справа

                        pixels = Convert.ToInt32(dtu.GetNodeValue("HorizontalSpace"));
                        if (pixels > 0)
                        {
                            i.SetAttribute("hspace", pixels.ToString());
                        }

                        #endregion

                        #region Поля сверху/снизу

                        pixels = Convert.ToInt32(dtu.GetNodeValue("VerticalSpace"));
                        if (pixels > 0)
                        {
                            i.SetAttribute("vspace", pixels.ToString());
                        }

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
                        #region Ссылка на рисунок

                        i.SetAttribute("sdocument", "1");

                        #region Источник

                        var s = Path.Combine(Warehouse.Warehouse.RelativeImagesDirectory, "Pic.png");
                        i.SetAttribute("src", s);

                        var s_ = Path.Combine(Warehouse.Warehouse.RelativeImagesDirectory, Path.GetFileName(source));
                        i.SetAttribute("src_", s_);

                        #endregion

                        #region Всплывающая подсказка

                        var t = dtu.GetNodeValue("Title");
                        if (!string.IsNullOrEmpty(t))
                        {
                            i.SetAttribute("title_", t);
                        }

                        #endregion

                        #region Высота рисунка

                        var h = dtu.GetNodeValue("Height");
                        if (dtu.GetNodeValue("HeightUnit").Equals("% от размера окна"))
                        {
                            h += "%";
                        }
                        i.SetAttribute("height_", h);

                        #endregion

                        #region Ширина рисунка

                        var w = dtu.GetNodeValue("Width");
                        if (dtu.GetNodeValue("WidthUnit").Equals("% от размера окна"))
                        {
                            w += "%";
                        }
                        i.SetAttribute("width_", w);

                        #endregion

                        #region Выравнивание рисунка

                        var j = dtu.GetNodeValue("Justify");
                        if (j.Equals("по верхнему краю"))
                        {
                            i.SetAttribute("align_", "top");
                        }
                        if (j.Equals("по центру"))
                        {
                            i.SetAttribute("align_", "middle");
                        }
                        if (j.Equals("по нижнему краю"))
                        {
                            i.SetAttribute("align_", "bottom");
                        }
                        if (j.Equals("по левому краю"))
                        {
                            i.SetAttribute("align_", "left");
                        }
                        if (j.Equals("по правому краю"))
                        {
                            i.SetAttribute("align_", "right");
                        }

                        #endregion

                        #region Толщина рамки

                        var pixels = Convert.ToInt32(dtu.GetNodeValue("BorderPixels"));
                        if (pixels > 0)
                        {
                            i.SetAttribute("border_", pixels.ToString());
                        }

                        #endregion

                        #region Поля слева/справа

                        pixels = Convert.ToInt32(dtu.GetNodeValue("HorizontalSpace"));
                        if (pixels > 0)
                        {
                            i.SetAttribute("hspace_", pixels.ToString());
                        }

                        #endregion

                        #region Поля сверху/снизу

                        pixels = Convert.ToInt32(dtu.GetNodeValue("VerticalSpace"));
                        if (pixels > 0)
                        {
                            i.SetAttribute("vspace_", pixels.ToString());
                        }

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