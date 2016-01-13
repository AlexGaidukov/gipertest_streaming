using System;
using System.IO;
using System.Windows.Forms;
using VisualEditor.Utils.Helpers;

namespace VisualEditor.Logic.Helpers.AppSettings
{
    internal class AppSettingsManager
    {
        private static AppSettingsManager instance;
        private const string appSettingsFileName = "AppSettings.xml";
        private static XmlHelper xmlHelper;

        private AppSettingsManager()
        {

        }

        public static AppSettingsManager Instance
        {
            get { return instance ?? (instance = new AppSettingsManager()); }
        }

        public static XmlHelper SettingsContainer
        {
            get { return xmlHelper; }
        }

        #region Инициализация значений
        
        public void Initialize()
        {
            var path = Path.Combine(Application.StartupPath, appSettingsFileName);
            xmlHelper = new XmlHelper();

            if (!File.Exists(path))
            {
                InitializeDefaultAppSettings(xmlHelper);
                return;
            }

            try
            {
                xmlHelper.Load(path);
            }
            catch (Exception)
            {
                InitializeDefaultAppSettings(xmlHelper);
            }
        }

        #endregion

        #region Запись значений в файл

        public void SaveSettingsToFile()
        {
            var path = Path.Combine(Application.StartupPath, appSettingsFileName);

            try
            {
                xmlHelper.Save(path);
            }
            catch (Exception)
            {
                
            }
        }

        #endregion

        #region Получение и установка значений
        
        public string GetSettingByName(string settingName)
        {
            if (string.IsNullOrEmpty(settingName))
            {
                throw new ArgumentNullException();
            }

            var settingValue = xmlHelper.GetNodeValue(settingName);

            if (string.IsNullOrEmpty(settingValue))
            {
                throw new InvalidOperationException();
            }

            return settingValue;
        }

        public void SetSettingByName(string settingName, string settingValue)
        {
            if (string.IsNullOrEmpty(settingName) ||
                string.IsNullOrEmpty(settingValue))
            {
                throw new ArgumentNullException();
            }

            xmlHelper.SetNodeValue(settingName, settingValue);
        }

        #endregion

        #region Инициализация значений по умолчанию
        
        private static void InitializeDefaultAppSettings(XmlHelper xmlHelper)
        {
            xmlHelper.AppendNode(string.Empty, "AppSettings");

            xmlHelper.AppendNode("AppSettings", "Environment");
            xmlHelper.AppendNode("Environment", "RecentProjects");

            xmlHelper.AppendNode("Environment", "InitialDirectory");
            xmlHelper.AppendNode("Environment", "ShowInvalidCourseDialog");

            xmlHelper.AppendNode("AppSettings", "Saving");
            xmlHelper.AppendNode("Saving", "AutosavingInterval");

            xmlHelper.AppendNode("AppSettings", "Appearance");
            xmlHelper.AppendNode("Appearance", "WindowState");
            xmlHelper.AppendNode("Appearance", "Left");
            xmlHelper.AppendNode("Appearance", "Top");
            xmlHelper.AppendNode("Appearance", "Width");
            xmlHelper.AppendNode("Appearance", "Height");

            xmlHelper.SetNodeValue("InitialDirectory", Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory));
            xmlHelper.SetNodeValue("ShowInvalidCourseDialog", "True");

            xmlHelper.SetNodeValue("AutosavingInterval", 0.ToString());

            xmlHelper.SetNodeValue("WindowState", FormWindowState.Maximized.ToString());
            xmlHelper.SetNodeValue("Left", 0.ToString());
            xmlHelper.SetNodeValue("Top", 0.ToString());
            xmlHelper.SetNodeValue("Width", 0.ToString());
            xmlHelper.SetNodeValue("Height", 0.ToString());
        }

        #endregion
    }
}