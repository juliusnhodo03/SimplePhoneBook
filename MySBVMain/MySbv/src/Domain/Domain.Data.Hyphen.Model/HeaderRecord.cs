using System.Collections.ObjectModel;
using Domain.Data.Core;

namespace Domain.Data.Hyphen.Model
{
    public class HeaderRecord : EntityBase
    {
        public HeaderRecord()
        {
            BatchFiles = new Collection<BatchFile>();
        }

        public int HeaderRecordId { get; set; }
        public string BatchNumber { get; set; }
        public string MessageType { get; set; }
        public string TransmissionDate { get; set; }
        public string TransmissionTime { get; set; }
        public string Blank1 { get; set; }
        public virtual Collection<BatchFile> BatchFiles { get; set; }

        public override int Key
        {
            get { return HeaderRecordId; }
        }
    }
}