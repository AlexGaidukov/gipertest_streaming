using System.Drawing;
using System.Windows.Forms;

namespace VisualEditor.Logic.Controls.Docking.Documents
{
    internal class AudioDocument : DocumentBase
    {
        public AudioDocument()
        {
            Text = "Предварительное прослуши ...";
            MainForm.Instance.Text = string.Concat("Предаварительное прослушивание аудио - ", Application.ProductName);
            Icon = Icon.FromHandle(Properties.Resources.AudioSmall.GetHicon());
        }
    }
}