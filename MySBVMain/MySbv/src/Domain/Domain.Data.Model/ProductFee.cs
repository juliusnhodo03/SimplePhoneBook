using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Domain.Data.Core;

namespace Domain.Data.Model
{
    public class ProductFee : EntityBase
    {
        #region Mapped

        [Column(Order = 0), Key, ForeignKey("Product")]
        public int ProductId { get; set; }

        [Column(Order = 1), Key, ForeignKey("Fee")]
        public int FeeId { get; set; }
        public bool IsActive { get; set; }
        public virtual Product Product { get; set; }
        public virtual Fee Fee { get; set; }

        #endregion
        
        #region Not Mapped

        public override int Key
        {
            get { return ProductId; }
        }

        #endregion
    }
}