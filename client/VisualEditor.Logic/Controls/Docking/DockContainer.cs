using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using VisualEditor.Logic.Commands;
using VisualEditor.Logic.Controls.Docking.Documents;
using VisualEditor.Logic.Controls.Docking.SideWindows;
using VisualEditor.Logic.Controls.HtmlEditing;
using VisualEditor.Logic.Controls.Ribbon;
using VisualEditor.Logic.Controls.Ribbon.Extended;
using VisualEditor.Logic.Course.Preview;
using VisualEditor.Logic.Warehouse;
using VisualEditor.Utils.Controls.Docking;
using VisualEditor.Utils.Controls.Ribbon;
using VisualEditor.Utils.Docking;

namespace VisualEditor.Logic.Controls.Docking
{
    internal class DockContainer : DockPanel
    {
        private static DockContainer instance;
        private const string dockFileName = "Dock.xml";

        private DockContainer()
        {
            InitializeDockContainer();
        }

        public new List<DocumentBase> Documents { get; private set; }

        public static DockContainer Instance
        {
            get { return instance ?? (instance = new DockContainer()); }
        }

        public CourseWindow CourseWindow { get; private set; }
        public ConceptWindow ConceptWindow { get; private set; }
        public WarningWindow WarningWindow { get; private set; }

        public List<TrainingModuleDocument> TrainingModuleDocuments
        {
            get
            {
                return Documents.OfType<TrainingModuleDocument>().Select(document => document as TrainingModuleDocument).ToList();
            }
        }

        public List<QuestionDocument> QuestionDocuments
        {
            get
            {
                return Documents.OfType<QuestionDocument>().Select(document => document as QuestionDocument).ToList();
            }
        }

        public List<ResponseDocument> ResponseDocuments
        {
            get
            {
                return Documents.OfType<ResponseDocument>().Select(document => document as ResponseDocument).ToList();
            }
        }

        public static RibbonContextMenu DocumentTabContextMenu { get; private set; }

        private void InitializeDockContainer()
        {
            AllowEndUserNestedDocking = false;
            Dock = DockStyle.Fill;
            DocumentStyle = DocumentStyle.DockingWindow;

            CourseWindow = new CourseWindow();
            ConceptWindow = new ConceptWindow();
            WarningWindow = new WarningWindow();

            Documents = new List<DocumentBase>();

            ActiveDocumentChanged += DockContainer_ActiveDocumentChanged;
            ContentAdded += DockContainer_ContentAdded;

            DocumentTabContextMenu = new RibbonContextMenu();

            RibbonHelper.AddButton(DocumentTabContextMenu, CommandManager.Instance.GetCommand(CommandNames.Close));
            RibbonHelper.AddButton(DocumentTabContextMenu, CommandManager.Instance.GetCommand(CommandNames.CloseAllButThis));
            RibbonHelper.AddButton(DocumentTabContextMenu, CommandManager.Instance.GetCommand(CommandNames.CloseAll));
            RibbonHelper.AddSeparator(DocumentTabContextMenu);
            RibbonHelper.AddButton(DocumentTabContextMenu, CommandManager.Instance.GetCommand(CommandNames.ViewNode));
        }

        #region Восстановление и сохранение состояния плавающих окон

        public void LoadDockingWindowsLayout()
        {
            var path = Path.Combine(Application.StartupPath, dockFileName);
            // Не отображает плавающие окна, если файл с параметрами окон не найден.
            if (!File.Exists(path))
            {
                return;
            }

            try
            {
                var deserializeDockContent = new DeserializeDockContent(GetContentFromPersistString);
                LoadFromXml(path, deserializeDockContent);
            }
            catch (Exception)
            {
                // Не отображает плавающие окна, если во время загрузки произошло исключение.
            }
        }

        public void SaveDockingWindowsLayout()
        {
            var path = Path.Combine(Application.StartupPath, dockFileName);

            try
            {
                SaveAsXml(path);
            }
            catch (Exception)
            {
                // Не сохраняет состояние плавающих окон, если во время сохранения произошло исключение.
            }
        }

        private IDockContent GetContentFromPersistString(string persistString)
        {
            if (persistString == typeof(CourseWindow).ToString())
            {
                return CourseWindow;
            }

            if (persistString == typeof(ConceptWindow).ToString())
            {
                return ConceptWindow;
            }

            if (persistString == typeof(WarningWindow).ToString())
            {
                return WarningWindow;
            }

            return null;
        }

        #endregion

        private void DockContainer_ContentAdded(object sender, DockContentEventArgs e)
        {
            if (e.Content is DocumentBase)
            {
                if (Documents.Contains(e.Content as DocumentBase))
                {
                    return;
                }

                Documents.Add(e.Content as DocumentBase);
            }
        }

        #region Смена вкладок

        private void DockContainer_ActiveDocumentChanged(object sender, EventArgs e)
        {
            if (ActiveDocument == null)
            {
                MainForm.Instance.Text = Application.ProductName;
                EditorObserver.ActiveEditor = null;

                RibbonStatusStripEx.Instance.ClearOverwriteLabel();
                EditorObserver.RenderingStyle = Enums.RenderingStyle.NoActiveDocument;

                return;
            }

            #region Установка стиля рисования

            if (ActiveDocument is TrainingModuleDocument)
            {
                EditorObserver.RenderingStyle = Enums.RenderingStyle.TrainingModuleDocument;
            }

            if (ActiveDocument is QuestionDocument)
            {
                EditorObserver.RenderingStyle = Enums.RenderingStyle.QuestionDocument;
            }

            if (ActiveDocument is ResponseDocument)
            {
                EditorObserver.RenderingStyle = Enums.RenderingStyle.ResponseDocument;
            }

            if (ActiveDocument is PictureDocument ||
                ActiveDocument is AnimationDocument ||
                ActiveDocument is AudioDocument ||
                ActiveDocument is VideoDocument)
            {
                EditorObserver.RenderingStyle = Enums.RenderingStyle.MultimediaDocument;
            }

            #endregion

            #region Автоматическое закрытие вкладок

            foreach (var d in Documents)
            {
                if (d is PictureDocument ||
                    d is AnimationDocument ||
                    d is AudioDocument ||
                    d is VideoDocument ||
                    d is QuestionDocument ||
                    d is ResponseDocument)
                {
                    if (d != ActiveDocument)
                    {
                        d.Hide();
                    }
                }
            }

            #endregion

            #region Установка активного редактора

            if (ActiveDocument is TrainingModuleDocument)
            {
                var ad = ActiveDocument as TrainingModuleDocument;
                MainForm.Instance.Text = string.Concat(ad.TrainingModule.Text, " - ", Application.ProductName);
                EditorObserver.ActiveEditor = ad.HtmlEditingTool;
            }

            if (ActiveDocument is QuestionDocument)
            {
                var ad = ActiveDocument as QuestionDocument;
                MainForm.Instance.Text = string.Concat(ad.Question.Text, " - ", Application.ProductName);
                EditorObserver.ActiveEditor = ad.HtmlEditingTool;
            }

            if (ActiveDocument is ResponseDocument)
            {
                var ad = ActiveDocument as ResponseDocument;
                MainForm.Instance.Text = string.Concat(ad.Response.Text, " - ", Application.ProductName);
                EditorObserver.ActiveEditor = ad.HtmlEditingTool;
            }

            #endregion

            if (EditorObserver.ActiveEditor != null)
            {
                if (EditorObserver.ActiveEditor.HighlightedElement != null)
                {
                    EditorObserver.ActiveEditor.Highlight(EditorObserver.ActiveEditor.HighlightedElement, false);
                    EditorObserver.ActiveEditor.HighlightedElement = null;
                }
            }

            if (EditorObserver.HostEditorMode == Utils.Controls.HtmlEditing.Enums.HtmlEditingToolMode.Design)
            {
                if (EditorObserver.ActiveEditor.CanOverwrite)
                {
                    RibbonStatusStripEx.Instance.MakeOverwrite();
                }
                else
                {
                    RibbonStatusStripEx.Instance.MakeInsert();
                }
            }

            #region Управление BodyInnerHtml в зависимости от режима редактора

            if (EditorObserver.HostEditorMode == Utils.Controls.HtmlEditing.Enums.HtmlEditingToolMode.Preview)
            {
                // Переход из режима предварительного просмотра в режим редактирования.
                if (EditorObserver.ActiveEditor.Mode == Utils.Controls.HtmlEditing.Enums.HtmlEditingToolMode.Design &&
                    !Warehouse.Warehouse.IsHtmlSourceMode)
                {
                    // Переводит редактор в режим предварительного просмотра.
                    EditorObserver.ActiveEditor.Mode = Utils.Controls.HtmlEditing.Enums.HtmlEditingToolMode.Preview;

                    // Изменяет EditorMode активного редактора.
                    EditorObserver.ActiveEditor.Mode = Utils.Controls.HtmlEditing.Enums.HtmlEditingToolMode.Preview;

                    // Конвертирует DocumentHtml в PreviewHtml и устанавливает PreviewHtml.
                    PreviewConverter.Convert(EditorObserver.ActiveEditor);
                }

                // Переход из режима предварительного просмотра в режим исходного кода.
                if (Warehouse.Warehouse.IsHtmlSourceMode)
                {
                    var document = ActiveDocument as DocumentBase;
                    HtmlToolEmbeddingHelper.SwitchToHtmlSourceViewer(document.HtmlSourceViewer);
                    document.HtmlSourceViewer.Text = document.HtmlEditingTool.BodyInnerHtml;
                }
            }

            if (EditorObserver.HostEditorMode == Utils.Controls.HtmlEditing.Enums.HtmlEditingToolMode.Design)
            {
                // Переход из режима редактирования в режим предварительного просмотра.
                if (EditorObserver.ActiveEditor.Mode == Utils.Controls.HtmlEditing.Enums.HtmlEditingToolMode.Preview &&
                    !Warehouse.Warehouse.IsHtmlSourceMode)
                {
                    // Изменяет EditorMode активного редактора.
                    EditorObserver.ActiveEditor.Mode = Utils.Controls.HtmlEditing.Enums.HtmlEditingToolMode.Design;

                    // Обнуляет PreviewHtml и устанавливает прежний DocumentHtml.
                    if (HtmlEditingToolHelper.GetParentDocument(EditorObserver.ActiveEditor) is TrainingModuleDocument)
                    {
                        var tm = ((TrainingModuleDocument)HtmlEditingToolHelper.GetParentDocument(EditorObserver.ActiveEditor)).TrainingModule;
                        tm.PreviewHtml = string.Empty;
                        EditorObserver.ActiveEditor.BodyInnerHtml = tm.DocumentHtml;
                    }
                    else if (HtmlEditingToolHelper.GetParentDocument(EditorObserver.ActiveEditor) is QuestionDocument)
                    {
                        var q = ((QuestionDocument)HtmlEditingToolHelper.GetParentDocument(EditorObserver.ActiveEditor)).Question;
                        q.PreviewHtml = string.Empty;
                        EditorObserver.ActiveEditor.BodyInnerHtml = q.DocumentHtml;
                    }
                    else if (HtmlEditingToolHelper.GetParentDocument(EditorObserver.ActiveEditor) is ResponseDocument)
                    {
                        var r = ((ResponseDocument)HtmlEditingToolHelper.GetParentDocument(EditorObserver.ActiveEditor)).Response;
                        r.PreviewHtml = string.Empty;
                        EditorObserver.ActiveEditor.BodyInnerHtml = r.DocumentHtml;
                    }

                    // Переводит редактор в режим визуального редактирования.
                    EditorObserver.ActiveEditor.Mode = Utils.Controls.HtmlEditing.Enums.HtmlEditingToolMode.Design;
                }

                // Переход из режима редактирования в режим исходного кода.
                if (Warehouse.Warehouse.IsHtmlSourceMode)
                {
                    var document = ActiveDocument as DocumentBase;
                    HtmlToolEmbeddingHelper.SwitchToHtmlSourceViewer(document.HtmlSourceViewer);
                    document.HtmlSourceViewer.Text = document.HtmlEditingTool.BodyInnerHtml;
                }
            }

            // Делает редактор видимым, если отменен режим просмотра исходного кода.
            if (!Warehouse.Warehouse.IsHtmlSourceMode)
            {
                var document = ActiveDocument as DocumentBase;
                HtmlToolEmbeddingHelper.SwitchToHtmlEditingTool(document.HtmlEditingTool);
            }

            #endregion
        }

        #endregion
    }
}