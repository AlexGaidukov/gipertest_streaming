using System.Xml;
using VisualEditor.Logic.Course.Items;
using VisualEditor.Logic.Course.Items.Questions;


namespace VisualEditor.Logic.IO.Questions
{
    internal class OuterQuestionXmlWriter : QuestionXmlWriter
    {
        public OuterQuestionXmlWriter(Question question)
        {
            this.question = question;
        }

        public override void WriteXml(XmlTextWriter xmlWriter)
        {
            var q = question as OuterQuestion;

            xmlWriter.WriteStartElement("question");
            xmlWriter.WriteAttributeString("name", q.Text);
            xmlWriter.WriteAttributeString("id", "#module{" + q.Id.ToString().ToUpper() + "}");
            if (q.NextQuestion != null)
            {
                xmlWriter.WriteAttributeString("next_question", "#module{" + q.NextQuestion.Id.ToString().ToUpper() + "}");
            }
            xmlWriter.WriteAttributeString("type", "outer");
            xmlWriter.WriteAttributeString("time", q.TimeRestriction.ToString());

            #region Wsdl

            xmlWriter.WriteStartElement("wsdl");
            xmlWriter.WriteAttributeString("address", q.Url);
            xmlWriter.WriteAttributeString("testid", q.TestId);
            xmlWriter.WriteEndElement();

            #endregion

            #region Task

            xmlWriter.WriteStartElement("task");
            xmlWriter.WriteAttributeString("id", q.TaskId);
            xmlWriter.WriteAttributeString("name", q.TaskName);
            xmlWriter.WriteAttributeString("subject", q.SubjectName);
            xmlWriter.WriteAttributeString("testname", q.TestName);
            xmlWriter.WriteStartElement("declaration");
            xmlWriter.WriteCData(q.DocumentHtml);
            xmlWriter.WriteEndElement();
            xmlWriter.WriteEndElement();

            #endregion

            if (!(question.Parent is Group))
            {
                xmlWriter.WriteStartElement("mark");
                xmlWriter.WriteAttributeString("value", q.Marks.ToString());
                if (question.Profile != null)
                {
                    xmlWriter.WriteAttributeString("concept_id", "#elem{" + question.Profile.Id.ToString().ToUpper() + "}");
                }
                xmlWriter.WriteFullEndElement();
            }

            xmlWriter.WriteEndElement();
        }
    }
}