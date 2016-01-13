using VisualEditor.Logic.Controls.Ribbon.Panels;
using VisualEditor.Utils.Controls.Ribbon;

namespace VisualEditor.Logic.Controls.Ribbon.Tabs
{
    internal class EmbeddingTab : RibbonTab
    {
        private static EmbeddingTab instance;

        private const string text = "Вставка";

        public EmbeddingTab()
        {
            InitializeTab();
        }

        public static EmbeddingTab Instance
        {
            get { return instance ?? (instance = new EmbeddingTab()); }
        }

        #region Инициализация вкладки вставки
        
        private void InitializeTab()
        {
            Text = text;
            RibbonHelper.AddPanel(this, new TablesPanel());
            RibbonHelper.AddPanel(this, new MultimediaPanel());
            RibbonHelper.AddPanel(this, new LinksPanel());
            RibbonHelper.AddPanel(this, new StylesPanel());
            RibbonHelper.AddPanel(this, new SymbolsPanel());
            RibbonHelper.AddPanel(this, new AdditionallyPanel());
        }

        #endregion
    }
}