namespace VisualEditor.Logic.Commands.Course
{
    internal class MistakeCorrection : AbstractCommand
    {
        public MistakeCorrection()
        {
            name = CommandNames.MistakeCorrection;
            text = CommandTexts.MistakeCorrection;
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