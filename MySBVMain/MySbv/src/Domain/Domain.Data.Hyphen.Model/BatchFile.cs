using System;
using System.Collections.ObjectModel;
using Domain.Data.Core;

namespace Domain.Data.Hyphen.Model
{
    public class BatchFile : EntityBase
    {
        public BatchFile()
        {
            LoadReports = new Collection<LoadReport>();
            TransactionDetailRecords = new Collection<TransactionDetailRecord>();
            TransactionDetailRecordResponses = new Collection<TransactionDetailRecordResponse>();
        }

        public int BatchFileId { get; set; }
        public int BatchCount { get; set; }
        public DateTime DateTimeCreated { get; set; }
        public int HeaderRecordId { get; set; }
        public bool IsSent { get; set; }
        public int TrailerRecordId { get; set; }
        public string FileName { get; set; }
        public virtual HeaderRecord HeaderRecord { get; set; }
        public virtual TrailerRecord TrailerRecord { get; set; }
        public virtual Collection<LoadReport> LoadReports { get; set; }
        public virtual Collection<TransactionDetailRecord> TransactionDetailRecords { get; set; }
        public virtual Collection<TransactionDetailRecordResponse> TransactionDetailRecordResponses { get; set; }

        public override int Key
        {
            get { return BatchFileId; }
        }
    }
}