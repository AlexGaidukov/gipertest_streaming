using System;
using System.Windows.Forms;

namespace VisualEditor.Utils.Controls
{
    public class UpDown : NumericUpDown
    {
        public event EventHandler UpButtonClicked = null;
        public event EventHandler DownButtonClicked = null;

        public void OnUpButtonClicked(EventArgs e)
        {
            var eventCopy = UpButtonClicked;
            if (eventCopy != null)
            {
                eventCopy(this, e);
            }
        }

        public void OnDownButtonClicked(EventArgs e)
        {
            var eventCopy = DownButtonClicked;
            if (eventCopy != null)
            {
                eventCopy(this, e);
            }
        }

        public override void DownButton()
        {
            try
            {                
                OnDownButtonClicked(new EventArgs());
            }
            catch
            {
            }

            base.DownButton();
        }

        public override void UpButton()
        {
            try
            {               
                OnUpButtonClicked(new EventArgs());
            }
            catch
            {
            }

            base.UpButton();
        }
    }
}