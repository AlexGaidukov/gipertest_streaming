using VisualEditor.Logic.Course.Items;

namespace VisualEditor.Logic.Commands.Course
{
    internal class RenameItem : AbstractCommand
    {
        public RenameItem()
        {
            name = CommandNames.RenameItem;
            text = CommandTexts.RenameItem;
        }

        public override void Execute(object @object)
        {
            if (!Enabled)
            {
                return;
            }

            if (Warehouse.Warehouse.Instance.CourseTree.ContainsFocus)
            {
                var cn = Warehouse.Warehouse.Instance.CourseTree.CurrentNode;

                if (cn == null)
                {
                    return;
                }

                if (cn is InConceptParent ||
                    cn is OutConceptParent ||
                    cn is InDummyConcept ||
                    cn is OutDummyConcept)
                //|| 
                //cn is Response)
                {
                    return;
                }

                Warehouse.Warehouse.Instance.CourseTree.LabelEdit = true;
                if (!cn.IsEditing)
                {
                    cn.BeginEdit();
                }
            }

            if (Warehouse.Warehouse.Instance.ConceptTree.ContainsFocus)
            {
                var cn = Warehouse.Warehouse.Instance.ConceptTree.CurrentNode;

                if (cn == null)
                {
                    return;
                }

                Warehouse.Warehouse.Instance.ConceptTree.LabelEdit = true;
                if (!cn.IsEditing)
                {
                    cn.BeginEdit();
                }
            }

            Warehouse.Warehouse.IsProjectModified = true;
        }
    }
}