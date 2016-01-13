using System;
using System.Collections.Generic;
using System.Windows.Forms;
using VisualEditor.Logic.Dialogs;
using VisualEditor.Logic.Helpers;
using VisualEditor.Logic.Warehouse;
using VisualEditor.Utils.Controls.HtmlEditing;
using VisualEditor.Utils.ExceptionHandling;
using VisualEditor.Logic.Course.Items;
using VisualEditor.Logic.IO;

namespace VisualEditor.Logic.Commands.Embedding
{
    internal class StyleSmall : AbstractCommand
    {
        private const string operationCantBePerformedMessage = "Невозможно выполнить операцию. Попробуйте повтротить снова.";

        public StyleSmall()
        {
            name = CommandNames.StyleSmall;
            text = CommandTexts.StyleSmall;
            image = Properties.Resources.StyleSmall;
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

            if (!EditorObserver.ActiveEditor.IsSelection)
            {
                return;
            }

            using (var sd = new StyleDialog())
            {
                if (sd.ShowDialog(EditorObserver.DialogOwner) == DialogResult.OK)
                {
                    var styleName = sd.DataTransferUnit.GetNodeValue("StyleName");
                    Dictionary<string, string> d = null;

                    if (styleName.Equals("удалить стиль"))
                    {
                        d = new Dictionary<string, string>
                                {
                                    {"class", "attention"}
                                };

                        try
                        { 
                            HtmlEditingToolHelper.DeleteSurroundWithHtml(EditorObserver.ActiveEditor, "DIV", d);
                        }
                        catch (Exception exception)
                        {
                            ExceptionManager.Instance.LogException(exception);
                            UIHelper.ShowMessage(operationCantBePerformedMessage, MessageBoxButtons.OK,
                                MessageBoxIcon.Error);
                            return;
                        }
                    }

                    if (styleName.Equals("внимание"))
                    {
                        d = new Dictionary<string, string>
                                {
                                    {"class", "attention"}
                                };

                        try
                        {
                            HtmlEditingToolHelper.SurroundWithStyleHtml(EditorObserver.ActiveEditor, "DIV", d);
                        }
                        catch (Exception exception)
                        {
                            ExceptionManager.Instance.LogException(exception);
                            UIHelper.ShowMessage(operationCantBePerformedMessage, MessageBoxButtons.OK,
                                MessageBoxIcon.Error);
                            return;
                        }
                    }

                    if (styleName.Equals("выводы"))
                    {
                        d = new Dictionary<string, string>
                                {
                                    {"class", "conclusion"}
                                };

                        try
                        {
                            HtmlEditingToolHelper.SurroundWithStyleHtml(EditorObserver.ActiveEditor, "DIV", d);
                        }
                        catch (Exception exception)
                        {
                            ExceptionManager.Instance.LogException(exception);
                            UIHelper.ShowMessage(operationCantBePerformedMessage, MessageBoxButtons.OK,
                                MessageBoxIcon.Error);
                            return;
                        }
                    }

                    if (styleName.Equals("определение"))
                    {
                        d = new Dictionary<string, string>
                                {
                                    {"class", "definition"}
                                };

                        try
                        {
                            HtmlEditingToolHelper.SurroundWithStyleHtml(EditorObserver.ActiveEditor, "DIV", d);
                        }
                        catch (Exception exception)
                        {
                            ExceptionManager.Instance.LogException(exception);
                            UIHelper.ShowMessage(operationCantBePerformedMessage, MessageBoxButtons.OK,
                                MessageBoxIcon.Error);
                            return;
                        }
                    }

                    if (styleName.Equals("ключевые слова"))
                    {
                        d = new Dictionary<string, string>
                                {
                                    {"class", "definition_name"}
                                };

                        try
                        {
                            HtmlEditingToolHelper.SurroundWithStyleHtml(EditorObserver.ActiveEditor, "SPAN", d);
                        }
                        catch (Exception exception)
                        {
                            ExceptionManager.Instance.LogException(exception);
                            UIHelper.ShowMessage(operationCantBePerformedMessage, MessageBoxButtons.OK,
                                MessageBoxIcon.Error);
                            return;
                        }
                    }

                    if (styleName.Equals("рамка"))
                    {
                        d = new Dictionary<string, string>
                                {
                                    {"class", "eq_border"}
                                };

                        try
                        {
                            HtmlEditingToolHelper.SurroundWithStyleHtml(EditorObserver.ActiveEditor, "DIV", d);
                        }
                        catch (Exception exception)
                        {
                            ExceptionManager.Instance.LogException(exception);
                            UIHelper.ShowMessage(operationCantBePerformedMessage, MessageBoxButtons.OK,
                                MessageBoxIcon.Error);
                            return;
                        }
                    }

                    if (styleName.Equals("текст примера"))
                    {
                        d = new Dictionary<string, string>
                                {
                                    {"class", "example"}
                                };

                        try
                        {
                            HtmlEditingToolHelper.SurroundWithStyleHtml(EditorObserver.ActiveEditor, "DIV", d);
                        }
                        catch (Exception exception)
                        {
                            ExceptionManager.Instance.LogException(exception);
                            UIHelper.ShowMessage(operationCantBePerformedMessage, MessageBoxButtons.OK,
                                MessageBoxIcon.Error);
                            return;
                        }
                    }

                    if (styleName.Equals("формула"))
                    {
                        d = new Dictionary<string, string>
                                {
                                    {"class", "formula"}
                                };

                        try
                        {
                            HtmlEditingToolHelper.SurroundWithStyleHtml(EditorObserver.ActiveEditor, "DIV", d);
                        }
                        catch (Exception exception)
                        {
                            ExceptionManager.Instance.LogException(exception);
                            UIHelper.ShowMessage(operationCantBePerformedMessage, MessageBoxButtons.OK,
                                MessageBoxIcon.Error);
                            return;
                        }
                    }

                    if (styleName.Equals("подсказка"))
                    {
                        var hintText = sd.DataTransferUnit.GetNodeValue("HintText");

                        d = new Dictionary<string, string>
                                {
                                    {"class", "hint_name"},
                                    {"title", hintText}
                                };

                        try
                        {
                            HtmlEditingToolHelper.SurroundWithStyleHtml(EditorObserver.ActiveEditor, "SPAN", d);
                        }
                        catch (Exception exception)
                        {
                            ExceptionManager.Instance.LogException(exception);
                            UIHelper.ShowMessage(operationCantBePerformedMessage, MessageBoxButtons.OK,
                                MessageBoxIcon.Error);
                            return;
                        }
                    }

                    if (styleName.Equals("информация"))
                    {
                        d = new Dictionary<string, string>
                                {
                                    {"class", "information"}
                                };

                        try
                        {
                            HtmlEditingToolHelper.SurroundWithStyleHtml(EditorObserver.ActiveEditor, "DIV", d);
                        }
                        catch (Exception exception)
                        {
                            ExceptionManager.Instance.LogException(exception);
                            UIHelper.ShowMessage(operationCantBePerformedMessage, MessageBoxButtons.OK,
                                MessageBoxIcon.Error);
                            return;
                        }
                    }

                    if (styleName.Equals("задание"))
                    {
                        d = new Dictionary<string, string>
                                {
                                    {"class", "task"}
                                };

                        try
                        {
                            HtmlEditingToolHelper.SurroundWithStyleHtml(EditorObserver.ActiveEditor, "DIV", d);
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
}