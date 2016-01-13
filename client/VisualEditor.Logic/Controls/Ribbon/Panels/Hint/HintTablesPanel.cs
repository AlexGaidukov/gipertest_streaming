using VisualEditor.Logic.Commands;
using VisualEditor.Logic.Commands.Hint;
using VisualEditor.Utils.Controls.Ribbon;

namespace VisualEditor.Logic.Controls.Ribbon.Panels.Hint
{
    internal class HintTablesPanel : RibbonPanel
    {
        private const string text = "Таблицы";

        public HintTablesPanel()
        {
            InitializePanel();
        }

        private void InitializePanel()
        {
            Text = text;
            ButtonMoreVisible = false;
            FlowsTo = RibbonPanelFlowDirection.Bottom;

            RibbonHelper.AddButton(this, new HintTable());
            CommandManager.Instance.GetCommand(CommandNames.HintTable).Enabled = true;
            CommandManager.Instance.GetCommand(CommandNames.HintTableSmall).Enabled = true;
        }
    }
}