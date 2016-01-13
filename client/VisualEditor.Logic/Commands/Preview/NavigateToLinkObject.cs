using System;
using System.Windows.Forms;
using VisualEditor.Logic.Controls.Docking;
using VisualEditor.Logic.Controls.Docking.Documents;
using VisualEditor.Logic.Helpers;
using VisualEditor.Logic.IO;
using VisualEditor.Logic.Warehouse;
using VisualEditor.Utils.Controls.HtmlEditing;
using VisualEditor.Utils.ExceptionHandling;

namespace VisualEditor.Logic.Commands.Preview
{
    internal class NavigateToLinkObject : AbstractCommand
    {
        private const string linkNotSetMessage = "Ссылка на внешнюю компетенцию не настроена.";
        private const string operationCantBePerformedMessage = "Невозможно выполнить операцию. Попробуйте повтротить снова.";


        public NavigateToLinkObject()
        {
            name = CommandNames.NavigateToLinkObject;
            text = CommandTexts.NavigateToLinkObject;
        }

        public override void Execute(object @object)
        {
            if (!Enabled)
            {
                return;
            }

            // Снимает выделение компетенции, если оно было.
            if (EditorObserver.ActiveEditor != null)
            {
                if (EditorObserver.ActiveEditor.HighlightedElement != null)
                {
                    EditorObserver.ActiveEditor.Highlight(EditorObserver.ActiveEditor.HighlightedElement, false);
                    EditorObserver.ActiveEditor.HighlightedElement = null;
                }
            }

            // Снимает выделение.
            if (EditorObserver.ActiveEditor != null)
            {
                EditorObserver.ActiveEditor.Unselect();
            }

            var he = @object as HtmlElement;
            var href = he.GetAttribute("href");
            var tmid = Warehouse.Warehouse.Instance.GetTrainingModuleIdByObjectId(new Guid(TrainingModuleXmlWriter.ExtractRelativeHref(href)));

            if (tmid.Equals(Guid.Empty))
            {
                MessageBox.Show(linkNotSetMessage, 
                    System.Windows.Forms.Application.ProductName, 
                    MessageBoxButtons.OK, MessageBoxIcon.Information);

                return;
            }

            var b = ShowTrainingModuleDocument(tmid);

            if (b)
            {
                Navigate(he);
            }
            else
            {
                var tmd = CreateTrainingModuleDocument(tmid);
                PreviewObserver.AddDocument(tmd);
                Navigate(he);
            }
        }

        private static bool ShowTrainingModuleDocument(Guid id)
        {
            var dc = DockContainer.Instance;

            foreach (var d in dc.TrainingModuleDocuments)
            {
                if (d.TrainingModule.Id.Equals(id))
                {
                    d.Show();

                    return true;
                }
            }

            return false;
        }

        private static void Navigate(HtmlElement hel)
        {
            var ans = EditorObserver.ActiveEditor.GetElementsByTagName(TagNames.AnchorTagName);
            foreach (HtmlElement he in ans)
            {
                if (he.Id != null)
                {
                    if (he.Id.Equals(TrainingModuleXmlWriter.ExtractRelativeHref(hel.GetAttribute("href"))))
                    {
                        try
                        {
                            EditorObserver.ActiveEditor.ScrollToHtmlElement(he);
                        }
                        catch (Exception exception)
                        {
                            ExceptionManager.Instance.LogException(exception);
                        }

                        EditorObserver.ActiveEditor.HighlightedElement = he;

                        try
                        {
                            EditorObserver.ActiveEditor.Highlight(he, true);
                        }
                        catch (Exception exception)
                        {
                            ExceptionManager.Instance.LogException(exception);
                        }

                        break;
                    }
                }
            }
        }

        private static TrainingModuleDocument CreateTrainingModuleDocument(Guid id)
        {
            var tm = Warehouse.Warehouse.GetTrainingModuleById(id);

            tm.TrainingModuleDocument = new TrainingModuleDocument
            {
                TrainingModule = tm,
                Text = tm.Text
            };

            try
            {
                tm.TrainingModuleDocument.HtmlEditingTool.Mode = Utils.Controls.HtmlEditing.Enums.HtmlEditingToolMode.Design;
            }
            catch (Exception exception)
            {
                ExceptionManager.Instance.LogException(exception);
                UIHelper.ShowMessage(operationCantBePerformedMessage,
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                return null;
            }
            
            //tm.TrainingModuleDocument.VisualHtmlEditor.SetDefaultFont();
            Controls.HtmlEditing.HtmlEditingToolHelper.SetDefaultDocumentHtml(tm.TrainingModuleDocument.HtmlEditingTool);
            tm.TrainingModuleDocument.HtmlEditingTool.BodyInnerHtml = tm.DocumentHtml;
            // Блокирует команду Undo.
            tm.TrainingModuleDocument.HtmlEditingTool.Mode = Utils.Controls.HtmlEditing.Enums.HtmlEditingToolMode.Preview;
            tm.TrainingModuleDocument.HtmlEditingTool.Mode = Utils.Controls.HtmlEditing.Enums.HtmlEditingToolMode.Design;
            tm.TrainingModuleDocument.Show();

            if (tm.Text.Length > 22)
            {
                tm.TrainingModuleDocument.Text = tm.Text.Substring(0, 22) + "...";
            }
            else
            {
                tm.TrainingModuleDocument.Text = tm.Text;
            }

            return tm.TrainingModuleDocument;
        }
    }
}