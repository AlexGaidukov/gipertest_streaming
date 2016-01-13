using System.Drawing;
using System.Windows.Forms;

namespace VisualEditor.Logic.Controls.Docking.Documents
{
    internal class PictureDocument : DocumentBase
    {
        public PictureDocument()
        {
            Text = "Предварительный просмотр ...";
            MainForm.Instance.Text = string.Concat("Предварительный просмотр рисунка - ", Application.ProductName);
            Icon = Icon.FromHandle(Properties.Resources.PictureSmall.GetHicon());
        }
    }
}