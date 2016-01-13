using VisualEditor.Logic.Commands;
using VisualEditor.Logic.Commands.Concept;
using VisualEditor.Utils.Controls.Ribbon;

namespace VisualEditor.Logic.Controls.Ribbon.Panels
{
    internal class ConceptPanel : RibbonPanel
    {
        private const string text = "Компетенция";

        public ConceptPanel()
        {
            InitializePanel();
        }

        private void InitializePanel()
        {
            Text = text;
            ButtonMoreVisible = false;
            FlowsTo = RibbonPanelFlowDirection.Bottom;

            var b = RibbonHelper.AddButton(this, new NavigateToConcept());
            b.MaxSizeMode = RibbonElementSizeMode.Medium;
            b = RibbonHelper.AddButton(this, new Profile());
            b.MaxSizeMode = RibbonElementSizeMode.Medium;
            b.Image = b.SmallImage = Properties.Resources.Profile;
            b = RibbonHelper.AddButton(this, CommandManager.Instance.GetCommand(CommandNames.ProfileOptions));
            b.MaxSizeMode = RibbonElementSizeMode.Medium;
            RibbonHelper.AddSeparator(this);
            RibbonHelper.AddButton(this, new DeleteConcept());
        }
    }
}