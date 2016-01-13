using System.Drawing;
using VisualEditor.Logic.Course.Items;

namespace VisualEditor.Logic.Controls.Docking.Documents
{
    internal class ResponseDocument : DocumentBase
    {
        public ResponseDocument()
        {
            Icon = Icon.FromHandle(Properties.Resources.Response.GetHicon());
        }

        public Response Response { get; set; }
    }
}