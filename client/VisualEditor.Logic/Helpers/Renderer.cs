using System;
using System.Windows.Forms;
using VisualEditor.Logic.Commands;
using VisualEditor.Logic.Controls;
using VisualEditor.Logic.Controls.Ribbon;
using VisualEditor.Logic.Controls.Ribbon.Tabs;
using VisualEditor.Logic.Course.Items;
using VisualEditor.Logic.Warehouse;
using VisualEditor.Utils.Controls.HtmlEditing;

namespace VisualEditor.Logic.Helpers
{
    internal class Renderer
    {
        private static Renderer instance;
        private const int invalidateInterval = 50;

        #region Таймеры

        private Timer projectTimer;
        private Timer editorModesPanelTimer;
        private Timer mainTabTimer;
        private Timer bufferPanelTimer;
        private Timer embeddingTabTimer;
        private Timer structurePanelTimer;
        private Timer conceptPanelTimer;
        private Timer fontStyleTimer;
        private Timer editorContextMenuTimer;
        private Timer hintDialogEmbeddingMenuTimer;

        #endregion

        private Renderer()
        {
            InitializeRenderer();
        }

        public static Renderer Instance
        {
            get { return instance ?? (instance = new Renderer()); }
        }

        #region InitializeRenderer

        private void InitializeRenderer()
        {
            projectTimer = new Timer { Interval = invalidateInterval };
            projectTimer.Tick += projectTimer_Tick;

            editorModesPanelTimer = new Timer { Interval = invalidateInterval };
            editorModesPanelTimer.Tick += editorModesPanelTimer_Tick;

            mainTabTimer = new Timer { Interval = invalidateInterval };
            mainTabTimer.Tick += mainTabTimer_Tick;

            bufferPanelTimer = new Timer { Interval = invalidateInterval };
            bufferPanelTimer.Tick += bufferPanelTimer_Tick;

            embeddingTabTimer = new Timer { Interval = invalidateInterval };
            embeddingTabTimer.Tick += embeddingTabTimer_Tick;

            structurePanelTimer = new Timer { Interval = invalidateInterval };
            structurePanelTimer.Tick += structurePanelTimer_Tick;

            conceptPanelTimer = new Timer { Interval = invalidateInterval };
            conceptPanelTimer.Tick += conceptPanelTimer_Tick;

            fontStyleTimer = new Timer { Interval = invalidateInterval };
            fontStyleTimer.Tick += fontStyleTimer_Tick;

            editorContextMenuTimer = new Timer { Interval = invalidateInterval };
            editorContextMenuTimer.Tick += editorContextMenuTimer_Tick;

            hintDialogEmbeddingMenuTimer = new Timer { Interval = invalidateInterval };
            hintDialogEmbeddingMenuTimer.Tick += hintDialogEmbeddingMenuTimer_Tick;
        }

        #endregion

        #region projectTimer_Tick

        private void projectTimer_Tick(object sender, EventArgs e)
        {
            try
            {
                if (!MainForm.Instance.Visible)
                {
                    return;
                }

                if (Warehouse.Warehouse.IsProjectBeingDesigned)
                {
                    if (EditorObserver.HostEditorMode == Utils.Controls.HtmlEditing.Enums.HtmlEditingToolMode.Design)
                    {
                        // Оптимизирует отрисовку.
                        if (!CommandManager.Instance.GetCommand(CommandNames.SaveProjectSmall).Enabled)
                        {
                            if (!Warehouse.Warehouse.IsHtmlSourceMode)
                            {
                                CommandManager.Instance.GetCommand(CommandNames.NewProject).Enabled = true;
                                CommandManager.Instance.GetCommand(CommandNames.OpenProject).Enabled = true;
                                CommandManager.Instance.GetCommand(CommandNames.SaveProjectSmall).Enabled = true;
                                CommandManager.Instance.GetCommand(CommandNames.SaveProject).Enabled = true;
                                CommandManager.Instance.GetCommand(CommandNames.SaveProjectAs).Enabled = true;
                                CommandManager.Instance.GetCommand(CommandNames.CloseProject).Enabled = true;
                                //CommandManager.Instance.GetCommand(CommandNames.AboutAuthors).Enabled = true;
                            }
                            else
                            {
                                CommandManager.Instance.GetCommand(CommandNames.NewProject).Enabled = false;
                                CommandManager.Instance.GetCommand(CommandNames.OpenProject).Enabled = false;
                                CommandManager.Instance.GetCommand(CommandNames.SaveProjectSmall).Enabled = false;
                                CommandManager.Instance.GetCommand(CommandNames.SaveProject).Enabled = false;
                                CommandManager.Instance.GetCommand(CommandNames.SaveProjectAs).Enabled = false;
                                CommandManager.Instance.GetCommand(CommandNames.CloseProject).Enabled = false;
                            }
                        }
                    }

                    if (EditorObserver.HostEditorMode == Utils.Controls.HtmlEditing.Enums.HtmlEditingToolMode.Preview)
                    {
                        // Оптимизирует отрисовку.
                        if (CommandManager.Instance.GetCommand(CommandNames.SaveProjectSmall).Enabled)
                        {
                            CommandManager.Instance.GetCommand(CommandNames.NewProject).Enabled = false;
                            CommandManager.Instance.GetCommand(CommandNames.OpenProject).Enabled = false;
                            CommandManager.Instance.GetCommand(CommandNames.SaveProjectSmall).Enabled = false;
                            CommandManager.Instance.GetCommand(CommandNames.SaveProject).Enabled = false;
                            CommandManager.Instance.GetCommand(CommandNames.SaveProjectAs).Enabled = false;
                            CommandManager.Instance.GetCommand(CommandNames.CloseProject).Enabled = false;
                        }
                    }

                    if (Warehouse.Warehouse.IsHtmlSourceMode)
                    {
                        // Оптимизирует отрисовку.
                        if (CommandManager.Instance.GetCommand(CommandNames.SaveProjectSmall).Enabled)
                        {
                            CommandManager.Instance.GetCommand(CommandNames.NewProject).Enabled = false;
                            CommandManager.Instance.GetCommand(CommandNames.OpenProject).Enabled = false;
                            CommandManager.Instance.GetCommand(CommandNames.SaveProjectSmall).Enabled = false;
                            CommandManager.Instance.GetCommand(CommandNames.SaveProject).Enabled = false;
                            CommandManager.Instance.GetCommand(CommandNames.SaveProjectAs).Enabled = false;
                            CommandManager.Instance.GetCommand(CommandNames.CloseProject).Enabled = false;
                        }
                    }
                }
                else
                {
                    // Оптимизирует отрисовку.
                    if (CommandManager.Instance.GetCommand(CommandNames.SaveProjectSmall).Enabled)
                    {
                        CommandManager.Instance.GetCommand(CommandNames.SaveProjectSmall).Enabled = false;
                        CommandManager.Instance.GetCommand(CommandNames.SaveProject).Enabled = false;
                        CommandManager.Instance.GetCommand(CommandNames.SaveProjectAs).Enabled = false;
                        CommandManager.Instance.GetCommand(CommandNames.CloseProject).Enabled = false;
                    }
                }
            }
            catch { }
        }

        #endregion

        #region editorModesPanelTimer_Tick

        private void editorModesPanelTimer_Tick(object sender, EventArgs e)
        {
            try
            {
                if (!MainRibbon.Instance.ActiveTab.Equals(MainTab.Instance))
                {
                    return;
                }

                if (EditorObserver.RenderingStyle.Equals(Enums.RenderingStyle.EmptyEnvironment) ||
                    EditorObserver.RenderingStyle.Equals(Enums.RenderingStyle.NoActiveDocument))
                {
                    CommandManager.Instance.GetCommand(CommandNames.Design).Enabled = false;
                    CommandManager.Instance.GetCommand(CommandNames.Preview).Enabled = false;
                    CommandManager.Instance.GetCommand(CommandNames.Source).Enabled = false;
                    CommandManager.Instance.GetCommand(CommandNames.Design).Checked = false;
                    CommandManager.Instance.GetCommand(CommandNames.Preview).Checked = false;
                    CommandManager.Instance.GetCommand(CommandNames.Source).Checked = false;

                    CommandManager.Instance.GetCommand(CommandNames.NavigateBackward).Enabled = false;
                    CommandManager.Instance.GetCommand(CommandNames.NavigateForward).Enabled = false;
                }

                if (EditorObserver.RenderingStyle.Equals(Enums.RenderingStyle.MultimediaDocument))
                {
                    if (EditorObserver.HostEditorMode == Utils.Controls.HtmlEditing.Enums.HtmlEditingToolMode.Preview)
                    {
                        CommandManager.Instance.GetCommand(CommandNames.Design).Enabled = false;
                        CommandManager.Instance.GetCommand(CommandNames.Preview).Enabled = false;
                        CommandManager.Instance.GetCommand(CommandNames.Source).Enabled = false;
                        CommandManager.Instance.GetCommand(CommandNames.Design).Checked = false;
                        CommandManager.Instance.GetCommand(CommandNames.Preview).Checked = false;
                        CommandManager.Instance.GetCommand(CommandNames.Source).Checked = false;
                    }
                }

                if (EditorObserver.RenderingStyle.Equals(Enums.RenderingStyle.TrainingModuleDocument) ||
                    EditorObserver.RenderingStyle.Equals(Enums.RenderingStyle.QuestionDocument) ||
                    EditorObserver.RenderingStyle.Equals(Enums.RenderingStyle.ResponseDocument))
                {
                    CommandManager.Instance.GetCommand(CommandNames.Design).Enabled = true;
                    CommandManager.Instance.GetCommand(CommandNames.Preview).Enabled = true;
                    CommandManager.Instance.GetCommand(CommandNames.Source).Enabled = true;

                    if (EditorObserver.HostEditorMode == Utils.Controls.HtmlEditing.Enums.HtmlEditingToolMode.Design &&
                        !Warehouse.Warehouse.IsHtmlSourceMode)
                    {
                        CommandManager.Instance.GetCommand(CommandNames.Design).Checked = true;
                    }
                    else
                    {
                        CommandManager.Instance.GetCommand(CommandNames.Design).Checked = false;
                    }

                    if (EditorObserver.HostEditorMode == Utils.Controls.HtmlEditing.Enums.HtmlEditingToolMode.Preview &&
                        !Warehouse.Warehouse.IsHtmlSourceMode)
                    {
                        CommandManager.Instance.GetCommand(CommandNames.Preview).Checked = true;
                    }
                    else
                    {
                        CommandManager.Instance.GetCommand(CommandNames.Preview).Checked = false;
                    }

                    if (Warehouse.Warehouse.IsHtmlSourceMode)
                    {
                        CommandManager.Instance.GetCommand(CommandNames.Source).Checked = true;
                    }
                    else
                    {
                        CommandManager.Instance.GetCommand(CommandNames.Source).Checked = false;
                    }

                    #region Кнопки назад/вперед

                    CommandManager.Instance.GetCommand(CommandNames.NavigateBackward).Enabled = PreviewObserver.CanNavigateBackward();
                    CommandManager.Instance.GetCommand(CommandNames.NavigateForward).Enabled = PreviewObserver.CanNavigateForward();

                    #endregion
                }
            }
            catch { }
        }

        #endregion

        #region mainTabTimer_Tick

        private void mainTabTimer_Tick(object sender, EventArgs e)
        {
            try
            {
                if (!MainRibbon.Instance.ActiveTab.Equals(MainTab.Instance))
                {
                    return;
                }

                if (EditorObserver.RenderingStyle.Equals(Enums.RenderingStyle.EmptyEnvironment) ||
                    EditorObserver.RenderingStyle.Equals(Enums.RenderingStyle.NoActiveDocument))
                {
                    // Оптимизирует отрисовку.
                    if (CommandManager.Instance.GetCommand(CommandNames.FontName).Enabled)
                    {
                        CommandManager.Instance.GetCommand(CommandNames.FontName).Enabled = false;
                        CommandManager.Instance.GetCommand(CommandNames.FontSize).Enabled = false;
                        //CommandManager.Instance.GetCommand(CommandNames.FontSizeUp).Enabled = false;
                        //CommandManager.Instance.GetCommand(CommandNames.FontSizeDown).Enabled = false;
                        CommandManager.Instance.GetCommand(CommandNames.Bold).Enabled = false;
                        CommandManager.Instance.GetCommand(CommandNames.Italic).Enabled = false;
                        CommandManager.Instance.GetCommand(CommandNames.Underline).Enabled = false;
                        CommandManager.Instance.GetCommand(CommandNames.StrikeThrough).Enabled = false;
                        CommandManager.Instance.GetCommand(CommandNames.Inferior).Enabled = false;
                        CommandManager.Instance.GetCommand(CommandNames.Ascender).Enabled = false;
                        CommandManager.Instance.GetCommand(CommandNames.BackColor).Enabled = false;
                        CommandManager.Instance.GetCommand(CommandNames.ForeColor).Enabled = false;
                        CommandManager.Instance.GetCommand(CommandNames.RemoveFormat).Enabled = false;

                        MainTab.Instance.Panels[1].ButtonMoreEnabled = false;

                        CommandManager.Instance.GetCommand(CommandNames.JustifyLeft).Enabled = false;
                        CommandManager.Instance.GetCommand(CommandNames.JustifyCenter).Enabled = false;
                        CommandManager.Instance.GetCommand(CommandNames.JustifyRight).Enabled = false;
                        CommandManager.Instance.GetCommand(CommandNames.JustifyFull).Enabled = false;
                        CommandManager.Instance.GetCommand(CommandNames.Outdent).Enabled = false;
                        CommandManager.Instance.GetCommand(CommandNames.Indent).Enabled = false;
                        CommandManager.Instance.GetCommand(CommandNames.UnorderedList).Enabled = false;
                        CommandManager.Instance.GetCommand(CommandNames.OrderedList).Enabled = false;

                        CommandManager.Instance.GetCommand(CommandNames.Find).Enabled = false;
                        //CommandManager.Instance.GetCommand(CommandNames.Replace).Enabled = false;
                    }
                }
                else
                {
                    if (EditorObserver.HostEditorMode == Utils.Controls.HtmlEditing.Enums.HtmlEditingToolMode.Design &&
                        !Warehouse.Warehouse.IsHtmlSourceMode)
                    {
                        if (EditorObserver.ActiveEditor != null)
                        {
                            // Оптимизирует отрисовку.
                            if (!CommandManager.Instance.GetCommand(CommandNames.FontName).Enabled)
                            {
                                CommandManager.Instance.GetCommand(CommandNames.FontName).Enabled = true;
                                CommandManager.Instance.GetCommand(CommandNames.FontSize).Enabled = true;
                                //CommandManager.Instance.GetCommand(CommandNames.FontSizeUp).Enabled = true;
                                //CommandManager.Instance.GetCommand(CommandNames.FontSizeDown).Enabled = true;
                                CommandManager.Instance.GetCommand(CommandNames.Bold).Enabled = true;
                                CommandManager.Instance.GetCommand(CommandNames.Italic).Enabled = true;
                                CommandManager.Instance.GetCommand(CommandNames.Underline).Enabled = true;
                                CommandManager.Instance.GetCommand(CommandNames.StrikeThrough).Enabled = true;
                                CommandManager.Instance.GetCommand(CommandNames.Inferior).Enabled = true;
                                CommandManager.Instance.GetCommand(CommandNames.Ascender).Enabled = true;
                                CommandManager.Instance.GetCommand(CommandNames.BackColor).Enabled = true;
                                CommandManager.Instance.GetCommand(CommandNames.ForeColor).Enabled = true;
                                CommandManager.Instance.GetCommand(CommandNames.RemoveFormat).Enabled = true;

                                MainTab.Instance.Panels[1].ButtonMoreEnabled = true;

                                CommandManager.Instance.GetCommand(CommandNames.JustifyLeft).Enabled = true;
                                CommandManager.Instance.GetCommand(CommandNames.JustifyCenter).Enabled = true;
                                CommandManager.Instance.GetCommand(CommandNames.JustifyRight).Enabled = true;
                                CommandManager.Instance.GetCommand(CommandNames.JustifyFull).Enabled = true;
                                CommandManager.Instance.GetCommand(CommandNames.Outdent).Enabled = true;
                                CommandManager.Instance.GetCommand(CommandNames.Indent).Enabled = true;
                                CommandManager.Instance.GetCommand(CommandNames.UnorderedList).Enabled = true;
                                CommandManager.Instance.GetCommand(CommandNames.OrderedList).Enabled = true;

                                CommandManager.Instance.GetCommand(CommandNames.Find).Enabled = true;
                                //CommandManager.Instance.GetCommand(CommandNames.Replace).Enabled = true;
                            }
                        }
                    }
                    if (EditorObserver.HostEditorMode == Utils.Controls.HtmlEditing.Enums.HtmlEditingToolMode.Preview &&
                        !Warehouse.Warehouse.IsHtmlSourceMode)
                    {
                        // Оптимизирует отрисовку.
                        if (CommandManager.Instance.GetCommand(CommandNames.FontName).Enabled)
                        {
                            CommandManager.Instance.GetCommand(CommandNames.FontName).Enabled = false;
                            CommandManager.Instance.GetCommand(CommandNames.FontSize).Enabled = false;
                            //CommandManager.Instance.GetCommand(CommandNames.FontSizeUp).Enabled = false;
                            //CommandManager.Instance.GetCommand(CommandNames.FontSizeDown).Enabled = false;
                            CommandManager.Instance.GetCommand(CommandNames.Bold).Enabled = false;
                            CommandManager.Instance.GetCommand(CommandNames.Italic).Enabled = false;
                            CommandManager.Instance.GetCommand(CommandNames.Underline).Enabled = false;
                            CommandManager.Instance.GetCommand(CommandNames.StrikeThrough).Enabled = false;
                            CommandManager.Instance.GetCommand(CommandNames.Inferior).Enabled = false;
                            CommandManager.Instance.GetCommand(CommandNames.Ascender).Enabled = false;
                            CommandManager.Instance.GetCommand(CommandNames.BackColor).Enabled = false;
                            CommandManager.Instance.GetCommand(CommandNames.ForeColor).Enabled = false;
                            CommandManager.Instance.GetCommand(CommandNames.RemoveFormat).Enabled = false;

                            MainTab.Instance.Panels[1].ButtonMoreEnabled = false;

                            CommandManager.Instance.GetCommand(CommandNames.JustifyLeft).Enabled = false;
                            CommandManager.Instance.GetCommand(CommandNames.JustifyCenter).Enabled = false;
                            CommandManager.Instance.GetCommand(CommandNames.JustifyRight).Enabled = false;
                            CommandManager.Instance.GetCommand(CommandNames.JustifyFull).Enabled = false;
                            CommandManager.Instance.GetCommand(CommandNames.Outdent).Enabled = false;
                            CommandManager.Instance.GetCommand(CommandNames.Indent).Enabled = false;
                            CommandManager.Instance.GetCommand(CommandNames.UnorderedList).Enabled = false;
                            CommandManager.Instance.GetCommand(CommandNames.OrderedList).Enabled = false;

                            CommandManager.Instance.GetCommand(CommandNames.Find).Enabled = true;
                            //CommandManager.Instance.GetCommand(CommandNames.Replace).Enabled = false;
                        }
                    }
                    if (Warehouse.Warehouse.IsHtmlSourceMode)
                    {
                        // Оптимизирует отрисовку.
                        if (CommandManager.Instance.GetCommand(CommandNames.FontName).Enabled)
                        {
                            CommandManager.Instance.GetCommand(CommandNames.FontName).Enabled = false;
                            CommandManager.Instance.GetCommand(CommandNames.FontSize).Enabled = false;
                            //CommandManager.Instance.GetCommand(CommandNames.FontSizeUp).Enabled = false;
                            //CommandManager.Instance.GetCommand(CommandNames.FontSizeDown).Enabled = false;
                            CommandManager.Instance.GetCommand(CommandNames.Bold).Enabled = false;
                            CommandManager.Instance.GetCommand(CommandNames.Italic).Enabled = false;
                            CommandManager.Instance.GetCommand(CommandNames.Underline).Enabled = false;
                            CommandManager.Instance.GetCommand(CommandNames.StrikeThrough).Enabled = false;
                            CommandManager.Instance.GetCommand(CommandNames.Inferior).Enabled = false;
                            CommandManager.Instance.GetCommand(CommandNames.Ascender).Enabled = false;
                            CommandManager.Instance.GetCommand(CommandNames.BackColor).Enabled = false;
                            CommandManager.Instance.GetCommand(CommandNames.ForeColor).Enabled = false;
                            CommandManager.Instance.GetCommand(CommandNames.RemoveFormat).Enabled = false;

                            MainTab.Instance.Panels[1].ButtonMoreEnabled = false;

                            CommandManager.Instance.GetCommand(CommandNames.JustifyLeft).Enabled = false;
                            CommandManager.Instance.GetCommand(CommandNames.JustifyCenter).Enabled = false;
                            CommandManager.Instance.GetCommand(CommandNames.JustifyRight).Enabled = false;
                            CommandManager.Instance.GetCommand(CommandNames.JustifyFull).Enabled = false;
                            CommandManager.Instance.GetCommand(CommandNames.Outdent).Enabled = false;
                            CommandManager.Instance.GetCommand(CommandNames.Indent).Enabled = false;
                            CommandManager.Instance.GetCommand(CommandNames.UnorderedList).Enabled = false;
                            CommandManager.Instance.GetCommand(CommandNames.OrderedList).Enabled = false;

                            CommandManager.Instance.GetCommand(CommandNames.Find).Enabled = false;
                            //CommandManager.Instance.GetCommand(CommandNames.Replace).Enabled = false;
                        }
                    }
                }
            }
            catch { }
        }

        #endregion

        #region bufferPanelTimer_Tick

        private void bufferPanelTimer_Tick(object sender, EventArgs e)
        {
            try
            {
                if (!EditorObserver.RenderingStyle.Equals(Enums.RenderingStyle.EmptyEnvironment) &&
            !EditorObserver.RenderingStyle.Equals(Enums.RenderingStyle.NoActiveDocument) &&
            !Warehouse.Warehouse.IsHtmlSourceMode)
                {
                    if (EditorObserver.ActiveEditor != null)
                    {
                        CommandManager.Instance.GetCommand(CommandNames.Undo).Enabled = EditorObserver.ActiveEditor.CanUndo;
                        CommandManager.Instance.GetCommand(CommandNames.Redo).Enabled = EditorObserver.ActiveEditor.CanRedo;
                        CommandManager.Instance.GetCommand(CommandNames.Cut).Enabled = EditorObserver.ActiveEditor.CanCut;
                        CommandManager.Instance.GetCommand(CommandNames.Copy).Enabled = EditorObserver.ActiveEditor.CanCopy;

                        var data = Clipboard.GetDataObject();
                        if (EditorObserver.ActiveEditor.CanPaste ||
                            data.GetDataPresent(DataFormats.Bitmap))
                        {
                            CommandManager.Instance.GetCommand(CommandNames.Paste).Enabled = true;
                        }
                        else
                        {
                            CommandManager.Instance.GetCommand(CommandNames.Paste).Enabled = false;
                        }
                    }
                }
                else
                {
                    CommandManager.Instance.GetCommand(CommandNames.Undo).Enabled = false;
                    CommandManager.Instance.GetCommand(CommandNames.Redo).Enabled = false;
                    CommandManager.Instance.GetCommand(CommandNames.Cut).Enabled = false;
                    CommandManager.Instance.GetCommand(CommandNames.Copy).Enabled = false;
                    CommandManager.Instance.GetCommand(CommandNames.Paste).Enabled = false;
                }
            }
            catch { }
        }

        #endregion

        #region embeddingTabTimer_Tick

        private void embeddingTabTimer_Tick(object sender, EventArgs e)
        {
            try
            {
                if (!MainRibbon.Instance.ActiveTab.Equals(EmbeddingTab.Instance))
                {
                    return;
                }

                #region Кнопка Удалить ссылку

                if (EditorObserver.RenderingStyle.Equals(Enums.RenderingStyle.EmptyEnvironment) ||
                    EditorObserver.RenderingStyle.Equals(Enums.RenderingStyle.NoActiveDocument))
                {
                    CommandManager.Instance.GetCommand(CommandNames.DeleteLink).Enabled = false;
                }

                if (EditorObserver.RenderingStyle.Equals(Enums.RenderingStyle.TrainingModuleDocument))
                {
                    if (EditorObserver.HostEditorMode == Utils.Controls.HtmlEditing.Enums.HtmlEditingToolMode.Design)
                    {
                        if (EditorObserver.ActiveEditor.ActiveElement != null)
                        {
                            if (EditorObserver.ActiveEditor.ActiveElement.TagName.Equals(TagNames.AnchorTagName))
                            {
                                if (!EditorObserver.ActiveEditor.ActiveElement.GetAttribute("href").Equals(string.Empty))
                                {
                                    CommandManager.Instance.GetCommand(CommandNames.DeleteLink).Enabled = true;
                                }
                            }
                            else
                            {
                                CommandManager.Instance.GetCommand(CommandNames.DeleteLink).Enabled = false;
                            }
                        }
                    }
                    else if (EditorObserver.HostEditorMode == Utils.Controls.HtmlEditing.Enums.HtmlEditingToolMode.Preview)
                    {
                        CommandManager.Instance.GetCommand(CommandNames.DeleteLink).Enabled = false;
                    }
                }

                if (EditorObserver.RenderingStyle.Equals(Enums.RenderingStyle.QuestionDocument) ||
                    EditorObserver.RenderingStyle.Equals(Enums.RenderingStyle.ResponseDocument))
                {
                    if (EditorObserver.HostEditorMode == Utils.Controls.HtmlEditing.Enums.HtmlEditingToolMode.Design)
                    {
                        if (EditorObserver.ActiveEditor.ActiveElement != null)
                        {
                            if (EditorObserver.ActiveEditor.ActiveElement.TagName.Equals(TagNames.AnchorTagName))
                            {
                                if (!EditorObserver.ActiveEditor.ActiveElement.GetAttribute("href").Equals(string.Empty))
                                {
                                    CommandManager.Instance.GetCommand(CommandNames.DeleteLink).Enabled = true;
                                }
                            }
                            else
                            {
                                CommandManager.Instance.GetCommand(CommandNames.DeleteLink).Enabled = false;
                            }
                        }
                    }
                    else if (EditorObserver.HostEditorMode == Utils.Controls.HtmlEditing.Enums.HtmlEditingToolMode.Preview)
                    {
                        CommandManager.Instance.GetCommand(CommandNames.DeleteLink).Enabled = false;
                    }
                }

                #endregion

                if (EditorObserver.RenderingStyle.Equals(Enums.RenderingStyle.EmptyEnvironment) ||
                    EditorObserver.RenderingStyle.Equals(Enums.RenderingStyle.NoActiveDocument))
                {
                    // Оптимизирует отрисовку.
                    //if (CommandManager.Instance.GetCommand(CommandNames.Table).Enabled ||
                    //    CommandManager.Instance.GetCommand(CommandNames.Concept).Enabled)
                    {
                        CommandManager.Instance.GetCommand(CommandNames.Table).Enabled = false;

                        CommandManager.Instance.GetCommand(CommandNames.Picture).Enabled = false;
                        CommandManager.Instance.GetCommand(CommandNames.Animation).Enabled = false;
                        CommandManager.Instance.GetCommand(CommandNames.Audio).Enabled = false;
                        CommandManager.Instance.GetCommand(CommandNames.Video).Enabled = false;
                        CommandManager.Instance.GetCommand(CommandNames.StreamingVideo).Enabled = false;

                        CommandManager.Instance.GetCommand(CommandNames.Bookmark).Enabled = false;
                        CommandManager.Instance.GetCommand(CommandNames.Concept).Enabled = false;
                        CommandManager.Instance.GetCommand(CommandNames.Link).Enabled = false;
                        //CommandManager.Instance.GetCommand(CommandNames.CutLink).Enabled = false;
                        //CommandManager.Instance.GetCommand(CommandNames.DeleteLink).Enabled = false;

                        CommandManager.Instance.GetCommand(CommandNames.Style).Enabled = false;

                        CommandManager.Instance.GetCommand(CommandNames.Equation).Enabled = false;
                        CommandManager.Instance.GetCommand(CommandNames.Symbol).Enabled = false;

                        CommandManager.Instance.GetCommand(CommandNames.HorizontalRule).Enabled = false;
                        CommandManager.Instance.GetCommand(CommandNames.Break).Enabled = false;
                    }
                }

                if (Warehouse.Warehouse.IsHtmlSourceMode)
                {
                    CommandManager.Instance.GetCommand(CommandNames.Table).Enabled = false;

                    CommandManager.Instance.GetCommand(CommandNames.Picture).Enabled = false;
                    CommandManager.Instance.GetCommand(CommandNames.Animation).Enabled = false;
                    CommandManager.Instance.GetCommand(CommandNames.Audio).Enabled = false;
                    CommandManager.Instance.GetCommand(CommandNames.Video).Enabled = false;
                    CommandManager.Instance.GetCommand(CommandNames.StreamingVideo).Enabled = false;

                    CommandManager.Instance.GetCommand(CommandNames.Bookmark).Enabled = false;
                    CommandManager.Instance.GetCommand(CommandNames.Concept).Enabled = false;
                    CommandManager.Instance.GetCommand(CommandNames.Link).Enabled = false;
                    //CommandManager.Instance.GetCommand(CommandNames.CutLink).Enabled = false;
                    //CommandManager.Instance.GetCommand(CommandNames.DeleteLink).Enabled = false;

                    CommandManager.Instance.GetCommand(CommandNames.Style).Enabled = false;

                    CommandManager.Instance.GetCommand(CommandNames.Equation).Enabled = false;
                    CommandManager.Instance.GetCommand(CommandNames.Symbol).Enabled = false;

                    CommandManager.Instance.GetCommand(CommandNames.HorizontalRule).Enabled = false;
                    CommandManager.Instance.GetCommand(CommandNames.Break).Enabled = false;
                }

                if (EditorObserver.RenderingStyle.Equals(Enums.RenderingStyle.TrainingModuleDocument))
                {
                    if (EditorObserver.HostEditorMode == Utils.Controls.HtmlEditing.Enums.HtmlEditingToolMode.Design &&
                        !Warehouse.Warehouse.IsHtmlSourceMode)
                    {
                        if (EditorObserver.ActiveEditor.IsSelectionValid())
                        {
                            if (EditorObserver.ActiveEditor.IsSelection)
                            {
                                CommandManager.Instance.GetCommand(CommandNames.Table).Enabled = false;

                                CommandManager.Instance.GetCommand(CommandNames.Picture).Enabled = false;
                                CommandManager.Instance.GetCommand(CommandNames.Animation).Enabled = false;
                                CommandManager.Instance.GetCommand(CommandNames.Audio).Enabled = false;
                                CommandManager.Instance.GetCommand(CommandNames.Video).Enabled = false;
                                CommandManager.Instance.GetCommand(CommandNames.StreamingVideo).Enabled = false;

                                CommandManager.Instance.GetCommand(CommandNames.Bookmark).Enabled = false;
                                CommandManager.Instance.GetCommand(CommandNames.Concept).Enabled = false;//
                                CommandManager.Instance.GetCommand(CommandNames.Link).Enabled = true;
                                //CommandManager.Instance.GetCommand(CommandNames.CutLink).Enabled = false;
                                //CommandManager.Instance.GetCommand(CommandNames.DeleteLink).Enabled = false;

                                //  //CommandManager.Instance.GetCommand(CommandNames.SaveToImsQti).Enabled = true;
                                //  ////CommandManager.Instance.GetCommand(CommandNames.LoadFromImsQti).Enabled = true;

                                CommandManager.Instance.GetCommand(CommandNames.Style).Enabled = true;

                                CommandManager.Instance.GetCommand(CommandNames.Equation).Enabled = false;
                                CommandManager.Instance.GetCommand(CommandNames.Symbol).Enabled = false;

                                CommandManager.Instance.GetCommand(CommandNames.HorizontalRule).Enabled = false;
                                CommandManager.Instance.GetCommand(CommandNames.Break).Enabled = false;
                            }
                            else
                            {
                                CommandManager.Instance.GetCommand(CommandNames.Table).Enabled = true;

                                CommandManager.Instance.GetCommand(CommandNames.Picture).Enabled = true;
                                CommandManager.Instance.GetCommand(CommandNames.Animation).Enabled = true;
                                CommandManager.Instance.GetCommand(CommandNames.Audio).Enabled = true;
                                CommandManager.Instance.GetCommand(CommandNames.Video).Enabled = true;
                                CommandManager.Instance.GetCommand(CommandNames.StreamingVideo).Enabled = true;

                                CommandManager.Instance.GetCommand(CommandNames.Bookmark).Enabled = true;
                                CommandManager.Instance.GetCommand(CommandNames.Concept).Enabled = true;//
                                CommandManager.Instance.GetCommand(CommandNames.Link).Enabled = false;
                                //CommandManager.Instance.GetCommand(CommandNames.CutLink).Enabled = false;
                                //CommandManager.Instance.GetCommand(CommandNames.DeleteLink).Enabled = false;

                                CommandManager.Instance.GetCommand(CommandNames.Style).Enabled = false;

                                CommandManager.Instance.GetCommand(CommandNames.Equation).Enabled = true;
                                CommandManager.Instance.GetCommand(CommandNames.Symbol).Enabled = true;

                                CommandManager.Instance.GetCommand(CommandNames.HorizontalRule).Enabled = true;
                                CommandManager.Instance.GetCommand(CommandNames.Break).Enabled = true;
                            }
                        }
                        else
                        {
                            // Оптимизирует отрисовку.
                            if (CommandManager.Instance.GetCommand(CommandNames.Table).Enabled ||
                                CommandManager.Instance.GetCommand(CommandNames.Concept).Enabled)
                            {
                                CommandManager.Instance.GetCommand(CommandNames.Table).Enabled = false;

                                CommandManager.Instance.GetCommand(CommandNames.Picture).Enabled = false;
                                CommandManager.Instance.GetCommand(CommandNames.Animation).Enabled = false;
                                CommandManager.Instance.GetCommand(CommandNames.Audio).Enabled = false;
                                CommandManager.Instance.GetCommand(CommandNames.Video).Enabled = false;
                                CommandManager.Instance.GetCommand(CommandNames.StreamingVideo).Enabled = false;

                                CommandManager.Instance.GetCommand(CommandNames.Bookmark).Enabled = false;
                                CommandManager.Instance.GetCommand(CommandNames.Concept).Enabled = false;
                                CommandManager.Instance.GetCommand(CommandNames.Link).Enabled = false;
                                //CommandManager.Instance.GetCommand(CommandNames.CutLink).Enabled = false;
                                //CommandManager.Instance.GetCommand(CommandNames.DeleteLink).Enabled = false;

                                CommandManager.Instance.GetCommand(CommandNames.Style).Enabled = false;

                                CommandManager.Instance.GetCommand(CommandNames.Equation).Enabled = false;
                                CommandManager.Instance.GetCommand(CommandNames.Symbol).Enabled = false;

                                CommandManager.Instance.GetCommand(CommandNames.HorizontalRule).Enabled = false;
                                CommandManager.Instance.GetCommand(CommandNames.Break).Enabled = false;
                            }
                        }
                    }

                    if (EditorObserver.HostEditorMode == Utils.Controls.HtmlEditing.Enums.HtmlEditingToolMode.Preview &&
                        !Warehouse.Warehouse.IsHtmlSourceMode)
                    {
                        CommandManager.Instance.GetCommand(CommandNames.Table).Enabled = false;

                        CommandManager.Instance.GetCommand(CommandNames.Picture).Enabled = false;
                        CommandManager.Instance.GetCommand(CommandNames.Animation).Enabled = false;
                        CommandManager.Instance.GetCommand(CommandNames.Audio).Enabled = false;
                        CommandManager.Instance.GetCommand(CommandNames.Video).Enabled = false;
                        CommandManager.Instance.GetCommand(CommandNames.StreamingVideo).Enabled = false;

                        CommandManager.Instance.GetCommand(CommandNames.Bookmark).Enabled = false;
                        CommandManager.Instance.GetCommand(CommandNames.Concept).Enabled = false;
                        CommandManager.Instance.GetCommand(CommandNames.Link).Enabled = false;
                        //CommandManager.Instance.GetCommand(CommandNames.CutLink).Enabled = false;
                        //CommandManager.Instance.GetCommand(CommandNames.DeleteLink).Enabled = false;

                        CommandManager.Instance.GetCommand(CommandNames.Style).Enabled = false;

                        CommandManager.Instance.GetCommand(CommandNames.Equation).Enabled = false;
                        CommandManager.Instance.GetCommand(CommandNames.Symbol).Enabled = false;

                        CommandManager.Instance.GetCommand(CommandNames.HorizontalRule).Enabled = false;
                        CommandManager.Instance.GetCommand(CommandNames.Break).Enabled = false;
                    }
                }

                if (EditorObserver.RenderingStyle.Equals(Enums.RenderingStyle.QuestionDocument) ||
                    EditorObserver.RenderingStyle.Equals(Enums.RenderingStyle.ResponseDocument))
                {
                    if (EditorObserver.HostEditorMode == Utils.Controls.HtmlEditing.Enums.HtmlEditingToolMode.Design &&
                        !Warehouse.Warehouse.IsHtmlSourceMode)
                    {
                        if (EditorObserver.ActiveEditor.IsSelectionValid())
                        {
                            if (EditorObserver.ActiveEditor.IsSelection)
                            {
                                // Оптимизирует отрисовку.
                                //if (!CommandManager.Instance.GetCommand(CommandNames.Style).Enabled)
                                {
                                    CommandManager.Instance.GetCommand(CommandNames.Table).Enabled = false;

                                    CommandManager.Instance.GetCommand(CommandNames.Picture).Enabled = false;
                                    CommandManager.Instance.GetCommand(CommandNames.Animation).Enabled = false;
                                    CommandManager.Instance.GetCommand(CommandNames.Audio).Enabled = false;
                                    CommandManager.Instance.GetCommand(CommandNames.Video).Enabled = false;
                                    CommandManager.Instance.GetCommand(CommandNames.StreamingVideo).Enabled = false;

                                    CommandManager.Instance.GetCommand(CommandNames.Bookmark).Enabled = false;
                                    CommandManager.Instance.GetCommand(CommandNames.Concept).Enabled = false;
                                    CommandManager.Instance.GetCommand(CommandNames.Link).Enabled = false;
                                    //CommandManager.Instance.GetCommand(CommandNames.CutLink).Enabled = false;
                                    //CommandManager.Instance.GetCommand(CommandNames.DeleteLink).Enabled = false;

                                    CommandManager.Instance.GetCommand(CommandNames.Style).Enabled = true;

                                    CommandManager.Instance.GetCommand(CommandNames.Equation).Enabled = false;
                                    CommandManager.Instance.GetCommand(CommandNames.Symbol).Enabled = false;

                                    CommandManager.Instance.GetCommand(CommandNames.HorizontalRule).Enabled = false;
                                    CommandManager.Instance.GetCommand(CommandNames.Break).Enabled = false;
                                }
                            }
                            else
                            {
                                // Оптимизирует отрисовку.
                                //if (!CommandManager.Instance.GetCommand(CommandNames.Table).Enabled)
                                {
                                    CommandManager.Instance.GetCommand(CommandNames.Table).Enabled = true;

                                    CommandManager.Instance.GetCommand(CommandNames.Picture).Enabled = true;
                                    CommandManager.Instance.GetCommand(CommandNames.Animation).Enabled = true;
                                    CommandManager.Instance.GetCommand(CommandNames.Audio).Enabled = true;
                                    CommandManager.Instance.GetCommand(CommandNames.Video).Enabled = true;
                                    CommandManager.Instance.GetCommand(CommandNames.StreamingVideo).Enabled = true;

                                    CommandManager.Instance.GetCommand(CommandNames.Bookmark).Enabled = false;
                                    CommandManager.Instance.GetCommand(CommandNames.Concept).Enabled = false;
                                    CommandManager.Instance.GetCommand(CommandNames.Link).Enabled = false;
                                    //CommandManager.Instance.GetCommand(CommandNames.CutLink).Enabled = false;
                                    //CommandManager.Instance.GetCommand(CommandNames.DeleteLink).Enabled = false;

                                    CommandManager.Instance.GetCommand(CommandNames.Style).Enabled = false;

                                    CommandManager.Instance.GetCommand(CommandNames.Equation).Enabled = true;
                                    CommandManager.Instance.GetCommand(CommandNames.Symbol).Enabled = true;

                                    CommandManager.Instance.GetCommand(CommandNames.HorizontalRule).Enabled = true;
                                    CommandManager.Instance.GetCommand(CommandNames.Break).Enabled = true;
                                }
                            }
                        }
                        else
                        {
                            CommandManager.Instance.GetCommand(CommandNames.Table).Enabled = false;

                            CommandManager.Instance.GetCommand(CommandNames.Picture).Enabled = false;
                            CommandManager.Instance.GetCommand(CommandNames.Animation).Enabled = false;
                            CommandManager.Instance.GetCommand(CommandNames.Audio).Enabled = false;
                            CommandManager.Instance.GetCommand(CommandNames.Video).Enabled = false;
                            CommandManager.Instance.GetCommand(CommandNames.StreamingVideo).Enabled = false;

                            CommandManager.Instance.GetCommand(CommandNames.Bookmark).Enabled = false;
                            CommandManager.Instance.GetCommand(CommandNames.Concept).Enabled = false;
                            CommandManager.Instance.GetCommand(CommandNames.Link).Enabled = false;
                            //CommandManager.Instance.GetCommand(CommandNames.CutLink).Enabled = false;
                            //CommandManager.Instance.GetCommand(CommandNames.DeleteLink).Enabled = false;

                            CommandManager.Instance.GetCommand(CommandNames.Style).Enabled = false;

                            CommandManager.Instance.GetCommand(CommandNames.Equation).Enabled = false;
                            CommandManager.Instance.GetCommand(CommandNames.Symbol).Enabled = false;

                            CommandManager.Instance.GetCommand(CommandNames.HorizontalRule).Enabled = false;
                            CommandManager.Instance.GetCommand(CommandNames.Break).Enabled = false;
                        }
                    }
                    else if (EditorObserver.HostEditorMode == Utils.Controls.HtmlEditing.Enums.HtmlEditingToolMode.Preview &&
                        !Warehouse.Warehouse.IsHtmlSourceMode)
                    {
                        CommandManager.Instance.GetCommand(CommandNames.Table).Enabled = false;

                        CommandManager.Instance.GetCommand(CommandNames.Picture).Enabled = false;
                        CommandManager.Instance.GetCommand(CommandNames.Animation).Enabled = false;
                        CommandManager.Instance.GetCommand(CommandNames.Audio).Enabled = false;
                        CommandManager.Instance.GetCommand(CommandNames.Video).Enabled = false;
                        CommandManager.Instance.GetCommand(CommandNames.StreamingVideo).Enabled = false;

                        CommandManager.Instance.GetCommand(CommandNames.Bookmark).Enabled = false;
                        CommandManager.Instance.GetCommand(CommandNames.Concept).Enabled = false;
                        CommandManager.Instance.GetCommand(CommandNames.Link).Enabled = false;
                        //CommandManager.Instance.GetCommand(CommandNames.CutLink).Enabled = false;
                        //CommandManager.Instance.GetCommand(CommandNames.DeleteLink).Enabled = false;

                        CommandManager.Instance.GetCommand(CommandNames.Style).Enabled = false;

                        CommandManager.Instance.GetCommand(CommandNames.Equation).Enabled = false;
                        CommandManager.Instance.GetCommand(CommandNames.Symbol).Enabled = false;

                        CommandManager.Instance.GetCommand(CommandNames.HorizontalRule).Enabled = false;
                        CommandManager.Instance.GetCommand(CommandNames.Break).Enabled = false;
                    }
                }
            }
            catch { }
        }

        #endregion

        #region structurePanelTimer_Tick

        private void structurePanelTimer_Tick(object sender, EventArgs e)
        {
            try
            {
                if (!MainForm.Instance.Visible)
                {
                    return;
                }

                if (!EditorObserver.RenderingStyle.Equals(Enums.RenderingStyle.EmptyEnvironment))
                {
                    if (EditorObserver.HostEditorMode == Utils.Controls.HtmlEditing.Enums.HtmlEditingToolMode.Design)
                    {
                        var cn = Warehouse.Warehouse.Instance.CourseTree.CurrentNode;

                        if (cn == null)
                        {
                            CommandManager.Instance.GetCommand(CommandNames.AddItem).Enabled = false;
                            CommandManager.Instance.GetCommand(CommandNames.SaveToImsQti).Enabled = false;
                            CommandManager.Instance.GetCommand(CommandNames.LoadFromImsQti).Enabled = false;
                            return;
                        }

                        if (cn is TestModule ||
                            cn is Question)
                        {
                            CommandManager.Instance.GetCommand(CommandNames.SaveToImsQti).Enabled = true;
                        }
                        else
                        {
                            CommandManager.Instance.GetCommand(CommandNames.SaveToImsQti).Enabled = false;
                        }

                        if (cn is TrainingModule ||
                            cn is TestModule ||
                            cn is Group ||
                            cn is CourseRoot)
                        {
                            CommandManager.Instance.GetCommand(CommandNames.LoadFromImsQti).Enabled = true;
                        }
                        else
                        {
                            CommandManager.Instance.GetCommand(CommandNames.LoadFromImsQti).Enabled = false;
                        }

                        if (cn is InConceptParent ||
                            cn is OutConceptParent ||
                            cn is InDummyConcept ||
                            cn is OutDummyConcept ||
                            cn is Response)
                        {
                            if (CommandManager.Instance.GetCommand(CommandNames.AddItem).Enabled)
                            {
                                CommandManager.Instance.GetCommand(CommandNames.AddItem).Enabled = false;
                                //CommandManager.Instance.GetCommand(CommandNames.SaveToImsQti).Enabled = false;
                                //CommandManager.Instance.GetCommand(CommandNames.LoadFromImsQti).Enabled = false;
                            }
                        }
                        else
                        {
                            if (!CommandManager.Instance.GetCommand(CommandNames.AddItem).Enabled)
                            {
                                CommandManager.Instance.GetCommand(CommandNames.AddItem).Enabled = true;

                                // CommandManager.Instance.GetCommand(CommandNames.SaveToImsQti).Enabled = true;


                            }
                        }


                        if (cn is TestModule ||
                            cn is Group ||
                            cn is Question)
                        {
                            CommandManager.Instance.GetCommand(CommandNames.ItemOptions).Enabled = true;
                        }
                        else
                        {
                            CommandManager.Instance.GetCommand(CommandNames.ItemOptions).Enabled = false;
                        }

                        if (cn is TrainingModule ||
                            cn is TestModule ||
                            cn is Group ||
                            cn is Question ||
                            cn is Response ||
                            cn is InDummyConcept)
                        {
                            CommandManager.Instance.GetCommand(CommandNames.DeleteItem).Enabled = true;
                        }
                        else
                        {
                            CommandManager.Instance.GetCommand(CommandNames.DeleteItem).Enabled = false;
                        }
                    }
                    else if (EditorObserver.HostEditorMode == Utils.Controls.HtmlEditing.Enums.HtmlEditingToolMode.Preview)
                    {
                        CommandManager.Instance.GetCommand(CommandNames.AddItem).Enabled = false;
                        CommandManager.Instance.GetCommand(CommandNames.SaveToImsQti).Enabled = false;
                         CommandManager.Instance.GetCommand(CommandNames.LoadFromImsQti).Enabled = false;
                        CommandManager.Instance.GetCommand(CommandNames.ItemOptions).Enabled = false;
                        CommandManager.Instance.GetCommand(CommandNames.DeleteItem).Enabled = false;
                    }
                }
                else
                {
                    CommandManager.Instance.GetCommand(CommandNames.AddItem).Enabled = false;
                    //CommandManager.Instance.GetCommand(CommandNames.SaveToImsQti).Enabled = false;
                    //CommandManager.Instance.GetCommand(CommandNames.LoadFromImsQti).Enabled = false;
                    CommandManager.Instance.GetCommand(CommandNames.ItemOptions).Enabled = false;
                    CommandManager.Instance.GetCommand(CommandNames.DeleteItem).Enabled = false;
                }
            }
            catch { }
        }

        #endregion

        #region conceptPanelTimer_Tick

        private void conceptPanelTimer_Tick(object sender, EventArgs e)
        {
            try
            {
                if (EditorObserver.RenderingStyle.Equals(Enums.RenderingStyle.EmptyEnvironment))
                {
                    CommandManager.Instance.GetCommand(CommandNames.NavigateToConcept).Enabled = false;
                    CommandManager.Instance.GetCommand(CommandNames.Profile).Checked = false;
                    CommandManager.Instance.GetCommand(CommandNames.Profile).Enabled = false;
                    CommandManager.Instance.GetCommand(CommandNames.ProfileOptions).Enabled = false;
                    CommandManager.Instance.GetCommand(CommandNames.ProfileOptionsSmall).Enabled = false;
                    CommandManager.Instance.GetCommand(CommandNames.DeleteConcept).Enabled = false;
                }
                else
                {
                    var c = Warehouse.Warehouse.Instance.ConceptTree.CurrentNode;

                    if (EditorObserver.HostEditorMode == Utils.Controls.HtmlEditing.Enums.HtmlEditingToolMode.Design)
                    {
                        if (c != null)
                        {
                            CommandManager.Instance.GetCommand(CommandNames.NavigateToConcept).Enabled = true;

                            CommandManager.Instance.GetCommand(CommandNames.Profile).Enabled = true;
                            if (c.IsProfile)
                            {
                                CommandManager.Instance.GetCommand(CommandNames.Profile).Checked = true;
                                CommandManager.Instance.GetCommand(CommandNames.ProfileOptions).Enabled = true;
                                CommandManager.Instance.GetCommand(CommandNames.ProfileOptionsSmall).Enabled = true;
                            }
                            else
                            {
                                CommandManager.Instance.GetCommand(CommandNames.Profile).Checked = false;
                                CommandManager.Instance.GetCommand(CommandNames.ProfileOptions).Enabled = false;
                                CommandManager.Instance.GetCommand(CommandNames.ProfileOptionsSmall).Enabled = false;
                            }

                            CommandManager.Instance.GetCommand(CommandNames.DeleteConcept).Enabled = true;
                        }
                        else
                        {
                            CommandManager.Instance.GetCommand(CommandNames.NavigateToConcept).Enabled = false;
                            CommandManager.Instance.GetCommand(CommandNames.Profile).Checked = false;
                            CommandManager.Instance.GetCommand(CommandNames.Profile).Enabled = false;
                            CommandManager.Instance.GetCommand(CommandNames.ProfileOptions).Enabled = false;
                            CommandManager.Instance.GetCommand(CommandNames.ProfileOptionsSmall).Enabled = false;
                            CommandManager.Instance.GetCommand(CommandNames.DeleteConcept).Enabled = false;
                        }
                    }
                    else
                    {
                        if (c != null)
                        {
                            CommandManager.Instance.GetCommand(CommandNames.NavigateToConcept).Enabled = true;
                            CommandManager.Instance.GetCommand(CommandNames.Profile).Checked = false;
                            CommandManager.Instance.GetCommand(CommandNames.Profile).Enabled = false;
                            CommandManager.Instance.GetCommand(CommandNames.ProfileOptions).Enabled = false;
                            CommandManager.Instance.GetCommand(CommandNames.ProfileOptionsSmall).Enabled = false;
                            CommandManager.Instance.GetCommand(CommandNames.DeleteConcept).Enabled = false;
                        }
                        else
                        {
                            CommandManager.Instance.GetCommand(CommandNames.NavigateToConcept).Enabled = false;
                            CommandManager.Instance.GetCommand(CommandNames.Profile).Checked = false;
                            CommandManager.Instance.GetCommand(CommandNames.Profile).Enabled = false;
                            CommandManager.Instance.GetCommand(CommandNames.ProfileOptions).Enabled = false;
                            CommandManager.Instance.GetCommand(CommandNames.ProfileOptionsSmall).Enabled = false;
                            CommandManager.Instance.GetCommand(CommandNames.DeleteConcept).Enabled = false;
                        }
                    }
                }
            }
            catch { }
        }

        #endregion

        #region fontStyleTimer_Tick

        private void fontStyleTimer_Tick(object sender, EventArgs e)
        {

        }

        #endregion

        #region editorContextMenuTimer_Tick

        private void editorContextMenuTimer_Tick(object sender, EventArgs e)
        {
            try
            {
                if (EditorObserver.RenderingStyle.Equals(Enums.RenderingStyle.TrainingModuleDocument))
                {
                    if (EditorObserver.HostEditorMode == Utils.Controls.HtmlEditing.Enums.HtmlEditingToolMode.Design)
                    {
                        if (EditorObserver.ActiveEditor.IsSelectionValid())
                        {
                            if (EditorObserver.ActiveEditor.IsSelection)
                            {
                                // Оптимизирует отрисовку.
                                if (CommandManager.Instance.GetCommand(CommandNames.TableSmall).Enabled)
                                {
                                    CommandManager.Instance.GetCommand(CommandNames.TableSmall).Enabled = false;

                                    CommandManager.Instance.GetCommand(CommandNames.PictureSmall).Enabled = false;
                                    CommandManager.Instance.GetCommand(CommandNames.AnimationSmall).Enabled = false;
                                    CommandManager.Instance.GetCommand(CommandNames.AudioSmall).Enabled = false;
                                    CommandManager.Instance.GetCommand(CommandNames.VideoSmall).Enabled = false;

                                    CommandManager.Instance.GetCommand(CommandNames.BookmarkSmall).Enabled = false;
                                    CommandManager.Instance.GetCommand(CommandNames.ConceptSmall).Enabled = false;//
                                    CommandManager.Instance.GetCommand(CommandNames.LinkSmall).Enabled = true;

                                    CommandManager.Instance.GetCommand(CommandNames.StyleSmall).Enabled = true;

                                    CommandManager.Instance.GetCommand(CommandNames.EquationSmall).Enabled = false;
                                    CommandManager.Instance.GetCommand(CommandNames.SymbolSmall).Enabled = false;
                                }
                            }
                            else
                            {
                                // Оптимизирует отрисовку.
                                if (!CommandManager.Instance.GetCommand(CommandNames.TableSmall).Enabled)
                                {
                                    CommandManager.Instance.GetCommand(CommandNames.TableSmall).Enabled = true;

                                    CommandManager.Instance.GetCommand(CommandNames.PictureSmall).Enabled = true;
                                    CommandManager.Instance.GetCommand(CommandNames.AnimationSmall).Enabled = true;
                                    CommandManager.Instance.GetCommand(CommandNames.AudioSmall).Enabled = true;
                                    CommandManager.Instance.GetCommand(CommandNames.VideoSmall).Enabled = true;

                                    CommandManager.Instance.GetCommand(CommandNames.BookmarkSmall).Enabled = true;
                                    CommandManager.Instance.GetCommand(CommandNames.ConceptSmall).Enabled = true;//
                                    CommandManager.Instance.GetCommand(CommandNames.LinkSmall).Enabled = false;

                                    CommandManager.Instance.GetCommand(CommandNames.StyleSmall).Enabled = false;

                                    CommandManager.Instance.GetCommand(CommandNames.EquationSmall).Enabled = true;
                                    CommandManager.Instance.GetCommand(CommandNames.SymbolSmall).Enabled = true;
                                }
                            }
                        }
                        else
                        {
                            // Оптимизирует отрисовку.
                            if (CommandManager.Instance.GetCommand(CommandNames.TableSmall).Enabled)
                            {
                                CommandManager.Instance.GetCommand(CommandNames.TableSmall).Enabled = false;

                                CommandManager.Instance.GetCommand(CommandNames.PictureSmall).Enabled = false;
                                CommandManager.Instance.GetCommand(CommandNames.AnimationSmall).Enabled = false;
                                CommandManager.Instance.GetCommand(CommandNames.AudioSmall).Enabled = false;
                                CommandManager.Instance.GetCommand(CommandNames.VideoSmall).Enabled = false;

                                CommandManager.Instance.GetCommand(CommandNames.BookmarkSmall).Enabled = false;
                                CommandManager.Instance.GetCommand(CommandNames.ConceptSmall).Enabled = false;
                                CommandManager.Instance.GetCommand(CommandNames.LinkSmall).Enabled = false;

                                CommandManager.Instance.GetCommand(CommandNames.StyleSmall).Enabled = false;

                                CommandManager.Instance.GetCommand(CommandNames.EquationSmall).Enabled = false;
                                CommandManager.Instance.GetCommand(CommandNames.SymbolSmall).Enabled = false;
                            }
                        }
                    }
                }

                if (EditorObserver.RenderingStyle.Equals(Enums.RenderingStyle.QuestionDocument) ||
                    EditorObserver.RenderingStyle.Equals(Enums.RenderingStyle.ResponseDocument))
                {
                    if (EditorObserver.HostEditorMode == Utils.Controls.HtmlEditing.Enums.HtmlEditingToolMode.Design)
                    {
                        if (EditorObserver.ActiveEditor.IsSelectionValid())
                        {
                            if (EditorObserver.ActiveEditor.IsSelection)
                            {
                                // Оптимизирует отрисовку.
                                if (CommandManager.Instance.GetCommand(CommandNames.TableSmall).Enabled)
                                {
                                    CommandManager.Instance.GetCommand(CommandNames.TableSmall).Enabled = false;

                                    CommandManager.Instance.GetCommand(CommandNames.PictureSmall).Enabled = false;
                                    CommandManager.Instance.GetCommand(CommandNames.AnimationSmall).Enabled = false;
                                    CommandManager.Instance.GetCommand(CommandNames.AudioSmall).Enabled = false;
                                    CommandManager.Instance.GetCommand(CommandNames.VideoSmall).Enabled = false;

                                    CommandManager.Instance.GetCommand(CommandNames.BookmarkSmall).Enabled = false;
                                    CommandManager.Instance.GetCommand(CommandNames.ConceptSmall).Enabled = false;
                                    CommandManager.Instance.GetCommand(CommandNames.LinkSmall).Enabled = false;

                                    CommandManager.Instance.GetCommand(CommandNames.StyleSmall).Enabled = true;

                                    CommandManager.Instance.GetCommand(CommandNames.EquationSmall).Enabled = false;
                                    CommandManager.Instance.GetCommand(CommandNames.SymbolSmall).Enabled = false;
                                }
                            }
                            else
                            {
                                // Оптимизирует отрисовку.
                                if (!CommandManager.Instance.GetCommand(CommandNames.TableSmall).Enabled)
                                {
                                    CommandManager.Instance.GetCommand(CommandNames.TableSmall).Enabled = true;

                                    CommandManager.Instance.GetCommand(CommandNames.PictureSmall).Enabled = true;
                                    CommandManager.Instance.GetCommand(CommandNames.AnimationSmall).Enabled = true;
                                    CommandManager.Instance.GetCommand(CommandNames.AudioSmall).Enabled = true;
                                    CommandManager.Instance.GetCommand(CommandNames.VideoSmall).Enabled = true;

                                    CommandManager.Instance.GetCommand(CommandNames.BookmarkSmall).Enabled = false;
                                    CommandManager.Instance.GetCommand(CommandNames.ConceptSmall).Enabled = false;
                                    CommandManager.Instance.GetCommand(CommandNames.LinkSmall).Enabled = false;

                                    CommandManager.Instance.GetCommand(CommandNames.StyleSmall).Enabled = false;

                                    CommandManager.Instance.GetCommand(CommandNames.EquationSmall).Enabled = true;
                                    CommandManager.Instance.GetCommand(CommandNames.SymbolSmall).Enabled = true;
                                }
                            }
                        }
                        else
                        {
                            // Оптимизирует отрисовку.
                            if (CommandManager.Instance.GetCommand(CommandNames.TableSmall).Enabled)
                            {
                                CommandManager.Instance.GetCommand(CommandNames.TableSmall).Enabled = false;

                                CommandManager.Instance.GetCommand(CommandNames.PictureSmall).Enabled = false;
                                CommandManager.Instance.GetCommand(CommandNames.AnimationSmall).Enabled = false;
                                CommandManager.Instance.GetCommand(CommandNames.AudioSmall).Enabled = false;
                                CommandManager.Instance.GetCommand(CommandNames.VideoSmall).Enabled = false;

                                CommandManager.Instance.GetCommand(CommandNames.BookmarkSmall).Enabled = false;
                                CommandManager.Instance.GetCommand(CommandNames.ConceptSmall).Enabled = false;
                                CommandManager.Instance.GetCommand(CommandNames.LinkSmall).Enabled = false;

                                CommandManager.Instance.GetCommand(CommandNames.StyleSmall).Enabled = false;

                                CommandManager.Instance.GetCommand(CommandNames.EquationSmall).Enabled = false;
                                CommandManager.Instance.GetCommand(CommandNames.SymbolSmall).Enabled = false;
                            }
                        }
                    }
                }
            }
            catch { }
        }

        #endregion

        #region hintDialogEmbeddingMenuTimer_Tick

        private void hintDialogEmbeddingMenuTimer_Tick(object sender, EventArgs e)
        {
            try
            {
                if (EditorObserver.ActiveEditor == null)
                {
                    return;
                }

                CommandManager.Instance.GetCommand(CommandNames.HintUndo).Enabled = EditorObserver.ActiveEditor.CanUndo;
                CommandManager.Instance.GetCommand(CommandNames.HintRedo).Enabled = EditorObserver.ActiveEditor.CanRedo;
                CommandManager.Instance.GetCommand(CommandNames.HintCut).Enabled = EditorObserver.ActiveEditor.CanCut;
                CommandManager.Instance.GetCommand(CommandNames.HintCopy).Enabled = EditorObserver.ActiveEditor.CanCopy;

                var data = Clipboard.GetDataObject();
                if (EditorObserver.ActiveEditor.CanPaste ||
                    data.GetDataPresent(DataFormats.Bitmap))
                {
                    CommandManager.Instance.GetCommand(CommandNames.HintPaste).Enabled = true;
                }
                else
                {
                    CommandManager.Instance.GetCommand(CommandNames.HintPaste).Enabled = false;
                }

                if (EditorObserver.ActiveEditor.IsSelectionValid())
                {
                    if (EditorObserver.ActiveEditor.IsSelection)
                    {
                        CommandManager.Instance.GetCommand(CommandNames.HintTable).Enabled = false;
                        CommandManager.Instance.GetCommand(CommandNames.HintTableSmall).Enabled = false;

                        CommandManager.Instance.GetCommand(CommandNames.HintPicture).Enabled = false;
                        CommandManager.Instance.GetCommand(CommandNames.HintPictureSmall).Enabled = false;
                        CommandManager.Instance.GetCommand(CommandNames.HintAnimation).Enabled = false;
                        CommandManager.Instance.GetCommand(CommandNames.HintAnimationSmall).Enabled = false;

                        CommandManager.Instance.GetCommand(CommandNames.HintStyle).Enabled = true;
                        CommandManager.Instance.GetCommand(CommandNames.HintStyleSmall).Enabled = true;

                        CommandManager.Instance.GetCommand(CommandNames.HintEquation).Enabled = false;
                        CommandManager.Instance.GetCommand(CommandNames.HintEquationSmall).Enabled = false;
                        CommandManager.Instance.GetCommand(CommandNames.HintSymbol).Enabled = false;
                        CommandManager.Instance.GetCommand(CommandNames.HintSymbolSmall).Enabled = false;
                    }
                    else
                    {
                        CommandManager.Instance.GetCommand(CommandNames.HintTable).Enabled = true;
                        CommandManager.Instance.GetCommand(CommandNames.HintTableSmall).Enabled = true;

                        CommandManager.Instance.GetCommand(CommandNames.HintPicture).Enabled = true;
                        CommandManager.Instance.GetCommand(CommandNames.HintPictureSmall).Enabled = true;
                        CommandManager.Instance.GetCommand(CommandNames.HintAnimation).Enabled = true;
                        CommandManager.Instance.GetCommand(CommandNames.HintAnimationSmall).Enabled = true;

                        CommandManager.Instance.GetCommand(CommandNames.HintStyle).Enabled = false;
                        CommandManager.Instance.GetCommand(CommandNames.HintStyleSmall).Enabled = false;

                        CommandManager.Instance.GetCommand(CommandNames.HintEquation).Enabled = true;
                        CommandManager.Instance.GetCommand(CommandNames.HintEquationSmall).Enabled = true;
                        CommandManager.Instance.GetCommand(CommandNames.HintSymbol).Enabled = true;
                        CommandManager.Instance.GetCommand(CommandNames.HintSymbolSmall).Enabled = true;
                    }
                }
                else
                {
                    // Оптимизирует отрисовку.
                    //if (CommandManager.Instance.GetCommand(CommandNames.Table).Enabled ||
                    //    CommandManager.Instance.GetCommand(CommandNames.Concept).Enabled)
                    //{
                    CommandManager.Instance.GetCommand(CommandNames.HintTable).Enabled = false;
                    CommandManager.Instance.GetCommand(CommandNames.HintTableSmall).Enabled = false;

                    CommandManager.Instance.GetCommand(CommandNames.HintPicture).Enabled = false;
                    CommandManager.Instance.GetCommand(CommandNames.HintPictureSmall).Enabled = false;
                    CommandManager.Instance.GetCommand(CommandNames.HintAnimation).Enabled = false;
                    CommandManager.Instance.GetCommand(CommandNames.HintAnimationSmall).Enabled = false;

                    CommandManager.Instance.GetCommand(CommandNames.HintStyle).Enabled = false;
                    CommandManager.Instance.GetCommand(CommandNames.HintStyleSmall).Enabled = false;

                    CommandManager.Instance.GetCommand(CommandNames.HintEquation).Enabled = false;
                    CommandManager.Instance.GetCommand(CommandNames.HintEquationSmall).Enabled = false;
                    CommandManager.Instance.GetCommand(CommandNames.HintSymbol).Enabled = false;
                    CommandManager.Instance.GetCommand(CommandNames.HintSymbolSmall).Enabled = false;
                    //}
                }
            }
            catch { }
        }

        #endregion

        #region StartRendering

        public void StartRendering()
        {
            projectTimer.Enabled = true;
            editorModesPanelTimer.Enabled = true;
            mainTabTimer.Enabled = true;
            bufferPanelTimer.Enabled = true;
            embeddingTabTimer.Enabled = true;
            structurePanelTimer.Enabled = true;
            conceptPanelTimer.Enabled = true;
            fontStyleTimer.Enabled = true;
            editorContextMenuTimer.Enabled = true;
        }

        public void StartHintDialogRendering()
        {
            hintDialogEmbeddingMenuTimer.Enabled = true;
        }

        #endregion

        #region StopRendering

        public void StopRendering()
        {
            projectTimer.Enabled = false;
            editorModesPanelTimer.Enabled = false;
            mainTabTimer.Enabled = false;
            bufferPanelTimer.Enabled = false;
            embeddingTabTimer.Enabled = false;
            structurePanelTimer.Enabled = false;
            conceptPanelTimer.Enabled = false;
            fontStyleTimer.Enabled = false;
            editorContextMenuTimer.Enabled = false;
        }

        public void StopHintDialogRendering()
        {
            hintDialogEmbeddingMenuTimer.Enabled = false;
        }

        #endregion
    }
}