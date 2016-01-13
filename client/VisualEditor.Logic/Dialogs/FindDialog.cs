using System;
using System.Windows.Forms;
using VisualEditor.Logic.Warehouse;

namespace VisualEditor.Logic.Dialogs
{
    internal partial class FindDialog : DialogBase
    {
        private const string searchingFinishedMessage = "Приложение {0} завершило поиск документа.";

        public FindDialog()
        {
            InitializeComponent();
            FormBorderStyle = FormBorderStyle.FixedToolWindow;
            HelpKeyword = "Поиск";
            findWhatTextBox.Select();
        }

        private void findNextButton_Click(object sender, EventArgs e)
        {
            var b = EditorObserver.ActiveEditor.Find(findWhatTextBox.Text, forwardRadioButton.Checked, 
                        caseCheckBox.Checked, wholeWordCheckBox.Checked);
            if (!b)
            {
                MessageBox.Show(string.Format(searchingFinishedMessage, Application.ProductName),
                    Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void findWhatTextBox_TextChanged(object sender, EventArgs e)
        {
            CheckState();
        }

        private void CheckState()
        {
            findNextButton.Enabled = findWhatTextBox.Text != string.Empty;
        }
    }
}