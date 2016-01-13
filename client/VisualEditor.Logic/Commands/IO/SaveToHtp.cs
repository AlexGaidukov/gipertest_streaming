using System;
using System.IO;
using System.Windows.Forms;
using VisualEditor.Logic.Controls.Docking;
using VisualEditor.Logic.Controls.Ribbon.Extended;
using VisualEditor.Logic.Dialogs;
using VisualEditor.Logic.Helpers;
using VisualEditor.Logic.Helpers.AppSettings;
using VisualEditor.Logic.IO.Wrappers;
using VisualEditor.Logic.Warehouse;
using VisualEditor.Utils.ExceptionHandling;

namespace VisualEditor.Logic.Commands.IO
{
    internal class SaveToHtp : AbstractCommand
    {
        private const string operationCantBePerformedMessage = "Невозможно выполнить операцию. Попробуйте повтротить снова.";

        public SaveToHtp()
        {
            name = CommandNames.SaveToHtp;
            text = CommandTexts.SaveToHtp;
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

            #region Сохранение контента редакторов

            foreach (var tmd in DockContainer.Instance.TrainingModuleDocuments)
            {
                if (tmd.HtmlEditingTool.Mode == Utils.Controls.HtmlEditing.Enums.HtmlEditingToolMode.Design)
                {
                    tmd.TrainingModule.DocumentHtml = tmd.HtmlEditingTool.BodyInnerHtml;
                }
            }

            foreach (var qd in DockContainer.Instance.QuestionDocuments)
            {
                if (qd.HtmlEditingTool.Mode == Utils.Controls.HtmlEditing.Enums.HtmlEditingToolMode.Design)
                {
                    qd.Question.DocumentHtml = qd.HtmlEditingTool.BodyInnerHtml;
                }
            }

            foreach (var rd in DockContainer.Instance.ResponseDocuments)
            {
                if (rd.HtmlEditingTool.Mode == Utils.Controls.HtmlEditing.Enums.HtmlEditingToolMode.Design)
                {
                    rd.Response.DocumentHtml = rd.HtmlEditingTool.BodyInnerHtml;
                }
            }

            #endregion

            #region Проверка проекта на правильность

            if (@object != null)
            {
                var warnings = ProjectAnalyzer.Instance.Analyze();
                ProjectAnalyzer.Instance.FillWarningTree(warnings);
                if (AppSettingsHelper.DoInvalidCourseDialogShowing() &&
                    warnings.Count != 0)
                {
                    using (var icd = new InvalidCourseDialog())
                    {
                        if (icd.ShowDialog(EditorObserver.DialogOwner) == DialogResult.OK)
                        {
                            var dtu = icd.DataTransferUnit;
                            AppSettingsManager.Instance.SetSettingByName(SettingNames.ShowInvalidCourseDialog,
                                                                         (!bool.Parse(dtu.GetNodeValue("NeverShowAgain"))).ToString());
                        }
                    }
                }

                if (Warehouse.Warehouse.Instance.WarningTree.Nodes.Count != 0)
                {
                    CommandManager.Instance.GetCommand(CommandNames.Warnings).Execute(null);
                }
            }

            #endregion

            ////
            RibbonStatusStripEx.Instance.SetProgress(0);
            ////
            RibbonStatusStripEx.Instance.ProgressBarVisible = true;

            try
            {
                // Сохраняет xml в ProjectTrueLocation.
                CommandManager.Instance.GetCommand(CommandNames.SaveToXml).Execute(null);
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

            // Переносит xml из ProjectTrueLocation в ProjectEditorLocation.
            var sourcePath = Path.Combine(Warehouse.Warehouse.ProjectTrueLocation,
                                          string.Concat("ProjectName", ".xml"));
            var destPath = Path.Combine(Warehouse.Warehouse.ProjectEditorLocation,
                                        string.Concat("ProjectName", ".xml"));
            
            try
            {
                File.Move(sourcePath, destPath);
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

            #region Удаляет BOM (byte order mark) - 3 первых байта xml-файла

            try
            {
                var fs = File.Open(destPath, FileMode.Open, FileAccess.Read);
                var ms = new MemoryStream((int)fs.Length);
                fs.Read(ms.GetBuffer(), 0, (int)fs.Length);
                fs.Close();
                var bytes = ms.GetBuffer();
                ms = new MemoryStream(bytes, 3, bytes.Length - 3);
                fs = File.Open(destPath, FileMode.Create, FileAccess.Write);
                ms.WriteTo(fs);
                ms.Close();
                fs.Flush();
                fs.Close();
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

            #endregion

            try
            {
                // Сжимает GZip'ом.
                GZipHelper.Compress(destPath);
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
            RibbonStatusStripEx.Instance.SetProgress(95);
            ////

            // Упаковывает Tar'ом.
            //TarHelper.Pack(destPath);

            try
            {
                // Упаковываем Zip'ом.
                ZipHelper.Pack(destPath);
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

            // Удаляет gzip из ProjectEditorLocation.
            destPath = string.Concat(destPath, ".gz");

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

            // Переносит .htp в ProjectTrueLocation.
            sourcePath = Path.Combine(Warehouse.Warehouse.ProjectEditorLocation,
                                      string.Concat("ProjectName", ".htp"));
            destPath = Path.Combine(Warehouse.Warehouse.ProjectTrueLocation,
                                    string.Concat(Warehouse.Warehouse.ProjectFileName, ".htp"));
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

            try
            {
                File.Move(sourcePath, destPath);
            }
            catch (Exception exception)
            {
                ExceptionManager.Instance.LogException(exception);
                RibbonStatusStripEx.Instance.ProgressBarVisible = false;
                IsBusy = false;
                return;
            }

            ////
            RibbonStatusStripEx.Instance.SetProgress(100);
            ////
            RibbonStatusStripEx.Instance.ProgressBarVisible = false;

            IsBusy = false;
        }
    }
}