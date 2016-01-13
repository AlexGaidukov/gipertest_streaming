using System;
using System.Xml;
using VisualEditor.Logic.Course.Items;
using VisualEditor.Logic.Course.Items.Questions;

namespace VisualEditor.Logic.IO.Questions
{
    internal class OpenQuestionXmlWriter : QuestionXmlWriter
    {
        public OpenQuestionXmlWriter(Question question)
        {
            this.question = question;
        }

        public override void WriteXml(XmlTextWriter xmlWriter)
        {
            var q = question as OpenQuestion;

            xmlWriter.WriteStartElement("question");
            xmlWriter.WriteAttributeString("name", q.Text);
            xmlWriter.WriteAttributeString("id", "#module{" + q.Id.ToString().ToUpper() + "}");
            if (question.NextQuestion != null)
            {
                xmlWriter.WriteAttributeString("next_question", "#module{" + question.NextQuestion.Id.ToString().ToUpper() + "}");
            }
            // определяем, является ли ответ строкой или числом
            if (q.IsAnswerNumeric)
            {
                xmlWriter.WriteAttributeString("type", "explicit");
            }
            else
            {
                xmlWriter.WriteAttributeString("type", "edit");
            }
            xmlWriter.WriteAttributeString("time", q.TimeRestriction.ToString());

            #region Контент вопроса

            xmlWriter.WriteStartElement("html_text");
            xmlWriter.WriteAttributeString("id", "{" + Guid.NewGuid().ToString().ToUpper() + "}");
            xmlWriter.WriteCData(HtmlToXml(question.DocumentHtml));
            xmlWriter.WriteFullEndElement();

            #endregion

            #region Варианты ответа

            if (question.ResponseVariants != null)
            {
                foreach (ResponseVariant rv in question.ResponseVariants)
                {
                    xmlWriter.WriteStartElement("answer_variants");
                    xmlWriter.WriteAttributeString("type", "nonstrict");
                    xmlWriter.WriteAttributeString("weight", rv.Weight.ToString().Replace(",", "."));

                    #region Следующий вопрос в зависимости от варианта ответа!!!!!!!!!!!!!!!!!!!!
                    //var tm = Warehouse.Warehouse.Instance.CourseTree.CurrentNode.Parent as TestModule;
                     var tm = this.question.Parent as TestModule;
                     if (tm != null)
                     {
                         foreach (var question1 in tm.Questions)

                             if (question1.Text == rv.NextQuestion || "#module{" + question1.Id.ToString().ToUpper() + "}" == rv.NextQuestion)
                             {
                                 xmlWriter.WriteAttributeString("next_question", "#module{" + question1.Id.ToString().ToUpper() + "}");
                                 //xmlWriter.WriteAttributeString("next_question", question1.Text);
                             }
                     }
                    #endregion


                    xmlWriter.WriteAttributeString("value", rv.Responses[0].ToString());
                    xmlWriter.WriteString("");
                    if (!string.IsNullOrEmpty(rv.Hint))
                    {
                        xmlWriter.WriteStartElement("hint");
                        xmlWriter.WriteAttributeString("id", Guid.NewGuid().ToString());
                        xmlWriter.WriteCData(HtmlToXml(rv.Hint));
                        xmlWriter.WriteFullEndElement();
                    }
                    xmlWriter.WriteFullEndElement();
                }
            }
            else
            {
                xmlWriter.WriteStartElement("answer_variants");
                xmlWriter.WriteAttributeString("type", "nonstrict");
                xmlWriter.WriteAttributeString("weight", "0");
                xmlWriter.WriteFullEndElement();
            }

            #endregion

            #region Подсказка

            xmlWriter.WriteStartElement("hint");
            xmlWriter.WriteAttributeString("id", Guid.NewGuid().ToString());
            xmlWriter.WriteCData(HtmlToXml(question.Hint));
            xmlWriter.WriteFullEndElement();

            #endregion

            if (!(question.Parent is Group))
            {
                xmlWriter.WriteStartElement("mark");
                xmlWriter.WriteAttributeString("value", question.Marks.ToString());
                if (question.Profile != null)
                {
                    xmlWriter.WriteAttributeString("concept_id", "#elem{" + question.Profile.Id.ToString().ToUpper() + "}");
                }
                xmlWriter.WriteFullEndElement();
            }
            xmlWriter.WriteFullEndElement();
        }
    }
}