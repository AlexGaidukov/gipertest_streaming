using System;
using System.IO;
using System.Windows.Forms;
using VisualEditor.Logic.Helpers;
using VisualEditor.Logic.Helpers.AppSettings;
using VisualEditor.Logic.Warehouse;
using VisualEditor.Utils.ExceptionHandling;

namespace VisualEditor.Logic.Commands.Project
{
    internal class OpenProject : AbstractCommand
    {
        private const string projectOpenFailedMessage = "В процессе открытия проекта произошла ошибка. Попробуйте повторить снова.";

        public OpenProject()
        {
            name = CommandNames.OpenProject;
            text = CommandTexts.OpenProject;
            image = Properties.Resources.OpenProject;
        }

        public override void Execute(object @object)
        {
            if (!Enabled)
            {
                return;
            }

            if (!Warehouse.Warehouse.IsProjectBeingDesigned)
            {
                OpenNewProject();
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
                if (CloseProject.DialogResult != DialogResult.Cancel &&
                    !Warehouse.Warehouse.IsProjectBeingDesigned)
                {
                    OpenNewProject();
                }
            }
        }

        private static void OpenNewProject()
        {
            using (var openFileDialog = new OpenFileDialog())
            {
                openFileDialog.Title = "Открыть проект";
                openFileDialog.InitialDirectory = AppSettingsHelper.GetInitialDirectory();
                openFileDialog.Filter = "HTP (*.htp)|*.htp";

                if (openFileDialog.ShowDialog().Equals(DialogResult.OK))
                {
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

                    Warehouse.Warehouse.ProjectTrueLocation = Path.GetDirectoryName(openFileDialog.FileName);
                    Warehouse.Warehouse.ProjectFileName = Path.GetFileNameWithoutExtension(openFileDialog.FileName);
                    Warehouse.Warehouse.ProjectFileType = Path.GetExtension(openFileDialog.FileName);

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
                    
                    if (!Warehouse.Warehouse.Instance.RecentProjects.Contains(openFileDialog.FileName))
                    {
                        Warehouse.Warehouse.Instance.RecentProjects.Insert(0, openFileDialog.FileName);
                    }
                    else
                    {
                        Warehouse.Warehouse.Instance.RecentProjects.Remove(openFileDialog.FileName);
                        Warehouse.Warehouse.Instance.RecentProjects.Insert(0, openFileDialog.FileName);
                    }

                    if (Warehouse.Warehouse.Instance.RecentProjects.Count > RecentProject.MaxValue)
                    {
                        Warehouse.Warehouse.Instance.RecentProjects.RemoveRange(RecentProject.MaxValue,
                            Warehouse.Warehouse.Instance.RecentProjects.Count - RecentProject.MaxValue);
                    }

                    Warehouse.Warehouse.InvalidateRecentProjects();
                    
                    #endregion
                }
            }
        }
    }
}