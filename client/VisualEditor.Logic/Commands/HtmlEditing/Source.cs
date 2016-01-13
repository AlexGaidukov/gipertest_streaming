using System;
using VisualEditor.Logic.Controls.Docking;
using VisualEditor.Logic.Controls.HtmlEditing;
using VisualEditor.Logic.Warehouse;

namespace VisualEditor.Logic.Commands.HtmlEditing
{
    internal class Source : AbstractCommand
    {
        public Source()
        {
            name = CommandNames.Source;
            text = CommandTexts.Source;
            image = Properties.Resources.Source;
        }

        public override void Execute(object @object)
        {
            if (!Enabled)
            {
                return;
            }

            if (Warehouse.Warehouse.IsHtmlSourceMode)
            {
                return;
            }

            if (EditorObserver.ActiveEditor.HighlightedElement != null)
            {
                EditorObserver.ActiveEditor.Highlight(EditorObserver.ActiveEditor.HighlightedElement, false);
                EditorObserver.ActiveEditor.HighlightedElement = null;
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

            // Применяет режим просмотра исходного кода.
            Warehouse.Warehouse.IsHtmlSourceMode = true;

            var htmlSourceViewer = HtmlEditingToolHelper.GetParentDocument(EditorObserver.ActiveEditor).HtmlSourceViewer;
            HtmlToolEmbeddingHelper.SwitchToHtmlSourceViewer(htmlSourceViewer);
            htmlSourceViewer.Text = EditorObserver.ActiveEditor.BodyInnerHtml;
        }
    }
}