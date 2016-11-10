using System;
using System.IO;

namespace GoogleFileSync.Console
{
    public class FileWriterLog : ILog
    {
        public readonly IConfiguration _config;
        public readonly string _logFile;

        public FileWriterLog(IConfiguration config)
        {
            _config = config;
            _logFile = string.Format("{0}{1}.txt", _config.LogFolderDirectory, DateTime.Now.ToString("yyyy-MM-dd_hh-mm-ss"));
            EnsureLogFolder();
        }

        public void Error(Exception ex)
        {
            File.AppendAllText(_logFile, FormatMessage("ERROR", ex.ToString()));
        }

        public void Error(string message, params string[] args)
        {
            File.AppendAllText(_logFile, FormatMessage("ERROR", string.Format(message, args)));
        }

        public void Info(string message, params string[] args)
        {
            File.AppendAllText(_logFile, FormatMessage("INFO", string.Format(message, args)));
        }

        private string FormatMessage(string level, string message)
        {
            return string.Format("{0} | {1} : {2}{3}", level, DateTime.Now.ToString("dd-MM-yyyy hh:mm:ss"), message, Environment.NewLine);
        }

        private void EnsureLogFolder()
        {
            if (!Directory.Exists(_config.LogFolderDirectory))
            {
                Directory.CreateDirectory(_config.LogFolderDirectory);
            }
        }
    }
}
