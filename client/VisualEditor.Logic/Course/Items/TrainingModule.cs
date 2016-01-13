using System;
using System.Collections.Generic;
using System.Linq;
using VisualEditor.Logic.Controls.Docking.Documents;
using VisualEditor.Logic.IO;

namespace VisualEditor.Logic.Course.Items
{
    internal class TrainingModule : CourseItem
    {
        public TrainingModule()
        {
            InitializeModule();
        }

        public Guid Id { get; set; }
        public static int Count { get; set; }

        public List<TestModule> InTestModules
        {
            get
            {
                return Nodes.OfType<TestModule>().Select(node => node as TestModule).Where(testModule => testModule.TestType == Enums.TestType.InTest).ToList();
            }
        }

        public List<TestModule> OutTestModules
        {
            get
            {
                return Nodes.OfType<TestModule>().Select(node => node as TestModule).Where(testModule => testModule.TestType == Enums.TestType.OutTest).ToList();
            }
        }

        public List<TrainingModule> TrainingModules
        {
            get
            {
                return Nodes.OfType<TrainingModule>().Select(node => node as TrainingModule).ToList();
            }
        }

        public InConceptParent InConceptParent { get; private set; }
        public OutConceptParent OutConceptParent { get; private set; }

        public TrainingModuleDocument TrainingModuleDocument { get; set; }
        public string DocumentHtml { get; set; }
        public string PreviewHtml { get; set; }

        public TrainingModuleXmlWriter XmlWriter { get; private set; }
        public TrainingModuleXmlReader XmlReader { get; private set; }

        private void InitializeModule()
        {
            InConceptParent = new InConceptParent
                                  {
                                      Text = "Входы"
                                  };
            OutConceptParent = new OutConceptParent
                                   {
                                       Text = "Выходы"
                                   };
            Nodes.Add(InConceptParent);
            Nodes.Add(OutConceptParent);
            ImageIndex = SelectedImageIndex = 1;

            XmlWriter = new TrainingModuleXmlWriter(this);
            XmlReader = new TrainingModuleXmlReader(this);
        }
    }
}