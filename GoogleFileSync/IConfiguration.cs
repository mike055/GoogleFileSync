namespace GoogleFileSync
{
    public interface IConfiguration
    {
        string ApplicationName { get; }
        string ClientId { get; }
        string ClientSecret { get; }
        string SyncFromFolder { get;}
        string LogFolderDirectory { get; }
        string GoogleRootFolderName { get; }
    }
}
