using System;
using System.Drawing;
using System.Windows.Forms;
using VisualEditor.Logic.Dialogs;
using VisualEditor.Logic.Helpers;
using VisualEditor.Logic.Warehouse;
using VisualEditor.Utils.Controls.HtmlEditing;
using VisualEditor.Utils.ExceptionHandling;

namespace VisualEditor.Logic.Commands.Hint
{
    internal class HintTableSmall : AbstractCommand
    {
        private const string operationCantBePerformedMessage = "Невозможно вставить рисунок в редактор.";

        public HintTableSmall()
        {
            name = CommandNames.HintTableSmall;
            text = CommandTexts.HintTableSmall;
            image = Properties.Resources.TableSmall;
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

            using (var td = new TableDialog())
            {
                if (td.ShowDialog(EditorObserver.DialogOwner) == DialogResult.OK)
                {
                    var t = EditorObserver.ActiveEditor.Document.CreateElement(TagNames.TableTagName);

                    #region Высота таблицы

                    var height = td.DataTransferUnit.GetNodeValue("TableHeight");
                    if (td.DataTransferUnit.GetNodeValue("TableHeightUnit").Equals("% от размера окна"))
                    {
                        height += "%";
                    }
                    t.SetAttribute("height", height);

                    #endregion

                    #region Ширина таблицы

                    var width = td.DataTransferUnit.GetNodeValue("TableWidth");
                    if (td.DataTransferUnit.GetNodeValue("TableWidthUnit").Equals("% от размера окна"))
                    {
                        width += "%";
                    }
                    t.SetAttribute("width", width);

                    #endregion

                    #region Рамка

                    var pixels = Convert.ToInt32(td.DataTransferUnit.GetNodeValue("BorderPixels"));
                    if (pixels > 0)
                    {
                        t.SetAttribute("border", pixels.ToString());
                    }

                    #endregion

                    #region Поля

                    pixels = Convert.ToInt32(td.DataTransferUnit.GetNodeValue("MarginPixels"));
                    if (pixels > 0)
                    {
                        t.SetAttribute("cellspacing", pixels.ToString());
                    }

                    #endregion

                    #region Поля внутри ячейки

                    pixels = Convert.ToInt32(td.DataTransferUnit.GetNodeValue("InnerMarginPixels"));
                    if (pixels > 0)
                    {
                        t.SetAttribute("cellpadding", pixels.ToString());
                    }

                    #endregion

                    #region Выравнивание таблицы

                    var justify = td.DataTransferUnit.GetNodeValue("TableJustify");
                    if (justify.Equals("влево"))
                    {
                        t.SetAttribute("align", "left");
                    }
                    else if (justify.Equals("по центру"))
                    {
                        t.SetAttribute("align", "center");
                    }
                    else if (justify.Equals("вправо"))
                    {
                        t.SetAttribute("align", "right");
                    }

                    #endregion

                    #region Цвет фона

                    var bgcolor = td.DataTransferUnit.GetNodeValue("TableColor");
                    var color = bgcolor.Split(' ');
                    var red = Convert.ToByte(color[0]);
                    var green = Convert.ToByte(color[1]);
                    var blue = Convert.ToByte(color[2]);
                    t.SetAttribute("bgcolor", ColorTranslator.ToHtml(Color.FromArgb(red, green, blue)));

                    #endregion

                    #region Текст и выравнивание заголовка

                    var tableTitle = td.DataTransferUnit.GetNodeValue("TableTitle");
                    if (tableTitle.Length > 0)
                    {
                        var tableTitleLocation = td.DataTransferUnit.GetNodeValue("TableTitleLocation");

                        if (tableTitleLocation.Equals("слева"))
                        {
                            tableTitleLocation = "left";
                        }

                        if (tableTitleLocation.Equals("по центру"))
                        {
                            tableTitleLocation = "center";
                        }

                        if (tableTitleLocation.Equals("справа"))
                        {
                            tableTitleLocation = "right";
                        }

                        var c = EditorObserver.ActiveEditor.Document.CreateElement("CAPTION");
                        c.InnerHtml = tableTitle;
                        c.SetAttribute("align", tableTitleLocation);
                        t.AppendChild(c);
                    }

                    #endregion

                    #region Строки и столбцы

                    var rowsNumber = Convert.ToInt32(td.DataTransferUnit.GetNodeValue("RowsNumber"));
                    var columnsNumber = Convert.ToInt32(td.DataTransferUnit.GetNodeValue("ColumnsNumber"));

                    for (int r = 0; r < rowsNumber; r++)
                    {
                        var row = EditorObserver.ActiveEditor.Document.CreateElement(TagNames.TrTagName);
                        for (int c = 0; c < columnsNumber; c++)
                        {
                            var cell = EditorObserver.ActiveEditor.Document.CreateElement(TagNames.TdTagName);
                            row.AppendChild(cell);
                        }
                        t.AppendChild(row);
                    }

                    #endregion

                    try
                    {
                        HtmlEditingToolHelper.InsertHtml(EditorObserver.ActiveEditor, t);
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