using VisualEditor.Logic.Commands;
using VisualEditor.Logic.Commands.Hint;
using VisualEditor.Logic.Controls.Ribbon.Extended.Hint;
using VisualEditor.Utils.Controls.Ribbon;

namespace VisualEditor.Logic.Controls.Ribbon.Panels.Hint
{
    internal class HintFontPanel : RibbonPanel
    {
        private const string text = "Шрифт";

        public HintFontPanel()
        {
            InitializePanel();
        }

        private void InitializePanel()
        {
            Text = text;
            ButtonMoreVisible = true;
            FlowsTo = RibbonPanelFlowDirection.Right;

            var g = new RibbonItemGroup
                        {
                            DrawBackground = false
                        };
            var cb = RibbonHelper.AddComboBox(g, new HintFontName(), typeof(HintFontNameComboBox));
            cb.TextBoxWidth = 130;
            cb = RibbonHelper.AddComboBox(g, new HintFontSize(), typeof(HintFontSizeComboBox));
            cb.TextBoxWidth = 50;
            CommandManager.Instance.GetCommand(CommandNames.HintFontName).Enabled = true;
            CommandManager.Instance.GetCommand(CommandNames.HintFontSize).Enabled = true;
            RibbonHelper.AddGroup(this, g);
            
            g = new RibbonItemGroup();
            RibbonHelper.AddButton(g, new HintRemoveFormat());
            CommandManager.Instance.GetCommand(CommandNames.HintRemoveFormat).Enabled = true;
            RibbonHelper.AddGroup(this, g);
            
            g = new RibbonItemGroup();
            RibbonHelper.AddButton(g, new HintFontSizeUp());
            RibbonHelper.AddButton(g, new HintFontSizeDown());
            //CommandManager.Instance.GetCommand(CommandNames.HintFontSizeUp).Enabled = true;
            //CommandManager.Instance.GetCommand(CommandNames.HintFontSizeDown).Enabled = true;
            RibbonHelper.AddGroup(this, g);

            g = new RibbonItemGroup();
            RibbonHelper.AddButton(g, new HintBackColor());
            RibbonHelper.AddButton(g, new HintForeColor());
            CommandManager.Instance.GetCommand(CommandNames.HintBackColor).Enabled = true;
            CommandManager.Instance.GetCommand(CommandNames.HintForeColor).Enabled = true;
            RibbonHelper.AddGroup(this, g);

            g = new RibbonItemGroup();
            RibbonHelper.AddButton(g, new HintBold());
            RibbonHelper.AddButton(g, new HintItalic());
            RibbonHelper.AddButton(g, new HintUnderline());
            RibbonHelper.AddButton(g, new HintStrikeThrough());
            RibbonHelper.AddButton(g, new HintInferior());
            RibbonHelper.AddButton(g, new HintAscender());
            CommandManager.Instance.GetCommand(CommandNames.HintBold).Enabled = true;
            CommandManager.Instance.GetCommand(CommandNames.HintItalic).Enabled = true;
            CommandManager.Instance.GetCommand(CommandNames.HintUnderline).Enabled = true;
            CommandManager.Instance.GetCommand(CommandNames.HintStrikeThrough).Enabled = true;
            CommandManager.Instance.GetCommand(CommandNames.HintInferior).Enabled = true;
            CommandManager.Instance.GetCommand(CommandNames.HintAscender).Enabled = true;
            RibbonHelper.AddGroup(this, g);

            ButtonMoreClick += FontPanel_ButtonMoreClick;
        }

        void FontPanel_ButtonMoreClick(object sender, System.EventArgs e)
        {
            CommandManager.Instance.GetCommand(CommandNames.Font).Execute(null);
        }
    }
}