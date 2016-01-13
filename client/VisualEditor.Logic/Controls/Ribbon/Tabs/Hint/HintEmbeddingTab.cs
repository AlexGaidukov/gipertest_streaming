using VisualEditor.Logic.Controls.Ribbon.Panels.Hint;
using VisualEditor.Utils.Controls.Ribbon;

namespace VisualEditor.Logic.Controls.Ribbon.Tabs.Hint
{
    internal class HintEmbeddingTab : RibbonTab
    {
        private const string text = "Вставка";

        public HintEmbeddingTab()
        {
            InitializeTab();
        }

        private void InitializeTab()
        {
            Text = text;

            RibbonHelper.AddPanel(this, new HintTablesPanel());
            RibbonHelper.AddPanel(this, new HintMultimediaPanel());
            RibbonHelper.AddPanel(this, new HintStylesPanel());
            RibbonHelper.AddPanel(this, new HintSymbolsPanel());
            //RibbonHelper.AddPanel(this, new HintAdditionallyPanel());
        }
    }
}