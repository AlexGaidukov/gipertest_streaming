using System;
using System.Collections.Generic;
using System.Linq;
using VisualEditor.Logic.IO;

namespace VisualEditor.Logic.Course.Items
{
    internal class TestModule : CourseItem
    {
        public TestModule()
        {
            ImageIndex = SelectedImageIndex = 6;
            QuestionSequence = Enums.QuestionSequence.Random;

            XmlWriter = new TestModuleXmlWriter(this);
            XmlReader = new TestModuleXmlReader(this);

            success = false;
        }

        public Guid Id { get; set; }
        public Enums.TestType TestType { get; set; }

        public List<Group> Groups
        {
            get
            {
                return Nodes.OfType<Group>().Select(node => node as Group).ToList();
            }
        }

        public List<Question> Questions
        {
            get
            {
                return Nodes.OfType<Question>().Select(node => node as Question).ToList();
            }
        }

        public bool success;

        public int MistakesNumber { get; set; }
        public int TimeRestriction { get; set; }
        public bool Trainer { get; set; }
        public Enums.QuestionSequence QuestionSequence { get; set; }

        public TestModuleXmlWriter XmlWriter { get; private set; }
        public TestModuleXmlReader XmlReader { get; private set; }

        public static TestModule Clone(TestModule testModule)
        {
            var newTestModule = new TestModule
                                    {
                                        Text = testModule.Text,
                                        Id = testModule.Id,
                                        TestType = testModule.TestType,
                                        MistakesNumber = testModule.MistakesNumber,
                                        TimeRestriction = testModule.TimeRestriction,
                                        Trainer = testModule.Trainer,
                                        QuestionSequence = testModule.QuestionSequence
                                    };

            foreach (var testModuleGroup in testModule.Groups)
            {
                var group = Group.Clone(testModuleGroup);
                newTestModule.Nodes.Add(group);
            }

            foreach (var testModuleQuestion in testModule.Questions)
            {
                var question = Question.Clone(testModuleQuestion);
                testModule.Nodes.Add(question);
            }

            return newTestModule;
        }
    }
}