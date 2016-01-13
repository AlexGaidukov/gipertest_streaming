using System.Diagnostics;
using System.IO;
using System.Windows.Forms;

namespace VisualEditor.Logic.IO.Wrappers
{
    internal static class TarHelper
    {
        public static void Pack(string path)
        {
            var psi = new ProcessStartInfo
                          {
                              Arguments = string.Concat("-cf \"", Path.GetFileNameWithoutExtension(path),
                                                        ".htp\" --mode=\"a+rwx\" \"",
                                                        Path.GetFileNameWithoutExtension(path),
                                                        ".xml.gz\" \"", Warehouse.Warehouse.RelativeImagesDirectory,
                                                        "\" \"", Warehouse.Warehouse.RelativeFlashesDirectory, "\""),
                              CreateNoWindow = true,
                              FileName = Path.Combine(Application.StartupPath, "tar.exe"),
                              UseShellExecute = false,
                              WorkingDirectory = Warehouse.Warehouse.ProjectEditorLocation
                          };

            var ps = Process.Start(psi);
            while (!ps.HasExited)
            {
                Application.DoEvents();
            }
        }

        public static void Unpack(string path)
        {
            var psi = new ProcessStartInfo
                          {
                              Arguments = string.Concat("-xf \"", Path.GetFileNameWithoutExtension(path), ".htp\""),
                              CreateNoWindow = true,
                              FileName = Path.Combine(Application.StartupPath, "tar.exe"),
                              UseShellExecute = false,
                              WorkingDirectory = Warehouse.Warehouse.ProjectEditorLocation
                          };

            var ps = Process.Start(psi);
            while (!ps.HasExited)
            {
                Application.DoEvents();
            }
        }

        public static void UnpackOuter(string path)
        {
            var psi = new ProcessStartInfo
            {
                Arguments = string.Concat("-xf \"", Path.GetFileNameWithoutExtension(path), ".htp\""),
                CreateNoWindow = true,
                FileName = Path.Combine(Application.StartupPath, "tar.exe"),
                UseShellExecute = false,
                WorkingDirectory = Warehouse.Warehouse.OuterProjectEditorLocation
            };

            var ps = Process.Start(psi);
            while (!ps.HasExited)
            {
                Application.DoEvents();
            }
        }
    }
}