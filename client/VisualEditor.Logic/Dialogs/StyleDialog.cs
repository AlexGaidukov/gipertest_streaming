using VisualEditor.Utils.Helpers;

namespace VisualEditor.Logic.Dialogs
{
    internal partial class StyleDialog : DialogBase
    {
        public StyleDialog()
        {
            InitializeComponent();
            InitializeDialog();
        }

        public XmlHelper DataTransferUnit { get; set; }

        #region InitializeDialog
        
        private void InitializeDialog()
        {
            DataTransferUnit = new XmlHelper();
            DataTransferUnit.AppendNode(string.Empty, "Data");
            DataTransferUnit.AppendNode("Data", "StyleName");
            DataTransferUnit.AppendNode("Data", "HintText");

            styleNameComboBox.Text = "нет";
            HelpKeyword = "Контент";
            styleNameComboBox.Select();
        }

        #endregion

        private void okButton_Click(object sender, System.EventArgs e)
        {
            DataTransferUnit.SetNodeValue("StyleName", styleNameComboBox.Text);
            DataTransferUnit.SetNodeValue("HintText", hintTextTextBox.Text);

            Warehouse.Warehouse.IsProjectModified = true;
            DialogResult = System.Windows.Forms.DialogResult.OK;
        }

        private void styleComboBox_SelectedIndexChanged(object sender, System.EventArgs e)
        {
            CheckState();
        }

        private void CheckState()
        {
             ///
            okButton.Enabled = !styleNameComboBox.Text.Equals("нет");
            hintTextTextBox.Enabled = styleNameComboBox.Text.Equals("подсказка");
        }
    }
}