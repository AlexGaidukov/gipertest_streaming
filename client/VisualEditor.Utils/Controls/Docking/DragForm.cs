using System;
using System.Windows.Forms;
using VisualEditor.Utils.Controls.Docking.Win32;

namespace VisualEditor.Utils.Controls.Docking
{
    internal class DragForm : Form
    {
        public DragForm()
        {
            FormBorderStyle = FormBorderStyle.None;
            ShowInTaskbar = false;
            SetStyle(ControlStyles.Selectable, false);
            Enabled = false;
        }

        protected override CreateParams CreateParams
        {
            get
            {
                CreateParams createParams = base.CreateParams;
                createParams.ExStyle |= (int)WindowExStyles.WS_EX_TOOLWINDOW;
                return createParams;
            }
        }

        protected override void WndProc(ref Message m)
        {
            if (m.Msg == (int)Msgs.WM_NCHITTEST)
            {
                m.Result = (IntPtr)HitTest.HTTRANSPARENT;
                return;
            }

            base.WndProc (ref m);
        }

        public virtual void Show(bool bActivate)
        {
            if (bActivate)
                Show();
            else
                NativeMethods.ShowWindow(Handle, (int)ShowWindowStyles.SW_SHOWNOACTIVATE);
        }
    }
}