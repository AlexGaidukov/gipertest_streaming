using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace VisualEditor.Logic.Course.Items
{
    internal class Concept : TreeNode
    {
        public Concept()
        {
            InDummyConcepts = new List<InDummyConcept>();
            ImageIndex = SelectedImageIndex = 1;
        }

        public Guid Id { get; set; }
        public Guid ModuleId { get; set; }
        public List<InDummyConcept> InDummyConcepts { get; private set; }
        public OutDummyConcept OutDummyConcept { get; set; }
        public Enums.ConceptType Type { get; set; }
        public bool IsProfile { get; set; }
        public float LowerBound { get; set; }
    }
}