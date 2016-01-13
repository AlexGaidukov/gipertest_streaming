using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using mshtml;
using VisualEditor.Utils.Helpers;

namespace VisualEditor.Utils.Controls.HtmlEditing
{
    /// <summary>
    /// Класс средства визуального редактирования.
    /// </summary>
    public class HtmlEditingTool : WebBrowser
    {
        private const int minFontSize = 1;
        private const int maxFontSize = 7;
        private string defaultHtml;

        #region Конструктор
        
        public HtmlEditingTool()
        {
            DocumentText = Defaults.Html;
            AllowNavigation = false;
            AllowWebBrowserDrop = false;
            IsWebBrowserContextMenuEnabled = false;
            ScriptErrorsSuppressed = true;
            mode = Enums.HtmlEditingToolMode.Preview;
            ActiveElement = Document.Body;
        }

        #endregion

        #region Инициализация html кода средства визуального редактирования

        public void SetBaseTag(Dictionary<string, string> attributes)
        {
            if (attributes.IsNull())
            {
                throw new ArgumentNullException();
            }

            var baseElement = Document.CreateElement(TagNames.BaseTagName);
            foreach (KeyValuePair<string, string> attribute in attributes)
            {
                baseElement.SetAttribute(attribute.Key, attribute.Value);
            }

            var headElement = GetElementsByTagName(TagNames.HeadTagName)[0];
            headElement.AppendChild(baseElement);
        }

        public void SetLinkTag(Dictionary<string, string> attributes)
        {
            if (attributes.IsNull())
            {
                throw new ArgumentNullException();
            }

            var linkElement = Document.CreateElement(TagNames.LinkTagName);
            foreach (KeyValuePair<string, string> attribute in attributes)
            {
                linkElement.SetAttribute(attribute.Key, attribute.Value);
            }

            var bodyElement = GetElementsByTagName(TagNames.BodyTagName)[0];
            bodyElement.InsertAdjacentElement(HtmlElementInsertionOrientation.BeforeBegin, linkElement);
        }

        #endregion

        #region Режим средства визуального редактирования
        
        private Enums.HtmlEditingToolMode mode;

        public event EventHandler HtmlEditingToolModeChanging;
        public event EventHandler HtmlEditingToolModeChanged;

        public Enums.HtmlEditingToolMode Mode
        {
            get { return mode; }
            set
            {
                OnHtmlEditingToolModeChanging();

                if (value == Enums.HtmlEditingToolMode.Design)
                {
                    SetDesignMode();
                }
                else if (value == Enums.HtmlEditingToolMode.Preview)
                {
                    SetPreviewMode();
                }
                mode = value;
                RestoreHtml();

                OnHtmlEditingToolModeChanged();
            }
        }

        public virtual void OnHtmlEditingToolModeChanging()
        {
            if (HtmlEditingToolModeChanging.IsNull())
            {
                return;
            }

            HtmlEditingToolModeChanging(this, EventArgs.Empty);
        }

        public virtual void OnHtmlEditingToolModeChanged()
        {
            if (HtmlEditingToolModeChanged.IsNull())
            {
                return;
            }

            HtmlEditingToolModeChanged(this, EventArgs.Empty);
        }

        private void SetDesignMode()
        {
            if (Document.IsNull())
            {
                throw new NullReferenceException();
            }

            //HtmlEditingToolHelper.WaitForIdleness(this);

            try
            {
                Document.ExecCommand(CommandIdentifiers.EditModeId, false, null);
            }
            catch
            {
                throw new InvalidOperationException(CommandIdentifiers.EditModeId);
            }

            HtmlEditingToolHelper.WaitForValidBody(this);
            AttachEventHandlers();
            //SetLiveResize(true);
        }

        private void SetPreviewMode()
        {
            if (Document.IsNull())
            {
                throw new NullReferenceException();
            }

            //HtmlEditingToolHelper.WaitForIdleness(this);

            try
            {
                Document.ExecCommand(CommandIdentifiers.PreviewModeId, false, null);
            }
            catch
            {
                throw new InvalidOperationException(CommandIdentifiers.PreviewModeId);
            }

            HtmlEditingToolHelper.WaitForValidBody(this);
            AttachEventHandlers();
            //SetLiveResize(false);
        }

        #endregion

        #region Html код средства визуального редактирования

        public event EventHandler BodyInnerHtmlChanging;
        public event EventHandler BodyInnerHtmlChanged;

        public string BodyInnerHtml
        {
            get
            {
                if (Document.IsNull() ||
                    Document.Body.IsNull())
                {
                    throw new NullReferenceException();
                }

                return Document.Body.InnerHtml;
            }
            set
            {
                OnBodyInnerHtmlChanging();
                
                if (Document.IsNull() ||
                    Document.Body.IsNull())
                {
                    throw new NullReferenceException();
                }
                Document.Body.InnerHtml = value;
                
                OnBodyInnerHtmlChanged();
            }
        }

      public void SetDefaultHtml()
        {
          defaultHtml = BodyInnerHtml;
        }
      private void RestoreHtml(bool DoAlways = true)
        {
          if (DoAlways || string.IsNullOrEmpty(BodyInnerHtml))
            BodyInnerHtml = defaultHtml;
        }

        public virtual void OnBodyInnerHtmlChanging()
        {
            if (BodyInnerHtmlChanging.IsNull())
            {
                return;
            }

            BodyInnerHtmlChanging(this, EventArgs.Empty);
        }

        public virtual void OnBodyInnerHtmlChanged()
        {
            if (BodyInnerHtmlChanged.IsNull())
            {
                return;
            }

            BodyInnerHtmlChanged(this, EventArgs.Empty);
        }

        #endregion

        #region Основные события средства визуального редактирования

        private void AttachEventHandlers()
        {
            if (Document.IsNull() ||
                Document.Body.IsNull())
            {
                throw new NullReferenceException();
            }

            Document.Body.MouseEnter += Body_MouseEnter;
            Document.Body.MouseMove += Body_MouseMove;
            Document.Body.MouseDown += Body_MouseDown;
            Document.Body.MouseUp += Body_MouseUp;
            Document.Body.MouseLeave += Body_MouseLeave;
            Document.Body.DoubleClick += Body_DoubleClick;

            Document.Body.KeyDown += Body_KeyDown;
            Document.Body.KeyUp += Body_KeyUp;

            Document.Body.GotFocus += Body_GotFocus;
            Document.Body.LostFocus += Body_LostFocus;

            Document.Body.DragLeave += Body_DragLeave;
            Document.Body.DragOver += Body_DragOver;
            Document.Body.DragEnd += Body_DragEnd;
        }

        public new event EventHandler MouseEnter;
        public new event EventHandler<MouseEventArgs> MouseMove;
        public new event EventHandler<MouseEventArgs> MouseDown;
        public new event EventHandler<MouseEventArgs> MouseUp;
        public new event EventHandler MouseLeave;
        public new event EventHandler<MouseEventArgs> MouseDoubleClick;

        public new event EventHandler<KeyEventArgs> KeyDown;
        public new event EventHandler<KeyEventArgs> KeyUp;

        public new event EventHandler Enter;
        public new event EventHandler Leave;

        public new event EventHandler DragLeave;
        public new event EventHandler<DragEventArgs> DragOver;
        public new event EventHandler<DragEventArgs> DragDrop;

        private void Body_MouseEnter(object sender, HtmlElementEventArgs e)
        {
            if (MouseEnter.IsNull())
            {
                return;
            }

            MouseEnter(this, EventArgs.Empty);
        }

        private void Body_MouseMove(object sender, HtmlElementEventArgs e)
        {
            if (MouseMove.IsNull())
            {
                return;
            }

            MouseMove(this, new MouseEventArgs(e.MouseButtonsPressed, 0, e.MousePosition.X, e.MousePosition.Y, 0));
        }

        private void Body_MouseDown(object sender, HtmlElementEventArgs e)
        {
            if (MouseDown.IsNull())
            {
                return;
            }

            MouseDown(this, new MouseEventArgs(e.MouseButtonsPressed, 1, e.MousePosition.X, e.MousePosition.Y, 0));
        }

        private void Body_MouseUp(object sender, HtmlElementEventArgs e)
        {
            if (MouseUp.IsNull())
            {
                return;
            }

            MouseUp(this, new MouseEventArgs(e.MouseButtonsPressed, 1, e.MousePosition.X, e.MousePosition.Y, 0));
        }

        private void Body_MouseLeave(object sender, HtmlElementEventArgs e)
        {
            if (MouseLeave.IsNull())
            {
                return;
            }

            MouseLeave(this, EventArgs.Empty);
        }

        private void Body_DoubleClick(object sender, HtmlElementEventArgs e)
        {
            if (MouseDoubleClick.IsNull())
            {
                return;
            }

            MouseDoubleClick(this, new MouseEventArgs(MouseButtons.Left, 2, e.MousePosition.X, e.MousePosition.Y, 0));
        }

        private void Body_KeyDown(object sender, HtmlElementEventArgs e)
        {
            if (KeyDown.IsNull())
            {
                return;
            }

            var keyData = (Keys)e.KeyPressedCode;

            if (e.AltKeyPressed)
            {
                keyData = keyData | Keys.Alt;
            }

            if (e.CtrlKeyPressed)
            {
                keyData = keyData | Keys.Control;
            }

            if (e.ShiftKeyPressed)
            {
                keyData = keyData | Keys.Shift;
            }

            KeyDown(this, new KeyEventArgs(keyData));
        }

        private void Body_KeyUp(object sender, HtmlElementEventArgs e)
        {
            if (KeyUp.IsNull())
            {
                return;
            }

            var keyData = (Keys)e.KeyPressedCode;

            if (e.AltKeyPressed)
            {
                keyData = keyData | Keys.Alt;
            }

            if (e.CtrlKeyPressed)
            {
                keyData = keyData | Keys.Control;
            }

            if (e.ShiftKeyPressed)
            {
                keyData = keyData | Keys.Shift;
            }

            KeyUp(this, new KeyEventArgs(keyData));
        }

        private void Body_GotFocus(object sender, HtmlElementEventArgs e)
        {
            if (Enter.IsNull())
            {
                return;
            }

            Enter(this, EventArgs.Empty);
        }

        private void Body_LostFocus(object sender, HtmlElementEventArgs e)
        {
            if (Leave.IsNull())
            {
                return;
            }

            Leave(this, EventArgs.Empty);
        }

        private void Body_DragLeave(object sender, HtmlElementEventArgs e)
        {
            if (DragLeave.IsNull())
            {
                return;
            }

            DragLeave(this, EventArgs.Empty);
        }

        private void Body_DragOver(object sender, HtmlElementEventArgs e)
        {

        }

        private void Body_DragEnd(object sender, HtmlElementEventArgs e)
        {

        }

        #endregion

        #region Операции визуального редактирования

        public void ChangeBold()
        {
            if (mode == Enums.HtmlEditingToolMode.Preview)
            {
                throw new InvalidOperationException();
            }

            if (Document.IsNull())
            {
                throw new NullReferenceException(CommandIdentifiers.BoldId);
            }

            try
            {
                Document.ExecCommand(CommandIdentifiers.BoldId, false, null);
            }
            catch
            {
                throw new InvalidOperationException(CommandIdentifiers.BoldId);
            }
        }

        public void ChangeItalic()
        {
            if (mode == Enums.HtmlEditingToolMode.Preview)
            {
                throw new InvalidOperationException();
            }

            if (Document.IsNull())
            {
                throw new NullReferenceException(CommandIdentifiers.ItalicId);
            }

            try
            {
                Document.ExecCommand(CommandIdentifiers.ItalicId, false, null);
            }
            catch
            {
                throw new InvalidOperationException(CommandIdentifiers.ItalicId);
            }
        }

        public void ChangeUnderline()
        {
            if (mode == Enums.HtmlEditingToolMode.Preview)
            {
                throw new InvalidOperationException();
            }

            if (Document.IsNull())
            {
                throw new NullReferenceException(CommandIdentifiers.UnderlineId);
            }

            try
            {
                Document.ExecCommand(CommandIdentifiers.UnderlineId, false, null);
            }
            catch
            {
                throw new InvalidOperationException(CommandIdentifiers.UnderlineId);
            }
        }

        public void ChangeStrikeThrough()
        {
            if (mode == Enums.HtmlEditingToolMode.Preview)
            {
                throw new InvalidOperationException();
            }

            if (Document.IsNull())
            {
                throw new NullReferenceException(CommandIdentifiers.StrikeThroughId);
            }

            try
            {
                Document.ExecCommand(CommandIdentifiers.StrikeThroughId, false, null);
            }
            catch
            {
                throw new InvalidOperationException(CommandIdentifiers.StrikeThroughId);
            }
        }

        public void ChangeUnorderedList()
        {
            if (mode == Enums.HtmlEditingToolMode.Preview)
            {
                throw new InvalidOperationException();
            }

            if (Document.IsNull())
            {
                throw new NullReferenceException(CommandIdentifiers.UnorderedListId);
            }

            try
            {
                Document.ExecCommand(CommandIdentifiers.UnorderedListId, false, null);
            }
            catch
            {
                throw new InvalidOperationException(CommandIdentifiers.UnorderedListId);
            }
        }

        public void ChangeOrderedList()
        {
            if (mode == Enums.HtmlEditingToolMode.Preview)
            {
                throw new InvalidOperationException();
            }

            if (Document.IsNull())
            {
                throw new NullReferenceException(CommandIdentifiers.OrderedListId);
            }

            try
            {
                Document.ExecCommand(CommandIdentifiers.OrderedListId, false, null);
            }
            catch
            {
                throw new InvalidOperationException(CommandIdentifiers.OrderedListId);
            }
        }

        public void InsertHorizontalRule()
        {
            if (mode == Enums.HtmlEditingToolMode.Preview)
            {
                throw new InvalidOperationException();
            }

            if (Document.IsNull())
            {
                throw new NullReferenceException(CommandIdentifiers.InsertHorizontalRuleId);
            }

            try
            {
                Document.ExecCommand(CommandIdentifiers.InsertHorizontalRuleId, false, null);
            }
            catch
            {
                throw new InvalidOperationException(CommandIdentifiers.InsertHorizontalRuleId);
            }
        }

        public void InsertParagraph()
        {
            if (mode == Enums.HtmlEditingToolMode.Preview)
            {
                throw new InvalidOperationException();
            }

            if (Document.IsNull())
            {
                throw new NullReferenceException(CommandIdentifiers.InsertParagraphId);
            }

            try
            {
                Document.ExecCommand(CommandIdentifiers.InsertParagraphId, false, null);
            }
            catch
            {
                throw new InvalidOperationException(CommandIdentifiers.InsertParagraphId);
            }
        }

        public void SetFontName(string fontName)
        {
            if (string.IsNullOrEmpty(fontName))
            {
                throw new ArgumentNullException();
            }

            if (mode == Enums.HtmlEditingToolMode.Preview)
            {
                throw new InvalidOperationException();
            }

            if (Document.IsNull())
            {
                throw new NullReferenceException(CommandIdentifiers.FontNameId);
            }

            try
            {
                Document.ExecCommand(CommandIdentifiers.FontNameId, false, fontName);
            }
            catch
            {
                throw new InvalidOperationException(CommandIdentifiers.FontNameId);
            }
        }

        public void SetFontSize(int fontSize)
        {
            if (fontSize < minFontSize || fontSize > maxFontSize)
            {
                throw new ArgumentOutOfRangeException();
            }

            if (mode == Enums.HtmlEditingToolMode.Preview)
            {
                throw new InvalidOperationException();
            }

            if (Document.IsNull())
            {
                throw new NullReferenceException(CommandIdentifiers.FontSizeId);
            }

            try
            {
                Document.ExecCommand(CommandIdentifiers.FontSizeId, false, fontSize.ToString());
            }
            catch
            {
                throw new InvalidOperationException(CommandIdentifiers.FontSizeId);
            }
        }

        public void SetDefaultFont()
        {
            SetFontName(Defaults.FontName);
            SetFontSize(Defaults.FontSize);
        }

        public void JustifyLeft()
        {
            if (mode == Enums.HtmlEditingToolMode.Preview)
            {
                throw new InvalidOperationException();
            }

            if (Document.IsNull())
            {
                throw new NullReferenceException(CommandIdentifiers.JustifyLeftId);
            }

            try
            {
                Document.ExecCommand(CommandIdentifiers.JustifyLeftId, false, null);
            }
            catch
            {
                throw new InvalidOperationException(CommandIdentifiers.JustifyLeftId);
            }
        }

        public void JustifyCenter()
        {
            if (mode == Enums.HtmlEditingToolMode.Preview)
            {
                throw new InvalidOperationException();
            }

            if (Document.IsNull())
            {
                throw new NullReferenceException(CommandIdentifiers.JustifyCenterId);
            }

            try
            {
                Document.ExecCommand(CommandIdentifiers.JustifyCenterId, false, null);
            }
            catch
            {
                throw new InvalidOperationException(CommandIdentifiers.JustifyCenterId);
            }
        }

        public void JustifyRight()
        {
            if (mode == Enums.HtmlEditingToolMode.Preview)
            {
                throw new InvalidOperationException();
            }

            if (Document.IsNull())
            {
                throw new NullReferenceException(CommandIdentifiers.JustifyRightId);
            }

            try
            {
                Document.ExecCommand(CommandIdentifiers.JustifyRightId, false, null);
            }
            catch
            {
                throw new InvalidOperationException(CommandIdentifiers.JustifyRightId);
            }
        }

        public void JustifyFull()
        {
            if (mode == Enums.HtmlEditingToolMode.Preview)
            {
                throw new InvalidOperationException();
            }

            if (Document.IsNull())
            {
                throw new NullReferenceException(CommandIdentifiers.JustifyFullId);
            }

            try
            {
                Document.ExecCommand(CommandIdentifiers.JustifyFullId, false, null);
            }
            catch
            {
                throw new InvalidOperationException(CommandIdentifiers.JustifyFullId);
            }
        }

        public void JustifyNone()
        {
            if (mode == Enums.HtmlEditingToolMode.Preview)
            {
                throw new InvalidOperationException();
            }

            if (Document.IsNull())
            {
                throw new NullReferenceException(CommandIdentifiers.JustifyNoneId);
            }

            try
            {
                Document.ExecCommand(CommandIdentifiers.JustifyNoneId, false, null);
            }
            catch
            {
                throw new InvalidOperationException(CommandIdentifiers.JustifyNoneId);
            }
        }

        private void SetLiveResize(bool doLiveResizing)
        {
            if (Document.IsNull())
            {
                throw new NullReferenceException();
            }

            try
            {
                Document.ExecCommand(CommandIdentifiers.LiveResizeId, false, doLiveResizing);
            }
            catch
            {
                throw new InvalidOperationException(CommandIdentifiers.LiveResizeId);
            }
        }

        public void Indent()
        {
            if (mode == Enums.HtmlEditingToolMode.Preview)
            {
                throw new InvalidOperationException();
            }

            if (Document.IsNull())
            {
                throw new NullReferenceException(CommandIdentifiers.IndentId);
            }

            try
            {
                Document.ExecCommand(CommandIdentifiers.IndentId, false, null);
            }
            catch
            {
                throw new InvalidOperationException(CommandIdentifiers.IndentId);
            }
        }

        public void Outdent()
        {
            if (mode == Enums.HtmlEditingToolMode.Preview)
            {
                throw new InvalidOperationException();
            }

            if (Document.IsNull())
            {
                throw new NullReferenceException(CommandIdentifiers.OutdentId);
            }

            try
            {
                Document.ExecCommand(CommandIdentifiers.OutdentId, false, null);
            }
            catch
            {
                throw new InvalidOperationException(CommandIdentifiers.OutdentId);
            }
        }

        public void SetForeColor(Color color)
        {
            if (color.IsNull())
            {
                throw new ArgumentNullException();
            }

            if (mode == Enums.HtmlEditingToolMode.Preview)
            {
                throw new InvalidOperationException();
            }

            if (Document.IsNull())
            {
                throw new NullReferenceException(CommandIdentifiers.ForeColorId);
            }

            try
            {
                Document.ExecCommand(CommandIdentifiers.ForeColorId, false, ColorTranslator.ToHtml(color));
            }
            catch
            {
                throw new InvalidOperationException(CommandIdentifiers.ForeColorId);
            }
        }

        public void SetBackColor(Color color)
        {
            if (color.IsNull())
            {
                throw new ArgumentNullException();
            }

            if (mode == Enums.HtmlEditingToolMode.Preview)
            {
                throw new InvalidOperationException();
            }

            if (Document.IsNull())
            {
                throw new NullReferenceException(CommandIdentifiers.BackColorId);
            }

            try
            {
                Document.ExecCommand(CommandIdentifiers.BackColorId, false, ColorTranslator.ToHtml(color));
            }
            catch
            {
                throw new InvalidOperationException(CommandIdentifiers.BackColorId);
            }
        }

        public void SetInferior()
        {
            if (mode == Enums.HtmlEditingToolMode.Preview)
            {
                throw new InvalidOperationException();
            }

            HtmlEditingToolHelper.SurroundWithHtml(this, CommandIdentifiers.SetInferior, new Dictionary<string, string>());
        }

        public void SetAscender()
        {
            if (mode == Enums.HtmlEditingToolMode.Preview)
            {
                throw new InvalidOperationException();
            }

            HtmlEditingToolHelper.SurroundWithHtml(this, CommandIdentifiers.SetAscender, new Dictionary<string, string>());
        }

        public void RemoveFormat()
        {
            if (mode == Enums.HtmlEditingToolMode.Preview)
            {
                throw new InvalidOperationException();
            }

            if (Document.IsNull())
            {
                throw new NullReferenceException(CommandIdentifiers.RemoveFormatId);
            }

            try
            {
                Document.ExecCommand(CommandIdentifiers.RemoveFormatId, false, null);
            }
            catch
            {
                throw new InvalidOperationException(CommandIdentifiers.RemoveFormatId);
            }
        }

        public void Cut()
        {
            if (mode == Enums.HtmlEditingToolMode.Preview)
            {
                throw new InvalidOperationException();
            }

            if (Document.IsNull())
            {
                throw new NullReferenceException(CommandIdentifiers.CutId);
            }

            try
            {
                Document.ExecCommand(CommandIdentifiers.CutId, false, null);
            }
            catch
            {
                throw new InvalidOperationException(CommandIdentifiers.CutId);
            }
        }

        public void Copy()
        {
            if (mode == Enums.HtmlEditingToolMode.Preview)
            {
                throw new InvalidOperationException();
            }

            if (Document.IsNull())
            {
                throw new NullReferenceException(CommandIdentifiers.CopyId);
            }

            try
            {
                Document.ExecCommand(CommandIdentifiers.CopyId, false, null);
            }
            catch
            {
                throw new InvalidOperationException(CommandIdentifiers.CopyId);
            }
        }

        public void Paste()
        {
            if (mode == Enums.HtmlEditingToolMode.Preview)
            {
                throw new InvalidOperationException();
            }

            if (Document.IsNull())
            {
                throw new NullReferenceException(CommandIdentifiers.PasteId);
            }

            try
            {
                Document.ExecCommand(CommandIdentifiers.PasteId, false, null);
            }
            catch
            {
                throw new InvalidOperationException(CommandIdentifiers.PasteId);
            }
        }

        public void Delete()
        {
            if (mode == Enums.HtmlEditingToolMode.Preview)
            {
                throw new InvalidOperationException();
            }

            if (Document.IsNull())
            {
                throw new NullReferenceException(CommandIdentifiers.DeleteId);
            }

            try
            {
                Document.ExecCommand(CommandIdentifiers.DeleteId, false, null);
            }
            catch
            {
                throw new InvalidOperationException(CommandIdentifiers.DeleteId);
            }
        }

        public void SelectAll()
        {
            if (mode == Enums.HtmlEditingToolMode.Preview)
            {
                throw new InvalidOperationException();
            }

            if (Document.IsNull())
            {
                throw new NullReferenceException(CommandIdentifiers.SelectAllId);
            }

            try
            {
                Document.ExecCommand(CommandIdentifiers.SelectAllId, false, null);
            }
            catch
            {
                throw new InvalidOperationException(CommandIdentifiers.SelectAllId);
            }
        }

        public void Unselect()
        {
            if (Document.IsNull())
            {
                throw new NullReferenceException(CommandIdentifiers.UnselectId);
            }

            try
            {
                Document.ExecCommand(CommandIdentifiers.UnselectId, false, null);
            }
            catch
            {
                throw new InvalidOperationException(CommandIdentifiers.UnselectId);
            }
        }

        public void Undo()
        {
            if (mode == Enums.HtmlEditingToolMode.Preview)
            {
                throw new InvalidOperationException();
            }

            if (Document.IsNull())
            {
                throw new NullReferenceException(CommandIdentifiers.UndoId);
            }

            try
            {
                Document.ExecCommand(CommandIdentifiers.UndoId, false, null);
            }
            catch
            {
                throw new InvalidOperationException(CommandIdentifiers.UndoId);
            }
        }

        public void Redo()
        {
            if (mode == Enums.HtmlEditingToolMode.Preview)
            {
                throw new InvalidOperationException();
            }

            if (Document.IsNull())
            {
                throw new NullReferenceException(CommandIdentifiers.RedoId);
            }

            try
            {
                Document.ExecCommand(CommandIdentifiers.RedoId, false, null);
            }
            catch
            {
                throw new InvalidOperationException(CommandIdentifiers.RedoId);
            }
        }

        public void Overwrite(bool doOverwriting)
        {
            if (mode == Enums.HtmlEditingToolMode.Preview)
            {
                throw new InvalidOperationException();
            }

            if (Document.IsNull())
            {
                throw new NullReferenceException(CommandIdentifiers.OverwriteId);
            }

            try
            {
                Document.ExecCommand(CommandIdentifiers.OverwriteId, false, doOverwriting);
            }
            catch
            {
                throw new InvalidOperationException(CommandIdentifiers.OverwriteId);
            }
        }

        public void Unlink()
        {
            if (mode == Enums.HtmlEditingToolMode.Preview)
            {
                throw new InvalidOperationException();
            }

            if (Document.IsNull())
            {
                throw new NullReferenceException(CommandIdentifiers.UnlinkId);
            }

            try
            {
                Document.ExecCommand(CommandIdentifiers.UnlinkId, false, null);
            }
            catch
            {
                throw new InvalidOperationException(CommandIdentifiers.UnlinkId);
            }
        }

        public void Unbookmark()
        {
            if (mode == Enums.HtmlEditingToolMode.Preview)
            {
                throw new InvalidOperationException();
            }

            if (Document.IsNull())
            {
                throw new NullReferenceException(CommandIdentifiers.UnbookmarkId);
            }

            try
            {
                Document.ExecCommand(CommandIdentifiers.UnbookmarkId, false, null);
            }
            catch
            {
                throw new InvalidOperationException(CommandIdentifiers.UnbookmarkId);
            }
        }

        public void ScrollToHtmlElement(HtmlElement htmlElement)
        {
            if (htmlElement.IsNull())
            {
                throw new ArgumentNullException();
            }

            htmlElement.ScrollIntoView(true);
        }

        public void InsertBreak()
        {
            if (mode == Enums.HtmlEditingToolMode.Preview)
            {
                throw new InvalidOperationException();
            }

            HtmlEditingToolHelper.InsertHtml(this, TagNames.BreakTagName, new Dictionary<string, string>());
        }

        /// <summary>
        /// Осуществляет поиск текста.
        /// </summary>
        /// <param name="text">Текст для поиска.</param>
        /// <param name="doForward">Направление поиска.</param>
        /// <param name="matchCase">Учитывать регистр.</param>
        /// <param name="matchWholeWord">Только слово целиком.</param>
        /// <returns>Успешность поиска.</returns>
        public bool Find(string text, bool doForward, bool matchCase, bool matchWholeWord)
        {
            if (string.IsNullOrEmpty(text))
            {
                throw new ArgumentNullException();
            }

            if (Document.IsNull())
            {
                throw new NullReferenceException();
            }

            var document = Document.DomDocument as IHTMLDocument2;

            if (document == null)
            {
                throw new NullReferenceException();
            }

            var bodyElement = document.body as IHTMLBodyElement;

            if (bodyElement == null)
            {
                throw new NullReferenceException();
            }

            IHTMLTxtRange textRange;
            if (document.selection != null)
            {
                textRange = document.selection.createRange() as IHTMLTxtRange;

                if (textRange == null)
                {
                    throw new NullReferenceException();
                }

                var dup = textRange.duplicate();
                dup.collapse(true);

                if (textRange.isEqual(dup))
                {
                    textRange = bodyElement.createTextRange();

                    if (textRange == null)
                    {
                        throw new NullReferenceException();
                    }
                }
                else
                {
                    if (doForward)
                    {
                        textRange.moveStart("character", 1);
                    }
                    else
                    {
                        textRange.moveEnd("character", -1);
                    }
                }
            }
            else
            {
                textRange = bodyElement.createTextRange();
            }

            var flags = 0;

            if (matchCase)
            {
                flags += 4;
            }

            if (matchWholeWord)
            {
                flags += 2;
            }

            var success = textRange.findText(text, doForward ? 999999 : -999999, flags);

            if (success)
            {
                textRange.select();
                textRange.scrollIntoView(!doForward);
            }

            return success;
        }

        #endregion

        #region Выделенный контент средства визуального редактирования

        internal IHTMLDocument2 GetNativeHtmlDocument()
        {
            if (Document.IsNull() ||
                Document.DomDocument.IsNull())
            {
                throw new NullReferenceException();
            }

            IHTMLDocument2 nativeHtmlDocument;

            try
            {
                nativeHtmlDocument = Document.DomDocument as IHTMLDocument2;
            }
            catch
            {
                throw new InvalidOperationException();
            }

            return nativeHtmlDocument;
        }

        public bool IsSelectionValid()
        {
            var nativeHtmlDocument = GetNativeHtmlDocument();
            IHTMLTxtRange textRange;

            try
            {
                textRange = nativeHtmlDocument.selection.createRange() as IHTMLTxtRange;
            }
            catch
            {
                textRange = null;
            }

            if (textRange.IsNull())
            {
                return false;
            }

            return true;
        }

        public string GetSelection()
        {
            if (!IsSelectionValid())
            {
                return string.Empty;
            }

            var nativeHtmlDocument = GetNativeHtmlDocument();
            IHTMLTxtRange textRange;
            try
            {
                textRange = nativeHtmlDocument.selection.createRange() as IHTMLTxtRange;
            }
            catch
            {
                throw new InvalidOperationException();
            }

            return textRange.text ?? string.Empty;
        }

        public bool IsSelection
        {
            get { return GetSelection() != string.Empty; }
        }

        public void ReplaceSelection(string text)
        {
            if (string.IsNullOrEmpty(text))
            {
                throw new ArgumentNullException();
            }

            if (mode == Enums.HtmlEditingToolMode.Preview)
            {
                throw new InvalidOperationException();
            }

            if (!IsSelection)
            {
                return;
            }

            var nativeHtmlDocument = GetNativeHtmlDocument();
            IHTMLTxtRange textRange;
            try
            {
                textRange = nativeHtmlDocument.selection.createRange() as IHTMLTxtRange;
            }
            catch
            {
                throw new InvalidOperationException();
            }

            if (textRange.IsNull())
            {
                throw new NullReferenceException();
            }

            try
            {
                nativeHtmlDocument.selection.clear();
                textRange.pasteHTML(text);
                textRange.collapse(false);
                textRange.select();
            }
            catch
            {
                throw new InvalidOperationException();
            }
        }

        public void Highlight(HtmlElement htmlElement, bool doHighlighting)
        {
            if (htmlElement.IsNull())
            {
                throw new ArgumentNullException();
            }

            htmlElement.Style = doHighlighting ? string.Concat("background-color: ",
                                                               ColorTranslator.ToHtml(Defaults.HighlightColor)) : string.Empty;
        }

        #endregion

        #region Определение стиля текста

        public bool IsBold
        {
            get
            {
                var nativeHtmlDocument = GetNativeHtmlDocument();

                bool isBold = false;
                try
                {
                    isBold = nativeHtmlDocument.queryCommandState(CommandIdentifiers.BoldId);
                }
                catch
                {
                    throw new InvalidOperationException(CommandIdentifiers.BoldId);
                }

                return isBold;
            }
        }

        public bool IsItalic
        {
            get
            {
                var nativeHtmlDocument = GetNativeHtmlDocument();

                bool isItalic = false;
                try
                {
                    isItalic = nativeHtmlDocument.queryCommandState(CommandIdentifiers.ItalicId);
                }
                catch
                {
                    throw new InvalidOperationException(CommandIdentifiers.ItalicId);
                }

                return isItalic;
            }
        }

        public bool IsUnderline
        {
            get
            {
                var nativeHtmlDocument = GetNativeHtmlDocument();

                bool isUnderline = false;
                try
                {
                    isUnderline = nativeHtmlDocument.queryCommandState(CommandIdentifiers.UnderlineId);
                }
                catch
                {
                    throw new InvalidOperationException(CommandIdentifiers.UnderlineId);
                }

                return isUnderline;
            }
        }

        public bool IsStrikeThrough
        {
            get
            {
                var nativeHtmlDocument = GetNativeHtmlDocument();

                bool isStrikeThrough = false;
                try
                {
                    isStrikeThrough = nativeHtmlDocument.queryCommandState(CommandIdentifiers.StrikeThroughId);
                }
                catch
                {
                    throw new InvalidOperationException(CommandIdentifiers.StrikeThroughId);
                }

                return isStrikeThrough;
            }
        }

        public bool IsInferior
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        public bool IsAscender
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        public bool IsUnorderedList
        {
            get
            {
                var nativeHtmlDocument = GetNativeHtmlDocument();

                bool isUnorderedList = false;
                try
                {
                    isUnorderedList = nativeHtmlDocument.queryCommandState(CommandIdentifiers.UnorderedListId);
                }
                catch
                {
                    throw new InvalidOperationException(CommandIdentifiers.UnorderedListId);
                }

                return isUnorderedList;
            }
        }

        public bool IsOrderedList
        {
            get
            {
                var nativeHtmlDocument = GetNativeHtmlDocument();

                bool isOrderedList = false;
                try
                {
                    isOrderedList = nativeHtmlDocument.queryCommandState(CommandIdentifiers.OrderedListId);
                }
                catch
                {
                    throw new InvalidOperationException(CommandIdentifiers.OrderedListId);
                }

                return isOrderedList;
            }
        }

        public string GetFontName()
        {
            var nativeHtmlDocument = GetNativeHtmlDocument();

            string fontName = string.Empty;
            try
            {
                fontName = nativeHtmlDocument.queryCommandValue(CommandIdentifiers.FontNameId).ToString();
            }
            catch
            {
                throw new InvalidOperationException(CommandIdentifiers.FontNameId);
            }

            return fontName;
        }

        public int GetFontSize()
        {
            var nativeHtmlDocument = GetNativeHtmlDocument();

            int fontSize = 0;
            try
            {
                fontSize = Convert.ToInt32(nativeHtmlDocument.queryCommandValue(CommandIdentifiers.FontSizeId));
            }
            catch 
            {
                throw new InvalidOperationException(CommandIdentifiers.FontSizeId);
            }

            return fontSize;
        }

        public bool IsJustifyLeft
        {
            get
            {
                var nativeHtmlDocument = GetNativeHtmlDocument();

                bool isJustifyLeft = false;
                try
                {
                    isJustifyLeft = nativeHtmlDocument.queryCommandState(CommandIdentifiers.JustifyLeftId);
                }
                catch
                {
                    throw new InvalidOperationException(CommandIdentifiers.JustifyLeftId);
                }

                return isJustifyLeft;
            }
        }

        public bool IsJustifyCenter
        {
            get
            {
                var nativeHtmlDocument = GetNativeHtmlDocument();

                bool isJustifyCenter = false;
                try
                {
                    isJustifyCenter = nativeHtmlDocument.queryCommandState(CommandIdentifiers.JustifyCenterId);
                }
                catch
                {
                    throw new InvalidOperationException(CommandIdentifiers.JustifyCenterId);
                }

                return isJustifyCenter;
            }
        }

        public bool IsJustifyRight
        {
            get
            {
                var nativeHtmlDocument = GetNativeHtmlDocument();

                bool isJustifyRight = false;
                try
                {
                    isJustifyRight = nativeHtmlDocument.queryCommandState(CommandIdentifiers.JustifyRightId);
                }
                catch
                {
                    throw new InvalidOperationException(CommandIdentifiers.JustifyRightId);
                }

                return isJustifyRight;
            }
        }

        public bool IsJustifyFull
        {
            get
            {
                var nativeHtmlDocument = GetNativeHtmlDocument();

                bool isJustifyFull = false;
                try
                {
                    isJustifyFull = nativeHtmlDocument.queryCommandState(CommandIdentifiers.JustifyFullId);
                }
                catch
                {
                    throw new InvalidOperationException(CommandIdentifiers.JustifyFullId);
                }

                return isJustifyFull;
            }
        }

        public Color GetForeColor()
        {
            var nativeHtmlDocument = GetNativeHtmlDocument();

            Color color = Color.Empty;
            try
            {
                color = HtmlEditingToolHelper.ConvertToColor(nativeHtmlDocument.queryCommandValue(CommandIdentifiers.ForeColorId).ToString());
            }
            catch
            {
                throw new InvalidOperationException(CommandIdentifiers.ForeColorId);
            }

            return color;
        }

        public Color GetBackColor()
        {
            var nativeHtmlDocument = GetNativeHtmlDocument();

            Color color = Color.Empty;
            try
            {
                color = HtmlEditingToolHelper.ConvertToColor(nativeHtmlDocument.queryCommandValue(CommandIdentifiers.BackColorId).ToString());
            }
            catch
            {
                throw new InvalidOperationException(CommandIdentifiers.BackColorId);
            }

            return color;
        }

        public bool CanCut
        {
            get
            {
                var nativeHtmlDocument = GetNativeHtmlDocument();

                bool canCut = false;
                try
                {
                    canCut = nativeHtmlDocument.queryCommandEnabled(CommandIdentifiers.CutId);
                }
                catch
                {
                    throw new InvalidOperationException(CommandIdentifiers.CutId);
                }

                return canCut;                
            }
        }

        public bool CanCopy
        {
            get
            {
                var nativeHtmlDocument = GetNativeHtmlDocument();

                bool canCopy = false;
                try
                {
                    canCopy = nativeHtmlDocument.queryCommandEnabled(CommandIdentifiers.CopyId);
                }
                catch
                {
                    throw new InvalidOperationException(CommandIdentifiers.CopyId);
                }

                return canCopy; 
            }
        }

        public bool CanPaste
        {
            get
            {
                var nativeHtmlDocument = GetNativeHtmlDocument();

                bool canPaste = false;
                try
                {
                    canPaste = nativeHtmlDocument.queryCommandEnabled(CommandIdentifiers.PasteId);
                }
                catch
                {
                    throw new InvalidOperationException(CommandIdentifiers.PasteId);
                }

                return canPaste;  
            }
        }

        public bool CanDelete
        {
            get
            {
                var nativeHtmlDocument = GetNativeHtmlDocument();

                bool canDelete = false;
                try
                {
                    canDelete = nativeHtmlDocument.queryCommandEnabled(CommandIdentifiers.DeleteId);
                }
                catch
                {
                    throw new InvalidOperationException(CommandIdentifiers.DeleteId);
                }

                return canDelete; 
            }
        }

        public bool CanUndo
        {
            get
            {
                var nativeHtmlDocument = GetNativeHtmlDocument();

                bool canUndo = false;
                try
                {
                    canUndo = nativeHtmlDocument.queryCommandEnabled(CommandIdentifiers.UndoId);
                }
                catch
                {
                    throw new InvalidOperationException(CommandIdentifiers.UndoId);
                }

                return canUndo; 
            }
        }

        public bool CanRedo
        {
            get
            {
                var nativeHtmlDocument = GetNativeHtmlDocument();

                bool canRedo = false;
                try
                {
                    canRedo = nativeHtmlDocument.queryCommandEnabled(CommandIdentifiers.RedoId);
                }
                catch
                {
                    throw new InvalidOperationException(CommandIdentifiers.RedoId);
                }

                return canRedo; 
            }
        }

        public bool CanOverwrite
        {
            get
            {
                var nativeHtmlDocument = GetNativeHtmlDocument();

                bool canOverwrite = false;
                //try
                {
                    canOverwrite = nativeHtmlDocument.queryCommandState(CommandIdentifiers.OverwriteId);
                }
                //catch
                //{
                //   throw new InvalidOperationException(CommandIdentifiers.OverwriteId);
                //}

                return canOverwrite; 
            }
        }

        #endregion
        
        #region Получение html элементов
        
        public List<HtmlElement> GetElementsByTagName(string tagName)
        {
            if (string.IsNullOrEmpty(tagName))
            {
                throw new ArgumentNullException();
            }

            if (Document.IsNull())
            {
                throw new NullReferenceException();
            }

            var elements = new List<HtmlElement>(); 
            foreach (HtmlElement htmlElement in Document.All)
            {
                if (htmlElement.TagName.ToUpper() == tagName.ToUpper())
                {
                    elements.Add(htmlElement); 
                }
            }

            return elements;
        }

        public List<HtmlElement> Images
        {
            get { return GetElementsByTagName(TagNames.ImageTagName); }
        }

        public List<HtmlElement> Links
        {
            get { return GetElementsByTagName(TagNames.AnchorTagName); }
        }

        public List<HtmlElement> Tables
        {
            get { return GetElementsByTagName(TagNames.TableTagName); }
        }

        public HtmlElement GetActiveElement(Point point)
        {
            if (point.IsNull())
            {
                throw new ArgumentNullException();
            }

            if (Document.IsNull())
            {
                throw new NullReferenceException();
            }

            var htmlElement = Document.GetElementFromPoint(point);
            if (htmlElement.IsNull())
            {
                throw new NullReferenceException();
            }

            // Возвращает рисунок, ссылку или таблицу.
            if (htmlElement.TagName.ToUpper() == TagNames.ImageTagName.ToUpper() ||
                htmlElement.TagName.ToUpper() == TagNames.AnchorTagName.ToUpper() ||
                htmlElement.TagName.ToUpper() == TagNames.TableTagName.ToUpper())
            {
                return htmlElement;
            }

            // Возвращает таблицу.
            if (htmlElement.TagName.ToUpper() == TagNames.CaptionTagName.ToUpper() ||
                htmlElement.TagName.ToUpper() == TagNames.TrTagName.ToUpper())
            {
                if (htmlElement.Parent.IsNull())
                {
                    throw new NullReferenceException();
                }

                if (htmlElement.Parent.TagName.ToUpper() == TagNames.TableTagName.ToUpper())
                {
                    return htmlElement.Parent;
                }
            }

            // Возвращает таблицу.
            if (htmlElement.TagName.ToUpper() == TagNames.TdTagName.ToUpper())
            {
                return htmlElement;
            }

            // Возвращает тело документа.
            return Document.Body;
        }

        public HtmlElement HighlightedElement { get; set; }
        public HtmlElement ActiveElement { get; set; }

        #endregion
    }
}