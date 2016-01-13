using System.Drawing;
using System.Windows.Forms;

namespace VisualEditor.Logic.Controls.Docking.Documents
{
    internal class AnimationDocument : DocumentBase
    {
        public AnimationDocument()
        {
            Text = "Предварительный просмотр ...";
            MainForm.Instance.Text = string.Concat("Предварительный просмотр анимации - ", Application.ProductName);
            Icon = Icon.FromHandle(Properties.Resources.AnimationSmall.GetHicon());
        }
    }
}