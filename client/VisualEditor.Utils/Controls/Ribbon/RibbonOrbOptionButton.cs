using System.ComponentModel;

namespace VisualEditor.Utils.Controls.Ribbon
{
    public class RibbonOrbOptionButton : RibbonButton
    {
        #region Ctors

        public RibbonOrbOptionButton()
        {

        }

        public RibbonOrbOptionButton(string text)
            : this()
        {
            Text = text;
        }

        #endregion

        #region Props
        public override System.Drawing.Image Image
        {
            get
            {
                return base.Image;
            }
            set
            {
                base.Image = value;

                SmallImage = value;
            }
        }

        [Browsable(false)]
        public override System.Drawing.Image SmallImage
        {
            get
            {
                return base.SmallImage;
            }
            set
            {
                base.SmallImage = value;
            }
        }

        #endregion
    }
}