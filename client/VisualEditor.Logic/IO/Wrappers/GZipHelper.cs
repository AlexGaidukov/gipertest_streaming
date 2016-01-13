using System.Diagnostics;
using System.IO;
using System.Windows.Forms;

namespace VisualEditor.Logic.IO.Wrappers
{
    internal static class GZipHelper
    {
        public static void Compress(string path)
        {
            var psi = new ProcessStartInfo
                          {
                              Arguments = string.Concat("-9 \"", path, "\""),
                              CreateNoWindow = true,
                              FileName = Path.Combine(Application.StartupPath, "gzip.exe"),
                              UseShellExecute = false
                          };

            var ps = Process.Start(psi);
            while (!ps.HasExited)
            {
                Application.DoEvents();
            }
        }

        public static void Decompress(string path)
        {
            var psi = new ProcessStartInfo
                          {
                              Arguments = string.Concat("-df \"", Path.Combine(Path.GetDirectoryName(path), Path.GetFileNameWithoutExtension(path)), ".gz\""),
                              CreateNoWindow = true,
                              FileName = Path.Combine(Application.StartupPath, "gzip.exe"),
                              UseShellExecute = false
                          };

            var ps = Process.Start(psi);
            while (!ps.HasExited)
            {
                Application.DoEvents();
            }
        }
    }
}