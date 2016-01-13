using System;
using System.IO;
using System.Windows.Forms;
using VisualEditor.Logic.Helpers.AppSettings;
using VisualEditor.Utils.Helpers;

namespace VisualEditor.Logic.Dialogs
{
    internal partial class AnimationDialog : DialogBase
    {
        public AnimationDialog()
        {
            InitializeComponent();
            InitializeDialog();
        }

        public XmlHelper DataTransferUnit { get; set; }

        public void InitializeDialog()
        {
            DataTransferUnit = new XmlHelper();
            DataTransferUnit.AppendNode(string.Empty, "Data");
            DataTransferUnit.AppendNode("Data", "Source");
            DataTransferUnit.AppendNode("Data", "LinkText");

            HelpKeyword = "Анимация";
            sourceButton.Select();
        }

        private void sourceButton_Click(object sender, EventArgs e)
        {
            using (var openFileDialog = new OpenFileDialog())
            {
                openFileDialog.Filter = "SWF (*.swf)|*.swf";
                openFileDialog.InitialDirectory = AppSettingsHelper.GetInitialDirectory();

                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    sourceTextBox.Text = openFileDialog.FileName;

                    AppSettingsManager.Instance.SetSettingByName(SettingNames.InitialDirectory, 
                        Path.GetDirectoryName(openFileDialog.FileName));
                }
            }
        }

        private void sourceTextBox_TextChanged(object sender, EventArgs e)
        {
            CheckState();
        }

        private void CheckState()
        {
            okButton.Enabled = sourceTextBox.Text != string.Empty;
        }

        private void okButton_Click(object sender, EventArgs e)
        {
            DataTransferUnit.SetNodeValue("Source", sourceTextBox.Text);
            DataTransferUnit.SetNodeValue("LinkText", linkTextTextBox.Text);

            Warehouse.Warehouse.IsProjectModified = true;
            DialogResult = DialogResult.OK;
        }
    }
}