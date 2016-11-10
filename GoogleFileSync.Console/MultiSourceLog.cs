using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GoogleFileSync.Console
{
    public class MultiSourceLog : ILog
    {
        private readonly ILog _consoleLogger;
        private readonly ILog _fileLogger;

        public MultiSourceLog(ILog consoleLogger, ILog fileLogger)
        {
            _fileLogger = fileLogger;
            _consoleLogger = consoleLogger;
        }

        public void Error(Exception ex)
        {
            _consoleLogger.Error(ex);
            _fileLogger.Error(ex);
        }

        public void Error(string message, params string[] args)
        {
            _consoleLogger.Error(message, args);
            _fileLogger.Error(message, args);
        }

        public void Info(string message, params string[] args)
        {
            _consoleLogger.Info(message, args);
            _fileLogger.Info(message, args);
        }
    }
}
