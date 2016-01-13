using VisualEditor.Logic.Commands;
using VisualEditor.Logic.Controls.Ribbon.Tabs.Hint;

namespace VisualEditor.Logic.Controls.Ribbon
{
    internal class HintRibbon : Utils.Controls.Ribbon.Ribbon
    {
        public HintRibbon()
        {
            InitializeRibbon();
        }

        private void InitializeRibbon()
        {
            OrbVisible = false;
            QuickAcessToolbar.DropDownButtonVisible = false;
            QuickAccessVisible = false;

            RibbonHelper.AddButton(QuickAcessToolbar, CommandManager.Instance.GetCommand(CommandNames.HintUndo));
            RibbonHelper.AddButton(QuickAcessToolbar, CommandManager.Instance.GetCommand(CommandNames.HintRedo));
            CommandManager.Instance.GetCommand(CommandNames.HintUndo).Enabled = true;
            CommandManager.Instance.GetCommand(CommandNames.HintRedo).Enabled = true;

            RibbonHelper.AddTab(this, new HintMainTab());
            RibbonHelper.AddTab(this, new HintEmbeddingTab());
        }
    }
}