using System.Configuration;

namespace Hyphen.Integration.Service
{
    public class SchedulerConfiguration : ConfigurationSection
    {
        #region Fields

        private SchedulerConfiguration _settings;

        #endregion

        #region Properties

        /// <summary>
        ///     The notification/check interval in minutes.
        /// </summary>
        [ConfigurationProperty("Interval", DefaultValue = 30, IsRequired = true)]
        [IntegerValidator(MinValue = 0, MaxValue = 1440)]
        public int Interval
        {
            get { return (int) this["Interval"]; }
        }
        
        public SchedulerConfiguration Configuration
        {
            get { return _settings; }
        }

        #endregion

        #region Methods

        public void Initialize()
        {
            _settings = ConfigurationManager.GetSection("SchedulerConfiguration") as SchedulerConfiguration;
        }

        #endregion
    }
}