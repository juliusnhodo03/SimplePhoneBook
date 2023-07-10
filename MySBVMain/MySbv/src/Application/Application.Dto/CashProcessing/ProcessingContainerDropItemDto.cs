using System;
using System.ComponentModel.DataAnnotations;

namespace Application.Dto.CashProcessing
{
    public class ProcessingContainerDropItemDto
    {
        public int ContainerDropItemId { get; set; }

        public int ContainerDropId { get; set; }

        public int DenominationId { get; set; }
    
        public int ValueInCents { get; set; }
    
        [Display(Name = "Denomination Count")]
        public int? Count { get; set; }

        [Display(Name = "Cash Value")]
        public decimal Value { get; set; }
        
        [Display(Name = "Denomination Count")]
		public int? DiscrepancyCount { get; set; }

        [Display(Name = "Cash Value")]
        public decimal? DiscrepancyValue { get; set; }

        public int? ActualCount { get; set; }

        [Display(Name = "Cash Value")]
        public decimal? ActualValue { get; set; }

        public bool IsNotDeleted { get; set; }

        public int CreatedById { get; set; }

        public int LastChangedById { get; set; }

        public DateTime CreateDate { get; set; }

        public DateTime LastChangedDate { get; set; }

		public string DenominationType { get; set; }
		public string DenominationName { get; set; }

    }
}