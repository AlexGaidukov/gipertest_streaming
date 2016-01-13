using VisualEditor.Logic.Controls;
using VisualEditor.Logic.Dialogs;

namespace VisualEditor.Logic.Commands.Application
{
    internal class AppSettings : AbstractCommand
    {
        public AppSettings()
        {
            name = CommandNames.AppSettings;
            text = CommandTexts.AppSettings;
            image = Properties.Resources.AppSettings;
        }

        public override void Execute(object @object)
        {
            if (!Enabled)
            {
                return;
            }

            using (var asd = new AppSettingsDialog())
            {
                asd.ShowDialog(MainForm.Instance);
            }
        }
    }
}