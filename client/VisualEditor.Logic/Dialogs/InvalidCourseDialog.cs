using System;
using System.Windows.Forms;
using VisualEditor.Utils.Helpers;

namespace VisualEditor.Logic.Dialogs
{
    internal partial class InvalidCourseDialog : DialogBase
    {
        public InvalidCourseDialog()
        {
            InitializeComponent();
            InitializeDialog();
        }

        public XmlHelper DataTransferUnit { get; set; }

        private void InitializeDialog()
        {
            DataTransferUnit = new XmlHelper();
            DataTransferUnit.AppendNode(string.Empty, "Data");
            DataTransferUnit.AppendNode("Data", "NeverShowAgain");

            Text = Application.ProductName;
        }

        private void okButton_Click(object sender, EventArgs e)
        {
            DataTransferUnit.SetNodeValue("NeverShowAgain", neverShowCheckBox.Checked.ToString());
            DialogResult = DialogResult.OK;
        }
    }
}