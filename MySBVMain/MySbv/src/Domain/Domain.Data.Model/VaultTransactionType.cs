using Domain.Data.Core;

namespace Domain.Data.Model
{
    public class VaultTransactionType : EntityBase, IIdentity
    {
        public int VaultTransactionTypeId { get; set; }
        public int Code { get; set; }
        public string Name { get; set; }
        public string LookUpKey { get; set; }
        public string Description { get; set; }
        public override int Key
        {
            get { return VaultTransactionTypeId; }
        }
    }
}