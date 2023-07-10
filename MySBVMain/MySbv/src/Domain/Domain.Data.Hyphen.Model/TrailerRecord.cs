using System.Collections.ObjectModel;
using Domain.Data.Core;

namespace Domain.Data.Hyphen.Model
{
    public class TrailerRecord : EntityBase
    {
        public TrailerRecord()
        {
            BatchFiles = new Collection<BatchFile>();
        }

        public int TrailerRecordId { get; set; }
        public string BatchNumber { get; set; }
        public string Checksum { get; set; }
        public string MessageType { get; set; }
        public string NumberOfTransactions { get; set; }
        public string TotalValue { get; set; }
        public virtual Collection<BatchFile> BatchFiles { get; set; }

        public override int Key
        {
            get { return TrailerRecordId; }
        }
    }
}