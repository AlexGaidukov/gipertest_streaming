using System;
using System.Windows.Forms;
using VisualEditor.Logic.Controls.Docking;
using VisualEditor.Logic.Controls.Docking.Documents;
using VisualEditor.Logic.Warehouse;
using HtmlEditingToolHelper=VisualEditor.Logic.Controls.HtmlEditing.HtmlEditingToolHelper;

namespace VisualEditor.Logic.Commands.Concept
{
    internal class NavigateToConcept : AbstractCommand
    {
        private const string noExternalLinkMessage = "Ссылка на внешнюю компетенцию не настроена.";

        public NavigateToConcept()
        {
            name = CommandNames.NavigateToConcept;
            text = CommandTexts.NavigateToConcept;
        }

        public override void Execute(object @object)
        {
            if (!Enabled)
            {
                return;
            }

            if (Warehouse.Warehouse.Instance.ConceptTree.CurrentNode.Type == Enums.ConceptType.Internal)
            {
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

                // Переходит к компетенции.
                var c = Warehouse.Warehouse.Instance.ConceptTree.CurrentNode;
                var b = ShowTrainingModuleDocument(c.ModuleId);

                if (b)
                {
                    Navigate(c.Id);
                }
                else
                {
                    var tmd = CreateTrainingModuleDocument(c.ModuleId);
                    PreviewObserver.AddDocument(tmd);
                    Navigate(c.Id);
                }
            }
            else
            {
                MessageBox.Show(noExternalLinkMessage, System.Windows.Forms.Application.ProductName, 
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
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

        private static void Navigate(Guid id)
        {
            var ans = EditorObserver.ActiveEditor.Links;
            foreach (HtmlElement he in ans)
            {
                if (he.Id != null)
                {
                    if (he.Id.Equals(id.ToString()))
                    {
                        EditorObserver.ActiveEditor.ScrollToHtmlElement(he);
                        EditorObserver.ActiveEditor.HighlightedElement = he;
                        EditorObserver.ActiveEditor.Highlight(he, true);
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
                                                Text = tm.Text,
                                                HtmlEditingTool =
                                                    {Mode = Utils.Controls.HtmlEditing.Enums.HtmlEditingToolMode.Design}
                                            };

            //tm.TrainingModuleDocument.VisualHtmlEditor.SetDefaultFont();
            HtmlEditingToolHelper.SetDefaultDocumentHtml(tm.TrainingModuleDocument.HtmlEditingTool);
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