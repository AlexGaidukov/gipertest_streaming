using System;
using System.Windows.Forms;
using VisualEditor.Logic.Helpers;
using VisualEditor.Logic.Warehouse;
using VisualEditor.Utils.ExceptionHandling;

namespace VisualEditor.Logic.Commands.Hint
{
    internal class HintUnorderedList : AbstractCommand
    {
        private const string operationCantBePerformedMessage = "Невозможно выполнить операцию. Попробуйте повтротить снова.";

        public HintUnorderedList()
        {
            name = CommandNames.HintUnorderedList;
            text = CommandTexts.HintUnorderedList;
            image = Properties.Resources.UnorderedList;
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
                EditorObserver.ActiveEditor.ChangeUnorderedList();
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