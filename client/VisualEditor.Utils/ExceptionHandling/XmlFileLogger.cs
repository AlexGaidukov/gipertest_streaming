using System;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Text;
using System.Windows.Forms;
using VisualEditor.Utils.Helpers;

namespace VisualEditor.Utils.ExceptionHandling
{
    public class XmlFileLogger : IExceptionLogger
    {
        private const string exceptionDirectory = "Bug reports";

        public void Log(Exception exception)
        {
            lock (this)
            {
                var path = Path.Combine(Application.StartupPath, exceptionDirectory);

                if (!Directory.Exists(path))
                {
                    // Операция может вызвать исключение.
                    Directory.CreateDirectory(path);
                }

                path = Path.Combine(path, string.Concat(Guid.NewGuid().ToString(), ".xml"));

                var xmlHelper = new XmlHelper();
                xmlHelper.AppendNode(string.Empty, "ExceptionInfo");
                xmlHelper.AppendNode("ExceptionInfo", "ProductName");
                xmlHelper.AppendNode("ExceptionInfo", "ProductVersion");
                xmlHelper.AppendNode("ExceptionInfo", "Date");
                xmlHelper.AppendNode("ExceptionInfo", "ComputerName");
                xmlHelper.AppendNode("ExceptionInfo", "UserName");
                xmlHelper.AppendNode("ExceptionInfo", "OS");
                xmlHelper.AppendNode("ExceptionInfo", "Culture");
                xmlHelper.AppendNode("ExceptionInfo", "Resolution");
                xmlHelper.AppendNode("ExceptionInfo", "SystemUpTime");
                xmlHelper.AppendNode("ExceptionInfo", "AppUpTime");
                xmlHelper.AppendNode("ExceptionInfo", "TotalMemory");
                xmlHelper.AppendNode("ExceptionInfo", "AvailableMemory");
                xmlHelper.AppendNode("ExceptionInfo", "ExceptionClasses");
                xmlHelper.AppendNode("ExceptionInfo", "ExceptionMessages");
                xmlHelper.AppendNode("ExceptionInfo", "StackTraces");
                xmlHelper.AppendNode("ExceptionInfo", "LoadedModules");

                xmlHelper.SetNodeValue("ProductName", Application.ProductName);
                xmlHelper.SetNodeValue("ProductVersion", Application.ProductVersion);
                xmlHelper.SetNodeValue("Date", DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss"));
                xmlHelper.SetNodeValue("ComputerName", SystemInformation.ComputerName);
                xmlHelper.SetNodeValue("UserName", SystemInformation.UserName);
                xmlHelper.SetNodeValue("OS", Environment.OSVersion.ToString());
                xmlHelper.SetNodeValue("Culture", CultureInfo.CurrentCulture.Name);
                xmlHelper.SetNodeValue("Resolution", SystemInformation.PrimaryMonitorSize.ToString());
                xmlHelper.SetNodeValue("SystemUpTime", ExceptionContextInfo.GetSystemUpTime().ToString());
                xmlHelper.SetNodeValue("AppUpTime", (DateTime.Now - Process.GetCurrentProcess().StartTime).ToString());
                var memoryStatus = new ExceptionContextInfo.MEMORYSTATUSEX();
                if (ExceptionContextInfo.GlobalMemoryStatusEx(memoryStatus))
                {
                    xmlHelper.SetNodeValue("TotalMemory", memoryStatus.ullTotalPhys/(1024*1024) + "Mb");
                    xmlHelper.SetNodeValue("AvailableMemory", memoryStatus.ullAvailPhys/(1024*1024) + "Mb");
                }
                xmlHelper.SetNodeValue("ExceptionClasses", ExceptionContextInfo.GetExceptionTypeStack(exception));
                xmlHelper.SetNodeValue("ExceptionMessages", ExceptionContextInfo.GetExceptionMessageStack(exception));
                xmlHelper.SetNodeValue("StackTraces", ExceptionContextInfo.GetExceptionCallStack(exception));

                var currentProcess = Process.GetCurrentProcess();
                var processInfo = new StringBuilder();
                foreach (ProcessModule module in currentProcess.Modules)
                {
                    processInfo.AppendLine(module.FileName + " " + module.FileVersionInfo.FileVersion);
                }
                xmlHelper.SetNodeValue("LoadedModules", processInfo.ToString());

                // Операция может вызвать исключение.
                xmlHelper.Save(path);
            }
        }
    }
}