using System;
using System.Windows.Forms;
using VisualEditor.Logic.Controls.Docking.Documents;
using VisualEditor.Logic.Controls.HtmlEditing;
using VisualEditor.Logic.Controls.Ribbon.Extended;
using VisualEditor.Logic.Helpers;
using VisualEditor.Logic.Warehouse;
using VisualEditor.Utils.ExceptionHandling;

namespace VisualEditor.Logic.Commands.HtmlEditing
{
    internal class Design : AbstractCommand
    {
        private const string operationCantBePerformedMessage = "Невозможно выполнить операцию. Попробуйте повтротить снова.";

        public Design()
        {
            name = CommandNames.Design;
            text = CommandTexts.Design;
            image = Properties.Resources.Design;
        }

        public override void Execute(object @object)
        {
            if (!Enabled)
            {
                return;
            }

            if (EditorObserver.HostEditorMode == Utils.Controls.HtmlEditing.Enums.HtmlEditingToolMode.Design &&
                !Warehouse.Warehouse.IsHtmlSourceMode)
            {
                return;
            }

            if (EditorObserver.ActiveEditor == null)
            {
                return;
            }

            if (EditorObserver.ActiveEditor.HighlightedElement != null)
            {
                EditorObserver.ActiveEditor.Highlight(EditorObserver.ActiveEditor.HighlightedElement, false);
                EditorObserver.ActiveEditor.HighlightedElement = null;
            }

            try
            {
                // Переводит редактор в режим визуального редактирования.
                EditorObserver.ActiveEditor.Mode = Utils.Controls.HtmlEditing.Enums.HtmlEditingToolMode.Design;

                // Отменяет режим просмотра исходного кода.
                Warehouse.Warehouse.IsHtmlSourceMode = false;
                // Изменяет HostEditorMode.
                EditorObserver.HostEditorMode = Utils.Controls.HtmlEditing.Enums.HtmlEditingToolMode.Design;
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

            // Обнуляет PreviewHtml и устанавливает прежний DocumentHtml.
            if (HtmlEditingToolHelper.GetParentDocument(EditorObserver.ActiveEditor) is TrainingModuleDocument)
            {
                var tm = ((TrainingModuleDocument)HtmlEditingToolHelper.GetParentDocument(EditorObserver.ActiveEditor)).TrainingModule;
                tm.PreviewHtml = string.Empty;
                EditorObserver.ActiveEditor.BodyInnerHtml = tm.DocumentHtml;
                HtmlEditingToolHelper.SetDefaultStyle(htmlEditingTool);
            }

            if (EditorObserver.ActiveEditor.CanOverwrite)
            {
                RibbonStatusStripEx.Instance.MakeOverwrite();
            }
            else
            {
                RibbonStatusStripEx.Instance.MakeInsert();
            }
            RibbonStatusStripEx.Instance.SetMessage(string.Empty);

            Warehouse.Warehouse.Instance.CourseTree.HandleContextMenu();
        }
    }
}