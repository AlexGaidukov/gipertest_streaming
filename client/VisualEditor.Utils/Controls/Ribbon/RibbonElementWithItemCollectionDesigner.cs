using System;
using System.ComponentModel;
using System.ComponentModel.Design;

namespace VisualEditor.Utils.Controls.Ribbon
{
    internal abstract class RibbonElementWithItemCollectionDesigner : ComponentDesigner
    {
        #region Props

        public abstract Controls.Ribbon.Ribbon Ribbon { get; }

        public abstract RibbonItemCollection Collection { get; }

        public override DesignerVerbCollection Verbs
        {
            get
            {
                return new DesignerVerbCollection(new[] { 
                                                            new DesignerVerb("Add Button", AddButton),
                                                            new DesignerVerb("Add ButtonList", AddButtonList),
                                                            new DesignerVerb("Add ItemGroup", AddItemGroup),
                                                            new DesignerVerb("Add Separator", AddSeparator),
                                                            new DesignerVerb("Add TextBox", AddTextBox),
                                                            new DesignerVerb("Add ComboBox", AddComboBox),
                                                            new DesignerVerb("Add ColorChooser", AddColorChooser)
                                                        });
            }
        }

        #endregion

        #region Methods

        private void CreateItem(Type t)
        {
            CreateItem(Ribbon, Collection, t);
        }

        protected virtual void CreateItem(Controls.Ribbon.Ribbon ribbon, RibbonItemCollection collection, Type t)
        {
            IDesignerHost host = GetService(typeof(IDesignerHost)) as IDesignerHost;

            if (host != null && collection != null && ribbon != null)
            {
                DesignerTransaction transaction = host.CreateTransaction("AddRibbonItem_" + Component.Site.Name);

                MemberDescriptor member = TypeDescriptor.GetProperties(Component)["Items"];
                base.RaiseComponentChanging(member);

                RibbonItem item = host.CreateComponent(t) as RibbonItem;

                if (!(item is RibbonSeparator)) item.Text = item.Site.Name;

                collection.Add(item);
                ribbon.OnRegionsChanged();

                base.RaiseComponentChanged(member, null, null);
                transaction.Commit();
            }
        }

        protected virtual void AddButton(object sender, EventArgs e)
        {
            CreateItem(typeof(RibbonButton));
        }

        protected virtual void AddButtonList(object sender, EventArgs e)
        {
            CreateItem(typeof(RibbonButtonList));
        }

        protected virtual void AddItemGroup(object sender, EventArgs e)
        {
            CreateItem(typeof(RibbonItemGroup));
        }

        protected virtual void AddSeparator(object sender, EventArgs e)
        {
            CreateItem(typeof(RibbonSeparator));
        }

        protected virtual void AddTextBox(object sender, EventArgs e)
        {
            CreateItem(typeof(RibbonTextBox));
        }

        protected virtual void AddComboBox(object sender, EventArgs e)
        {
            CreateItem(typeof(RibbonComboBox));
        }

        protected virtual void AddColorChooser(object sender, EventArgs e)
        {
            CreateItem(typeof(RibbonColorChooser));
        }

        #endregion
    }
}