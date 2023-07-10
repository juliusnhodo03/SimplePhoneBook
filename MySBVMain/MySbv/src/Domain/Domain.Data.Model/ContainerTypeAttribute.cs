using Domain.Data.Core;

namespace Domain.Data.Model
{
    public class ContainerTypeAttribute : EntityBase
    {
        #region Mapped

        public int ContainerTypeAttributeId { get; set; }
        public int ContainerTypeId { get; set; }
        public string Attribute1 { get; set; }
        public int Attribute1MinLength { get; set; }
        public int Attribute1MaxLength { get; set; }
        public string Attribute2 { get; set; }
        public int? Attribute2MaxLength { get; set; }
        public int? Attribute2MinLength { get; set; }
        public string Attribute3 { get; set; }
        public int? Attribute3MinLength { get; set; }
        public int? Attribute3MaxLength { get; set; }
        public string Attribute4 { get; set; }
        public int? Attribute4MinLength { get; set; }
        public int? Attribute4MaxLength { get; set; }
        public string Attribute5 { get; set; }
        public int? Attribute5MaxLength { get; set; }
        public int? Attribute5MinLength { get; set; }
        public virtual ContainerType ContainerType { get; set; }

        #endregion

        #region Not Mapped

        public override int Key
        {
            get { return ContainerTypeAttributeId; }
        }

        #endregion
    }
}