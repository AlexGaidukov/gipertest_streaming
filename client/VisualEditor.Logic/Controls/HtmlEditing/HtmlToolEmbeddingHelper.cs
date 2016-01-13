using System;
using System.Windows.Forms;
using VisualEditor.Utils.Controls.HtmlEditing;
using VisualEditor.Utils.Helpers;

namespace VisualEditor.Logic.Controls.HtmlEditing
{
    public static class HtmlToolEmbeddingHelper
    {
        #region Создание средства визуального редактирования и просмоторщика html кода
        
        public static HtmlEditingTool CreateHtmlEditingTool()
        {
            return new HtmlEditingTool();
        }

        public static RichTextBox CreateHtmlSourceViewer()
        {
            return new RichTextBox();
        }

        #endregion

        #region Встраивание средства визуального редактирования и просмоторщика html кода
        
        public static void EmbedHtmlEditingTool(HtmlEditingTool htmlEditingTool, Control parent)
        {
            if (htmlEditingTool.IsNull() ||
                parent.IsNull())
            {
                throw new ArgumentNullException();
            }

            htmlEditingTool.Dock = DockStyle.Fill;
            parent.Controls.Add(htmlEditingTool);
        }

        public static void EmbedHtmlSourceViewer(RichTextBox htmlSourceViewer, Control parent)
        {
            if (htmlSourceViewer.IsNull() ||
                parent.IsNull())
            {
                throw new ArgumentNullException();
            }

            htmlSourceViewer.Dock = DockStyle.Fill;
            htmlSourceViewer.BorderStyle = BorderStyle.None;
            htmlSourceViewer.ReadOnly = true;
            htmlSourceViewer.ScrollBars = RichTextBoxScrollBars.Both;
            parent.Controls.Add(htmlSourceViewer);
        }

        #endregion

        #region Переключение между средством визуального редактирования и просмоторщиком html кода
        
        public static void SwitchToHtmlEditingTool(HtmlEditingTool htmlEditingTool)
        {
            if (htmlEditingTool.IsNull())
            {
                throw new ArgumentNullException();
            }

            htmlEditingTool.BringToFront();
        }

        public static void SwitchToHtmlSourceViewer(RichTextBox richTextBox)
        {
            if (richTextBox.IsNull())
            {
                throw new ArgumentNullException();
            }

            richTextBox.BringToFront();
        }

        #endregion
    }
}