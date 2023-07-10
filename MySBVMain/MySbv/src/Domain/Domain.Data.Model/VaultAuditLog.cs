using Domain.Data.Core;

namespace Domain.Data.Model
{
    public class VaultAuditLog : EntityBase
    {
        public int VaultAuditLogId { get; set; }
        public int? VaultTransactionXmlId { get; set; }
        public string OldValue { get; set; }
        public string NewValue { get; set; }
        public string TableName { get; set; }
        public string ColumnName { get; set; }
        public VaultTransactionXml VaultTransactionXml { get; set; }

        public override int Key
        {
            get { return VaultAuditLogId; }
        }
    }
}