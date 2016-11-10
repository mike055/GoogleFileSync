using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GoogleFileSync.Console
{
    public class ConfigurationProvider : IConfiguration
    {
        public string ApplicationName
        {
            get { return System.Configuration.ConfigurationManager.AppSettings["ApplicationName"]; }
        }

        public string ClientId
        {
            get { return System.Configuration.ConfigurationManager.AppSettings["ClientId"]; }
        }

        public string ClientSecret
        {
            get { return System.Configuration.ConfigurationManager.AppSettings["ClientSecret"]; }
        }

        public string SyncFromFolder
        {
            get { return System.Configuration.ConfigurationManager.AppSettings["SyncFromFolder"]; }
        }

        public string LogFolderDirectory
        {
            get { return System.Configuration.ConfigurationManager.AppSettings["LogFolderDirectory"]; }
        }

        public string GoogleRootFolderName
        {
            get { return System.Configuration.ConfigurationManager.AppSettings["GoogleRootFolderName"]; }
        }
    }
}
