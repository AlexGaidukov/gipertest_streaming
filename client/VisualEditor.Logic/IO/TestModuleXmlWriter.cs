using System;
using System.Xml;
using VisualEditor.Logic.Course.Items;

namespace VisualEditor.Logic.IO
{
    internal class TestModuleXmlWriter
    {
        private readonly TestModule testModule;

        public TestModuleXmlWriter(TestModule testModule)
        {
            this.testModule = testModule;
        }

        public void WriteXml(XmlTextWriter xmlWriter)
        {
            xmlWriter.WriteStartElement("module");
            xmlWriter.WriteAttributeString("name", testModule.Text);
            if (testModule.Trainer)
            {
                xmlWriter.WriteAttributeString("type", "training");
            }
            else
            {
                xmlWriter.WriteAttributeString("type", "test");
            }

            // Если ид не был прочитан при загрузке проекта.
            if (testModule.Id.Equals(Guid.Empty))
            {
                testModule.Id = Guid.NewGuid();
            }

            xmlWriter.WriteAttributeString("id", "#module{" + testModule.Id.ToString().ToUpper() + "}");
            xmlWriter.WriteAttributeString("order", testModule.QuestionSequence.ToString().ToLower());
            xmlWriter.WriteAttributeString("errlimit", testModule.MistakesNumber.ToString());
            xmlWriter.WriteAttributeString("time", testModule.TimeRestriction.ToString());
            if (testModule.TestType.Equals(Enums.TestType.InTest))
            {
                xmlWriter.WriteAttributeString("io", "i");
            }
            else if (testModule.TestType.Equals(Enums.TestType.OutTest))
            {
                xmlWriter.WriteAttributeString("io", "o");
            }

            #region Группы

            foreach (var g in testModule.Groups)
            {
                g.XmlWriter.WriteXml(xmlWriter);
            }

            #endregion

            #region Вопросы

            foreach (var q in testModule.Questions)
            {
                q.XmlWriter.WriteXml(xmlWriter);
            }

            #endregion

            xmlWriter.WriteFullEndElement();
        }
    }
}