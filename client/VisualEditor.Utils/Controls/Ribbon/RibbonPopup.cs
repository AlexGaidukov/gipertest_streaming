using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace VisualEditor.Utils.Controls.Ribbon
{
    [ToolboxItem(false)]
    public class RibbonPopup : Control
    {
        #region Fields

        private bool _forceClose;
        #endregion

        #region Events

        public event EventHandler Showed;

        /// <summary>
        /// Raised when the popup is closed
        /// </summary>
        public event EventHandler Closed;

        /// <summary>
        /// Raised when the popup is about to be closed
        /// </summary>
        public event ToolStripDropDownClosingEventHandler Closing;

        /// <summary>
        /// Raised when the Popup is about to be opened
        /// </summary>
        public event CancelEventHandler Opening;

        #endregion

        #region Ctor

        public RibbonPopup()
        {
            SetStyle(ControlStyles.Selectable, false);   
        }

        #endregion

        #region Props

        /// <summary>
        /// Gets the related ToolStripDropDown
        /// </summary>
        public ToolStripDropDown ToolStripDropDown { get; set; }

        /// <summary>
        /// Gets or sets the previous popup in the chain
        /// </summary>
        public RibbonPopup PreviousPopup { get; set; }

        /// <summary>
        /// Gets or sets the next popup in the chain
        /// </summary>
        public RibbonPopup NextPopup { get; set; }

        #endregion

        #region Methods

        /// <summary>
        /// Shows this Popup on the specified location of the screen
        /// </summary>
        /// <param name="screenLocation"></param>
        public void Show(Point screenLocation)
        {
            var host = new ToolStripControlHost(this);
            ToolStripDropDown = new ToolStripDropDown();
            ToolStripDropDown.Items.Clear();
            ToolStripDropDown.Items.Add(host);
            
            ToolStripDropDown.Padding = Padding.Empty;
            ToolStripDropDown.Margin = Padding.Empty;
            host.Padding = Padding.Empty;
            host.Margin = Padding.Empty;

            ToolStripDropDown.Opening += ToolStripDropDown_Opening;
            ToolStripDropDown.Closing += ToolStripDropDown_Closing;
            ToolStripDropDown.Closed += ToolStripDropDown_Closed;
            ToolStripDropDown.Show(screenLocation);
            MouseWheel += ToolStripDropDown_MouseWheel;

            OnShowed(EventArgs.Empty);
        }

        void ToolStripDropDown_MouseWheel(object sender, MouseEventArgs e)
        {
            MessageBox.Show("");
        }
        
        /// <summary>
        /// Handles the Opening event of the ToolStripDropDown
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void ToolStripDropDown_Opening(object sender, CancelEventArgs e)
        {
            OnOpening(e);
        }

        protected virtual void OnOpening(CancelEventArgs e)
        {
            if (Opening != null)
            {
                Opening(this, e);
            }
        }

        /// <summary>
        /// Handles the Closing event of the ToolStripDropDown
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void ToolStripDropDown_Closing(object sender, ToolStripDropDownClosingEventArgs e)
        {
            OnClosing(e);
        }

        /// <summary>
        /// Handles the closed event of the ToolStripDropDown
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void ToolStripDropDown_Closed(object sender, ToolStripDropDownClosedEventArgs e)
        {
            OnClosed(EventArgs.Empty);
        }

        /// <summary>
        /// Closes this popup and every other next to it in the chain
        /// </summary>
        public void Close()
        {
            if (ToolStripDropDown != null)
            {
                ToolStripDropDown.Close();
            }
        }

        /// <summary>
        /// Closes this popup and all popups connected with it
        /// </summary>
        public void CloseAll()
        {
            _forceClose = true;

            Close();

            if (PreviousPopup != null)
            {
                PreviousPopup.CloseAll();
            }
        }

        /// <summary>
        /// Raises the <see cref="Closing"/> event
        /// </summary>
        /// <param name="e"></param>
        protected virtual void OnClosing(ToolStripDropDownClosingEventArgs e)
        {
            if (NextPopup != null && !_forceClose)
            {
                //e.Cancel = true;
                //return;
            }

            if (Closing != null)
            {
                Closing(this, e);
            }

            _forceClose = false;
        }

        /// <summary>
        /// Raises the <see cref="Closed"/> event
        /// </summary>
        /// <param name="e"></param>
        protected virtual void OnClosed(EventArgs e)
        {
            if (Closed != null)
            {
                Closed(this, e);
            }

            if (NextPopup != null)
            {
                NextPopup.Close();
            }

            if (PreviousPopup != null && PreviousPopup.NextPopup.Equals(this))
            {
                PreviousPopup.NextPopup = null;
            }
        }

        /// <summary>
        /// Raises the Showed event
        /// </summary>
        /// <param name="e"></param>
        protected virtual void OnShowed(EventArgs e)
        {
            if (Showed != null)
            {
                Showed(this, e);
            }
        }

        protected override void WndProc(ref Message m)
        {
            if (m.Msg == 0x86)
            {
                m.Result = IntPtr.Zero;
            }
            else
            {
                base.WndProc(ref m);
            }

        }

        #endregion
    }
}