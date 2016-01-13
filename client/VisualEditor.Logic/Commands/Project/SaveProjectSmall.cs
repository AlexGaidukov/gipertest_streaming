using System;
using System.Windows.Forms;
using VisualEditor.Logic.Helpers;
using VisualEditor.Utils.ExceptionHandling;

namespace VisualEditor.Logic.Commands.Project
{
    internal class SaveProjectSmall : AbstractCommand
    {
        private const string projectSaveFailedMessage = "В процессе сохранения проекта произошла ошибка. Попробуйте повторить снова.";

        public SaveProjectSmall()
        {
            name = CommandNames.SaveProjectSmall;
            text = CommandTexts.SaveProjectSmall;
            image = Properties.Resources.SaveProjectSmall;
        }

        public override void Execute(object @object)
        {
            if (!Enabled)
            {
                return;
            }

            if (!Warehouse.Warehouse.IsProjectSavedToFile)
            {
                CommandManager.Instance.GetCommand(CommandNames.SaveProjectAs).Execute(null);
            }
            else
            {
                if (Warehouse.Warehouse.ProjectFileType.Equals(".htp"))
                {
                    try
                    {
                        CommandManager.Instance.GetCommand(CommandNames.SaveToHtp).Execute(this);
                        Warehouse.Warehouse.IsProjectModified = false;
                    }
                    catch (Exception exception)
                    {
                        ExceptionManager.Instance.LogException(exception);
                        UIHelper.ShowMessage(projectSaveFailedMessage,
                            MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }
                }
            }
        }
    }
}