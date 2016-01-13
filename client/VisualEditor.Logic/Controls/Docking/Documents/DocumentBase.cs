using System.Windows.Forms;
using VisualEditor.Logic.Controls.HtmlEditing;
using VisualEditor.Utils.Controls.Docking;
using VisualEditor.Utils.Controls.HtmlEditing;
using HtmlToolEmbeddingHelper=VisualEditor.Logic.Controls.HtmlEditing.HtmlToolEmbeddingHelper;

namespace VisualEditor.Logic.Controls.Docking.Documents
{
    internal abstract class DocumentBase : DockContent
    {
        protected DocumentBase()
        {
            HtmlEditingTool = HtmlToolEmbeddingHelper.CreateHtmlEditingTool();
            HtmlToolEmbeddingHelper.EmbedHtmlEditingTool(HtmlEditingTool, this);
            HtmlSourceViewer = HtmlToolEmbeddingHelper.CreateHtmlSourceViewer();
            HtmlToolEmbeddingHelper.EmbedHtmlSourceViewer(HtmlSourceViewer, this);
            HtmlToolEmbeddingHelper.SwitchToHtmlEditingTool(HtmlEditingTool);

            new HtmlToolMediaHelper(HtmlEditingTool);
            new HtmlToolContextMenuHelper(HtmlEditingTool);

            DockAreas = DockAreas.Document;
            HideOnClose = true;

            TabPageContextMenuStrip = DockContainer.DocumentTabContextMenu;
        }

        public HtmlEditingTool HtmlEditingTool { get; private set; }
        public RichTextBox HtmlSourceViewer { get; private set; }

        public new virtual void Show()
        {
            Show(DockContainer.Instance);
        }
    }
}