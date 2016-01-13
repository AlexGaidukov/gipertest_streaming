using System;
using System.Drawing;
using System.Windows.Forms;
using VisualEditor.Logic.Controls.Docking;
using VisualEditor.Logic.Helpers.AppSettings;
using VisualEditor.Utils.Helpers;

namespace VisualEditor.Logic.Helpers
{
    internal static class UIHelper
    {
        #region Восстановление состояния главного окна

        public static void PrepareMainFormUI(Form mainForm)
        {
            if (mainForm.IsNull())
            {
                throw new ArgumentNullException();
            }

            PrepareMainFormLocationAndSize(mainForm);
            DockContainer.Instance.LoadDockingWindowsLayout();
            AppSettingsHelper.LoadRecentProjects(AppSettingsManager.SettingsContainer);
            Warehouse.Warehouse.InvalidateRecentProjects();
        }

        private static void PrepareMainFormLocationAndSize(Form mainForm)
        {
            if (mainForm.IsNull())
            {
                throw new ArgumentNullException();
            }

            var formWindowState = AppSettingsManager.Instance.GetSettingByName(SettingNames.WindowState);
            if (formWindowState.Equals(FormWindowState.Maximized.ToString()) ||
                formWindowState.Equals(FormWindowState.Minimized.ToString()))
            {
                mainForm.WindowState = FormWindowState.Maximized;
                return;
            }

            var left = Convert.ToInt32(AppSettingsManager.Instance.GetSettingByName(SettingNames.Left));
            var top = Convert.ToInt32(AppSettingsManager.Instance.GetSettingByName(SettingNames.Top));
            var width = Convert.ToInt32(AppSettingsManager.Instance.GetSettingByName(SettingNames.Width));
            var height = Convert.ToInt32(AppSettingsManager.Instance.GetSettingByName(SettingNames.Height));

            var location = new Point(left, top);
            var size = new Size(width, height);

            // Восстанавливает предыдущее положение формы, если значения положения и размера ненулевые.
            if (!location.IsEmpty && !size.IsEmpty)
            {
                mainForm.Location = location;
                mainForm.Size = size;
            }
            else
            {
                mainForm.WindowState = FormWindowState.Maximized;
            }
        }

        #endregion

        #region Сохранение состояния главного окна
        
        public static void SaveMainFormUI(Form mainForm)
        {
            if (mainForm.IsNull())
            {
                throw new ArgumentNullException();
            }

            SaveMainFormLocationAndSize(mainForm);
            DockContainer.Instance.SaveDockingWindowsLayout();
            AppSettingsHelper.SaveRecentProjects(AppSettingsManager.SettingsContainer);
        }

        private static void SaveMainFormLocationAndSize(Form mainForm)
        {
            if (mainForm.IsNull())
            {
                throw new ArgumentNullException();
            }

            AppSettingsManager.Instance.SetSettingByName(SettingNames.WindowState, mainForm.WindowState.ToString());
            AppSettingsManager.Instance.SetSettingByName(SettingNames.Left, mainForm.Left.ToString());
            AppSettingsManager.Instance.SetSettingByName(SettingNames.Top, mainForm.Top.ToString());
            AppSettingsManager.Instance.SetSettingByName(SettingNames.Width, mainForm.Width.ToString());
            AppSettingsManager.Instance.SetSettingByName(SettingNames.Height, mainForm.Height.ToString());
        }

        #endregion

        public static void ShowMessage(string text, MessageBoxButtons buttons, MessageBoxIcon icon)
        {
            MessageBox.Show(text, Application.ProductName, buttons, icon);
        }
    }
}