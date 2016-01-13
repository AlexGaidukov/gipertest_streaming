using VisualEditor.Logic.Controls.Docking;

namespace VisualEditor.Logic.Commands.Document
{
    internal class Close : AbstractCommand
    {
        public Close()
        {
            name = CommandNames.Close;
            text = CommandTexts.Close;
        }

        public override void Execute(object @object)
        {
            if (!Enabled)
            {
                return;
            }

            if (DockContainer.Instance.ActiveDocument != null)
            {
                DockContainer.Instance.ActiveDocument.DockHandler.Hide();
            }
        }
    }
}