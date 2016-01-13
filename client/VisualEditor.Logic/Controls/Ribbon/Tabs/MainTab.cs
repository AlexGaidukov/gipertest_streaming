using VisualEditor.Logic.Controls.Ribbon.Panels;
using VisualEditor.Utils.Controls.Ribbon;

namespace VisualEditor.Logic.Controls.Ribbon.Tabs
{
    internal class MainTab : RibbonTab
    {
        private static MainTab instance;

        private const string text = "Главная";

        public MainTab()
        {
            InitializeTab();
        }

        public static MainTab Instance
        {
            get { return instance ?? (instance = new MainTab()); }
        }

        #region Инициализация главной вкладки
        
        private void InitializeTab()
        {
            Text = text;
            RibbonHelper.AddPanel(this, new EditorModesPanel());
            RibbonHelper.AddPanel(this, new FontPanel());
            RibbonHelper.AddPanel(this, new ParagraphPanel());
            RibbonHelper.AddPanel(this, new BufferPanel());
            RibbonHelper.AddPanel(this, new EditingPanel());
            RibbonHelper.AddPanel(this, new FloatingWindowsPanel());
        }

        #endregion
    }
}