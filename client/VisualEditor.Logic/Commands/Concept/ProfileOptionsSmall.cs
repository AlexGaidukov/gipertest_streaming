using System;
using System.Windows.Forms;
using VisualEditor.Logic.Dialogs;
using VisualEditor.Logic.Warehouse;

namespace VisualEditor.Logic.Commands.Concept
{
    internal class ProfileOptionsSmall : AbstractCommand
    {
        public ProfileOptionsSmall()
        {
            name = CommandNames.ProfileOptionsSmall;
            text = CommandTexts.ProfileOptionsSmall;
        }

        public override void Execute(object @object)
        {
            if (!Enabled)
            {
                return;
            }

            var с = Warehouse.Warehouse.Instance.ConceptTree.CurrentNode;

            if (с == null)
            {
                return;
            }

            using (var pd = new ProfileDialog())
            {
                var dtu = pd.DataTransferUnit;
                dtu.SetNodeValue("LowerBound", с.LowerBound.ToString());
                pd.InitializeData();

                if (pd.ShowDialog(EditorObserver.DialogOwner) == DialogResult.OK)
                {
                    с.LowerBound = (float)Convert.ToDouble(pd.DataTransferUnit.GetNodeValue("LowerBound"));
                }
            }
        }
    }
}