using System;
using System.Windows.Forms;
using Microsoft.Win32;
using VisualEditor.Utils.ExceptionHandling;

namespace VisualEditor.Utils.Helpers
{
    public static class RegistryHelper
    {
        public static void RegisterFileAssociation()
        {
            /*try
            {
                string keyName = @"HKEY_LOCAL_MACHINE\SOFTWARE\Classes\htpfile\Shell\Open\command";
                Registry.SetValue(keyName, "", string.Concat("\"", Application.ExecutablePath, "\" ", "\"%1\""));
                keyName = @"HKEY_LOCAL_MACHINE\SOFTWARE\Classes\htpfile\DefaultIcon";
                Registry.SetValue(keyName, "", string.Concat("\"", Application.ExecutablePath, "\""));
            }
            catch (Exception exception)
            {
                ExceptionManager.Instance.LogException(exception);
            }*/
        }
    }
}