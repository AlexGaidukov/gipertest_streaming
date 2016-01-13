using System;
using System.IO;
using System.Windows.Forms;
using VisualEditor.Logic.Controls.Ribbon.Extended;
using VisualEditor.Logic.Helpers;
using VisualEditor.Logic.IO.Wrappers;
using VisualEditor.Utils.ExceptionHandling;

namespace VisualEditor.Logic.Commands.IO
{
    internal class OuterLoadFromHtp : AbstractCommand
    {
        private const string operationCantBePerformedMessage = "Невозможно выполнить операцию. Попробуйте повтротить снова.";

        public OuterLoadFromHtp()
        {
            name = CommandNames.OuterLoadFromHtp;
            text = CommandTexts.OuterLoadFromHtp;
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

            // Копирует htp в ProjectEditorLocation.
            var sourcePath = Path.Combine(Warehouse.Warehouse.OuterProjectTrueLocation,
                                          string.Concat(Warehouse.Warehouse.OuterProjectFileName, ".htp"));
            var destPath = Path.Combine(Warehouse.Warehouse.OuterProjectEditorLocation,
                                        string.Concat(Warehouse.Warehouse.OuterProjectFileName, ".htp"));

            try
            {
                File.Copy(sourcePath, destPath);
            }
            catch (Exception exception)
            {
                ExceptionManager.Instance.LogException(exception);
                UIHelper.ShowMessage(operationCantBePerformedMessage, MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
                IsBusy = false;
                return;
            }

            try
            {
                // Распаковывает htp.
                ZipHelper.UnpackOuter(destPath);
            }
            catch
            {
                try
                {
                    // Распаковывает htp.
                    TarHelper.UnpackOuter(destPath);
                }
                catch (Exception exception)
                {
                    ExceptionManager.Instance.LogException(exception);
                    UIHelper.ShowMessage(operationCantBePerformedMessage, MessageBoxButtons.OK,
                        MessageBoxIcon.Error);
                    IsBusy = false;
                    return;
                }
            }

            // Получает имя файла gzip.
            var files = Directory.GetFiles(Warehouse.Warehouse.OuterProjectEditorLocation);
            foreach (var f in files)
            {
                if (Path.GetExtension(f).ToLower().Equals(".gz"))
                {
                    Warehouse.Warehouse.OuterProjectArchiveName = Path.GetFileNameWithoutExtension(f);
                    break;
                }
            }

            // Разархивирует gzip.
            sourcePath = Path.Combine(Warehouse.Warehouse.OuterProjectEditorLocation,
                                      string.Concat(Warehouse.Warehouse.OuterProjectArchiveName, ".gz"));

            try
            {
                GZipHelper.Decompress(sourcePath);
            }
            catch (Exception exception)
            {
                ExceptionManager.Instance.LogException(exception);
                UIHelper.ShowMessage(operationCantBePerformedMessage, MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
                IsBusy = false;
                return;
            }

            try
            {
                // Загружает xml.
                CommandManager.Instance.GetCommand(CommandNames.OuterLoadFromXml).Execute(null);
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
            destPath = Path.Combine(Warehouse.Warehouse.OuterProjectEditorLocation, Warehouse.Warehouse.OuterProjectArchiveName);

            try
            {
                File.Delete(destPath);
            }
            catch (Exception exception)
            {
                ExceptionManager.Instance.LogException(exception);
                IsBusy = false;
                return;
            }

            Warehouse.Warehouse.OuterProjectArchiveName = string.Empty;

            // Удаляет htp из ProjectEditorLocation.
            destPath = Path.Combine(Warehouse.Warehouse.OuterProjectEditorLocation,
                                    string.Concat(Warehouse.Warehouse.OuterProjectFileName, ".htp"));

            try
            {
                File.Delete(destPath);
            }
            catch (Exception exception)
            {
                ExceptionManager.Instance.LogException(exception);
                IsBusy = false;
                return;
            }

            // Удаляет config.cfn ProjectEditorLocation.
            destPath = Path.Combine(Warehouse.Warehouse.OuterProjectEditorLocation, "config.cfn");
            if (File.Exists(destPath))
            {
                try
                {
                    File.Delete(destPath);
                }
                catch (Exception exception)
                {
                    ExceptionManager.Instance.LogException(exception);
                    IsBusy = false;
                    return;
                }
            }

            IsBusy = false;
        }
    }
}