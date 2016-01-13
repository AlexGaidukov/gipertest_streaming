using System;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Windows.Forms.Design.Behavior;

namespace VisualEditor.Utils.Controls.Ribbon
{
    public class RibbonTabDesigner : ComponentDesigner
    {
        Adorner panelAdorner;

        public override DesignerVerbCollection Verbs
        {
            get
            {
                return new DesignerVerbCollection(new [] { 
                                                             new DesignerVerb("Add Panel", AddPanel)
                                                         });
            }
        }

        public RibbonTab Tab
        {
            get { return Component as RibbonTab; }
        }

        public void AddPanel(object sender, EventArgs e)
        {
            var host = GetService(typeof(IDesignerHost)) as IDesignerHost;

            if (host != null && Tab != null)
            {
                

                var transaction = host.CreateTransaction("AddPanel" + Component.Site.Name);
                var member = TypeDescriptor.GetProperties(Component)["Panels"];
                base.RaiseComponentChanging(member);

                var panel = host.CreateComponent(typeof(RibbonPanel)) as RibbonPanel;

                if (panel != null)
                {
                    panel.Text = panel.Site.Name;
                    Tab.Panels.Add(panel);
                    Tab.Owner.OnRegionsChanged();
                }

                base.RaiseComponentChanged(member, null, null);
                transaction.Commit();
            }
        }

        public override void Initialize(IComponent component)
        {
            base.Initialize(component);

            panelAdorner = new Adorner();

            BehaviorService bs = RibbonDesigner.Current.GetBehaviorService();

            if (bs == null) return;

            bs.Adorners.AddRange(new [] { panelAdorner });

            panelAdorner.Glyphs.Add(new RibbonPanelGlyph(bs, this, Tab));
        }
    }
}