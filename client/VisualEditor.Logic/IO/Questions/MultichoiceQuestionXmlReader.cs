using System;
using System.Globalization;
using System.Xml;
using VisualEditor.Logic.Course.Items;
using VisualEditor.Utils.ExceptionHandling;

namespace VisualEditor.Logic.IO.Questions
{
    internal class MultichoiceQuestionXmlReader : QuestionXmlReader
    {
        public MultichoiceQuestionXmlReader(Question question)
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

                        if (xmlReader.Name.Equals("html_text"))
                        {
                            var s = xmlReader.ReadElementString();
                            if (s != null)
                            {
                                question.DocumentHtml = XmlToHtml(s);
                            }
                        }

                        #endregion

                        #region Ответ

                        if (xmlReader.Name.Equals("answer"))
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

                        if (xmlReader.Name.Equals("answer_variants"))
                        {
                            
                            question.ResponseVariants.Add(new ResponseVariant(question));
                            var weight = xmlReader.GetAttribute("weight");
                            
                            var NQ = "";

                            if (xmlReader.GetAttribute("next_question") != null)
                            {

                                NQ = xmlReader.GetAttribute("next_question");
                            }

                            weight = weight.Replace(".", NumberFormatInfo.CurrentInfo.CurrencyDecimalSeparator);
                            weight = weight.Replace(",", NumberFormatInfo.CurrentInfo.CurrencyDecimalSeparator);
                            ((ResponseVariant)question.ResponseVariants[question.ResponseVariants.Count - 1]).Weight = double.Parse(weight);

                            ((ResponseVariant)question.ResponseVariants[question.ResponseVariants.Count - 1]).NextQuestion = NQ;

                            ((ResponseVariant)question.ResponseVariants[question.ResponseVariants.Count - 1]).XmlReader.ReadXml(xmlReader);
                        }

                        #endregion

                        #region Подсказка

                        if (xmlReader.Name.Equals("hint", StringComparison.OrdinalIgnoreCase))
                        {
                            try
                            {
                                var s = xmlReader.ReadString();
                                if (s != null)
                                {
                                    question.Hint = XmlToHtml(s);
                                }
                            }
                            catch { }
                        }

                        #endregion

                        if (xmlReader.Name.Equals("mark"))
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
                        if (xmlReader.Name.ToLower().Equals("question"))
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