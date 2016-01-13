using System;
using System.Windows.Forms;
using VisualEditor.Logic.Helpers;
using VisualEditor.Logic.Warehouse;
using VisualEditor.Utils.ExceptionHandling;

namespace VisualEditor.Logic.Commands.HtmlEditing
{
    internal class BackColor : AbstractCommand
    {
        private const string operationCantBePerformedMessage = "Невозможно выполнить операцию. Попробуйте повтротить снова.";

        public BackColor()
        {
            name = CommandNames.BackColor;
            text = CommandTexts.BackColor;
            image = Properties.Resources.BackColor;
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

            using (var cd = new ColorDialog())
            {
                if (cd.ShowDialog(EditorObserver.DialogOwner) == DialogResult.OK)
                {
                    try
                    {
                        EditorObserver.ActiveEditor.SetBackColor(cd.Color);
                        Warehouse.Warehouse.IsProjectModified = true;
                    }
                    catch (Exception exception)
                    {
                        ExceptionManager.Instance.LogException(exception);
                        UIHelper.ShowMessage(operationCantBePerformedMessage,
                            MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }
    }
}