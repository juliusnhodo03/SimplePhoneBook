using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Domain.Data.Core;

namespace Domain.Data.Nedbank.Model
{
    [Table("ServiceTypes", Schema = "Nedbank")]
    public class NedbankServiceType : EntityBase
    {
        public NedbankServiceType()
        {
            NedbankFileItems = new HashSet<NedbankFileItem>();
        }

        [Key]
        [StringLength(2)]
        public string ServiceType { get; set; }

        [Required]
        [StringLength(50)]
        public string Name { get; set; }

        [Required]
        [StringLength(17)]
        public string ShortName { get; set; }

        [Required]
        [StringLength(50)]
        public string LookUpKey { get; set; }

        [Required]
        [StringLength(100)]
        public string Description { get; set; }

        public virtual ICollection<NedbankFileItem> NedbankFileItems { get; set; }

        public override int Key
        {
            get { return 0; }
        }
    }
}