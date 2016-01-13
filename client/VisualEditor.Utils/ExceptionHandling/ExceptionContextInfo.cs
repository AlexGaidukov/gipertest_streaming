using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Text;

namespace VisualEditor.Utils.ExceptionHandling
{
    internal static class ExceptionContextInfo
    {
        #region Получение стека типов исключений

        public static string GetExceptionTypeStack(Exception e)
        {
            if (e.InnerException != null)
            {
                var message = new StringBuilder();
                message.AppendLine(GetExceptionTypeStack(e.InnerException));
                return (message.ToString());
            }

            return (e.GetType().ToString());
        }

        #endregion

        #region Получение стека сообщений

        public static string GetExceptionMessageStack(Exception e)
        {
            if (e.InnerException != null)
            {
                var message = new StringBuilder();
                message.AppendLine(GetExceptionMessageStack(e.InnerException));
                return (message.ToString());
            }

            return (e.Message);
        }

        #endregion

        #region Получение стека вызовов

        public static string GetExceptionCallStack(Exception e)
        {
            if (e.InnerException != null)
            {
                var message = new StringBuilder();
                message.AppendLine(GetExceptionCallStack(e.InnerException));
                message.AppendLine("Next call stack:");
                return (message.ToString());
            }

            return (e.StackTrace);
        }

        #endregion

        #region Получение информации о длительности работы системы

        public static TimeSpan GetSystemUpTime()
        {
            var upTime = new PerformanceCounter("System", "System Up Time");
            upTime.NextValue();
            return TimeSpan.FromSeconds(upTime.NextValue());
        }

        #endregion

        #region Получение информации о памяти

        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto)]
        public class MEMORYSTATUSEX
        {
            public uint dwLength;
            public uint dwMemoryLoad;
            public ulong ullTotalPhys;
            public ulong ullAvailPhys;
            public ulong ullTotalPageFile;
            public ulong ullAvailPageFile;
            public ulong ullTotalVirtual;
            public ulong ullAvailVirtual;
            public ulong ullAvailExtendedVirtual;

            public MEMORYSTATUSEX()
            {
                dwLength = (uint)Marshal.SizeOf(typeof(MEMORYSTATUSEX));
            }
        }

        [return: MarshalAs(UnmanagedType.Bool)]
        [DllImport("kernel32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        public static extern bool GlobalMemoryStatusEx([In, Out] MEMORYSTATUSEX lpBuffer);

        #endregion
    }
}