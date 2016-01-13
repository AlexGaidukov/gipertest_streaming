using System;
using System.Drawing;
using System.Windows.Forms;
using VisualEditor.Logic.Helpers;
using VisualEditor.Logic.Warehouse;
using VisualEditor.Utils.ExceptionHandling;

namespace VisualEditor.Logic.Commands.HtmlEditing
{
    internal class Font : AbstractCommand
    {
        private const string operationCantBePerformedMessage = "Невозможно выполнить операцию. Попробуйте повтротить снова.";

        public Font()
        {
            name = CommandNames.Font;
            text = CommandTexts.Font;
            image = Properties.Resources.Font;
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

            using (var fd = new FontDialog())
            {
                fd.ShowColor = true;

                #region Получение стиля шрифта

                var fst = FontStyle.Regular;

                if (EditorObserver.ActiveEditor.IsBold)
                {
                    fst |= FontStyle.Bold;
                }

                if (EditorObserver.ActiveEditor.IsItalic)
                {
                    fst |= FontStyle.Italic;
                }

                if (EditorObserver.ActiveEditor.IsUnderline)
                {
                    fst |= FontStyle.Underline;
                }

                if (EditorObserver.ActiveEditor.IsStrikeThrough)
                {
                    fst |= FontStyle.Strikeout;
                }

                #endregion

                #region Преобразование размера шрифта

                var fs = EditorObserver.ActiveEditor.GetFontSize();

                switch (fs)
                {
                    case 0:
                        fs = 12;
                        break;
                    case 1:
                        fs = 8;
                        break;
                    case 2:
                        fs = 10;
                        break;
                    case 3:
                        fs = 12;
                        break;
                    case 4:
                        fs = 14;
                        break;
                    case 5:
                        fs = 18;
                        break;
                    case 6:
                        fs = 24;
                        break;
                    case 7:
                        fs = 28;
                        break;
                }

                #endregion

                #region Преобразование имени шрифта

                var fn = EditorObserver.ActiveEditor.GetFontName();

                if (fn.Equals(string.Empty))
                {
                    fn = "Times New Roman";
                }

                #endregion

                fd.Font = new System.Drawing.Font(fn, fs, fst);

                if (fd.ShowDialog(EditorObserver.DialogOwner) == DialogResult.OK)
                {
                    fn = fd.Font.FontFamily.Name;
                    fs = (int)Math.Round(fd.Font.Size);
                    var b = fd.Font.Bold;
                    var i = fd.Font.Italic;
                    var u = fd.Font.Underline;
                    var s = fd.Font.Strikeout;
                    var c = fd.Color;

                    try
                    {
                        EditorObserver.ActiveEditor.SetFontName(fn);
                        Warehouse.Warehouse.IsProjectModified = true;
                    }
                    catch (Exception exception)
                    {
                        ExceptionManager.Instance.LogException(exception);
                        UIHelper.ShowMessage(operationCantBePerformedMessage,
                            MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }

                    #region Преобразование размера шрифта

                    try
                    {
                        switch (fs)
                        {
                            case 8:
                                fs = 1;
                                EditorObserver.ActiveEditor.SetFontSize(fs);
                                break;
                            case 10:
                                fs = 2;
                                EditorObserver.ActiveEditor.SetFontSize(fs);
                                break;
                            case 12:
                                fs = 3;
                                EditorObserver.ActiveEditor.SetFontSize(fs);
                                break;
                            case 14:
                                fs = 4;
                                EditorObserver.ActiveEditor.SetFontSize(fs);
                                break;
                            case 18:
                                fs = 5;
                                EditorObserver.ActiveEditor.SetFontSize(fs);
                                break;
                            case 24:
                                fs = 6;
                                EditorObserver.ActiveEditor.SetFontSize(fs);
                                break;
                            case 28:
                                fs = 7;
                                EditorObserver.ActiveEditor.SetFontSize(fs);
                                break;
                        }
                    }
                    catch (Exception exception)
                    {
                        ExceptionManager.Instance.LogException(exception);
                        UIHelper.ShowMessage(operationCantBePerformedMessage,
                            MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }

                    #endregion

                    #region Установка стиля шрифта

                    try
                    {
                        if (b)
                        {
                            if (!EditorObserver.ActiveEditor.IsBold)
                            {
                                EditorObserver.ActiveEditor.ChangeBold();
                            }
                        }
                        else
                        {
                            if (EditorObserver.ActiveEditor.IsBold)
                            {
                                EditorObserver.ActiveEditor.ChangeBold();
                            }
                        }

                        if (i)
                        {
                            if (!EditorObserver.ActiveEditor.IsItalic)
                            {
                                EditorObserver.ActiveEditor.ChangeItalic();
                            }
                        }
                        else
                        {
                            if (EditorObserver.ActiveEditor.IsItalic)
                            {
                                EditorObserver.ActiveEditor.ChangeItalic();
                            }
                        }

                        if (u)
                        {
                            if (!EditorObserver.ActiveEditor.IsUnderline)
                            {
                                EditorObserver.ActiveEditor.ChangeUnderline();
                            }
                        }
                        else
                        {
                            if (EditorObserver.ActiveEditor.IsUnderline)
                            {
                                EditorObserver.ActiveEditor.ChangeUnderline();
                            }
                        }

                        if (s)
                        {
                            if (!EditorObserver.ActiveEditor.IsStrikeThrough)
                            {
                                EditorObserver.ActiveEditor.ChangeStrikeThrough();
                            }
                        }
                        else
                        {
                            if (EditorObserver.ActiveEditor.IsStrikeThrough)
                            {
                                EditorObserver.ActiveEditor.ChangeStrikeThrough();
                            }
                        }
                    }
                    catch (Exception exception)
                    {
                        ExceptionManager.Instance.LogException(exception);
                        UIHelper.ShowMessage(operationCantBePerformedMessage,
                            MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }

                    #endregion

                    try
                    {
                        EditorObserver.ActiveEditor.SetForeColor(Color.FromArgb(c.R, c.G, c.B));
                        Warehouse.Warehouse.IsProjectModified = true;
                    }
                    catch (Exception exception)
                    {
                        ExceptionManager.Instance.LogException(exception);
                        UIHelper.ShowMessage(operationCantBePerformedMessage,
                            MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }
                }
            }
        }
    }
}