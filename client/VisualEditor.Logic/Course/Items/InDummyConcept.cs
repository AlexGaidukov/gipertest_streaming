namespace VisualEditor.Logic.Course.Items
{
    internal class InDummyConcept : CourseItem
    {
        public InDummyConcept()
        {
            ImageIndex = SelectedImageIndex = 4;
        }

        public Concept Concept { get; set; }
    }
}