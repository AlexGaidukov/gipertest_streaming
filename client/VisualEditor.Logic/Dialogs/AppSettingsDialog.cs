using System;
using System.Windows.Forms;
using VisualEditor.Logic.Helpers;
using VisualEditor.Logic.Helpers.AppSettings;

namespace VisualEditor.Logic.Dialogs
{
    internal partial class AppSettingsDialog : DialogBase
    {
        public AppSettingsDialog()
        {
            InitializeComponent();
            InitializeDialog();
        }

        public void InitializeDialog()
        {
            invalidCourseCheckBox.Checked = AppSettingsHelper.DoInvalidCourseDialogShowing();
            autosavingUpDown.Value = AppSettingsHelper.GetAutosavingInterval();
            autosavingCheckBox.Checked = autosavingUpDown.Enabled = (autosavingUpDown.Value != 0);
        }

        private void okButton_Click(object sender, EventArgs e)
        {
            AppSettingsManager.Instance.SetSettingByName(SettingNames.ShowInvalidCourseDialog,
                                                         invalidCourseCheckBox.Checked.ToString());
            AppSettingsManager.Instance.SetSettingByName(SettingNames.AutosavingInterval,
                                                         autosavingUpDown.Value.ToString());

            DialogResult = DialogResult.OK;

            UIHelper.ShowMessage("Для применения некоторых настроек необходимо перезапустить приложение.",
                                 MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void autosavingCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            autosavingUpDown.Enabled = autosavingCheckBox.Checked;

            if (!autosavingCheckBox.Checked)
            {
                autosavingUpDown.Value = 0;
            }
        }

        private void autosavingUpDown_ValueChanged(object sender, EventArgs e)
        {
            autosavingCheckBox.Checked = (autosavingUpDown.Value != 0);
        }
    }
}