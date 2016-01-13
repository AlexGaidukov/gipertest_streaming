namespace VisualEditor.Logic.Commands.Project
{
    internal class Exit : AbstractCommand
    {
        public Exit()
        {
            name = CommandNames.Exit;
            text = CommandTexts.Exit;
            image = Properties.Resources.Exit;
        }

        public override void Execute(object @object)
        {
            if (!Enabled)
            {
                return;
            }

            System.Windows.Forms.Application.Exit();
        }
    }
}