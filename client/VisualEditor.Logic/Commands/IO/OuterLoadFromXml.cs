using System;
using System.IO;
using System.Windows.Forms;
using System.Xml;
using VisualEditor.Logic.Course.Items;
using VisualEditor.Logic.Dialogs;

namespace VisualEditor.Logic.Commands.IO
{
    internal class OuterLoadFromXml : AbstractCommand
    {
        public OuterLoadFromXml()
        {
            name = CommandNames.OuterLoadFromXml;
            text = CommandTexts.OuterLoadFromXml;
        }

        public override void Execute(object @object)
        {
            if (!Enabled)
            {
                return;
            }

            var path = Path.Combine(Warehouse.Warehouse.OuterProjectEditorLocation, 
                Warehouse.Warehouse.OuterProjectArchiveName);

            #region Чтение структуры проекта, закладок, контента контролей

            var depth = 1;
            var ocst = AddItemFromOuterCourseDialog.OuterCourseTree;
            // Узел, в который добавляется следующий создаваемый узел.
            TreeNode activeNode = null;

            var xmlReader = new XmlTextReader(path);

            try
            {
                while (xmlReader.Read())
                {
                    System.Windows.Forms.Application.DoEvents();

                    if (xmlReader.NodeType == XmlNodeType.Element)
                    {
                        if (xmlReader.Name.Equals("project"))
                        {
                            #region Корень учебного курса

                            var cr = new CourseRoot
                                         {
                                             Text = xmlReader.GetAttribute("name")
                                         };

                            ocst.Nodes.Add(cr);
                            activeNode = cr;

                            #endregion
                        }
                        else if (xmlReader.Name.Equals("module"))
                        {
                            if (xmlReader.GetAttribute("type").Equals("test", StringComparison.OrdinalIgnoreCase) ||
                                     xmlReader.GetAttribute("type").Equals("training", StringComparison.OrdinalIgnoreCase))
                            {
                                #region Контроль

                                var tm = new TestModule
                                             {
                                                 Text = xmlReader.GetAttribute("name")
                                             };

                                #region Входной/выходной контроль

                                if (xmlReader.GetAttribute("io") != null)
                                {
                                    if (xmlReader.GetAttribute("io").Equals("i", StringComparison.OrdinalIgnoreCase))
                                    {
                                        tm.TestType = Enums.TestType.InTest;
                                    }
                                    else if (xmlReader.GetAttribute("io").Equals("o", StringComparison.OrdinalIgnoreCase))
                                    {
                                        tm.TestType = Enums.TestType.OutTest;
                                    }
                                }
                                else
                                {
                                    tm.TestType = Enums.TestType.OutTest;
                                }

                                #endregion

                                #region Тренажер

                                if (xmlReader.GetAttribute("type").Equals("test", StringComparison.OrdinalIgnoreCase))
                                {
                                    tm.Trainer = false;
                                }
                                else if (xmlReader.GetAttribute("type").Equals("training", StringComparison.OrdinalIgnoreCase))
                                {
                                    tm.Trainer = true;
                                }

                                #endregion

                                #region Последовательность вопросов

                                if (xmlReader.GetAttribute("order").Equals("natural", StringComparison.OrdinalIgnoreCase))
                                {
                                    tm.QuestionSequence = Enums.QuestionSequence.Natural;
                                }
                                else if (xmlReader.GetAttribute("order").Equals("random", StringComparison.OrdinalIgnoreCase))
                                {
                                    tm.QuestionSequence = Enums.QuestionSequence.Random;
                                }
                                else if (xmlReader.GetAttribute("order").Equals("network", StringComparison.OrdinalIgnoreCase))
                                {
                                    tm.QuestionSequence = Enums.QuestionSequence.Network;
                                }

                                #endregion

                                tm.MistakesNumber = int.Parse(xmlReader.GetAttribute("errlimit"));
                                tm.TimeRestriction = int.Parse(xmlReader.GetAttribute("time"));

                                #region Вложенность контролей

                                if (xmlReader.Depth == depth)
                                {
                                    activeNode.Nodes.Add(tm);
                                }
                                else if (xmlReader.Depth - depth > 0)
                                {
                                    activeNode = activeNode.Nodes[activeNode.Nodes.Count - 1];
                                    activeNode.Nodes.Add(tm);
                                }
                                else if (xmlReader.Depth - depth < 0)
                                {
                                    for (var i = 0; i < depth - xmlReader.Depth; i++)
                                    {
                                        activeNode = activeNode.Parent;
                                    }

                                    activeNode.Nodes.Add(tm);
                                }

                                depth = xmlReader.Depth;

                                #endregion

                                tm.XmlReader.ReadXml(xmlReader);

                                #region Замена q.NextQuestion на реальные, после того, как все вопросы считаны

                                foreach (var q in tm.Questions)
                                {
                                    if (q.NextQuestion != null)
                                    {
                                        q.NextQuestion = Warehouse.Warehouse.GetQuestionById(q.NextQuestion.Id);
                                    }
                                }

                                #endregion

                                #endregion
                            }
                        }
                    }
                }
            }
            catch { }
            finally
            {
                xmlReader.Close();
            }

            #endregion
        }
    }
}