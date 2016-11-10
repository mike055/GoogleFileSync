namespace GoogleFileSync.Google
{
    public class GoogleFile
    {
        public readonly string Id;
        public readonly string Title;

        public GoogleFile(string id, string title)
        {
            Id = id;
            Title = title;
        }
    }
}
