using System;
using System.Windows.Forms;
using VisualEditor.Logic.Controls.Docking;
using VisualEditor.Logic.Controls.HtmlEditing;
using VisualEditor.Logic.Controls.Ribbon.Extended;
using VisualEditor.Logic.Course.Preview;
using VisualEditor.Logic.Helpers;
using VisualEditor.Logic.Warehouse;
using VisualEditor.Utils.ExceptionHandling;

namespace VisualEditor.Logic.Commands.HtmlEditing
{
    internal class Preview : AbstractCommand
    {
        private const string operationCantBePerformedMessage = "Невозможно выполнить операцию. Попробуйте повтротить снова.";

        public Preview()
        {
            name = CommandNames.Preview;
            text = CommandTexts.Preview;
            image = Properties.Resources.Preview;
        }

        public override void Execute(object @object)
        {
            if (!Enabled)
            {
                return;
            }

            if (EditorObserver.HostEditorMode == Utils.Controls.HtmlEditing.Enums.HtmlEditingToolMode.Preview &&
                !Warehouse.Warehouse.IsHtmlSourceMode)
            {
                return;
            }

            if (EditorObserver.ActiveEditor == null)
            {
                return;
            }

            #region Сохранение контента редакторов

            foreach (var tmd in DockContainer.Instance.TrainingModuleDocuments)
            {
                if (tmd.HtmlEditingTool.Mode == Utils.Controls.HtmlEditing.Enums.HtmlEditingToolMode.Design)
                {
                    tmd.TrainingModule.DocumentHtml = tmd.HtmlEditingTool.BodyInnerHtml;
                    tmd.HtmlEditingTool.SetDefaultHtml();
                }
            }

            foreach (var qd in DockContainer.Instance.QuestionDocuments)
            {
                if (qd.HtmlEditingTool.Mode == Utils.Controls.HtmlEditing.Enums.HtmlEditingToolMode.Design)
                {
                    qd.Question.DocumentHtml = qd.HtmlEditingTool.BodyInnerHtml;
                    qd.HtmlEditingTool.SetDefaultHtml();
                }
            }

            foreach (var rd in DockContainer.Instance.ResponseDocuments)
            {
                if (rd.HtmlEditingTool.Mode == Utils.Controls.HtmlEditing.Enums.HtmlEditingToolMode.Design)
                {
                    rd.Response.DocumentHtml = rd.HtmlEditingTool.BodyInnerHtml;
                    rd.HtmlEditingTool.SetDefaultHtml();
                }
            }

            #endregion

            if (EditorObserver.ActiveEditor.HighlightedElement != null)
            {
                EditorObserver.ActiveEditor.Highlight(EditorObserver.ActiveEditor.HighlightedElement, false);
                EditorObserver.ActiveEditor.HighlightedElement = null;
            }

            try
            {
                // Переводит редактор в режим предварительного просмотра.
                EditorObserver.ActiveEditor.Mode = Utils.Controls.HtmlEditing.Enums.HtmlEditingToolMode.Preview;
            }
            catch (Exception exception)
            {
                ExceptionManager.Instance.LogException(exception);
                UIHelper.ShowMessage(operationCantBePerformedMessage,
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            var htmlEditingTool = HtmlEditingToolHelper.GetParentDocument(EditorObserver.ActiveEditor).HtmlEditingTool;
            HtmlToolEmbeddingHelper.SwitchToHtmlEditingTool(htmlEditingTool);

            try
            {
                // Конвертирует DocumentHtml в PreviewHtml и устанавливает PreviewHtml.
                PreviewConverter.Convert(EditorObserver.ActiveEditor);

                // Отменяет режим просмотра исходного кода.
                Warehouse.Warehouse.IsHtmlSourceMode = false;
                // Изменяет HostEditorMode.
                EditorObserver.HostEditorMode = Utils.Controls.HtmlEditing.Enums.HtmlEditingToolMode.Preview;
            }
            catch (Exception exception)
            {
                ExceptionManager.Instance.LogException(exception);
                UIHelper.ShowMessage(operationCantBePerformedMessage,
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;                
            }

            RibbonStatusStripEx.Instance.ClearOverwriteLabel();
            RibbonStatusStripEx.Instance.SetMessage("Режим предварительного просмотра");
        }
    }
}