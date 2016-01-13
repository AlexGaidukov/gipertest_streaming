using System.Drawing;
using VisualEditor.Logic.Course.Items;

namespace VisualEditor.Logic.Controls.Docking.Documents
{
    internal class TrainingModuleDocument : DocumentBase
    {
        public TrainingModuleDocument()
        {
            Icon = Icon.FromHandle(Properties.Resources.TrainingModule.GetHicon());
        }

        public TrainingModule TrainingModule { get; set; }
    }
}