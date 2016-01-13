using System.IO;
using System.Windows.Forms;

namespace VisualEditor.Logic.Dialogs
{
    internal /*abstract*/ class DialogBase : Form
    {
        protected DialogBase()
        {
            FormBorderStyle = FormBorderStyle.FixedSingle;
            KeyPreview = true;
            MaximizeBox = false;
            MinimizeBox = false;
            ShowIcon = false;
            ShowInTaskbar = false;
            StartPosition = FormStartPosition.CenterParent;
            KeyDown += DialogBase_KeyDown;
        }

        public string HelpKeyword { get; set; }

        private void DialogBase_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode.Equals(Keys.F1))
            {
                if (File.Exists(Commands.Help.Help.HelpPath))
                {
                    Help.ShowHelp(this, Commands.Help.Help.HelpPath, 
                        HelpNavigator.KeywordIndex, HelpKeyword);
                }
            }
        }
    }
}