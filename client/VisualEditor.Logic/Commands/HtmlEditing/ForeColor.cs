using System;
using System.Windows.Forms;
using VisualEditor.Logic.Helpers;
using VisualEditor.Logic.Warehouse;
using VisualEditor.Utils.ExceptionHandling;

namespace VisualEditor.Logic.Commands.HtmlEditing
{
    internal class ForeColor : AbstractCommand
    {
        private const string operationCantBePerformedMessage = "Невозможно выполнить операцию. Попробуйте повтротить снова.";

        public ForeColor()
        {
            name = CommandNames.ForeColor;
            text = CommandTexts.ForeColor;
            image = Properties.Resources.ForeColor;
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
                        EditorObserver.ActiveEditor.SetForeColor(cd.Color);
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