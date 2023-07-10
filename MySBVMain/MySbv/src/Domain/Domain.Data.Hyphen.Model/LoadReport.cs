using System;
using Domain.Data.Core;

namespace Domain.Data.Hyphen.Model
{
    public class LoadReport : EntityBase
    {
        public int LoadReportId { get; set; }
        public int BatchFileId { get; set; }
        public string BatchNumber { get; set; }
        public int TotalReceivedCount { get; set; }
        public decimal TotalReceivedAmount { get; set; }
        public decimal TotalRejectedAmount { get; set; }
        public int TotalRejectedCount { get; set; }
        public DateTime TransmissionDateTime { get; set; }
        public virtual BatchFile BatchFile { get; set; }

        public override int Key
        {
            get { return LoadReportId; }
        }
    }
}