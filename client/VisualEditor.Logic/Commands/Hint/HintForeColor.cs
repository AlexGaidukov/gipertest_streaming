using System;
using System.Windows.Forms;
using VisualEditor.Logic.Helpers;
using VisualEditor.Logic.Warehouse;
using VisualEditor.Utils.ExceptionHandling;

namespace VisualEditor.Logic.Commands.Hint
{
    internal class HintForeColor : AbstractCommand
    {
        private const string operationCantBePerformedMessage = "Невозможно выполнить операцию. Попробуйте повтротить снова.";

        public HintForeColor()
        {
            name = CommandNames.HintForeColor;
            text = CommandTexts.HintForeColor;
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