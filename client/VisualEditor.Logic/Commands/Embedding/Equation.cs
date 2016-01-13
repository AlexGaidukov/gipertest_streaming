namespace VisualEditor.Logic.Commands.Embedding
{
    internal class Equation : AbstractCommand
    {
        public Equation()
        {
            name = CommandNames.Equation;
            text = CommandTexts.Equation;
            image = Properties.Resources.Equation;
        }

        public override void Execute(object @object)
        {
            if (!Enabled)
            {
                return;
            }

            CommandManager.Instance.GetCommand(CommandNames.EquationSmall).Execute(null);
        }
    }
}