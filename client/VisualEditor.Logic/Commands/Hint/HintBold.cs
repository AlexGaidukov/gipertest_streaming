using System;
using System.Windows.Forms;
using VisualEditor.Logic.Helpers;
using VisualEditor.Logic.Warehouse;
using VisualEditor.Utils.ExceptionHandling;

namespace VisualEditor.Logic.Commands.Hint
{
    internal class HintBold : AbstractCommand
    {
        private const string operationCantBePerformedMessage = "Невозможно выполнить операцию. Попробуйте повтротить снова.";

        public HintBold()
        {
            name = CommandNames.HintBold;
            text = CommandTexts.HintBold;
            image = Properties.Resources.Bold;
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

            try
            {
                EditorObserver.ActiveEditor.ChangeBold();
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