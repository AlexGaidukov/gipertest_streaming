using System;
using System.Xml;
using VisualEditor.Logic.Course.Items;
using VisualEditor.Logic.Course.Items.Questions;
using VisualEditor.Utils.ExceptionHandling;

namespace VisualEditor.Logic.IO
{
    internal class GroupXmlReader
    {
        private readonly Group group;

        public GroupXmlReader(Group group)
        {
            this.group = group;
        }

        public void ReadXml(XmlTextReader xmlReader)
        {
            try
            {
                var isEndCycle = false;

                group.ChosenQuestionsCount = int.Parse(xmlReader.GetAttribute("count"));

                while (!isEndCycle && xmlReader.Read())
                {
                    if (xmlReader.NodeType == XmlNodeType.Element)
                    {
                        if (xmlReader.Name.Equals("question", StringComparison.OrdinalIgnoreCase))
                        {
                            var qname = xmlReader.GetAttribute("name");
                            var qid = xmlReader.GetAttribute("id").Substring(8, 36);
                            var qtype = xmlReader.GetAttribute("type");
                            var qtime = xmlReader.GetAttribute("time");
                            group.TimeRestriction = int.Parse(qtime);

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
                                q.XmlReader.ReadXml(xmlReader);
                                group.Nodes.Add(q);

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
                                q.XmlReader.ReadXml(xmlReader);
                                group.Nodes.Add(q);

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
                                q.XmlReader.ReadXml(xmlReader);
                                group.Nodes.Add(q);

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
                                q.XmlReader.ReadXml(xmlReader);
                                group.Nodes.Add(q);

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
                                q.XmlReader.ReadXml(xmlReader);
                                group.Nodes.Add(q);

                                #endregion
                            }
                        }

                        if (xmlReader.Name.Equals("mark", StringComparison.OrdinalIgnoreCase))
                        {
                            group.Marks = int.Parse(xmlReader.GetAttribute("value"));

                            try
                            {
                                var id = new Guid(xmlReader.GetAttribute("concept_id").Substring(6, 36));

                                foreach (Concept c in Warehouse.Warehouse.Instance.ConceptTree.Nodes)
                                {
                                    if (c.Id.Equals(id))
                                    {
                                        group.Profile = c;

                                        break;
                                    }
                                }
                            }
                            catch
                            {
                                group.Profile = null;
                            }

                            foreach (var q in group.Questions)
                            {
                                q.Marks = group.Marks;
                                q.Profile = group.Profile;
                            }
                        }
                    }
                    else if (xmlReader.NodeType == XmlNodeType.EndElement)
                    {
                        if (xmlReader.Name.Equals("group", StringComparison.OrdinalIgnoreCase))
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