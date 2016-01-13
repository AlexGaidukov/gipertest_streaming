using System.IO;
using System.Text;
using System.Windows.Forms;
using System.Xml;
using VisualEditor.Logic.Controls.Ribbon.Extended;
using VisualEditor.Logic.Course.Items;

namespace VisualEditor.Logic.Commands.IO
{
    internal class SaveToXml : AbstractCommand
    {
        public SaveToXml()
        {
            name = CommandNames.SaveToXml;
            text = CommandTexts.SaveToXml;
        }

        public override void Execute(object @object)
        {
            if (!Enabled)
            {
                return;
            }

            // Сохраняет xml в ProjectTrueLocation.
            var path = Path.Combine(Warehouse.Warehouse.ProjectTrueLocation,
                                    string.Concat("ProjectName", ".xml"));
            var xmlWriter = new XmlTextWriter(path, Encoding.UTF8)
                                {
                                    Formatting = Formatting.Indented
                                };
            xmlWriter.WriteStartDocument();
            xmlWriter.WriteStartElement("project");
            xmlWriter.WriteAttributeString("name", Warehouse.Warehouse.Instance.CourseTree.Nodes[0].Text);
            xmlWriter.WriteAttributeString("type", "new");

            WalkTree(Warehouse.Warehouse.Instance.CourseTree.Nodes);
            RibbonStatusStripEx.Instance.ModulesCount = modulesCount;

            WriteModules(Warehouse.Warehouse.Instance.CourseTree.Nodes[0] as CourseItem, xmlWriter);

            #region Запись компетенций

            // Запись внешних компетенций.
            foreach (var c in Warehouse.Warehouse.Instance.ExternalConcepts)
            {
                xmlWriter.WriteStartElement("concept");
                xmlWriter.WriteAttributeString("name", c.Text);
                xmlWriter.WriteAttributeString("id", "#elem{" + c.Id.ToString().ToUpper() + "}");
                xmlWriter.WriteAttributeString("io", "i");
                xmlWriter.WriteEndElement();
            }

            // Запись внутренних компетенций.
            foreach (var c in Warehouse.Warehouse.Instance.InternalConcepts)
            {
                xmlWriter.WriteStartElement("concept");
                xmlWriter.WriteAttributeString("name", c.Text);
                xmlWriter.WriteAttributeString("id", "#elem{" + c.Id.ToString().ToUpper() + "}");
                xmlWriter.WriteAttributeString("io", "o");
                xmlWriter.WriteEndElement();
            }

            // Запись внешних профилей.
            foreach (var c in Warehouse.Warehouse.Instance.ExternalConcepts)
            {
                if (c.IsProfile)
                {
                    xmlWriter.WriteStartElement("profile");
                    xmlWriter.WriteAttributeString("concept_id", "#elem{" + c.Id.ToString().ToUpper() + "}");
                    xmlWriter.WriteAttributeString("min", c.LowerBound.ToString());
                    xmlWriter.WriteEndElement();
                }
            }

            // Запись внутренних профилей.
            foreach (var c in Warehouse.Warehouse.Instance.InternalConcepts)
            {
                if (c.IsProfile)
                {
                    xmlWriter.WriteStartElement("profile");
                    xmlWriter.WriteAttributeString("concept_id", "#elem{" + c.Id.ToString().ToUpper() + "}");
                    xmlWriter.WriteAttributeString("min", c.LowerBound.ToString());
                    xmlWriter.WriteEndElement();
                }
            }

            #endregion

            xmlWriter.WriteFullEndElement();
            xmlWriter.Close();
        }

        private static void WriteModules(CourseItem ci, XmlTextWriter xw)
        {
            for (var i = 0; i < ci.Nodes.Count; i++)
            {
                if (ci.Nodes[i] is TrainingModule)
                {
                    #region Запись учебных модулей

                    var tm = ci.Nodes[i] as TrainingModule;

                    xw.WriteStartElement("module");
                    xw.WriteAttributeString("name", tm.Text);
                    xw.WriteAttributeString("type", "text");
                    xw.WriteAttributeString("id", "#module{" + tm.Id.ToString().ToUpper() + "}");

                    tm.XmlWriter.WriteXml(xw);

                    RibbonStatusStripEx.Instance.MakeProgressStep(0);
                    System.Windows.Forms.Application.DoEvents();

                    if (tm.Nodes.Count != 0)
                    {
                        WriteModules(tm, xw);
                    }

                    xw.WriteFullEndElement();

                    #endregion
                }
                else if (ci.Nodes[i] is TestModule)
                {
                    #region Запись контролей

                    var tm = ci.Nodes[i] as TestModule;
                    tm.XmlWriter.WriteXml(xw);

                    #endregion

                    RibbonStatusStripEx.Instance.MakeProgressStep(0);
                    System.Windows.Forms.Application.DoEvents();
                }
            }
        }

        #region Подсчет модулей

        int modulesCount = 0;

        private void WalkTree(TreeNodeCollection tnc)
        {
            for (var i = 0; i < tnc.Count; i++)
            {
                var tn = tnc[i];
                if (tn.Nodes.Count != 0)
                {
                    WalkTree(tnc[i].Nodes);
                }

                if (tn is TrainingModule)
                {
                    modulesCount++;
                }

                if (tn is TestModule)
                {
                    modulesCount++;
                }
            }
        }

        #endregion
    }
}