using System.Windows.Forms;
using VisualEditor.Logic.IO;

namespace VisualEditor.Logic.Commands.Concept
{
    internal class DeleteConcept : AbstractCommand
    {
        private const string deleteConceptMessage = "Вы уверены, что хотите удалить компетенцию?\nВсе ссылки на удаляемую компетенцию будут удалены.";

        public DeleteConcept()
        {
            name = CommandNames.DeleteConcept;
            text = CommandTexts.DeleteConcept;
            image = Properties.Resources.DeleteConcept;
        }

        public override void Execute(object @object)
        {
            if (!Enabled)
            {
                return;
            }

            var ct = Warehouse.Warehouse.Instance.ConceptTree;
            var c = ct.CurrentNode;

            if (c == null)
            {
                return;
            }

            if (MessageBox.Show(deleteConceptMessage, System.Windows.Forms.Application.ProductName, 
                MessageBoxButtons.OKCancel, MessageBoxIcon.Question) != DialogResult.OK)
            {
                return;
            }

            // Удаляет компетенцию из дерева компетенций.
            ct.Nodes.Remove(c);

            if (c.Type.Equals(Enums.ConceptType.Internal))
            {
                // Удаляет компетенцию из выходов.
                c.OutDummyConcept.Parent.Nodes.Remove(c.OutDummyConcept);

                // Удаляет компетенцию из входов.
                foreach (var idc in c.InDummyConcepts)
                {
                    idc.Parent.Nodes.Remove(idc);
                }
                c.InDummyConcepts.Clear();

                // Удаляет компетенцию из Html-кода.
                var tm = Warehouse.Warehouse.GetTrainingModuleById(c.ModuleId);
                if (tm.TrainingModuleDocument == null)
                {
                    #region Документ не создан

                    var html = string.Copy(tm.DocumentHtml);
                    var mlp = new MlParser();
                    string searchString;
                    string innerHtml;

                    searchString = c.Id.ToString();
                    while (html.Contains(searchString))
                    {
                        mlp.GetTagBounds(html, searchString);
                        mlp.ShiftLastIndex(ref html);

                        innerHtml = mlp.GetInnerHtml();
                        html = html.Remove(mlp.StartIndex, mlp.LastIndex - mlp.StartIndex + 1);
                        html = html.Insert(mlp.StartIndex, innerHtml);
                    }

                    tm.DocumentHtml = html;

                    #endregion
                }
                else
                {
                    #region Документ создан

                    var html = string.Copy(tm.TrainingModuleDocument.HtmlEditingTool.BodyInnerHtml);
                    var mlp = new MlParser();
                    string searchString;
                    string innerHtml;

                    searchString = c.Id.ToString();
                    while (html.Contains(searchString))
                    {
                        mlp.GetTagBounds(html, searchString);
                        mlp.ShiftLastIndex(ref html);

                        innerHtml = mlp.GetInnerHtml();
                        html = html.Remove(mlp.StartIndex, mlp.LastIndex - mlp.StartIndex + 1);
                        html = html.Insert(mlp.StartIndex, innerHtml);
                    }

                    tm.TrainingModuleDocument.HtmlEditingTool.BodyInnerHtml = html;

                    #endregion
                }

                // Удаляет ссылки на компетенцию.
                foreach (var lto in Warehouse.Warehouse.Instance.LinksToObjects)
                {
                    if (lto.ObjectId.Equals(c.Id))
                    {
                        if (lto.TrainingModule.TrainingModuleDocument == null)
                        {
                            #region Документ не создан

                            var html_ = string.Copy(lto.TrainingModule.DocumentHtml);
                            var mlp_ = new MlParser();
                            string searchString_;
                            string innerHtml;

                            searchString_ = lto.ObjectId.ToString();
                            while (html_.Contains(searchString_))
                            {
                                mlp_.GetTagBounds(html_, searchString_);
                                mlp_.ShiftLastIndex(ref html_);

                                innerHtml = mlp_.GetInnerHtml();
                                html_ = html_.Remove(mlp_.StartIndex, mlp_.LastIndex - mlp_.StartIndex + 1);
                                html_ = html_.Insert(mlp_.StartIndex, innerHtml);
                            }

                            lto.TrainingModule.DocumentHtml = html_;

                            #endregion
                        }
                        else
                        {
                            #region Документ создан

                            var html_ = string.Copy(lto.TrainingModule.TrainingModuleDocument.HtmlEditingTool.BodyInnerHtml);
                            var mlp_ = new MlParser();
                            string searchString_;
                            string innerHtml;

                            searchString_ = lto.ObjectId.ToString();
                            while (html_.Contains(searchString_))
                            {
                                mlp_.GetTagBounds(html_, searchString_);
                                mlp_.ShiftLastIndex(ref html_);

                                innerHtml = mlp_.GetInnerHtml();
                                html_ = html_.Remove(mlp_.StartIndex, mlp_.LastIndex - mlp_.StartIndex + 1);
                                html_ = html_.Insert(mlp_.StartIndex, innerHtml);
                            }

                            lto.TrainingModule.TrainingModuleDocument.HtmlEditingTool.BodyInnerHtml = html_;

                            #endregion
                        }
                    }
                }
            }

            if (ct.Nodes.Count.Equals(0))
            {
                ct.CurrentNode = null;
            }

            Warehouse.Warehouse.IsProjectModified = true;
        }
    }
}