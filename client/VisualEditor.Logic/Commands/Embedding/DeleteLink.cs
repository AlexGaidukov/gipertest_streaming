using VisualEditor.Logic.Warehouse;

namespace VisualEditor.Logic.Commands.Embedding
{
    internal class DeleteLink : AbstractCommand
    {
        public DeleteLink()
        {
            name = CommandNames.DeleteLink;
            text = CommandTexts.DeleteLink;
        }

        public override void Execute(object @object)
        {
            if (!Enabled)
            {
                return;
            }

            if (EditorObserver.ActiveEditor == null)
            {
                return;
            }

            EditorObserver.ActiveEditor.Unlink();
            EditorObserver.ActiveEditor.ActiveElement = EditorObserver.ActiveEditor.Document.Body;
            Warehouse.Warehouse.IsProjectModified = true;
        }
    }
}