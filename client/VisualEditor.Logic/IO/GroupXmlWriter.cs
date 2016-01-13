using System.Xml;
using VisualEditor.Logic.Course.Items;

namespace VisualEditor.Logic.IO
{
    internal class GroupXmlWriter
    {
        private readonly Group group;

        public GroupXmlWriter(Group group)
        {
            this.group = group;
        }

        public void WriteXml(XmlTextWriter xmlWriter)
        {
            xmlWriter.WriteStartElement("group");
            xmlWriter.WriteAttributeString("count", group.ChosenQuestionsCount.ToString());
            xmlWriter.WriteAttributeString("name", group.Text);

            #region Вопросы

            foreach (var q in group.Questions)
            {
                q.XmlWriter.WriteXml(xmlWriter);
            }

            #endregion

            xmlWriter.WriteStartElement("mark");
            xmlWriter.WriteAttributeString("value", group.Marks.ToString());
            if (group.Profile != null)
            {
                xmlWriter.WriteAttributeString("concept_id", "#elem{" + group.Profile.Id.ToString().ToUpper() + "}");
            }
            xmlWriter.WriteFullEndElement();
            xmlWriter.WriteFullEndElement();
        }
    }
}