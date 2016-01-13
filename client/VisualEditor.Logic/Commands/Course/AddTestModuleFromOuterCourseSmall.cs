using System.IO;
using System.Windows.Forms;
using VisualEditor.Logic.Course.Items;
using VisualEditor.Logic.Dialogs;
using VisualEditor.Logic.Helpers.AppSettings;
using VisualEditor.Logic.Warehouse;

namespace VisualEditor.Logic.Commands.Course
{
    internal class AddTestModuleFromOuterCourseSmall : AbstractCommand
    {
        public AddTestModuleFromOuterCourseSmall()
        {
            name = CommandNames.AddTestModuleFromOuterCourseSmall;
            text = CommandTexts.AddTestModuleFromOuterCourseSmall;
            image = Properties.Resources.TestModule;
        }

        public override void Execute(object @object)
        {
            if (!Enabled)
            {
                return;
            }

            var path = string.Empty;

            using (var openFileDialog = new OpenFileDialog())
            {
                openFileDialog.Title = "Открыть проект";
                openFileDialog.InitialDirectory = AppSettingsHelper.GetInitialDirectory();
                openFileDialog.Filter = "HTP (*.htp)|*.htp";

                if (!openFileDialog.ShowDialog().Equals(DialogResult.OK))
                {
                    return;
                }

                path = openFileDialog.FileName;
            }

            if (string.IsNullOrEmpty(path))
            {
                return;
            }

            Warehouse.Warehouse.OuterProjectTrueLocation = Path.GetDirectoryName(path);
            Warehouse.Warehouse.OuterProjectFileName = Path.GetFileNameWithoutExtension(path);

            using (var aifocd = new AddItemFromOuterCourseDialog())
            {
                aifocd.InitializeData("Добавление контроля из другого учебного курса", "Добавить контроль", typeof(TestModule));
                aifocd.ShowDialog(EditorObserver.DialogOwner);
            }
        }
    }
}