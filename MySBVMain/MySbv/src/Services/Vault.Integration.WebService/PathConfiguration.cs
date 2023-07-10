using System.Configuration;

namespace Vault.Integration.WebService
{
    public class PathConfiguration : ConfigurationSection
    {
        #region Fields

        private PathConfiguration _settings;

        #endregion

        #region Properties

        /// <summary>
        ///     The notification/check interval in minutes.
        /// </summary>
        [ConfigurationProperty("Path", DefaultValue = @"C:\", IsRequired = true)]
        public string Path
        {
            get { return (string) this["Path"]; }
        }


        public PathConfiguration Configuration
        {
            get { return _settings; }
        }

        #endregion

        #region Methods

        internal void Initialize()
        {
            _settings = ConfigurationManager.GetSection("PathConfiguration") as PathConfiguration;
        }

        #endregion
    }
}