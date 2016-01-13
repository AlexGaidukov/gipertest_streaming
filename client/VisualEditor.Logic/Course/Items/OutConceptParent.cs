using System.Collections.Generic;
using System.Linq;

namespace VisualEditor.Logic.Course.Items
{
    internal class OutConceptParent : CourseItem
    {
        public OutConceptParent()
        {
            ImageIndex = SelectedImageIndex = 3;
        }

        public List<OutDummyConcept> OutDummyConcepts
        {
            get
            {
                return Nodes.OfType<OutDummyConcept>().Select(node => node as OutDummyConcept).ToList();
            }
        }
    }
}