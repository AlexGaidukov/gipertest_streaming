namespace VisualEditor.Logic.Course.Items
{
    internal class OutDummyConcept : CourseItem
    {
        public OutDummyConcept()
        {
            ImageIndex = SelectedImageIndex = 5;
        }

        public Concept Concept { get; set; }
    }
}