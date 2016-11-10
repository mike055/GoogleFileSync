using GoogleFileSync.SourceFiles;
using GoogleFileSync.Google;

namespace GoogleFileSync.Console
{
    class Program
    {
        
        static void Main(string[] args)
        {
            var config = new ConfigurationProvider();

            var consoleLog = new ConsoleWriterLog();
            var fileLog = new FileWriterLog(config);
            var multiLog = new MultiSourceLog(consoleLog, fileLog);

            var orchestrator = new SyncOrchestrator(new FileSystemSourceFileProvider(new MimeTypeResolver(), multiLog), new GoogleDriveService(config), multiLog);
            orchestrator.Sync(config.SyncFromFolder);
        }
    }
}
