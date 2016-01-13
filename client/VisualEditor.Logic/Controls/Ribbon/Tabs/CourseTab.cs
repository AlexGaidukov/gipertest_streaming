using VisualEditor.Logic.Controls.Ribbon.Panels;
using VisualEditor.Utils.Controls.Ribbon;

namespace VisualEditor.Logic.Controls.Ribbon.Tabs
{
    internal class CourseTab : RibbonTab
    {
        private static CourseTab instance;

        private const string text = "Учебный курс";

        public CourseTab()
        {
            InitializeTab();
        }

        public static CourseTab Instance
        {
            get { return instance ?? (instance = new CourseTab()); }
        }

        #region Инициализация вкладки работы с учебным курсом

        private void InitializeTab()
        {
            Text = text;
            //RibbonHelper.AddPanel(this, new RevisionModePanel());
            RibbonHelper.AddPanel(this, new StructurePanel());
            RibbonHelper.AddPanel(this, new ConceptPanel());
            //RibbonHelper.AddPanel(this, new AboutAuthorsPanel());
            RibbonHelper.AddPanel(this, new ImsQtiPanel());
        }

        #endregion
    }
}