using VisualEditor.Logic.Commands.Application;
using VisualEditor.Logic.Commands.Help;
using VisualEditor.Logic.Commands.HtmlEditing;
using VisualEditor.Logic.Commands.Project;
using VisualEditor.Logic.Controls.Ribbon.Tabs;

namespace VisualEditor.Logic.Controls.Ribbon
{
    internal class MainRibbon : Utils.Controls.Ribbon.Ribbon
    {
        private static MainRibbon instance;

        public MainRibbon()
        {
            InitializeRibbon();
        }

        public static MainRibbon Instance
        {
            get { return instance ?? (instance = new MainRibbon()); }
        }

        #region Инициализация риббон
        
        private void InitializeRibbon()
        {
            QuickAcessToolbar.DropDownButtonVisible = false;
            RibbonHelper.AddButton(QuickAcessToolbar, new SaveProjectSmall());
            RibbonHelper.AddButton(QuickAcessToolbar, new Undo());
            RibbonHelper.AddButton(QuickAcessToolbar, new Redo());
            RibbonHelper.AddButton(QuickAcessToolbar, new Help());

            OrbImage = Properties.Resources.VisualEditor;
            RibbonHelper.AddOrbMenuItem(this, new NewProject());
            RibbonHelper.AddOrbMenuItem(this, new OpenProject());
            RibbonHelper.AddOrbMenuItem(this, new SaveProject());
            RibbonHelper.AddOrbMenuItem(this, new SaveProjectAs());
            RibbonHelper.AddSeparator(this);
            RibbonHelper.AddOrbMenuItem(this, new CloseProject());

            RibbonHelper.AddOrbOptionButton(this, new Exit());
            RibbonHelper.AddOrbOptionButton(this, new AppSettings());

            OrbDropDown.Width = 600;

            RibbonHelper.AddTab(this, MainTab.Instance);
            RibbonHelper.AddTab(this, EmbeddingTab.Instance);
            RibbonHelper.AddTab(this, CourseTab.Instance);
        }

        #endregion
    }
}