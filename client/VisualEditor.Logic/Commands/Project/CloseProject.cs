using System;
using System.Windows.Forms;
using VisualEditor.Logic.Controls.Docking;
using VisualEditor.Logic.Course.Items;
using VisualEditor.Logic.Warehouse;
using VisualEditor.Utils.ExceptionHandling;

namespace VisualEditor.Logic.Commands.Project
{
    internal class CloseProject : AbstractCommand
    {
        private const string saveChangesMessage = "Сохранить изменения в {0}?";

        public CloseProject()
        {
            name = CommandNames.CloseProject;
            text = CommandTexts.CloseProject;
            image = Properties.Resources.CloseProject;
        }

        public static DialogResult DialogResult { get; private set; }

        public override void Execute(object @object)
        {
            if (!Enabled)
            {
                return;
            }

            if (Warehouse.Warehouse.IsProjectModified)
            {
                var dr = MessageBox.Show(string.Format(saveChangesMessage, Warehouse.Warehouse.Instance.CourseTree.Nodes[0].Text),
                        System.Windows.Forms.Application.ProductName, MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question);

                if (dr.Equals(DialogResult.Yes))
                {
                    CommandManager.Instance.GetCommand(CommandNames.SaveProjectSmall).Execute(null);

                    if (SaveProjectAs.DialogResult != DialogResult.Cancel)
                    {
                        CloseCurrentProject();
                    }
                }

                if (dr.Equals(DialogResult.No))
                {
                    CloseCurrentProject();
                }

                DialogResult = dr;
            }
            else
            {
                CloseCurrentProject();

                DialogResult = DialogResult.None;
            }
        }

        private static void CloseCurrentProject()
        {
            Warehouse.Warehouse.ProjectTrueLocation = Warehouse.Warehouse.ProjectFileName =
            Warehouse.Warehouse.ProjectFileType = Warehouse.Warehouse.ProjectArchiveName = string.Empty;
            Warehouse.Warehouse.IsProjectSavedToFile = false;
            TrainingModule.Count = 0;

            Warehouse.Warehouse.IsProjectModified = false;

            var dc = DockContainer.Instance;
            foreach (var d in dc.Documents)
            {
                if (!d.Equals(dc.ActiveDocument))
                {
                    d.Hide();
                }
            }

            if (dc.ActiveDocument != null)
            {
                dc.ActiveDocument.DockHandler.Hide();
            }

            DockContainer.Instance.Documents.Clear();

            EditorObserver.RenderingStyle = Enums.RenderingStyle.EmptyEnvironment;

            Warehouse.Warehouse.Instance.CourseTree.Nodes.Clear();
            Warehouse.Warehouse.Instance.CourseTree.CurrentNode = null;
            Warehouse.Warehouse.Instance.ConceptTree.Nodes.Clear();
            Warehouse.Warehouse.Instance.ConceptTree.CurrentNode = null;

            Warehouse.Warehouse.Instance.Bookmarks.Clear();
            Warehouse.Warehouse.Instance.TrainingModules.Clear();

            Warehouse.Warehouse.Instance.LinksToObjects.Clear();

            PreviewObserver.ClearDocuments();

            try
            {
                Warehouse.Warehouse.DeleteDirectories();
            }
            catch (Exception exception)
            {
                ExceptionManager.Instance.LogException(exception);
                throw;
            }
        }
    }
}