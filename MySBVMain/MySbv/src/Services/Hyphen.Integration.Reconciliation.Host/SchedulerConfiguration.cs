using System.Configuration;

namespace Hyphen.Integration.Reconciliation.Host
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
        [ConfigurationProperty("Interval", DefaultValue = 180, IsRequired = true)]
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