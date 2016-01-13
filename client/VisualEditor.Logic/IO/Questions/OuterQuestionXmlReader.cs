using System;
using System.Xml;
using VisualEditor.Logic.Course.Items;
using VisualEditor.Logic.Course.Items.Questions;

namespace VisualEditor.Logic.IO.Questions
{
    internal class OuterQuestionXmlReader : QuestionXmlReader
    {
        public OuterQuestionXmlReader(Question question)
        {
            this.question = question;
        }

        public override void ReadXml(XmlTextReader xmlReader)
        {
            var q = question as OuterQuestion;
            var endCycle = false;

            while (!endCycle && xmlReader.Read())
            {
                if (xmlReader.NodeType == XmlNodeType.Element)
                {
                    if (xmlReader.Name.Equals("wsdl", StringComparison.OrdinalIgnoreCase))
                    {
                        q.Url = xmlReader.GetAttribute("address");
                        q.TestId = xmlReader.GetAttribute("testid");
                    }
                    else if (xmlReader.Name.Equals("task", StringComparison.OrdinalIgnoreCase))
                    {
                        q.TaskId = xmlReader.GetAttribute("id");
                        q.TaskName = xmlReader.GetAttribute("name");
                        q.TestName = xmlReader.GetAttribute("testname");
                        q.SubjectName = xmlReader.GetAttribute("subject");
                    }
                    else if (xmlReader.Name.Equals("declaration", StringComparison.OrdinalIgnoreCase))
                    {
                        var s = xmlReader.ReadElementString();
                        if (s != null)
                        {
                            q.Declaration = s;
                            question.DocumentHtml = XmlToHtml(s);
                        }
                    }
                    else if (xmlReader.Name.Equals("mark", StringComparison.OrdinalIgnoreCase))
                    {
                        q.Marks = int.Parse(xmlReader.GetAttribute("value"));

                        try
                        {
                            var id = new Guid(xmlReader.GetAttribute("concept_id").Substring(6, 36));

                            foreach (Concept c in Warehouse.Warehouse.Instance.ConceptTree.Nodes)
                            {
                                if (c.Id.Equals(id))
                                {
                                    question.Profile = c;

                                    break;
                                }
                            }
                        }
                        catch
                        {
                            question.Profile = null;
                        }
                    }
                }
                else if (xmlReader.NodeType == XmlNodeType.EndElement)
                {
                    if (xmlReader.Name.Equals("question", StringComparison.OrdinalIgnoreCase))
                    {
                        endCycle = true;
                    }
                }
            }
        }
    }
}