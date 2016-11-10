
using Google.Apis.Auth.OAuth2;
using Google.Apis.Drive.v2;
using Google.Apis.Drive.v2.Data;
using Google.Apis.Services;
using Google.Apis.Util.Store;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace GoogleFileSync.Google
{
    public interface IGoogleDriveService
    {
        GoogleFolder GetRootFolder();
        GoogleFolder CreateRootFolder();

        bool DoesFileExistInFolder(GoogleFolder folder, string fileName);
        GoogleFile UploadFile(GoogleFolder parentFolder, string fileName, string fullLocation, string mimeType);

        bool DoesFolderExistInFolder(GoogleFolder folder, string folderName);
        GoogleFolder CreateFolder(GoogleFolder parentFolder, string folderName);
    }

    public class GoogleDriveService : IGoogleDriveService
    {
        static string[] Scopes = { DriveService.Scope.Drive };
        
        private readonly DriveService _service;
        private readonly IConfiguration _config;

        public GoogleDriveService(IConfiguration config)
        {
            _config = config;
            _service = Connect();
        }

        public DriveService Connect()
        {
            var credential = GoogleWebAuthorizationBroker.AuthorizeAsync(
                    new ClientSecrets() { ClientId = _config.ClientId, ClientSecret = _config.ClientSecret },
                    Scopes,
                    "user",
                    CancellationToken.None,
                    new FileDataStore("MB.GoogleDrive.Auth.Store", true)).Result;
            System.Console.WriteLine("Credentials saved");


            // Create Drive API service.
            return new DriveService(new BaseClientService.Initializer()
            {
                HttpClientInitializer = credential,
                ApplicationName = _config.ApplicationName,
            });
        }

        public GoogleFolder CreateFolder(GoogleFolder parentFolder, string folderName)
        {
            var guid = Guid.NewGuid();

            File body = new File();
            body.Title = folderName;
            body.Description = string.Format("Create by FileSync on: {0}", DateTime.Now);
            body.MimeType = "application/vnd.google-apps.folder";

            if(parentFolder != null)
            {
                body.Parents = new List<ParentReference>() { new ParentReference() { Id = parentFolder.Id } };
            }

            File file = _service.Files.Insert(body).Execute();

            return new GoogleFolder(file.Id, file.Title);
        }

        public GoogleFolder CreateRootFolder()
        {
            return CreateFolder(null, _config.GoogleRootFolderName);
        }

        public bool DoesFileExistInFolder(GoogleFolder parentFolder, string fileName)
        {
            // Define parameters of request.
            FilesResource.ListRequest findFolderRequest = _service.Files.List();
            findFolderRequest.Q = string.Format("trashed=false and '{0}' in parents and title='{1}'", parentFolder.Id, fileName);

            // List files.
            IList<File> files = findFolderRequest.Execute().Items;
            return files.Any();
        }

        public bool DoesFolderExistInFolder(GoogleFolder parentFolder, string folderName)
        {
            // Define parameters of request.
            FilesResource.ListRequest findFolderRequest = _service.Files.List();
            findFolderRequest.Q = string.Format("trashed=false and mimeType = 'application/vnd.google-apps.folder' and '{0}' in parents and title='{1}'", parentFolder.Id, folderName);

            // List files.
            IList<File> files = findFolderRequest.Execute().Items;
            return files.Any();
        }

        public GoogleFolder GetRootFolder()
        {
            // Define parameters of request.
            FilesResource.ListRequest findFolderRequest = _service.Files.List();
            findFolderRequest.Q = string.Format("trashed=false and mimeType = 'application/vnd.google-apps.folder' and title='{0}'", _config.GoogleRootFolderName);

            // List files.
            IList<File> files = findFolderRequest.Execute().Items;
            if(files.Any())
            {
                return new GoogleFolder(files[0].Id, files[0].Title);
            }

            return null;
        }

        public GoogleFile UploadFile(GoogleFolder parentFolder, string fileName, string fullLocation, string mimeType)
        {
            if (System.IO.File.Exists(fullLocation))
            {
                File body = new File();
                body.Title = fileName;
                body.Description = "File synced from local location: " + fullLocation;
                body.MimeType = mimeType;
                body.Parents = new List<ParentReference>() { new ParentReference() { Id = parentFolder.Id } };

                // File's content.
                byte[] byteArray = System.IO.File.ReadAllBytes(fullLocation);
                System.IO.MemoryStream stream = new System.IO.MemoryStream(byteArray);
                try
                {
                    FilesResource.InsertMediaUpload request = _service.Files.Insert(body, stream, mimeType);
                    request.Upload();
                    var uploadedFile =  request.ResponseBody;
                    return new GoogleFile(uploadedFile.Id, uploadedFile.Title);
                }
                catch (Exception e)
                {
                    //todo log
                    return null;
                }
            }
            else
            {
                //todo log
                return null;
            }
        }
    }



}
