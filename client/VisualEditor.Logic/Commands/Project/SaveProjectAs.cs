using System;
using System.IO;
using System.Windows.Forms;
using VisualEditor.Logic.Helpers;
using VisualEditor.Logic.Helpers.AppSettings;
using VisualEditor.Utils.ExceptionHandling;

namespace VisualEditor.Logic.Commands.Project
{
    internal class SaveProjectAs : AbstractCommand
    {
        private const string projectSaveFailedMessage = "В процессе сохранения проекта произошла ошибка. Попробуйте повторить снова.";

        public SaveProjectAs()
        {
            name = CommandNames.SaveProjectAs;
            text = CommandTexts.SaveProjectAs;
            image = Properties.Resources.SaveProjectAs;
        }

        public static DialogResult DialogResult { get; set; }

        public override void Execute(object @object)
        {
            if (!Enabled)
            {
                return;
            }

            using (var saveFileDialog = new SaveFileDialog())
            {
                saveFileDialog.FileName = Warehouse.Warehouse.Instance.CourseTree.Nodes[0].Text;
                saveFileDialog.Title = "Сохранить проект";
                saveFileDialog.InitialDirectory = AppSettingsHelper.GetInitialDirectory();
                saveFileDialog.Filter = "HTP (*.htp)|*.htp";

                var dr = saveFileDialog.ShowDialog();

                if (dr == DialogResult.OK)
                {
                    Warehouse.Warehouse.ProjectTrueLocation = Path.GetDirectoryName(saveFileDialog.FileName);
                    Warehouse.Warehouse.ProjectFileName = Path.GetFileNameWithoutExtension(saveFileDialog.FileName);
                    Warehouse.Warehouse.ProjectFileType = Path.GetExtension(saveFileDialog.FileName);

                    if (Warehouse.Warehouse.ProjectFileType.Equals(".htp"))
                    {
                        try
                        {
                            CommandManager.Instance.GetCommand(CommandNames.SaveToHtp).Execute(this);
                        }
                        catch (Exception exception)
                        {
                            ExceptionManager.Instance.LogException(exception);
                            UIHelper.ShowMessage(projectSaveFailedMessage,
                                MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return;
                        }
                    }

                    Warehouse.Warehouse.IsProjectSavedToFile = true;
                    Warehouse.Warehouse.IsProjectModified = false;
                    AppSettingsManager.Instance.SetSettingByName(SettingNames.InitialDirectory, 
                        Warehouse.Warehouse.ProjectTrueLocation);

                    #region Недавние проекты

                    if (!Warehouse.Warehouse.Instance.RecentProjects.Contains(saveFileDialog.FileName))
                    {
                        Warehouse.Warehouse.Instance.RecentProjects.Insert(0, saveFileDialog.FileName);
                    }
                    else
                    {
                        Warehouse.Warehouse.Instance.RecentProjects.Remove(saveFileDialog.FileName);
                        Warehouse.Warehouse.Instance.RecentProjects.Insert(0, saveFileDialog.FileName);
                    }

                    if (Warehouse.Warehouse.Instance.RecentProjects.Count > RecentProject.MaxValue)
                    {
                        Warehouse.Warehouse.Instance.RecentProjects.RemoveRange(RecentProject.MaxValue,
                            Warehouse.Warehouse.Instance.RecentProjects.Count - RecentProject.MaxValue);
                    }

                    Warehouse.Warehouse.InvalidateRecentProjects();

                    #endregion
                }

                DialogResult = dr;
            }
        }
    }
}