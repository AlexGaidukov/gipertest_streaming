using System.Collections.Generic;
using System.Linq;

namespace VisualEditor.Logic.Course.Items
{
    internal class InConceptParent : CourseItem
    {
        public InConceptParent()
        {
            ImageIndex = SelectedImageIndex = 2;
        }

        public List<InDummyConcept> InDummyConcepts
        {
            get
            {
                return Nodes.OfType<InDummyConcept>().Select(node => node as InDummyConcept).ToList();
            }
        }
    }
}