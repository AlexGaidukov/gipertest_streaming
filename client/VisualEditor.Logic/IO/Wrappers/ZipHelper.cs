using System;
using System.IO;
using ICSharpCode.SharpZipLib.Zip;

namespace VisualEditor.Logic.IO.Wrappers
{
    internal static class ZipHelper
    {
        public static void Pack(string path)
        {
            var sourcePath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
            sourcePath = Path.Combine(sourcePath, Path.GetFileNameWithoutExtension(path));
            sourcePath = sourcePath + ".htp";
            var destPath = Path.Combine(Warehouse.Warehouse.ProjectEditorLocation, Path.GetFileNameWithoutExtension(path));
            destPath = destPath + ".htp";

            var fz = new FastZip
                         {
                             CreateEmptyDirectories = true
                         };
            fz.CreateZip(sourcePath, Warehouse.Warehouse.ProjectEditorLocation, true, "");

            File.Move(sourcePath, destPath);
        }

        public static void Unpack(string path)
        {
            var fz = new FastZip();
            fz.ExtractZip(path, Warehouse.Warehouse.ProjectEditorLocation, "");
        }

        public static void UnpackOuter(string path)
        {
            var fz = new FastZip();
            fz.ExtractZip(path, Warehouse.Warehouse.OuterProjectEditorLocation, "");
        }
    }
}