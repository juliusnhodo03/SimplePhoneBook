using System;
using System.Collections.Generic;
using Application.Dto.Site;

namespace Application.Dto.CashDeposit
{
    public class CashDepositDto
	{
        //[ScaffoldColumn(false)]
		public int CashDepositId { get; set; }

        //[Required(ErrorMessage = "Deposit Type is required")]
        //[Display(Name = "Deposit Type")]
		public int DepositTypeId { get; set; }

        //[Display(Name = "Merchant")]
        //[Required(ErrorMessage = "Merchant is required")]
		public int MerchantId { get; set; }

        //[Display(Name = "Site")]
        //[Required(ErrorMessage = "Site is required")]
		public int SiteId { get; set; }

        //[Display(Name = "Account Number")]
		public int AccountId { get; set; }

        //[Display(Name = "Product Type")]
		public int ProductTypeId { get; set; }

        //[Display(Name = "Narrative")]
		public string Narrative { get; set; }

        //[Display(Name = "Bank")]
        //[Range(1, 50, ErrorMessage = "Please select Bank")]
		public int BankId { get; set; }

        //[Required(ErrorMessage = "Deposit Reference is required")]
        //[Display(Name = "Deposit Reference")]
		public string ReferenceNumber { get; set; }

		public string InitialCitSerialNumber { get; set; }
        public bool IsSbvTellerVisible { get; set; }
        public string Country { get; set; }
	
        //[Display(Name = "Transaction Number")]
		public string TransactionReference { get; set; }

        //[Required]
        //[Display(Name = "Contract Number")]
		public string ContractNumber { get; set; }

		public string ReturnMessage { get; set; }

        //[Required]
        //[Display(Name = "Merchant Name")]
		public string MerchantName { get; set; }

        //[Required]
        //[Display(Name = "Total Deposited"), DataType(DataType.Currency)]
		public decimal DepositedAmount { get; set; }

        //[Display(Name = "Actual Deposit Amount"), DataType(DataType.Currency)]
		public decimal? ActualAmount { get; set; }
		
        //[Display(Name = "Device Number")]
		public int? DeviceId { get; set; }

        //[Display(Name = "Device Name")]
		public string DeviceName { get; set; }

        //[Required]
        //[Display(Name = "Site Name")]
		public string SiteName { get; set; }
		public string DepositTypeName { get; set; }

		public string SealSerialNumber { get; set; }

        //[Display(Name = "Cit Code")]
		public string CitCode { get; set; }
		public string ActionName { get; set; } 

		public int? ErrorCodeId { get; set; }
		public DateTime? CitDateTime { get; set; }
		public DateTime? ProcessedDateTime { get; set; }
		public bool? IsProcessed { get; set; }
		public int? ProcessedById { get; set; }
		public DateTime? SettledDateTime { get; set; }
		public bool? IsSettled { get; set; }
		public DateTime? SendDateTime { get; set; }
		public DateTime? SubmitDateTime { get; set; }
		public bool? IsSubmitted { get; set; }
		public decimal? DiscrepancyAmount { get; set; }
		public bool? HasDescripancy { get; set; }

        public int? SupervisorId { get; set; }

        //[ScaffoldColumn(false)]
        public int CreatedByUserId { get; set; }

        //[ScaffoldColumn(false)]
        public int LastChangedById { get; set; }

        //[ScaffoldColumn(false)]
        public int StatusId { get; set; }

        //[ScaffoldColumn(false)]
        public DateTime LastChangedDate { get; set; }

        //[ScaffoldColumn(false)]
        public bool ActiveStatus { get; set; }

        //[Display(Name = "Product Type Name")]
        public string ProductTypeName { get; set; }

        //[Display(Name = "Transaction Status")]
        public string TransactionStatusName { get; set; } 

        //[Display(Name = "Capture Date")]
        public DateTime? CreateDate { get; set; }

        //[Display(Name = "Capture Date")]
		public string CaptureDateString { get; set; }
		public string iTransUserName { get; set; }

        public string ActionState { get; set; }
		public int CreatedById { get; set; } 
		
        //[ScaffoldColumn(false)]
		public bool? DiscrepeancyIndicator { get; set; }

		public bool IsSbvTeller { get; set; }

        public bool IsHeadOfficeUser { get; set; } 
        public bool IsMerchantUser { get; set; } 

        public virtual DepositTypeDto DepositType { get; set; }

        public virtual SiteDto Site { get; set; }
        public virtual StatusDto Status { get; set; }

        public virtual List<ContainerDto> Containers { get; set; }
    }
}