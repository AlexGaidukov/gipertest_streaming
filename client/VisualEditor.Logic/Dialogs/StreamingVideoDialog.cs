namespace VisualEditor.Logic.Dialogs
{
    using System.Windows.Forms;

    using VisualEditor.Utils.Helpers;

    internal partial class StreamingVideoDialog : DialogBase
    {
        public XmlHelper DataTransferUnit { get; set; }

        public StreamingVideoDialog()
        {
            this.InitializeComponent();
            this.InitializeDialog();
        }

        public void InitializeDialog()
        {
            this.DataTransferUnit = new XmlHelper();
            this.DataTransferUnit.AppendNode(string.Empty, "Data");
            this.DataTransferUnit.AppendNode("Data", "Source");
            this.DataTransferUnit.AppendNode("Data", "LinkText");

            this.HelpKeyword = "Добавить стрим";
        }

        private void okButton_Click(object sender, System.EventArgs e)
        {
            DataTransferUnit.SetNodeValue("Source", this.linkTextBox.Text);

            Warehouse.Warehouse.IsProjectModified = true;
            DialogResult = DialogResult.OK;
        }

        private void linkTextBox_TextChanged(object sender, System.EventArgs e)
        {
            if (string.IsNullOrEmpty(this.linkTextBox.Text))
            {
                this.okButton.Enabled = false;
            }
            else
            {
                this.okButton.Enabled = true;
            }
        }
    }
}
