using VisualEditor.Logic.Commands;
using VisualEditor.Logic.Commands.Hint;
using VisualEditor.Utils.Controls.Ribbon;

namespace VisualEditor.Logic.Controls.Ribbon.Panels.Hint
{
    internal class HintMultimediaPanel : RibbonPanel
    {
        private const string text = "Мультимедиа";

        public HintMultimediaPanel()
        {
            InitializePanel();
        }

        private void InitializePanel()
        {
            Text = text;
            ButtonMoreVisible = false;
            FlowsTo = RibbonPanelFlowDirection.Bottom;

            RibbonHelper.AddButton(this, new HintPicture());
            RibbonHelper.AddButton(this, new HintAnimation());
            CommandManager.Instance.GetCommand(CommandNames.HintPicture).Enabled = true;
            CommandManager.Instance.GetCommand(CommandNames.HintPictureSmall).Enabled = true;
            CommandManager.Instance.GetCommand(CommandNames.HintAnimation).Enabled = true;
            CommandManager.Instance.GetCommand(CommandNames.HintAnimationSmall).Enabled = true;
        }
    }
}