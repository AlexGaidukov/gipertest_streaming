using System;
using System.Windows.Forms;
using VisualEditor.Utils.Helpers;

namespace VisualEditor.Logic.Dialogs
{
    internal partial class ProfileDialog : DialogBase
    {
        public ProfileDialog()
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
            DataTransferUnit.AppendNode("Data", "LowerBound");

            HelpKeyword = "Компетенция";
            lowerBoundUpDown.Select();
        }

        #endregion

        private void okButton_Click(object sender, System.EventArgs e)
        {
            DataTransferUnit.SetNodeValue("LowerBound", lowerBoundUpDown.Value.ToString());
            Warehouse.Warehouse.IsProjectModified = true;
            DialogResult = DialogResult.OK;
        }

        private void lowerBoundUpDown_KeyDown(object sender, KeyEventArgs e)
        {
            e.SuppressKeyPress = true;
        }

        public void InitializeData()
        {
            lowerBoundUpDown.Value = Convert.ToDecimal(DataTransferUnit.GetNodeValue("LowerBound"));
        }
    }
}