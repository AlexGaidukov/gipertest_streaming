using System.Drawing;
using VisualEditor.Logic.Course.Items;

namespace VisualEditor.Logic.Controls.Docking.Documents
{
    internal class QuestionDocument : DocumentBase
    {
        public QuestionDocument()
        {
            Icon = Icon.FromHandle(Properties.Resources.Question.GetHicon());
        }

        public Question Question { get; set; }
    }
}