namespace VisualEditor.Logic.Commands.Course
{
    internal class CutItem : AbstractCommand
    {
        public CutItem()
        {
            name = CommandNames.CutItem;
            text = CommandTexts.CutItem;
            image = Properties.Resources.CutItem;
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