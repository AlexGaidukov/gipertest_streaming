using System;
using System.IO;
using VisualEditor.Utils.ExceptionHandling;
using VisualEditor.Utils.Helpers;

namespace VisualEditor.Logic.Helpers.AppSettings
{
    internal static class AppSettingsHelper
    {
        public static string GetInitialDirectory()
        {
            var initialDirectory = AppSettingsManager.Instance.GetSettingByName(SettingNames.InitialDirectory);
            if (!Directory.Exists(initialDirectory))
            {
                return Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory);
            }

            return initialDirectory;
        }

        public static bool DoInvalidCourseDialogShowing()
        {
            bool doShowing = true;
            try
            {
                doShowing = Convert.ToBoolean(AppSettingsManager.Instance.GetSettingByName(SettingNames.ShowInvalidCourseDialog));
            }
            catch (Exception exception)
            {
                ExceptionManager.Instance.LogException(exception);
                doShowing = true;
            }

            return doShowing;            
        }

        public static decimal GetAutosavingInterval()
        {
            decimal autosavingInterval = 0;
            try
            {
                string stringAutosavingInterval = AppSettingsManager.Instance.GetSettingByName(SettingNames.AutosavingInterval);
                autosavingInterval = decimal.Parse(stringAutosavingInterval);
            }
            catch (Exception exception)
            {
                ExceptionManager.Instance.LogException(exception);
                autosavingInterval = 0;
            }

            if (autosavingInterval < 0 || autosavingInterval > 30)
            {
                autosavingInterval = 0;
            }

            return autosavingInterval;
        }

        public static void SaveRecentProjects(XmlHelper xh)
        {
            for (var i = 0; i < int.MaxValue; i++)
            {
                var nodeName = string.Format("ProjectPath{0}", i + 1);
                var value = xh.GetNodeValue(nodeName);

                if (value.Equals(string.Empty))
                {
                    break;
                }

                xh.RemoveNode(nodeName);
            }

            for (var i = 0; i < Warehouse.Warehouse.Instance.RecentProjects.Count; i++)
            {
                var nodeName = string.Format("ProjectPath{0}", i + 1);
                xh.AppendNode("RecentProjects", nodeName);
                xh.SetNodeValue(nodeName, Warehouse.Warehouse.Instance.RecentProjects[i]);
            }
        }

        public static void LoadRecentProjects(XmlHelper xh)
        {
            for (var i = 0; i < int.MaxValue; i++)
            {
                var nodeName = string.Format("ProjectPath{0}", i + 1);
                var value = xh.GetNodeValue(nodeName);

                if (value.Equals(string.Empty))
                {
                    break;
                }

                if (File.Exists(value))
                {
                    Warehouse.Warehouse.Instance.RecentProjects.Add(value);
                }
            }
        }
    }
}