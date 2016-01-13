using System;

namespace VisualEditor.Utils.ExceptionHandling
{
    public interface IExceptionLogger
    {
        void Log(Exception exception);
    }
}