using System.Collections.Generic;

namespace GoogleFileSync.SourceFiles
{
    public class SourceFolder
    {
        public readonly string FolderName;
        public readonly string FullLocation;
        public readonly IEnumerable<SourceFile> Files;
        public readonly IEnumerable<SourceFolder> Folders;

        public SourceFolder(string folderName, string fullLocation, IEnumerable<SourceFile> files, IEnumerable<SourceFolder> folders)
        {
            FolderName = folderName;
            FullLocation = fullLocation;
            Files = files;
            Folders = folders;
        }
    }
}
