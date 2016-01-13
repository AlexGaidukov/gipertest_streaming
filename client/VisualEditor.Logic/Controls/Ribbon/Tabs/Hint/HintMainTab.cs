using VisualEditor.Logic.Controls.Ribbon.Panels.Hint;
using VisualEditor.Utils.Controls.Ribbon;

namespace VisualEditor.Logic.Controls.Ribbon.Tabs.Hint
{
    internal class HintMainTab : RibbonTab
    {
        private const string text = "Главная";

        public HintMainTab()
        {
            InitializeTab();
        }

        private void InitializeTab()
        {
            Text = text;

            RibbonHelper.AddPanel(this, new HintFontPanel());
            RibbonHelper.AddPanel(this, new HintParagraphPanel());
            RibbonHelper.AddPanel(this, new HintBufferPanel());
        }
    }
}