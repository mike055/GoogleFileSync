using System;

namespace GoogleFileSync
{
    public interface ILog
    {
        void Error(string message, params string[] args);
        void Error(Exception ex);
        void Info(string message, params string[] args);
    }
}
