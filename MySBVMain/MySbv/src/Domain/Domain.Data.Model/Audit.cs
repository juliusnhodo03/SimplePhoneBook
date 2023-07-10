using System.ComponentModel.DataAnnotations.Schema;
using Domain.Data.Core;

namespace Domain.Data.Model
{
    [Table("AuditLog")]
    public class Audit : EntityBase
    {
        #region Mapped

        public int AuditId { get; set; }
        public string AuditState { get; set; }
        public string TableName { get; set; }
        public int RecordId { get; set; }
        public string ColumnName { get; set; }
        public string OriginalValue { get; set; }
        public string NewValue { get; set; }

        #endregion

        #region Not Mapped

        public override int Key
        {
            get { return AuditId; }
        }

        #endregion
        
    }
}