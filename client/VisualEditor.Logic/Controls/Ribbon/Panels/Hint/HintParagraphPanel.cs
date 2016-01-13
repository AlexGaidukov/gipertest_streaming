using VisualEditor.Logic.Commands;
using VisualEditor.Logic.Commands.Hint;
using VisualEditor.Utils.Controls.Ribbon;

namespace VisualEditor.Logic.Controls.Ribbon.Panels.Hint
{
    internal class HintParagraphPanel : RibbonPanel
    {
        private const string text = "Абзац";

        public HintParagraphPanel()
        {
            InitializePanel();
        }

        private void InitializePanel()
        {
            Text = text;
            ButtonMoreVisible = false;
            FlowsTo = RibbonPanelFlowDirection.Right;

            var g = new RibbonItemGroup();
            RibbonHelper.AddButton(g, new HintJustifyLeft());
            RibbonHelper.AddButton(g, new HintJustifyCenter());
            RibbonHelper.AddButton(g, new HintJustifyRight());
            RibbonHelper.AddButton(g, new HintJustifyFull());
            CommandManager.Instance.GetCommand(CommandNames.HintJustifyLeft).Enabled = true;
            CommandManager.Instance.GetCommand(CommandNames.HintJustifyCenter).Enabled = true;
            CommandManager.Instance.GetCommand(CommandNames.HintJustifyRight).Enabled = true;
            CommandManager.Instance.GetCommand(CommandNames.HintJustifyFull).Enabled = true;
            RibbonHelper.AddGroup(this, g);

            g = new RibbonItemGroup();
            RibbonHelper.AddButton(g, new HintOutdent());
            RibbonHelper.AddButton(g, new HintIndent());
            CommandManager.Instance.GetCommand(CommandNames.HintOutdent).Enabled = true;
            CommandManager.Instance.GetCommand(CommandNames.HintIndent).Enabled = true;
            RibbonHelper.AddGroup(this, g);

            g = new RibbonItemGroup();
            RibbonHelper.AddButton(g, new HintUnorderedList());
            RibbonHelper.AddButton(g, new HintOrderedList());
            CommandManager.Instance.GetCommand(CommandNames.HintUnorderedList).Enabled = true;
            CommandManager.Instance.GetCommand(CommandNames.HintOrderedList).Enabled = true;
            RibbonHelper.AddGroup(this, g);
        }
    }
}