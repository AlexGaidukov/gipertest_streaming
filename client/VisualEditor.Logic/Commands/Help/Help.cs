using System.IO;
using VisualEditor.Logic.Controls;

namespace VisualEditor.Logic.Commands.Help
{
    internal class Help : AbstractCommand
    {
        public Help()
        {
            name = CommandNames.Help;
            text = CommandTexts.Help;
            image = Properties.Resources.Help;
            HelpPath = Path.Combine(System.Windows.Forms.Application.StartupPath,
                string.Concat(System.Windows.Forms.Application.ProductName, ".chm"));
        }

        public static string HelpPath { get; set; }

        public override void Execute(object @object)
        {
            if (!Enabled)
            {
                return;
            }

            if (!File.Exists(HelpPath))
            {
                return;
            }

            System.Windows.Forms.Help.ShowHelp(MainForm.Instance, HelpPath);
        }
    }
}