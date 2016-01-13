namespace VisualEditor.Logic.Commands.Embedding
{
    internal class Concept : AbstractCommand
    {
        public Concept()
        {
            name = CommandNames.Concept;
            text = CommandTexts.Concept;
            image = Properties.Resources.Concept;
        }

        public override void Execute(object @object)
        {
            if (!Enabled)
            {
                return;
            }

            CommandManager.Instance.GetCommand(CommandNames.ConceptSmall).Execute(null);
        }
    }
}