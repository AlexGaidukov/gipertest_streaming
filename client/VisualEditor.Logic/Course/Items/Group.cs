using System.Collections.Generic;
using System.Linq;
using VisualEditor.Logic.IO;

namespace VisualEditor.Logic.Course.Items
{
    internal class Group : CourseItem
    {
        public Group()
        {
            ImageIndex = SelectedImageIndex = 7;

            XmlWriter = new GroupXmlWriter(this);
            XmlReader = new GroupXmlReader(this);
        }

        public List<Question> Questions
        {
            get
            {
                return Nodes.OfType<Question>().Select(node => node as Question).ToList();
            }
        }

        public int TimeRestriction { get; set; }
        public Concept Profile { get; set; }
        public int Marks { get; set; }
        public int ChosenQuestionsCount { get; set; }

        public GroupXmlWriter XmlWriter { get; private set; }
        public GroupXmlReader XmlReader { get; private set; }

        public static Group Clone(Group group)
        {
            var newGroup = new Group
                               {
                                   Text = group.Text,
                                   TimeRestriction = group.TimeRestriction,
                                   Profile = group.Profile,
                                   Marks = group.Marks,
                                   ChosenQuestionsCount = group.ChosenQuestionsCount
                               };

            foreach (var groupQuestion in group.Questions)
            {
                var question = Question.Clone(groupQuestion);
                newGroup.Nodes.Add(question);
            }

            return newGroup;
        }
    }
}