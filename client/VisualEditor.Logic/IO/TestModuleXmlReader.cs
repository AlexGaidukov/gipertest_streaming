using System;
using System.Xml;
using VisualEditor.Logic.Course.Items;
using VisualEditor.Logic.Course.Items.Questions;
using VisualEditor.Utils.ExceptionHandling;

namespace VisualEditor.Logic.IO
{
    internal class TestModuleXmlReader
    {
        private readonly TestModule testModule;

        public TestModuleXmlReader(TestModule testModule)
        {
            this.testModule = testModule;
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
                        if (xmlReader.Name.Equals("group", StringComparison.OrdinalIgnoreCase))
                        {
                            #region Группа

                            var g = new Group();

                            if (xmlReader.GetAttribute("name") != null)
                            {
                                g.Text = xmlReader.GetAttribute("name");
                            }
                            else
                            {
                                g.Text = string.Concat("Группа ", testModule.Groups.Count + 1);
                            }

                            g.ChosenQuestionsCount = int.Parse(xmlReader.GetAttribute("count"));
                            g.XmlReader.ReadXml(xmlReader);
                            testModule.Nodes.Add(g);

                            #endregion
                        }

                        if (xmlReader.Name.Equals("question", StringComparison.OrdinalIgnoreCase))
                        {
                            // Название вопроса.
                            var qname = xmlReader.GetAttribute("name");
                            // Идентификатор вопроса.
                            var qid = xmlReader.GetAttribute("id").Substring(8, 36);
                            // Тип вопроса.
                            var qtype = xmlReader.GetAttribute("type");
                            // Ограничение по времени на прохождение вопроса.
                            var qtime = xmlReader.GetAttribute("time");
                            // Идентификатор следующего вопроса.
                            var qNextQuestion = xmlReader.GetAttribute("next_question");
                            if (!string.IsNullOrEmpty(qNextQuestion))
                            {
                                qNextQuestion = qNextQuestion.Substring(8, 36);
                            }
                            
                            if (qtype.Equals("radio", StringComparison.OrdinalIgnoreCase))
                            {
                                #region Вопросы Одновариантный выбор

                                var q = new ChoiceQuestion
                                {
                                    Text = qname
                                };

                                try
                                {
                                    q.Id = new Guid(qid);
                                }
                                catch
                                {
                                    //q.Identifier = qid;
                                }

                                q.TimeRestriction = int.Parse(qtime);
                                if (!string.IsNullOrEmpty(qNextQuestion))
                                {
                                    // q_ - временное хранилище для идентификатора следующего вопроса.
                                    // После чтения всего контроля, q.NextQuestion заменяется на реальный вопрос.
                                    var q_ = new ChoiceQuestion
                                    {
                                        Id = new Guid(qNextQuestion)
                                    };
                                    q.NextQuestion = q_;
                                }
                                q.XmlReader.ReadXml(xmlReader);
                                testModule.Nodes.Add(q);

                                #endregion
                            }
                            else if (qtype.Equals("checkbox", StringComparison.OrdinalIgnoreCase))
                            {
                                #region Вопросы Множественный выбор

                                var q = new MultichoiceQuestion
                                {
                                    Text = qname
                                };

                                try
                                {
                                    q.Id = new Guid(qid);
                                }
                                catch
                                {
                                    //mq.Identifier = qid;
                                }

                                q.TimeRestriction = int.Parse(qtime);
                                if (!string.IsNullOrEmpty(qNextQuestion))
                                {
                                    // q_ - временное хранилище для идентификатора следующего вопроса.
                                    // После чтения всего контроля, q.NextQuestion заменяется на реальный вопрос.
                                    var q_ = new ChoiceQuestion
                                    {
                                        Id = new Guid(qNextQuestion)
                                    };
                                    q.NextQuestion = q_;
                                }
                                q.XmlReader.ReadXml(xmlReader);
                                testModule.Nodes.Add(q);

                                #endregion
                            }
                            else if (qtype.Equals("order", StringComparison.OrdinalIgnoreCase))
                            {
                                #region Вопросы Ранжирование

                                var q = new OrderingQuestion
                                {
                                    Text = qname
                                };

                                try
                                {
                                    q.Id = new Guid(qid);
                                }
                                catch
                                {
                                    //mq.Identifier = qid;
                                }

                                q.TimeRestriction = int.Parse(qtime);
                                if (!string.IsNullOrEmpty(qNextQuestion))
                                {
                                    // q_ - временное хранилище для идентификатора следующего вопроса.
                                    // После чтения всего контроля, q.NextQuestion заменяется на реальный вопрос.
                                    var q_ = new ChoiceQuestion
                                    {
                                        Id = new Guid(qNextQuestion)
                                    };
                                    q.NextQuestion = q_;
                                }
                                q.XmlReader.ReadXml(xmlReader);
                                testModule.Nodes.Add(q);

                                #endregion
                            }
                            else if (qtype.Equals("edit", StringComparison.OrdinalIgnoreCase) ||
                                qtype.Equals("explicit", StringComparison.OrdinalIgnoreCase))
                            {
                                #region Вопросы Открытый вопрос

                                var q = new OpenQuestion
                                {
                                    Text = qname
                                };

                                if (qtype.Equals("edit", StringComparison.OrdinalIgnoreCase))
                                {
                                    q.IsAnswerNumeric = false;
                                }
                                else if (qtype.Equals("explicit", StringComparison.OrdinalIgnoreCase))
                                {
                                    q.IsAnswerNumeric = true;
                                }

                                try
                                {
                                    q.Id = new Guid(qid);
                                }
                                catch
                                {
                                    //mq.Identifier = qid;
                                }

                                q.TimeRestriction = int.Parse(qtime);
                                if (!string.IsNullOrEmpty(qNextQuestion))
                                {
                                    // q_ - временное хранилище для идентификатора следующего вопроса.
                                    // После чтения всего контроля, q.NextQuestion заменяется на реальный вопрос.
                                    var q_ = new ChoiceQuestion
                                    {
                                        Id = new Guid(qNextQuestion)
                                    };
                                    q.NextQuestion = q_;
                                }
                                q.XmlReader.ReadXml(xmlReader);
                                testModule.Nodes.Add(q);

                                #endregion
                            }
                            else if (qtype.Equals("outer", StringComparison.OrdinalIgnoreCase))
                            {
                                #region Вопросы Внешний вопрос

                                var q = new OuterQuestion
                                {
                                    Text = qname
                                };

                                try
                                {
                                    q.Id = new Guid(qid);
                                }
                                catch
                                {
                                    //q.Identifier = qid;
                                }

                                q.TimeRestriction = int.Parse(qtime);
                                if (!string.IsNullOrEmpty(qNextQuestion))
                                {
                                    // q_ - временное хранилище для идентификатора следующего вопроса.
                                    // После чтения всего контроля, q.NextQuestion заменяется на реальный вопрос.
                                    var q_ = new ChoiceQuestion
                                    {
                                        Id = new Guid(qNextQuestion)
                                    };
                                    q.NextQuestion = q_;
                                }
                                q.XmlReader.ReadXml(xmlReader);
                                testModule.Nodes.Add(q);

                                #endregion
                            }
                        }
                    }
                    else if (xmlReader.NodeType == XmlNodeType.EndElement)
                    {
                        if (xmlReader.Name.Equals("module", StringComparison.OrdinalIgnoreCase))
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