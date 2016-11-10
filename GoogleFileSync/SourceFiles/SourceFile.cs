namespace GoogleFileSync.SourceFiles
{
    public class SourceFile
    {
        public readonly string FileName;
        public readonly string FullLocation;
        public readonly string MimeType;

        public SourceFile(string fileName, string fullLocation, string mimeType)
        {
            FileName = fileName;
            FullLocation = fullLocation;
            MimeType = mimeType;
        }
    }
}
