using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace GoogleFileSync.SourceFiles
{
    public interface ISourceFileProvider
    {
        SourceFolder GetForDirectory(string directory);
    }

    public class FileSystemSourceFileProvider : ISourceFileProvider
    {
        private readonly ILog _log;
        private readonly IMimeTypeResolver _mimeTypeReolver;
        private const string SEARCH_PATTERN = "*";

        public FileSystemSourceFileProvider(IMimeTypeResolver mimeTypeReolver, ILog log)
        {
            _log = log;
            _mimeTypeReolver = mimeTypeReolver;
        }

        public SourceFolder GetForDirectory(string directory)
        {
            try
            {
                return GetFolder(directory);
            }
            catch(Exception e)
            {
                _log.Error(e);
            }

            return null;
        }

        private IEnumerable<SourceFile> GetFilesForFolder(DirectoryInfo directory)
        {
            var files = new List<SourceFile>();

            foreach (FileInfo f in directory.GetFiles(SEARCH_PATTERN))
            {
                files.Add(new SourceFile(f.Name, f.FullName, _mimeTypeReolver.GetMimeType(f.Name)));
            }

            if (files.Any())
                return files;

            return null;
        }

        private SourceFolder GetFolder(string directory)
        {
            var directoryInfo = new DirectoryInfo(directory);

            var folders = new List<SourceFolder>();
            foreach (DirectoryInfo d in directoryInfo.GetDirectories())
            {
                folders.Add(GetFolder(d.FullName));   
            }

            if(!folders.Any())
            {
                folders = null;
            }

            return new SourceFolder(directory, directoryInfo.Name, GetFilesForFolder(directoryInfo), folders);
        }


    }

}
