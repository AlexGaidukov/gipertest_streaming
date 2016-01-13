using System;
using System.Drawing;
using System.Windows.Forms;
using VisualEditor.Utils.Controls.Ribbon;

namespace VisualEditor.Logic.Controls.Ribbon.Extended
{
    internal class RibbonStatusStripEx : RibbonStatusStrip
    {
        private ToolStripStatusLabel messageLabel;
        private ToolStripProgressBar progressBar;
        private ToolStripStatusLabel overwriteLabel;
        private ToolStripStatusLabel unhandledExceptionLabel;

        private double step;
        private int readModulesCount;

        private static RibbonStatusStripEx instance;

        private RibbonStatusStripEx()
        {           
            messageLabel = new ToolStripStatusLabel
                               {
                                   Spring = true,
                                   TextAlign = ContentAlignment.MiddleLeft
                               };
            progressBar = new ToolStripProgressBar();
            ProgressBarVisible = false;
            overwriteLabel = new ToolStripStatusLabel
                                 {
                                     TextAlign = ContentAlignment.MiddleLeft
                                 };
            unhandledExceptionLabel = new ToolStripStatusLabel
                                          {
                                              Image = Properties.Resources.DummyUnhandledException
                                          };
            Items.Add(messageLabel);
            Items.Add(progressBar);
            Items.Add(overwriteLabel);
            Items.Add(unhandledExceptionLabel);
        }

        public static RibbonStatusStripEx Instance
        {
            get { return instance ?? (instance = new RibbonStatusStripEx()); }
        }

        public bool ProgressBarVisible
        {
            set { progressBar.Visible = value; }
        }

        public int ModulesCount
        {
            set { step = 90.0 / value; }
        }

        public void ClearOverwriteLabel()
        {
            overwriteLabel.Text = string.Empty;
        }

        public void MakeInsert()
        {
            overwriteLabel.Text = "INS      ";
        }

        public void MakeOverwrite()
        {
            overwriteLabel.Text = "OVR      ";
        }

        public void SetMessage(string message)
        {
            messageLabel.Text = string.Concat("   ", message);
        }

        public void SetProgress(int value)
        {
            progressBar.Value = value;

            if (value.Equals(0))
            {
                step = 0;
                readModulesCount = 0;
            }
        }

        public void MakeProgressStep(int initialValue)
        {
            readModulesCount++;
            var value = initialValue + step * readModulesCount;
            if (value < 100)
            {
                progressBar.Value = (int)Math.Round(value);
            }
        }

        public ToolStripStatusLabel UnhandledExceptionLabel
        {
            get { return unhandledExceptionLabel; }
        }
    }
}