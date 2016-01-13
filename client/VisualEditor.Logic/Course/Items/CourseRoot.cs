using System.Collections.Generic;
using System.Linq;

namespace VisualEditor.Logic.Course.Items
{
    internal class CourseRoot : CourseItem
    {
        public CourseRoot()
        {
            var inConceptParent = new InConceptParent
                                      {
                                          Text = "Входы"
                                      };
            Nodes.Add(inConceptParent);
            ImageIndex = SelectedImageIndex = 0;
        }

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
    }
}