using System;
using System.Windows.Forms;
using VisualEditor.Logic.Controls.Ribbon.Extended;
using VisualEditor.Utils.ExceptionHandling;

namespace VisualEditor.Logic.Helpers
{
    internal class ExceptionHelper
    {
        private int tickCount;
        private const int maxTickCount = 6;
        private bool isExceptionLabelVisible;

        public ExceptionHelper()
        {
            ExceptionManager.ExceptionHandling += ExceptionManager_ExceptionHandling;
            RibbonStatusStripEx.Instance.UnhandledExceptionLabel.Click += UnhandledExceptionLabel_Click;
        }

        private void ExceptionManager_ExceptionHandling(object sender, EventArgs e)
        {
            tickCount = 0;
            RibbonStatusStripEx.Instance.UnhandledExceptionLabel.Image = Properties.Resources.UnhandledException;
            isExceptionLabelVisible = true;
            var exceptionTimer = new Timer
                                     {
                                         Interval = 800
                                     };
            exceptionTimer.Tick += exceptionTimer_Tick;
            exceptionTimer.Enabled = true;
        }

        private void exceptionTimer_Tick(object sender, EventArgs e)
        {
            if (tickCount % 2 == 0)
            {
                RibbonStatusStripEx.Instance.UnhandledExceptionLabel.Image = Properties.Resources.DummyUnhandledException;
                isExceptionLabelVisible = false;
            }
            else
            {
                RibbonStatusStripEx.Instance.UnhandledExceptionLabel.Image = Properties.Resources.UnhandledException;
                isExceptionLabelVisible = true;
            }

            tickCount++;

            if (tickCount == maxTickCount)
            {
                ((Timer)sender).Enabled = false;
            }
        }

        private void UnhandledExceptionLabel_Click(object sender, EventArgs e)
        {
            if (!isExceptionLabelVisible)
            {
                return;
            }

            UIHelper.ShowMessage("В процессе работы приложения возникла ошибка. Пожалуйста, свяжитесь с разработчиками.",
                MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }
}