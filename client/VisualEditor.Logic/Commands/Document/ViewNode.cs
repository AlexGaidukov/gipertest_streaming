using VisualEditor.Logic.Controls.Docking.Documents;
using VisualEditor.Logic.Controls.HtmlEditing;
using VisualEditor.Logic.Warehouse;

namespace VisualEditor.Logic.Commands.Document
{
    internal class ViewNode : AbstractCommand
    {
        public ViewNode()
        {
            name = CommandNames.ViewNode;
            text = CommandTexts.ViewNode;
        }

        public override void Execute(object @object)
        {
            if (!Enabled)
            {
                return;
            }

            var pd = HtmlEditingToolHelper.GetParentDocument(EditorObserver.ActiveEditor);

            if (pd is TrainingModuleDocument)
            {
                var tm = (pd as TrainingModuleDocument).TrainingModule;
                tm.Parent.Expand();

                Warehouse.Warehouse.Instance.CourseTree.CurrentNode = tm;
                Warehouse.Warehouse.Instance.CourseTree.HandleContextMenu();
            }
        }
    }
}