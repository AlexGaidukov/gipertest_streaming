using System.Collections.Generic;
using System.Windows.Forms;
using VisualEditor.Logic.Course.Items;
using VisualEditor.Logic.IO;

namespace VisualEditor.Logic.Commands.Course
{
    internal class DeleteItem : AbstractCommand
    {
        private const string deleteTrainingModuleMessage = "Вы уверены, что хотите удалить учебный модуль?\nВсе компетенции и закладки этого модуля и подмодулей,\nа также ссылки на них будут удалены.";
        private const string deleteTestModuleMessage = "Вы уверены, что хотите удалить контроль?";
        private const string deleteGroupMessage = "Вы уверены, что хотите удалить группу?";
        private const string deleteQuestionMessage = "Вы уверены, что хотите удалить вопрос?";

        public DeleteItem()
        {
            name = CommandNames.DeleteItem;
            text = CommandTexts.DeleteItem;
            image = Properties.Resources.DeleteItem;
        }

        public override void Execute(object @object)
        {
            if (!Enabled)
            {
                return;
            }

            var cn = Warehouse.Warehouse.Instance.CourseTree.CurrentNode;

            #region Удаление учебного модуля

            if (cn is TrainingModule)
            {
                if (MessageBox.Show(deleteTrainingModuleMessage, 
                    System.Windows.Forms.Application.ProductName, 
                    MessageBoxButtons.OKCancel, MessageBoxIcon.Question) != DialogResult.OK)
                {
                    return;
                }

                DeleteBranch(cn.Nodes);
                DeleteTrainingModule(cn as TrainingModule);
                cn.Parent.Nodes.Remove(cn);
            }

            #endregion

            #region Удаление контроля

            if (cn is TestModule)
            {
                if (MessageBox.Show(deleteTestModuleMessage,
                    System.Windows.Forms.Application.ProductName, 
                    MessageBoxButtons.OKCancel, MessageBoxIcon.Question) != DialogResult.OK)
                {
                    return;
                }

                cn.Parent.Nodes.Remove(cn);
            }

            #endregion

            #region Удаление группы

            if (cn is Group)
            {
                if (MessageBox.Show(deleteGroupMessage, System.Windows.Forms.Application.ProductName, 
                    MessageBoxButtons.OKCancel, MessageBoxIcon.Question) != DialogResult.OK)
                {
                    return;
                }

                cn.Parent.Nodes.Remove(cn);
            }

            #endregion

            #region Удаление вопроса

            if (cn is Question)
            {
                if (MessageBox.Show(deleteQuestionMessage, System.Windows.Forms.Application.ProductName, 
                    MessageBoxButtons.OKCancel, MessageBoxIcon.Question) != DialogResult.OK)
                {
                    return;
                }

                if (cn.Parent is Group)
                {
                    var g = cn.Parent as Group;

                    if (!g.ChosenQuestionsCount.Equals(0))
                    {
                        if (g.ChosenQuestionsCount > g.Questions.Count - 1)
                        {
                            g.ChosenQuestionsCount = g.Questions.Count - 1;
                        }
                    }

                    // Если удаляется последний вопрос в группе, параметры группы обнуляются.
                    if (g.Questions.Count.Equals(1))
                    {
                        g.TimeRestriction = 0;
                        g.Profile = null;
                        g.Marks = 0;
                    }
                }

                cn.Parent.Nodes.Remove(cn);

                var q = cn as Question;
                if (q.QuestionDocument != null)
                {
                    q.QuestionDocument.Hide();
                }
            }

            #endregion

            #region Удаление ответа

            if (cn is Response)
            {
                cn.Parent.Nodes.Remove(cn);

                var r = cn as Response;
                if (r.ResponseDocument != null)
                {
                    r.ResponseDocument.Hide();
                }
            }

            #endregion

            #region Удаление компетенции из входа

            if (cn is InDummyConcept)
            {
                cn.Parent.Nodes.Remove(cn);
            }

            #endregion

            Warehouse.Warehouse.IsProjectModified = true;
        }

        #region Удаление учебного модуля и всех подмодулей

        private static void DeleteBranch(TreeNodeCollection tnc)
        {
            for (var i = 0; i < tnc.Count; i++)
            {
                var tn = tnc[i];
                if (tn.Nodes.Count != 0)
                {
                    DeleteBranch(tnc[i].Nodes);
                }

                if (tn is TrainingModule)
                {
                    var tm = tn as TrainingModule;

                    DeleteTrainingModule(tm);
                }

                if (tn is Question)
                {
                    var q = tn as Question;

                    if (q.QuestionDocument != null)
                    {
                        q.QuestionDocument.Hide();
                    }
                }

                if (tn is Response)
                {
                    var r = tn as Response;

                    if (r.ResponseDocument != null)
                    {
                        r.ResponseDocument.Hide();
                    }
                }
            }
        }

        #endregion

        #region Удаление учебного модуля

        private static void DeleteTrainingModule(TrainingModule tm)
        {
            #region Ссылки на модули

            // Удаляет ссылки на модуль.
            foreach (var lto in Warehouse.Warehouse.Instance.LinksToObjects)
            {
                if (lto.ObjectId.Equals(tm.Id))
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

            #endregion

            #region Закладки и ссылки на закладки

            // Список закладок текущего учебного модуля.
            var bookmarks = new List<Bookmark>();

            foreach (var b in Warehouse.Warehouse.Instance.Bookmarks)
            {
                if (b.ModuleId.Equals(tm.Id))
                {
                    bookmarks.Add(b);
                }
            }

            foreach (var b in bookmarks)
            {
                // Удаляет закладку из списка закладок.
                Warehouse.Warehouse.Instance.Bookmarks.Remove(b);

                // Удаляет закладку из Html-кода.
                if (tm.TrainingModuleDocument == null)
                {
                    #region Документ не создан

                    var html = tm.DocumentHtml;
                    var mlp = new MlParser();
                    string searchString;
                    string value;
                    int index;

                    searchString = "class=bookmark";
                    index = 0;
                    while (html.Contains(searchString))
                    {
                        mlp.GetTagBounds(html, searchString, index);
                        value = mlp.GetValue("id");
                        mlp.ShiftLastIndex(ref html);

                        if (value.Equals(b.Id.ToString()))
                        {
                            html = html.Remove(mlp.StartIndex, mlp.LastIndex - mlp.StartIndex + 1);
                            index = mlp.StartIndex;
                        }
                        else
                        {
                            index = mlp.LastIndex;
                        }

                        if (index == mlp.StartIndex)
                        {
                            break;
                        }
                    }

                    tm.DocumentHtml = html;

                    #endregion
                }
                else
                {
                    #region Документ создан

                    var html = tm.TrainingModuleDocument.HtmlEditingTool.BodyInnerHtml;
                    var mlp = new MlParser();
                    string searchString;
                    string value;
                    int index;

                    searchString = "class=bookmark";
                    index = 0;
                    while (html.Contains(searchString))
                    {
                        mlp.GetTagBounds(html, searchString, index);
                        value = mlp.GetValue("id");
                        mlp.ShiftLastIndex(ref html);

                        if (value.Equals(b.Id.ToString()))
                        {
                            html = html.Remove(mlp.StartIndex, mlp.LastIndex - mlp.StartIndex + 1);
                            index = mlp.StartIndex;
                        }
                        else
                        {
                            index = mlp.LastIndex;
                        }

                        if (index == mlp.StartIndex)
                        {
                            break;
                        }
                    }

                    tm.TrainingModuleDocument.HtmlEditingTool.BodyInnerHtml = html;

                    #endregion
                }

                // Удаляет ссылки на закладку.
                foreach (var lto in Warehouse.Warehouse.Instance.LinksToObjects)
                {
                    if (lto.ObjectId.Equals(b.Id))
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

            #endregion

            #region Компетенции и ссылки на компетенции

            // Список компетенций текущего учебного модуля.
            var concepts = new List<Logic.Course.Items.Concept>();

            foreach (var odc in tm.OutConceptParent.OutDummyConcepts)
            {
                concepts.Add(odc.Concept);
            }

            foreach (var c in concepts)
            {
                // Удаляет компетенцию из дерева компетенций.
                Warehouse.Warehouse.Instance.ConceptTree.Nodes.Remove(c);

                if (Warehouse.Warehouse.Instance.ConceptTree.Nodes.Count.Equals(0))
                {
                    Warehouse.Warehouse.Instance.ConceptTree.CurrentNode = null;
                }

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
            }

            #endregion

            #region Ссылки

            // Список ссылок из текущего учебного модуля.
            var linksToObjects = new List<LinkToObject>();

            foreach (var lto in Warehouse.Warehouse.Instance.LinksToObjects)
            {
                if (lto.TrainingModule.Equals(tm))
                {
                    linksToObjects.Add(lto);
                }
            }

            foreach (var lto in linksToObjects)
            {
                Warehouse.Warehouse.Instance.LinksToObjects.Remove(lto);
            }

            #endregion

            Warehouse.Warehouse.Instance.TrainingModules.Remove(tm);

            if (tm.TrainingModuleDocument != null)
            {
                tm.TrainingModuleDocument.Hide();
            }
        }

        #endregion
    }
}