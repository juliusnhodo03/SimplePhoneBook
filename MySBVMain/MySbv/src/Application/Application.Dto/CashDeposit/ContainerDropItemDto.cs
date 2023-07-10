using System;

namespace Application.Dto.CashDeposit
{
    public class ContainerDropItemDto
    {
        public int ContainerDropItemId { get; set; }
        public int ContainerDropId { get; set; }
        public int DenominationId { get; set; }
        public int LastChangedById { get; set; }

        public int ValueInCents { get; set; }

        public string DenominationType { get; set; }
		public string DenominationName { get; set; }
        public bool ActiveStatus { get; set; }
        public DateTime LastChangedDate { get; set; }
		
        //[Display(Name = "Denomination Count")]
        public int Count { get; set; }

        //[Display(Name = "Cash Value")]
        public decimal Value { get; set; }

        //[Display(Name = "Cash Denomination")]
        public string Name { get; set; }

        public string DenominationValue { get; set; }

        //[Display(Name = "Denomination Count")]
        public int? DiscrepancyCount { get; set; }

        //[Display(Name = "Cash Value")]
        public decimal? DiscrepancyValue { get; set; }

        public int CreatedById { get; set; }

        public DateTime CreateDate { get; set; }
		public bool IsEqual { get; set; }
		public bool IsNotDeleted { get; set; }
    }
}