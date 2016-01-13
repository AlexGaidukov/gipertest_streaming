namespace VisualEditor.Logic.Commands.Course
{
    internal class VersionMaking : AbstractCommand
    {
        public VersionMaking()
        {
            name = CommandNames.VersionMaking;
            text = CommandTexts.VersionMaking;
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