using System;
using System.IO;
using System.Windows.Forms;
using VisualEditor.Logic.Controls.Ribbon.Extended;
using VisualEditor.Logic.Helpers;
using VisualEditor.Logic.IO.Wrappers;
using VisualEditor.Utils.ExceptionHandling;

namespace VisualEditor.Logic.Commands.IO
{
    internal class LoadFromHtp : AbstractCommand
    {
        private const string operationCantBePerformedMessage = "Невозможно выполнить операцию. Попробуйте повтротить снова.";

        public LoadFromHtp()
        {
            name = CommandNames.LoadFromHtp;
            text = CommandTexts.LoadFromHtp;
        }

        public static bool IsBusy { get; set; }

        public override void Execute(object @object)
        {
            if (!Enabled)
            {
                return;
            }

            if (IsBusy)
            {
                return;
            }

            IsBusy = true;

            ////
            RibbonStatusStripEx.Instance.SetProgress(0);
            ////
            RibbonStatusStripEx.Instance.ProgressBarVisible = true;

            // Копирует htp в ProjectEditorLocation.
            var sourcePath = Path.Combine(Warehouse.Warehouse.ProjectTrueLocation,
                                          string.Concat(Warehouse.Warehouse.ProjectFileName, ".htp"));
            var destPath = Path.Combine(Warehouse.Warehouse.ProjectEditorLocation,
                                        string.Concat("ProjectName", ".htp"));
            try
            {
                File.Copy(sourcePath, destPath);
            }
            catch (Exception exception)
            {
                ExceptionManager.Instance.LogException(exception);
                UIHelper.ShowMessage(operationCantBePerformedMessage, MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
                RibbonStatusStripEx.Instance.ProgressBarVisible = false;
                IsBusy = false;
                return;
            }

            try
            {
                // Распаковывает htp.
                ZipHelper.Unpack(destPath);
            }
            catch (Exception exception1)
            {
                try
                {
                    // Распаковывает htp.
                    TarHelper.Unpack(destPath);
                }
                catch (Exception exception2)
                {
                    ExceptionManager.Instance.LogException(exception2);
                    UIHelper.ShowMessage(operationCantBePerformedMessage, MessageBoxButtons.OK,
                        MessageBoxIcon.Error);
                    RibbonStatusStripEx.Instance.ProgressBarVisible = false;
                    IsBusy = false;
                    return;
                }
            }

            // Получает имя файла gzip.
            var files = Directory.GetFiles(Warehouse.Warehouse.ProjectEditorLocation);
            foreach (var f in files)
            {
                if (Path.GetExtension(f).ToLower().Equals(".gz"))
                {
                    Warehouse.Warehouse.ProjectArchiveName = Path.GetFileNameWithoutExtension(f);
                    break;
                }
            }

            // Разархивирует gzip.
            sourcePath = Path.Combine(Warehouse.Warehouse.ProjectEditorLocation,
                                      string.Concat(Warehouse.Warehouse.ProjectArchiveName, ".gz"));
            var tempSourcePath = Path.Combine(Warehouse.Warehouse.ProjectEditorLocation,
                                      string.Concat("ProjectName.xml", ".gz"));
            File.Move(sourcePath, tempSourcePath);
            sourcePath = tempSourcePath;
            Warehouse.Warehouse.ProjectArchiveName = Path.GetFileNameWithoutExtension(sourcePath);

            try
            {
                GZipHelper.Decompress(sourcePath);
            }
            catch (Exception exception)
            {
                ExceptionManager.Instance.LogException(exception);
                UIHelper.ShowMessage(operationCantBePerformedMessage, MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
                RibbonStatusStripEx.Instance.ProgressBarVisible = false;
                IsBusy = false;
                return;
            }

            ////
            RibbonStatusStripEx.Instance.SetProgress(5);
            ////

            try
            {
                // Загружает xml.
                CommandManager.Instance.GetCommand(CommandNames.LoadFromXml).Execute(null);
            }
            catch (Exception exception)
            {
                ExceptionManager.Instance.LogException(exception);
                UIHelper.ShowMessage(operationCantBePerformedMessage, MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
                RibbonStatusStripEx.Instance.ProgressBarVisible = false;
                IsBusy = false;
                return;
            }

            // Удаляет xml из ProjectEditorLocation.
            destPath = Path.Combine(Warehouse.Warehouse.ProjectEditorLocation, Warehouse.Warehouse.ProjectArchiveName);

            try
            {
                File.Delete(destPath);
            }
            catch (Exception exception)
            {
                ExceptionManager.Instance.LogException(exception);
                RibbonStatusStripEx.Instance.ProgressBarVisible = false;
                IsBusy = false;
                return;
            }

            Warehouse.Warehouse.ProjectArchiveName = string.Empty;

            // Удаляет htp из ProjectEditorLocation.
            try
            {
                destPath = Path.Combine(Warehouse.Warehouse.ProjectEditorLocation,
                                        string.Concat("ProjectName", ".htp"));
                File.Delete(destPath);
            }
            catch (Exception exception)
            {
                ExceptionManager.Instance.LogException(exception);
                RibbonStatusStripEx.Instance.ProgressBarVisible = false;
                IsBusy = false;
                return;
            }
            
            // Удаляет config.cfn ProjectEditorLocation.
            destPath = Path.Combine(Warehouse.Warehouse.ProjectEditorLocation, "config.cfn");
            if (File.Exists(destPath))
            {
                try
                {
                    File.Delete(destPath);
                }
                catch (Exception exception)
                {
                    ExceptionManager.Instance.LogException(exception);
                    RibbonStatusStripEx.Instance.ProgressBarVisible = false;
                    IsBusy = false;
                    return;
                }
            }

            RibbonStatusStripEx.Instance.ProgressBarVisible = false;
            IsBusy = false;
        }
    }
}