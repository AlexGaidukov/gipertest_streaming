namespace VisualEditor.Logic.Commands.Course
{
    internal class PasteItem : AbstractCommand
    {
        public PasteItem()
        {
            name = CommandNames.PasteItem;
            text = CommandTexts.PasteItem;
            image = Properties.Resources.PasteItem;
        }

        public override void Execute(object @object)
        {
            if (!Enabled)
            {
                return;
            }

            // POSTPONE: Продумать и реализовать логику вырезать/копировать/вставить для узлов дерева учебного курса.

            Warehouse.Warehouse.IsProjectModified = true;
        }
    }
}