using System;
using System.ComponentModel.DataAnnotations;

namespace Application.Dto.CashOrder
{
    public class CashOrderContainerDropItemDto
    {
        public CashOrderContainerDropItemDto()
        {
            ActiveStatus = true;
            LastChangedDate = DateTime.Now;
        }

        public int CashOrderContainerDropItemId { get; set; }
        public int CashOrderContainerDropId { get; set; }
        public int DenominationId { get; set; }

        public int ValueInCents { get; set; }

        public string DenominationType { get; set; }


        [Display(Name = "Denomination Count")]
        public int Count { get; set; }

        [Display(Name = "Cash Value")]
        public decimal Value { get; set; }

        [Display(Name = "Cash Denomination")]
        public string DenominationName { get; set; }

        public string DenominationValue { get; set; }
        public int? VerifiedCount { get; set; }
        public decimal? VerifiedValue { get; set; }

        public int? PackedCount { get; set; }
        public decimal? PackedValue { get; set; }

        public int CreatedById { get; set; }
        public DateTime CreateDate { get; set; }

        public int LastChangedById { get; set; }
        public DateTime LastChangedDate { get; set; }

        public bool ActiveStatus { get; set; }
        public bool IsEqual { get; set; }
        public DenominationDto Denomination { get; set; }
    }
}