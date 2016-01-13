using VisualEditor.Utils.Controls.Ribbon;

namespace VisualEditor.Logic.Controls.Ribbon.Panels
{
    internal class RevisionModePanel : RibbonPanel
    {
        private const string text = "Режим редактирования";

        public RevisionModePanel()
        {
            InitializePanel();
        }

        private void InitializePanel()
        {
            Text = text;
            ButtonMoreVisible = false;
            FlowsTo = RibbonPanelFlowDirection.Bottom;

            /*
            var b = RibbonHelper.AddButton(this, new CompleteAbilities());
            b.MaxSizeMode = RibbonElementSizeMode.Medium;
            b = RibbonHelper.AddButton(this, new MistakeCorrection());
            b.MaxSizeMode = RibbonElementSizeMode.Medium;
            b = RibbonHelper.AddButton(this, new VersionMaking());
            b.MaxSizeMode = RibbonElementSizeMode.Medium;
            */
        }
    }
}