using System;
using System.Globalization;
using System.IO;
using System.Windows.Forms;
using System.Xml;
using VisualEditor.Logic.Controls.Ribbon.Extended;
using VisualEditor.Logic.Course.Items;
using VisualEditor.Logic.IO;
using VisualEditor.Utils.ExceptionHandling;

namespace VisualEditor.Logic.Commands.IO
{
    internal class LoadFromXml : AbstractCommand
    {
        public LoadFromXml()
        {
            name = CommandNames.LoadFromXml;
            text = CommandTexts.LoadFromXml;
        }

        public override void Execute(object @object)
        {
            if (!Enabled)
            {
                return;
            }

            var path = Path.Combine(Warehouse.Warehouse.ProjectEditorLocation, Warehouse.Warehouse.ProjectArchiveName);

            #region Заполнение дерева компетенций

            var ct = Warehouse.Warehouse.Instance.ConceptTree;
            var xmlReader = new XmlTextReader(path);

            try
            {
                while (xmlReader.Read())
                {
                    System.Windows.Forms.Application.DoEvents();

                    if (xmlReader.NodeType == XmlNodeType.Element)
                    {
                        if (xmlReader.Name.Equals("concept"))
                        {
                            var io = xmlReader.GetAttribute("io");

                            // В старых файлах проекта атрибута "io" нет.
                            if (io == null || io.Equals("o"))
                            {
                                var c = new Logic.Course.Items.Concept
                                            {
                                                Id = new Guid(xmlReader.GetAttribute("id").Substring(6, 36)),
                                                Text = xmlReader.GetAttribute("name"),
                                                Type = Enums.ConceptType.Internal,
                                                ImageIndex = 1,
                                                SelectedImageIndex = 1
                                            };
                                ct.Nodes.Add(c);
                            }
                            else if (io.Equals("i"))
                            {
                                var c = new Logic.Course.Items.Concept
                                            {
                                                Id = new Guid(xmlReader.GetAttribute("id").Substring(6, 36)),
                                                Text = xmlReader.GetAttribute("name"),
                                                Type = Enums.ConceptType.External,
                                                ImageIndex = 1,
                                                SelectedImageIndex = 1
                                            };
                                ct.Nodes.Add(c);
                            }
                        }
                        else if (xmlReader.Name.Equals("profile"))
                        {
                            var id = new Guid(xmlReader.GetAttribute("concept_id").Substring(6, 36));

                            foreach (Logic.Course.Items.Concept c in ct.Nodes)
                            {
                                if (c.Id.Equals(id))
                                {
                                    var lb = xmlReader.GetAttribute("min");
                                    var separator = NumberFormatInfo.CurrentInfo.CurrencyDecimalSeparator;
                                    lb = lb.Replace(".", separator);
                                    lb = lb.Replace(",", separator);

                                    c.LowerBound = (float)Convert.ToDouble(lb);
                                    c.IsProfile = true;
                                    c.ImageIndex = 0;
                                    c.SelectedImageIndex = 0;

                                    break;
                                }
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

            ////
            RibbonStatusStripEx.Instance.SetProgress(10);
            ////

            #region Определение свойства ModuleId компетенций

            var mid = Guid.Empty;
            xmlReader = new XmlTextReader(path);
            var modulesCount = 0;

            try
            {
                while (xmlReader.Read())
                {
                    System.Windows.Forms.Application.DoEvents();

                    if (xmlReader.NodeType == XmlNodeType.Element)
                    {
                        if (xmlReader.Name.Equals("module"))
                        {
                            if (xmlReader.GetAttribute("type").Equals("text"))
                            {
                                mid = new Guid(xmlReader.GetAttribute("id").Substring(8, 36));
                            }

                            modulesCount++;
                        }
                        else if (xmlReader.Name.Equals("output"))
                        {
                            var id = new Guid(xmlReader.GetAttribute("concept_id").Substring(6, 36));

                            foreach (Logic.Course.Items.Concept c in ct.Nodes)
                            {
                                if (c.Id.Equals(id))
                                {
                                    c.ModuleId = new Guid(mid.ToString());

                                    break;
                                }
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

            RibbonStatusStripEx.Instance.ModulesCount = modulesCount;

            #endregion

            #region Чтение структуры проекта, закладок, контента контролей

            var depth = 1;
            var cst = Warehouse.Warehouse.Instance.CourseTree;
            // Узел, в который добавляется следующий создаваемый узел.
            TreeNode activeNode = null;

            xmlReader = new XmlTextReader(path);

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

                            var cr = new Logic.Course.Items.CourseRoot
                                         {
                                             Text = xmlReader.GetAttribute("name")
                                         };

                            cst.Nodes.Add(cr);
                            activeNode = cr;

                            #endregion
                        }
                        else if (xmlReader.Name.Equals("module"))
                        {
                            if (xmlReader.GetAttribute("type").Equals("text"))
                            {
                                #region Учебный модуль

                                mid = new Guid(xmlReader.GetAttribute("id").Substring(8, 36));
                                var tm = new TrainingModule
                                             {
                                                 Id = mid,
                                                 Text = xmlReader.GetAttribute("name")
                                             };

                                TrainingModule.Count++;
                                Warehouse.Warehouse.Instance.TrainingModules.Add(tm);

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
                            }
                            else if (xmlReader.GetAttribute("type").Equals("test", StringComparison.OrdinalIgnoreCase) ||
                                     xmlReader.GetAttribute("type").Equals("training", StringComparison.OrdinalIgnoreCase))
                            {
                                #region Контроль

                                mid = Guid.Empty;
                                var tm = new TestModule
                                             {
                                                 Text = xmlReader.GetAttribute("name"),
                                                 Id = new Guid(xmlReader.GetAttribute("id").Substring(8, 36))
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
                                else if (xmlReader.GetAttribute("type").Equals("training",
                                                                               StringComparison.OrdinalIgnoreCase))
                                {
                                    tm.Trainer = true;
                                }

                                #endregion

                                #region Последовательность вопросов

                                if (xmlReader.GetAttribute("order").Equals("natural", StringComparison.OrdinalIgnoreCase))
                                {
                                    tm.QuestionSequence = Enums.QuestionSequence.Natural;
                                }
                                else if (xmlReader.GetAttribute("order").Equals("random",
                                                                                StringComparison.OrdinalIgnoreCase))
                                {
                                    tm.QuestionSequence = Enums.QuestionSequence.Random;
                                }
                                else if (xmlReader.GetAttribute("order").Equals("network",
                                                                                StringComparison.OrdinalIgnoreCase))
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
                                    for (int i = 0; i < depth - xmlReader.Depth; i++)
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

                                RibbonStatusStripEx.Instance.MakeProgressStep(10);
                            }
                        }
                        else if (xmlReader.Name.Equals("input"))
                        {
                            #region Входная компетенция

                            var id = new Guid(xmlReader.GetAttribute("concept_id").Substring(6, 36));
                            Logic.Course.Items.Concept con = null;

                            foreach (Logic.Course.Items.Concept c in ct.Nodes)
                            {
                                if (c.Id.Equals(id))
                                {
                                    con = c;

                                    break;
                                }
                            }

                            var idc = new InDummyConcept
                            {
                                Text = con.Text,
                                Concept = con
                            };
                            idc.Concept.InDummyConcepts.Add(idc);

                            if (con.Type.Equals(Enums.ConceptType.Internal))
                            {
                                // Добавляет компетенцию-пустышку во входы учебного модуля.
                                var tm = Warehouse.Warehouse.GetTrainingModuleById(mid);
                                tm.InConceptParent.Nodes.Add(idc);
                            }
                            else if (con.Type.Equals(Enums.ConceptType.External))
                            {
                                // Добавляет компетенцию-пустышку во входы учебного курса.
                                cst.InConceptsParent.Nodes.Add(idc);
                            }

                            #endregion
                        }
                        else if (xmlReader.Name.Equals("output"))
                        {
                            #region Выходная компетенция

                            var id = new Guid(xmlReader.GetAttribute("concept_id").Substring(6, 36));
                            Logic.Course.Items.Concept con = null;

                            foreach (Logic.Course.Items.Concept c in ct.Nodes)
                            {
                                if (c.Id.Equals(id))
                                {
                                    con = c;

                                    break;
                                }
                            }

                            var odc = new OutDummyConcept
                                          {
                                              Text = con.Text,
                                              Concept = con
                                          };
                            odc.Concept.OutDummyConcept = odc;

                            // Добавляет компетенцию-пустышку в выходы учебного модуля.
                            var tm = Warehouse.Warehouse.GetTrainingModuleById(mid);
                            tm.OutConceptParent.Nodes.Add(odc);

                            #endregion
                        }
                        else if (xmlReader.Name.Equals("html_text"))
                        {
                            #region Чтение закладок

                            // Разбирает DocumentHtml только учебного модуля.
                            if (!mid.Equals(Guid.Empty))
                            {
                                var documentHtml = xmlReader.ReadElementString();
                                documentHtml = documentHtml.Trim();
                                TrainingModuleXmlReader.ReadBookmarksIds(documentHtml);
                            }

                            #endregion
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ExceptionManager.Instance.LogException(ex);
            }
            finally
            {
                xmlReader.Close();
            }

            #endregion

            #region Чтение контента учебных модулей

            xmlReader = new XmlTextReader(path);

            try
            {
                while (xmlReader.Read())
                {
                    System.Windows.Forms.Application.DoEvents();

                    if (xmlReader.NodeType == XmlNodeType.Element)
                    {
                        if (xmlReader.Name.Equals("module"))
                        {
                            if (xmlReader.GetAttribute("type").Equals("text"))
                            {
                                #region Учебный модуль

                                mid = new Guid(xmlReader.GetAttribute("id").Substring(8, 36));

                                #endregion
                            }
                            else if (xmlReader.GetAttribute("type").Equals("test", StringComparison.OrdinalIgnoreCase) ||
                                     xmlReader.GetAttribute("type").Equals("training", StringComparison.OrdinalIgnoreCase))
                            {
                                #region Контроль

                                mid = Guid.Empty;

                                #endregion
                            }
                        }
                        else if (xmlReader.Name.Equals("html_text"))
                        {
                            #region Контент документа

                            // Разбирает DocumentHtml только учебного модуля.
                            if (!mid.Equals(Guid.Empty))
                            {
                                var documentHtml = xmlReader.ReadElementString();
                                documentHtml = documentHtml.Trim();
                                var tm = Warehouse.Warehouse.GetTrainingModuleById(mid);
                                documentHtml = tm.XmlReader.XmlToHtml(documentHtml);
                                tm.DocumentHtml = documentHtml;
                            }

                            #endregion

                            RibbonStatusStripEx.Instance.MakeProgressStep(10);
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