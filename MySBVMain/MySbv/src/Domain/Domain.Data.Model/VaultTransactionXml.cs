using System;
using Domain.Data.Core;

namespace Domain.Data.Model
{
    public class VaultTransactionXml : EntityBase
    {
        #region Mapped

        public int VaultTransactionXmlId { get; set; }
        public int StatusId { get; set; }
        public string BagSerialNumber { get; set; }
        public DateTime TransactionDate { get; set; }
        public string TransactionTypeCode { get; set; }
        public string ErrorMessages { get; set; }
        public string XmlMessage { get; set; }
        public string XmlAwaitingApproval { get; set; }
        public int? ApprovedById { get; set; }
        public DateTime? ApprovedDate { get; set; }
        public virtual Status Status { get; set; }

        #endregion

        #region Not Mapped

        public override int Key
        {
            get { return VaultTransactionXmlId; }
        }

        #endregion
    }
}