using System;
using System.Xml;
using VisualEditor.Logic.Course.Items;
using VisualEditor.Logic.Course.Items.Questions;
using VisualEditor.Logic.IO.Questions;
using VisualEditor.Utils.ExceptionHandling;

namespace VisualEditor.Logic.IO
{
    internal class ResponseVariantXmlReader
    {
        private Question question;
        private ResponseVariant responseVariant;

        public ResponseVariantXmlReader(Question question, ResponseVariant responseVariant)
        {
            this.question = question;
            this.responseVariant = responseVariant;
        }

        public void ReadXml(XmlTextReader xmlReader)
        {
            try
            {
                var isEndCycle = false;

                while (!isEndCycle && xmlReader.Read())
                {
                    if (xmlReader.NodeType == XmlNodeType.Element)
                    {
                        if (xmlReader.Name.Equals("checked"))
                        {
                            foreach (var response in question.Responses)
                            {
                                if (response.NativeId.Equals(xmlReader.GetAttribute("answer_id")))
                                {
                                    // если вопрос типа Ordering, то в вариант ответа записываем порядок следования элементов ответа
                                    if (question is OrderingQuestion)
                                    {
                                        int place = 0;

                                        foreach (Response r in question.Responses)
                                        {
                                            responseVariant.Responses.Add(0);
                                        }

                                        if (int.TryParse(xmlReader.GetAttribute("value"), out place))
                                        {
                                            responseVariant.Responses[question.Responses.IndexOf(response)] = place;
                                        }
                                    }
                                    // иначе в вариант ответа включаем только правильные элементы ответа
                                    else
                                    {
                                        responseVariant.Responses.Add(response);
                                    }
                                }
                            }
                        }

                        if (xmlReader.Name.Equals("hint", StringComparison.OrdinalIgnoreCase))
                        {
                            try
                            {
                                responseVariant.Hint = QuestionXmlReader.XmlToHtml(xmlReader.ReadString());
                            }
                            catch { }
                        }
                    }
                    else if (xmlReader.NodeType == XmlNodeType.EndElement)
                    {
                        if (xmlReader.Name.ToLower().Equals("answer_variants"))
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