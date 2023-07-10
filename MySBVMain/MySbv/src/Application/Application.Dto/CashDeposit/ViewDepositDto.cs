using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Application.Dto.Site;

namespace Application.Dto.CashDeposit
{
    public class ViewDepositDto
    {
        [ScaffoldColumn(false)]
        public int CashDepositId { get; set; }

        [Display(Name = "Deposit Type")]
        [Range(1, 50, ErrorMessage = "Please select Deposit")]
        public int DepositTypeId { get; set; }

        [Display(Name = "Bank")]
        [Range(1, 50, ErrorMessage = "Please select Bank")]
        public int BankId { get; set; }

        public int InitialCitSerialNumber { get; set; }


        [Display(Name = "Merchant")]
        [Range(1, 200000000000, ErrorMessage = "Please select Merchant")]
        public long MerchantId { get; set; }

        [Display(Name = "Site")]
        [Range(1, 200000000000, ErrorMessage = "Please select Site")]
        public int SiteId { get; set; }

        [Display(Name = "Custom Deposit Reference")]
        public string Narrative { get; set; }

        //[Required(ErrorMessage = "Reference Number is Required")]
        //[Display(Name = "Deposit Reference")]
        //public string Narrative { get; set; }

        [Display(Name = "Total Deposit Amount")]
        [Required(ErrorMessage = "Total Deposit Amount is Required")]
        [Range(0.1, 5000000000000, ErrorMessage = "Enter correct Currency value")]
        public decimal TotalDepositAmount { get; set; }

        [Display(Name = "Total Deposit Amount (Actual)")]
        public decimal TotalDepositAmountActual { get; set; }

        [Display(Name = "CIT Code")]
        [Required(ErrorMessage = "CIT Code is Required")]
        public string CitCode { get; set; }

        public string CapturingSource { get; set; }
        public string Country { get; set; }

        [Display(Name = "Contract Number")]
        [Required(ErrorMessage = "Contract Number is Required")]
        public string ContractNumber { get; set; }

        [Display(Name = "Capture Date")]
        public string CaptureDateString { get; set; }

        [Display(Name = "Deposit Processed Date and Time")]
        public string DateProcessed { get; set; }

        [Display(Name = "Transaction Settlement Date and Time")]
        public string TransactionSettlementDate { get; set; }

        public string TransactionReferenceNumber { get; set; }

        [Display(Name = "Site Name")]
        public string Name { get; set; }

        [Display(Name = "Account Number")]
        public int AccountId { get; set; }

        public decimal NotesAmountLimit { get; set; }
        public decimal CoinsAmountLimit { get; set; }

        public int? SupervisorId { get; set; }
        public int? ProcessedById { get; set; }

        [ScaffoldColumn(false)]
        public int CreatedByUserId { get; set; }

        [ScaffoldColumn(false)]
        public int LastChangedById { get; set; }

        [ScaffoldColumn(false)]
        public int CapturingSourceId { get; set; }

        [ScaffoldColumn(false)]
        public int StatusId { get; set; }

        [ScaffoldColumn(false)]
        public DateTime LastChangedDate { get; set; }

        [ScaffoldColumn(false)]
        public bool ActiveStatus { get; set; }

        public int NumberOfCopiesPrinted { get; set; }

        public bool IsCustomReferenceVisible { get; set; }
        public bool HasTriedToEditSubmittedDeposit { get; set; }


        [Display(Name = "Transaction Status")]
        public string TransactionName { get; set; }

        public string DepositReference { get; set; }

        [Display(Name = "Capture Date")]
        public DateTime? CaptureDate { get; set; }

        public string ActionState { get; set; }

        public DateTime DepositStartDate { get; set; }
        public DateTime? DepositEndDate { get; set; }
        public bool? IsSubmitted { get; set; }
        public bool? IsDepositReferenceEditable { get; set; }

        [ScaffoldColumn(false)]
        public bool? DiscrepeancyIndicator { get; set; }

        public bool IsSbvTellerVisible { get; set; }

        public virtual StatusDto Status { get; set; }
        public virtual SiteDto Site { get; set; }
        public DepositTypeDto DepositType { get; set; }
        public virtual List<ContainerDto> Containers { get; set; }
    }
}