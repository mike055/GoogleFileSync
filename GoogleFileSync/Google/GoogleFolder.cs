namespace GoogleFileSync.Google
{
    public class GoogleFolder
    {
        public readonly string Id;
        public readonly string Title;

        public GoogleFolder(string id, string title)
        {
            Id = id;
            Title = title;
        }
    }
}
