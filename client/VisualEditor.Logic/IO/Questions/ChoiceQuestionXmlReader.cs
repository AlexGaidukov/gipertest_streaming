using System;
using System.Globalization;
using System.Xml;
using VisualEditor.Logic.Course.Items;
using VisualEditor.Utils.ExceptionHandling;

namespace VisualEditor.Logic.IO.Questions
{
    internal class ChoiceQuestionXmlReader : QuestionXmlReader
    {
        public ChoiceQuestionXmlReader(Question question)
        {
            this.question = question;
        }

        public override void ReadXml(XmlTextReader xmlReader)
        {
            try
            {
                var isEndCycle = false;

                while (!isEndCycle && xmlReader.Read())
                {
                    if (xmlReader.NodeType == XmlNodeType.Element)
                    {
                        #region Контент вопроса

                        if (xmlReader.Name.Equals("html_text", StringComparison.OrdinalIgnoreCase))
                        {
                            var s = xmlReader.ReadElementString();
                            if (s != null)
                            {
                                question.DocumentHtml = XmlToHtml(s);
                            }
                        }

                        #endregion

                        #region Ответ

                        if (xmlReader.Name.Equals("answer", StringComparison.OrdinalIgnoreCase))
                        {
                            var r = new Response
                                        {
                                            Text = string.Concat("Ответ ", question.Responses.Count + 1)
                                        };

                            r.XmlReader.ReadXml(xmlReader);
                            question.Nodes.Add(r);

                            try
                            {
                                //r.Id = new Guid(xmlReader.GetAttribute("id"));
                                r.Id = Guid.NewGuid();
                                r.NativeId = string.Concat("a", question.Responses.Count);
                            }
                            catch
                            {
                                //r.Identifier = reader.GetAttribute("id");
                            }
                        }

                        #endregion

                        #region Варианты ответа

                        if (xmlReader.Name.Equals("answer_variants", StringComparison.OrdinalIgnoreCase))
                        {
                            var rv = new ResponseVariant(question);

                            //rv.Type = xmlReader.GetAttribute("type");
                            var weight = xmlReader.GetAttribute("weight");
                            weight = weight.Replace(".", NumberFormatInfo.CurrentInfo.CurrencyDecimalSeparator);
                            weight = weight.Replace(",", NumberFormatInfo.CurrentInfo.CurrencyDecimalSeparator);
                            rv.Weight = double.Parse(weight);
                           

                            var gh = xmlReader.GetAttribute("next_question");


                            if (xmlReader.GetAttribute("next_question") != null)
                            {

                                rv.NextQuestion = xmlReader.GetAttribute("next_question");

                            }

                            rv.XmlReader.ReadXml(xmlReader);
                            question.ResponseVariants.Add(rv);
                        }

                        #endregion

                        #region Подсказка

                        if (xmlReader.Name.Equals("hint", StringComparison.OrdinalIgnoreCase))
                        {
                            try
                            {
                                string s = xmlReader.ReadString();
                                if (s != null)
                                {
                                    question.Hint = XmlToHtml(s);
                                }
                            }
                            catch { }
                        }

                        #endregion

                        if (xmlReader.Name.Equals("mark", StringComparison.OrdinalIgnoreCase))
                        {
                            question.Marks = int.Parse(xmlReader.GetAttribute("value"));

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
                        if (xmlReader.Name.ToLower().Equals("question", StringComparison.OrdinalIgnoreCase))
                        {
                            isEndCycle = true;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ExceptionManager.Instance.LogException(ex);
            }
        }
    }
}