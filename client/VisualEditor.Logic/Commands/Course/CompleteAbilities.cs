namespace VisualEditor.Logic.Commands.Course
{
    internal class CompleteAbilities : AbstractCommand
    {
        public CompleteAbilities()
        {
            name = CommandNames.CompleteAbilities;
            text = CommandTexts.CompleteAbilities;
        }

        public override void Execute(object @object)
        {
            if (!Enabled)
            {
                return;
            }

            // POSTPONE: Продумать и реализовать логику режимов редактирования курса.
        }
    }
}