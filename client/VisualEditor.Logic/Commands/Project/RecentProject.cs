using System;
using System.IO;
using System.Windows.Forms;
using VisualEditor.Logic.Helpers;
using VisualEditor.Logic.Helpers.AppSettings;
using VisualEditor.Logic.Warehouse;
using VisualEditor.Utils.ExceptionHandling;

namespace VisualEditor.Logic.Commands.Project
{
    internal class RecentProject : AbstractCommand
    {
        private const string wrongFilePathMessage = "Файл '{0}' не может быть открыт.\nСсылка на него будет удалена из списка недавних проектов.";
        private const string projectOpenFailedMessage = "В процессе открытия проекта произошла ошибка. Попробуйте повторить снова.";

        public RecentProject()
        {
            name = CommandNames.RecentProject;
            text = CommandTexts.RecentProject;
        }

        public static int MaxValue
        {
            get { return 10; }
        }

        public override void Execute(object @object)
        {
            if (!Enabled)
            {
                return;
            }

            if (!Warehouse.Warehouse.IsProjectBeingDesigned)
            {
                OpenProject();
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
                    UIHelper.ShowMessage(projectOpenFailedMessage,
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                
                if (!CloseProject.DialogResult.Equals(DialogResult.Cancel) &&
                    !Warehouse.Warehouse.IsProjectBeingDesigned)
                {
                    OpenProject();
                }
            }
        }

        private void OpenProject()
        {
            if (File.Exists(Text))
            {
                Warehouse.Warehouse.ProjectTrueLocation = Path.GetDirectoryName(Text);
                Warehouse.Warehouse.ProjectFileName = Path.GetFileNameWithoutExtension(Text);
                Warehouse.Warehouse.ProjectFileType = Path.GetExtension(Text);

                try
                {
                    Warehouse.Warehouse.CreateDirectories();
                }
                catch (Exception)
                {
                    UIHelper.ShowMessage(projectOpenFailedMessage,
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                if (Warehouse.Warehouse.ProjectFileType.Equals(".htp"))
                {
                    Warehouse.Warehouse.Instance.CourseTree.Enabled = false;
                    Warehouse.Warehouse.Instance.ConceptTree.Enabled = false;

                    try
                    {
                        CommandManager.Instance.GetCommand(CommandNames.LoadFromHtp).Execute(null);
                    }
                    catch (Exception exception)
                    {
                        ExceptionManager.Instance.LogException(exception);
                        UIHelper.ShowMessage(projectOpenFailedMessage,
                            MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    } 
                    
                    Warehouse.Warehouse.Instance.CourseTree.Enabled = true;
                    Warehouse.Warehouse.Instance.ConceptTree.Enabled = true;
                }

                Warehouse.Warehouse.IsProjectSavedToFile = true;
                Warehouse.Warehouse.IsProjectModified = false;
                AppSettingsManager.Instance.SetSettingByName(SettingNames.InitialDirectory,
                    Warehouse.Warehouse.ProjectTrueLocation);
                EditorObserver.RenderingStyle = Enums.RenderingStyle.NoActiveDocument;

                #region Недавние проекты

                Warehouse.Warehouse.Instance.RecentProjects.Remove(Text);
                Warehouse.Warehouse.Instance.RecentProjects.Insert(0, Text);

                Warehouse.Warehouse.InvalidateRecentProjects();

                #endregion
            }
            else
            {
                MessageBox.Show(string.Format(wrongFilePathMessage, Text),
                    System.Windows.Forms.Application.ProductName, 
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                
                Warehouse.Warehouse.Instance.RecentProjects.Remove(Text);
                Warehouse.Warehouse.InvalidateRecentProjects();
            }
        }
    }
}