using System.Drawing;
using System.Windows.Forms;

namespace VisualEditor.Logic.Controls.Docking.Documents
{
    internal class VideoDocument : DocumentBase
    {
        public VideoDocument()
        {
            Text = "Предварительный просмотр ...";
            MainForm.Instance.Text = string.Concat("Предварительный просмотр видео - ", Application.ProductName);
            Icon = Icon.FromHandle(Properties.Resources.VideoSmall.GetHicon());            
        }
    }
}