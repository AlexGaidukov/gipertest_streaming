namespace VisualEditor.Logic.Commands.Concept
{
    internal class Profile : AbstractCommand
    {
        public Profile()
        {
            name = CommandNames.Profile;
            text = CommandTexts.Profile;
            image = Properties.Resources.Profile;
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

            с.IsProfile = !с.IsProfile;

            if (с.IsProfile)
            {
                с.ImageIndex = с.SelectedImageIndex = 0;
            }
            else
            {
                с.ImageIndex = с.SelectedImageIndex = 1;
                с.LowerBound = 0;
            }

            Warehouse.Warehouse.IsProjectModified = true;
        }
    }
}