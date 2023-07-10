using Domain.Data.Core;

namespace Domain.Data.Hyphen.Model
{
    public class Configuration : EntityBase
    {
        public int ConfigurationId { get; set; }
        public string ConfigName { get; set; }
        public string DocumentType { get; set; }
        public string TransactionType { get; set; }
        public string DailyCutoffTime { get; set; }

        public override int Key
        {
            get { return ConfigurationId; }
        }
    }
}