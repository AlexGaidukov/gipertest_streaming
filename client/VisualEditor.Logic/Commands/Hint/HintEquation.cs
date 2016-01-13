namespace VisualEditor.Logic.Commands.Hint
{
    internal class HintEquation : AbstractCommand
    {
        public HintEquation()
        {
            name = CommandNames.HintEquation;
            text = CommandTexts.HintEquation;
            image = Properties.Resources.Equation;
        }

        public override void Execute(object @object)
        {
            if (!Enabled)
            {
                return;
            }

            CommandManager.Instance.GetCommand(CommandNames.HintEquationSmall).Execute(null);
        }
    }
}