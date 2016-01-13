using System;
using System.Windows.Forms;
using VisualEditor.Logic.Controls;
using VisualEditor.Utils.ExceptionHandling;

namespace VisualEditor
{
    public static class Program
    {
        [STAThread]
        static void Main(string[] args)
        {
            ExceptionManager.Instance.AddLogger(new XmlFileLogger());

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            MainForm.Args = args;
            Application.Run(MainForm.Instance);
        }
    }
}