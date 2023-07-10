using Domain.Data.Core;

namespace Domain.Data.Model
{
    public class RoleReport : EntityBase
    {
        public int RoleReportId { get; set; }

        public int RoleId { get; set; }
        public int ReportId { get; set; }

        public virtual Report Report { get; set; }

        public override int Key
        {
            get { return RoleReportId; }
        }
    }
}