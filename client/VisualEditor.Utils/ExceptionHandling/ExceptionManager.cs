using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.Windows.Forms;
using VisualEditor.Utils.Helpers;

namespace VisualEditor.Utils.ExceptionHandling
{
    public class ExceptionManager
    {
        private const string loggingFailedMessage = "Во время обработки отладочной информации произошла ошибка.";

        private static ExceptionManager instance;
        private readonly List<IExceptionLogger> loggers;

        public delegate void LogExceptionHandler(Exception e);
        public static event EventHandler ExceptionHandling;
        public static event EventHandler ExceptionHandled;

        private ExceptionManager()
        {
            loggers = new List<IExceptionLogger>();
            Application.ThreadException += (s, e) => HandleException(e.Exception);
            AppDomain.CurrentDomain.UnhandledException += (s, e) => HandleException((Exception)e.ExceptionObject);
        }

        public static ExceptionManager Instance
        {
            get { return instance ?? (instance = new ExceptionManager()); }
        }

        public void AddLogger(IExceptionLogger logger)
        {
            if (logger.IsNull())
            {
                throw new ArgumentNullException();
            }

            var recurrentLoggers = from tempLogger in loggers
                                   where tempLogger.GetType() == logger.GetType()
                                   select tempLogger;

            // Прерывает выполнение, если в списке логгеров уже есть логгер с данным типом.
            if (recurrentLoggers.GetEnumerator().MoveNext())
            {
                throw new InvalidOperationException();
            }

            loggers.Add(logger);
        }

        private void HandleException(Exception e)
        {
            if (!ExceptionHandling.IsNull())
            {
                ExceptionHandling(this, EventArgs.Empty);
            }

            var logExceptionHandler = new LogExceptionHandler(LogException);
            logExceptionHandler.BeginInvoke(e, LogCallback, null);
        }

        public void LogException(Exception exception)
        {
            foreach(var logger in loggers)
            {
                try
                {
                    logger.Log(exception);
                }
                catch (Exception)
                {
                    MessageBox.Show(loggingFailedMessage, Application.ProductName, 
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private static void LogCallback(IAsyncResult result)
        {
            var asyncResult = (AsyncResult)result;
            var logExceptionHandler = (LogExceptionHandler)asyncResult.AsyncDelegate;
            if (!asyncResult.EndInvokeCalled)
            {
                logExceptionHandler.EndInvoke(result);
            }

            if (!ExceptionHandled.IsNull())
            {
                ExceptionHandled(instance, EventArgs.Empty);
            }
        }
    }
}