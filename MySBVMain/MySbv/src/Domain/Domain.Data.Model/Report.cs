using Domain.Data.Core;

namespace Domain.Data.Model
{
    public class Report : EntityBase, IIdentity
    {
        public int ReportId { get; set; }
        public string Path { get; set; }

        public override int Key
        {
            get { return ReportId; }
        }

        public string Name { get; set; }
        public string Description { get; set; }
        public string LookUpKey { get; set; }
    }
}