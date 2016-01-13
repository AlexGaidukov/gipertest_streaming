using System;
using VisualEditor.Logic.Controls.Docking;
using VisualEditor.Logic.Controls.Docking.Documents;
using VisualEditor.Logic.Course.Items;
using VisualEditor.Logic.Warehouse;
using HtmlEditingToolHelper=VisualEditor.Logic.Controls.HtmlEditing.HtmlEditingToolHelper;

namespace VisualEditor.Logic.Commands.Course
{
    // POSTPONE: Когда редактор открывается посредством двойного клика по htp файлу,
    // документ переводится в режим кода и осуществляется навигация
    // по дереву учебного курса, возникает проблема.
    internal class ViewDocument : AbstractCommand
    {
        public ViewDocument()
        {
            name = CommandNames.ViewDocument;
            text = CommandTexts.ViewDocument;
        }

        public override void Execute(object @object)
        {
            if (!Enabled)
            {
                return;
            }

            var cn = Warehouse.Warehouse.Instance.CourseTree.CurrentNode;

            if (cn is TrainingModule)
            {
                var tm = cn as TrainingModule;
                var b = ShowTrainingModuleDocument(tm.Id);

                if (!b)
                {
                    var tmd = CreateTrainingModuleDocument(tm);
                    PreviewObserver.AddDocument(tmd);
                }
            }

            if (cn is Question)
            {
                var q = cn as Question;
                var b = ShowQuestionDocument(q.Id);

                if (!b)
                {
                    var qd = CreateQuestionDocument(q);
                    PreviewObserver.AddDocument(qd);
                }
            }

            if (cn is Response)
            {
                var r = cn as Response;
                var b = ShowResponseDocument(r.Id);

                if (!b)
                {
                    var rd = CreateResponseDocument(r);
                    PreviewObserver.AddDocument(rd);
                }
            }
        }

        #region Учебный модуль

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

        private static TrainingModuleDocument CreateTrainingModuleDocument(TrainingModule tm)
        {
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
            tm.TrainingModuleDocument.HtmlEditingTool.SetDefaultHtml();
            // Блокирует команду Undo.
            //tm.TrainingModuleDocument.HtmlEditingTool.Mode = Utils.Controls.HtmlEditing.Enums.HtmlEditingToolMode.Preview;
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

        #endregion

        #region Вопрос

        private static bool ShowQuestionDocument(Guid id)
        {
            var dc = DockContainer.Instance;

            foreach (var d in dc.QuestionDocuments)
            {
                if (d.Question.Id.Equals(id))
                {
                    d.Show();

                    return true;
                }
            }

            return false;
        }

        private static QuestionDocument CreateQuestionDocument(Question q)
        {
            q.QuestionDocument = new QuestionDocument
                                     {
                                         Question = q,
                                         Text = q.Text,
                                         HtmlEditingTool =
                                             {Mode = Utils.Controls.HtmlEditing.Enums.HtmlEditingToolMode.Design}
                                     };

            //q.QuestionDocument.VisualHtmlEditor.SetDefaultFont();
            HtmlEditingToolHelper.SetDefaultDocumentHtml(q.QuestionDocument.HtmlEditingTool);
            q.QuestionDocument.HtmlEditingTool.BodyInnerHtml = q.DocumentHtml;
            q.QuestionDocument.HtmlEditingTool.SetDefaultHtml();
            // Блокирует команду Undo.
            //q.QuestionDocument.HtmlEditingTool.Mode = Utils.Controls.HtmlEditing.Enums.HtmlEditingToolMode.Preview;
            q.QuestionDocument.HtmlEditingTool.Mode = Utils.Controls.HtmlEditing.Enums.HtmlEditingToolMode.Design;
            q.QuestionDocument.Show();

            if (q.Text.Length > 22)
            {
                q.QuestionDocument.Text = q.Text.Substring(0, 22) + "...";
            }
            else
            {
                q.QuestionDocument.Text = q.Text;
            }

            return q.QuestionDocument;
        }

        #endregion

        #region Элемент ответа

        private static bool ShowResponseDocument(Guid id)
        {
            var dc = DockContainer.Instance;

            foreach (var d in dc.ResponseDocuments)
            {
                if (d.Response.Id.Equals(id))
                {
                    d.Show();

                    return true;
                }
            }

            return false;
        }

        private static ResponseDocument CreateResponseDocument(Response r)
        {
            r.ResponseDocument = new ResponseDocument
                                     {
                                         Response = r,
                                         Text = r.Text,
                                         HtmlEditingTool =
                                             {Mode = Utils.Controls.HtmlEditing.Enums.HtmlEditingToolMode.Design}
                                     };

            //r.ResponseDocument.VisualHtmlEditor.SetDefaultFont();
            HtmlEditingToolHelper.SetDefaultDocumentHtml(r.ResponseDocument.HtmlEditingTool);
            r.ResponseDocument.HtmlEditingTool.BodyInnerHtml = r.DocumentHtml;
            r.ResponseDocument.HtmlEditingTool.SetDefaultHtml();
            // Блокирует команду Undo.
            //r.ResponseDocument.HtmlEditingTool.Mode = Utils.Controls.HtmlEditing.Enums.HtmlEditingToolMode.Preview;
            r.ResponseDocument.HtmlEditingTool.Mode = Utils.Controls.HtmlEditing.Enums.HtmlEditingToolMode.Design;
            r.ResponseDocument.Show();

            if (r.Text.Length > 22)
            {
                r.ResponseDocument.Text = r.Text.Substring(0, 22) + "...";
            }
            else
            {
                r.ResponseDocument.Text = r.Text;
            }

            return r.ResponseDocument;
        }

        #endregion
    }
}