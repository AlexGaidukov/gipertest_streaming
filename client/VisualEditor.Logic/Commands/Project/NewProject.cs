using System;
using System.Windows.Forms;
using VisualEditor.Logic.Course.Items;
using VisualEditor.Logic.Helpers;
using VisualEditor.Logic.Warehouse;
using VisualEditor.Utils.ExceptionHandling;

namespace VisualEditor.Logic.Commands.Project
{
    internal class NewProject : AbstractCommand
    {
        private const string newProjectFailedMessage = "В процессе создания проекта произошла ошибка. Попробуйте повторить снова.";
        
        public NewProject()
        {
            name = CommandNames.NewProject;
            text = CommandTexts.NewProject;
            image = Properties.Resources.NewProject;
        }

        public override void Execute(object @object)
        {
            if (!Enabled)
            {
                return;
            }

            if (!Warehouse.Warehouse.IsProjectBeingDesigned)
            {
                CreateNewProject();
            }
            else
            {
                try
                {
                    CommandManager.Instance.GetCommand(CommandNames.CloseProject).Execute(null);
                }
                catch (Exception exception)
                {
                    ExceptionManager.Instance.LogException(exception);
                    UIHelper.ShowMessage(newProjectFailedMessage,
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                if (CloseProject.DialogResult != DialogResult.Cancel &&
                    !Warehouse.Warehouse.IsProjectBeingDesigned)
                {
                    CreateNewProject();
                }
            }
        }

        private static void CreateNewProject()
        {
            try
            {
                Warehouse.Warehouse.CreateDirectories();
            }
            catch (Exception)
            {
                UIHelper.ShowMessage(newProjectFailedMessage, 
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            var cr = new CourseRoot
                         {
                             Text = "Учебный курс"
                         };
            Warehouse.Warehouse.Instance.CourseTree.Nodes.Add(cr);

            Warehouse.Warehouse.Instance.CourseTree.LabelEdit = true;
            if (!cr.IsEditing)
            {
                cr.BeginEdit();
            }

            Warehouse.Warehouse.Instance.CourseTree.CurrentNode = cr;
            EditorObserver.RenderingStyle = Enums.RenderingStyle.NoActiveDocument;
            Warehouse.Warehouse.IsProjectModified = true;
        }
    }
}