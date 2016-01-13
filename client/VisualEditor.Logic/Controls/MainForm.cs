using System;
using System.IO;
using System.Windows.Forms;
using VisualEditor.Logic.Commands;
using VisualEditor.Logic.Commands.Concept;
using VisualEditor.Logic.Commands.Course;
using VisualEditor.Logic.Commands.Document;
using VisualEditor.Logic.Commands.Embedding;
using VisualEditor.Logic.Commands.Hint;
using VisualEditor.Logic.Commands.HtmlEditing;
using VisualEditor.Logic.Commands.IO;
using VisualEditor.Logic.Commands.Preview;
using VisualEditor.Logic.Commands.Project;
using VisualEditor.Logic.Controls.Docking;
using VisualEditor.Logic.Controls.Ribbon;
using VisualEditor.Logic.Controls.Ribbon.Extended;
using VisualEditor.Logic.Helpers;
using VisualEditor.Logic.Helpers.AppSettings;
using VisualEditor.Logic.Warehouse;
using VisualEditor.Utils.Controls;
using VisualEditor.Utils.Helpers;

namespace VisualEditor.Logic.Controls
{
    public partial class MainForm : Form
    {
        private const string projectOpenFailedMessage = "В процессе открытия проекта произошла ошибка. Попробуйте повторить снова.";
        private static MainForm instance;

        private MainForm()
        {
            InitializeForm();
        }

        public static MainForm Instance
        {
            get { return instance ?? (instance = new MainForm()); }
        }

        public static string[] Args { get; set; }

        #region Инициализация главной формы

        private void InitializeForm()
        {
            SplashScreen.Instance.Show();
            SplashScreen.Instance.BuildVersion = string.Format("Версия {0}", Application.ProductVersion);
            SplashScreen.Instance.Message = "Инициализация интерфейса";
            RegistryHelper.RegisterFileAssociation();
            AppSettingsManager.Instance.Initialize();
            RegisterCommands();
            InitializeComponent();
            InitializeRibbon();
            InitializeStatusStrip();
            InitializeCommands();
            InitializeDockContainer();
            EditorObserver.HostEditorMode = Utils.Controls.HtmlEditing.Enums.HtmlEditingToolMode.Design;
            EditorObserver.RenderingStyle = Enums.RenderingStyle.EmptyEnvironment;
            SplashScreen.Instance.Message = "Запуск приложения";
            Text = Application.ProductName;
            new ExceptionHelper();
            Renderer.Instance.StartRendering();
            SplashScreen.Instance.Close();
        }

        #endregion

        #region Инициализация риббон
        
        private void InitializeRibbon()
        {
            splitContainer.Panel1.Controls.Add(MainRibbon.Instance);
            splitContainer.SplitterDistance = MainRibbon.Instance.Height;
        }

        #endregion

        #region Инициализация контейнера для плавающих окон
        
        private void InitializeDockContainer()
        {
            splitContainer.Panel2.Controls.Add(DockContainer.Instance);
        }

        #endregion

        #region Инициализация строки статуса
        
        private void InitializeStatusStrip()
        {
            Controls.Add(RibbonStatusStripEx.Instance);
        }

        #endregion

        #region Операции с командами
        
        private static void RegisterCommands()
        {
            CommandManager.Instance.Register(new AddTrainingModule());
            CommandManager.Instance.Register(new AddInTestModule());
            CommandManager.Instance.Register(new AddOutTestModule());
            CommandManager.Instance.Register(new AddTestModuleFromOuterCourse());
            CommandManager.Instance.Register(new AddTestModuleFromOuterCourseSmall());
            CommandManager.Instance.Register(new RenameItem());
            CommandManager.Instance.Register(new ViewDocument());
            CommandManager.Instance.Register(new ItemOptionsSmall());
            CommandManager.Instance.Register(new AddChoiceQuestionSmall());
            CommandManager.Instance.Register(new AddMultichoiceQuestionSmall());
            CommandManager.Instance.Register(new AddInteractiveQuestionSmall());
            CommandManager.Instance.Register(new AddOrderingQuestionSmall());
            CommandManager.Instance.Register(new AddOpenQuestionSmall());
            CommandManager.Instance.Register(new AddCorrespondenceQuestionSmall());
            CommandManager.Instance.Register(new AddOuterQuestionSmall());
            CommandManager.Instance.Register(new AddQuestionFromOuterCourseSmall());
            CommandManager.Instance.Register(new DeleteItem());
            CommandManager.Instance.Register(new AddResponse());
            CommandManager.Instance.Register(new AddGroup());
            CommandManager.Instance.Register(new ProfileOptionsSmall());
            CommandManager.Instance.Register(new Close());
            CommandManager.Instance.Register(new CloseAll());
            CommandManager.Instance.Register(new CloseAllButThis());
            CommandManager.Instance.Register(new ViewNode());
            CommandManager.Instance.Register(new TableSmall());
            CommandManager.Instance.Register(new EquationSmall());
            CommandManager.Instance.Register(new SymbolSmall());
            CommandManager.Instance.Register(new StyleSmall());
            CommandManager.Instance.Register(new AddChoiceQuestion());
            CommandManager.Instance.Register(new AddMultichoiceQuestion());
            CommandManager.Instance.Register(new AddInteractiveQuestion());
            CommandManager.Instance.Register(new AddOrderingQuestion());
            CommandManager.Instance.Register(new AddOpenQuestion());
            CommandManager.Instance.Register(new AddCorrespondenceQuestion());
            CommandManager.Instance.Register(new AddOuterQuestion());
            CommandManager.Instance.Register(new AddQuestionFromOuterCourse());
            CommandManager.Instance.Register(new LoadFromImsQti());
            CommandManager.Instance.Register(new SaveToImsQti());
            CommandManager.Instance.Register(new PictureSmall());
            CommandManager.Instance.Register(new AnimationSmall());
            CommandManager.Instance.Register(new AudioSmall());
            CommandManager.Instance.Register(new VideoSmall());
            CommandManager.Instance.Register(new AudioSmall());
            CommandManager.Instance.Register(new VideoSmall());
            CommandManager.Instance.Register(new BookmarkSmall());
            CommandManager.Instance.Register(new ConceptSmall());
            CommandManager.Instance.Register(new LinkSmall());
            CommandManager.Instance.Register(new Font());
            CommandManager.Instance.Register(new ProfileOptions());
            CommandManager.Instance.Register(new SaveToHtp());
            CommandManager.Instance.Register(new LoadFromHtp());
            CommandManager.Instance.Register(new SaveToXml());
            CommandManager.Instance.Register(new LoadFromXml());
            CommandManager.Instance.Register(new NavigateToLinkObject());
            CommandManager.Instance.Register(new OuterLoadFromHtp());
            CommandManager.Instance.Register(new OuterLoadFromXml());
            CommandManager.Instance.Register(new RecentProject());
            CommandManager.Instance.Register(new EditEquation());

            // POSTPONE: Реализовать.
            /*
            CommandManager.Instance.Register(new CutItem());
            */

            #region HintDialog

            CommandManager.Instance.Register(new HintUndo());
            CommandManager.Instance.Register(new HintRedo());
            CommandManager.Instance.Register(new HintTableSmall());
            CommandManager.Instance.Register(new HintAnimationSmall());
            CommandManager.Instance.Register(new HintPictureSmall());
            CommandManager.Instance.Register(new HintStyleSmall());
            CommandManager.Instance.Register(new HintEquationSmall());
            CommandManager.Instance.Register(new HintSymbolSmall());
            CommandManager.Instance.Register(new HintCut());
            CommandManager.Instance.Register(new HintCopy());
            CommandManager.Instance.Register(new HintPaste());

            #endregion
        }

        private static void InitializeCommands()
        {
            CommandManager.Instance.GetCommand(CommandNames.NewProject).Enabled = true;
            CommandManager.Instance.GetCommand(CommandNames.OpenProject).Enabled = true;
            CommandManager.Instance.GetCommand(CommandNames.AppSettings).Enabled = true;
            CommandManager.Instance.GetCommand(CommandNames.Exit).Enabled = true;
            CommandManager.Instance.GetCommand(CommandNames.Help).Enabled = true;
            CommandManager.Instance.GetCommand(CommandNames.AddTrainingModule).Enabled = true;
            CommandManager.Instance.GetCommand(CommandNames.AddInTestModule).Enabled = true;
            CommandManager.Instance.GetCommand(CommandNames.AddOutTestModule).Enabled = true;
            CommandManager.Instance.GetCommand(CommandNames.AddTestModuleFromOuterCourseSmall).Enabled = true;
            CommandManager.Instance.GetCommand(CommandNames.AddGroup).Enabled = true;
            CommandManager.Instance.GetCommand(CommandNames.AddChoiceQuestionSmall).Enabled = true;
            CommandManager.Instance.GetCommand(CommandNames.AddMultichoiceQuestionSmall).Enabled = true;
            CommandManager.Instance.GetCommand(CommandNames.AddInteractiveQuestionSmall).Enabled = true;
            CommandManager.Instance.GetCommand(CommandNames.AddOrderingQuestionSmall).Enabled = true;
            CommandManager.Instance.GetCommand(CommandNames.AddOpenQuestionSmall).Enabled = true;
            CommandManager.Instance.GetCommand(CommandNames.AddOuterQuestionSmall).Enabled = true;
            CommandManager.Instance.GetCommand(CommandNames.AddQuestionFromOuterCourseSmall).Enabled = true;
            CommandManager.Instance.GetCommand(CommandNames.AddResponse).Enabled = true;
            CommandManager.Instance.GetCommand(CommandNames.ItemOptionsSmall).Enabled = true;
            CommandManager.Instance.GetCommand(CommandNames.ViewDocument).Enabled = true;
            CommandManager.Instance.GetCommand(CommandNames.RenameItem).Enabled = true;
            CommandManager.Instance.GetCommand(CommandNames.Close).Enabled = true;
            CommandManager.Instance.GetCommand(CommandNames.CloseAll).Enabled = true;
            CommandManager.Instance.GetCommand(CommandNames.CloseAllButThis).Enabled = true;
            CommandManager.Instance.GetCommand(CommandNames.ViewNode).Enabled = true;
            CommandManager.Instance.GetCommand(CommandNames.Course).Enabled = true;
            CommandManager.Instance.GetCommand(CommandNames.Concepts).Enabled = true;
            CommandManager.Instance.GetCommand(CommandNames.Warnings).Enabled = true;
            CommandManager.Instance.GetCommand(CommandNames.AddChoiceQuestion).Enabled = true;
            CommandManager.Instance.GetCommand(CommandNames.AddMultichoiceQuestion).Enabled = true;
            CommandManager.Instance.GetCommand(CommandNames.AddInteractiveQuestion).Enabled = true;
            CommandManager.Instance.GetCommand(CommandNames.AddOrderingQuestion).Enabled = true;
            CommandManager.Instance.GetCommand(CommandNames.AddOpenQuestion).Enabled = true;
            //CommandManager.Instance.GetCommand(CommandNames.CorrespondenceQuestion).Enabled = true;
            //CommandManager.Instance.GetCommand(CommandNames.CorrespondenceQuestionSmall).Enabled = true;
            CommandManager.Instance.GetCommand(CommandNames.AddOuterQuestion).Enabled = true;
            CommandManager.Instance.GetCommand(CommandNames.Font).Enabled = true;
            CommandManager.Instance.GetCommand(CommandNames.AddTestModuleFromOuterCourse).Enabled = true;
            CommandManager.Instance.GetCommand(CommandNames.AddQuestionFromOuterCourse).Enabled = true;
            CommandManager.Instance.GetCommand(CommandNames.SaveToHtp).Enabled = true;
            CommandManager.Instance.GetCommand(CommandNames.LoadFromHtp).Enabled = true;
            CommandManager.Instance.GetCommand(CommandNames.SaveToXml).Enabled = true;
            CommandManager.Instance.GetCommand(CommandNames.LoadFromXml).Enabled = true;
            CommandManager.Instance.GetCommand(CommandNames.NavigateToLinkObject).Enabled = true;
            CommandManager.Instance.GetCommand(CommandNames.OuterLoadFromHtp).Enabled = true;
            CommandManager.Instance.GetCommand(CommandNames.OuterLoadFromXml).Enabled = true;
            CommandManager.Instance.GetCommand(CommandNames.EditEquation).Enabled = true;
            CommandManager.Instance.GetCommand(CommandNames.RecentProject).Enabled = true;
            CommandManager.Instance.GetCommand(CommandNames.SaveToImsQti).Enabled = true;
            CommandManager.Instance.GetCommand(CommandNames.LoadFromImsQti).Enabled = true;        
        }

        #endregion

        private void MainForm_Load(object sender, EventArgs e)
        {
            UIHelper.PrepareMainFormUI(this);
        }

        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            #region Для корректного завершения загрузки/сохранения учебного курса

            if (LoadFromHtp.IsBusy)
            {
                e.Cancel = true;
                return;
            }

            if (SaveToHtp.IsBusy)
            {
                e.Cancel = true;
                return;
            }

            #endregion

            SaveProjectAs.DialogResult = DialogResult.None;
            CommandManager.Instance.GetCommand(CommandNames.CloseProject).Execute(null);

            if (CloseProject.DialogResult.Equals(DialogResult.Cancel) ||
                SaveProjectAs.DialogResult.Equals(DialogResult.Cancel))
            {
                e.Cancel = true;
                return;
            }

            UIHelper.SaveMainFormUI(this);
            AppSettingsManager.Instance.SaveSettingsToFile();
        }

        private void MainForm_Shown(object sender, EventArgs e)
        {
            HandleStart(Args);
        }

        #region HandleStart

        private static void HandleStart(string[] args)
        {
            if (args.Length == 0)
            {
                return;
            }

            if (args[0] == null)
            {
                return;
            }

            Warehouse.Warehouse.ProjectTrueLocation = Path.GetDirectoryName(args[0]);
            Warehouse.Warehouse.ProjectFileName = Path.GetFileNameWithoutExtension(args[0]);
            Warehouse.Warehouse.ProjectFileType = Path.GetExtension(args[0]);

            try
            {
                Warehouse.Warehouse.CreateDirectories();
            }
            catch (Exception)
            {
                UIHelper.ShowMessage(projectOpenFailedMessage,
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            Warehouse.Warehouse.Instance.CourseTree.Enabled = false;
            Warehouse.Warehouse.Instance.ConceptTree.Enabled = false;

            if (Warehouse.Warehouse.ProjectFileType.Equals(".htp"))
            {
                CommandManager.Instance.GetCommand(CommandNames.LoadFromHtp).Execute(null);
            }

            Warehouse.Warehouse.Instance.CourseTree.Enabled = true;
            Warehouse.Warehouse.Instance.ConceptTree.Enabled = true;

            Warehouse.Warehouse.IsProjectSavedToFile = true;
            AppSettingsManager.Instance.SetSettingByName(SettingNames.InitialDirectory,
                Warehouse.Warehouse.ProjectTrueLocation);
            EditorObserver.RenderingStyle = Enums.RenderingStyle.NoActiveDocument;
        }

        #endregion
    }
}