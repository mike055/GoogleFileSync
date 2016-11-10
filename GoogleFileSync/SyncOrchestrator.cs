using GoogleFileSync.Google;
using GoogleFileSync.SourceFiles;
using System;

namespace GoogleFileSync
{
    public class SyncOrchestrator
    {
        private readonly ILog _log;
        private readonly ISourceFileProvider _sourceFileProvider;
        private readonly IGoogleDriveService _googleDriveService;

        public SyncOrchestrator(ISourceFileProvider sourceFileProvider, IGoogleDriveService googleDriveService, ILog log)
        {
            _log = log;
            _googleDriveService = googleDriveService;
            _sourceFileProvider = sourceFileProvider;
        }

        public void Sync(string directory)
        {
            var rootGoogleFolder = GetGoogleSyncFolder();
            var sourceFolder = _sourceFileProvider.GetForDirectory(directory);

            _log.Info("SourceFolder: {0}, GoogleFolder: {1}-{2}", sourceFolder.FullLocation, rootGoogleFolder.Id, rootGoogleFolder.Title);

            SyncFolder(sourceFolder, rootGoogleFolder);
        }

        public void SyncFolder(SourceFolder sourceFolder, GoogleFolder remoteFolder)
        {
            if (sourceFolder.Files != null)
            {
                foreach (var file in sourceFolder.Files)
                {
                    if(!_googleDriveService.DoesFileExistInFolder(remoteFolder, file.FileName))
                    {
                        try
                        {
                            var remoteFile = _googleDriveService.UploadFile(remoteFolder, file.FileName, file.FullLocation, file.MimeType);
                            _log.Info("File {0} uploaded, google id: {1}", file.FullLocation, remoteFile.Id);
                        }
                        catch(Exception ex)
                        {
                            _log.Error(ex);
                        }
                    }
                    else
                    {
                        _log.Info("File {0} already exists", file.FullLocation);
                    }
                }
            }
            if (sourceFolder.Folders != null)
            {
                foreach (var childFolder in sourceFolder.Folders)
                {
                    if(_googleDriveService.DoesFolderExistInFolder(remoteFolder, childFolder.FolderName))
                    {
                        try
                        {
                            var childRemoteFolder = _googleDriveService.CreateFolder(remoteFolder, childFolder.FolderName);
                            _log.Info("Folder {0} created, google id: {1}", childFolder.FullLocation, childRemoteFolder.Id);

                            SyncFolder(childFolder, childRemoteFolder);
                        }
                        catch (Exception ex)
                        {
                            _log.Error(ex);
                        }
                    }
                    else
                    {
                        _log.Info("Folder {0} already exists", childFolder.FullLocation);
                    }

                }
            }
        }

        private GoogleFolder GetGoogleSyncFolder()
        {
            var rootGoogleFolder = _googleDriveService.GetRootFolder();

            if (rootGoogleFolder == null)
            {
                rootGoogleFolder = _googleDriveService.CreateRootFolder();
            }

            return rootGoogleFolder;
        }
    }
}
