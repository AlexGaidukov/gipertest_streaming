namespace VisualEditor.Logic.Commands.Course
{
    internal class CopyItem : AbstractCommand
    {
        public CopyItem()
        {
            name = CommandNames.CopyItem;
            text = CommandTexts.CopyItem;
            image = Properties.Resources.CopyItem;
        }

        public override void Execute(object @object)
        {
            if (!Enabled)
            {
                return;
            }

            // POSTPONE: Продумать и реализовать логику вырезать/копировать/вставить для узлов дерева учебного курса.
        }
    }
}