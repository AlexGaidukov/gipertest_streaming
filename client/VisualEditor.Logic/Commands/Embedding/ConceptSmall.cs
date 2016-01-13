using System;
using System.Collections.Generic;
using System.Windows.Forms;
using VisualEditor.Logic.Controls.Docking.Documents;
using VisualEditor.Logic.Course.Items;
using VisualEditor.Logic.Dialogs;
using VisualEditor.Logic.Helpers;
using VisualEditor.Logic.Warehouse;
using VisualEditor.Utils.Controls.HtmlEditing;
using VisualEditor.Utils.ExceptionHandling;

namespace VisualEditor.Logic.Commands.Embedding
{
    internal class ConceptSmall : AbstractCommand
    {
        private const string operationCantBePerformedMessage = "Невозможно выполнить операцию. Попробуйте повтротить снова.";

        public ConceptSmall()
        {
            name = CommandNames.ConceptSmall;
            text = CommandTexts.ConceptSmall;
            image = Properties.Resources.ConceptSmall;
        }

        public override void Execute(object @object)
        {
            if (!Enabled)
            {
                return;
            }

            if (EditorObserver.ActiveEditor == null)
            {
                return;
            }

            if (EditorObserver.ActiveEditor.IsSelection)//
            {
                return;
            }

            using (var cd = new ConceptDialog())
            {
                if (cd.ShowDialog(EditorObserver.DialogOwner) == DialogResult.OK)
                {
                    var tm = ((TrainingModuleDocument)Controls.HtmlEditing.HtmlEditingToolHelper.GetParentDocument(EditorObserver.ActiveEditor)).TrainingModule;                        

                    // Добавляет компетенцию в дерево компетенций.
                    var c = new Logic.Course.Items.Concept
                                {
                                    Id = Guid.NewGuid(),
                                    ModuleId = tm.Id,
                                    Text = cd.DataTransferUnit.GetNodeValue("ConceptName")
                                };

                    if (cd.DataTransferUnit.GetNodeValue("ConceptType").Equals(Enums.ConceptType.Internal.ToString()))
                    {
                        c.Type = Enums.ConceptType.Internal;
                    }
                    else
                    {
                        c.Type = Enums.ConceptType.External;
                    }

                    Warehouse.Warehouse.Instance.ConceptTree.Nodes.Add(c);

                    if (c.Type.Equals(Enums.ConceptType.Internal))
                    {
                        #region Внутренняя компетенция

                        // Добавляет компетенцию-пустышку в дерево учебного курса.
                        var odc = new OutDummyConcept
                                      {
                                          Concept = c,
                                          Text = c.Text
                                      };
                        tm.OutConceptParent.Nodes.Add(odc);
                        odc.Concept.OutDummyConcept = odc;

                        // Добавляет компетенцию в Html-код.
                        var d = new Dictionary<string, string>
                                    {
                                        {"id", c.Id.ToString()},
                                        {"class", "concept"}
                                    };

                        try
                        {
                            HtmlEditingToolHelper.InsertHtml(EditorObserver.ActiveEditor, TagNames.AnchorTagName, d);
                        }
                        catch (Exception exception)
                        {
                            ExceptionManager.Instance.LogException(exception);
                            UIHelper.ShowMessage(operationCantBePerformedMessage, MessageBoxButtons.OK,
                                MessageBoxIcon.Error);
                            return;
                        }

                        #endregion
                    }
                    else
                    {
                        #region Внешняя компетенция

                        // Добавляет компетенцию-пустышку в дерево учебного курса.
                        var idc = new InDummyConcept
                                      {
                                          Concept = c,
                                          Text = c.Text
                                      };
                        Warehouse.Warehouse.Instance.CourseTree.InConceptsParent.Nodes.Add(idc);
                        idc.Concept.InDummyConcepts.Add(idc);

                        #endregion
                    }
                }
            }
        }
    }
}