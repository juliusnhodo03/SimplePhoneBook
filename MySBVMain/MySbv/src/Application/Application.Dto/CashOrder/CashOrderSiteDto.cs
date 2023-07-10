namespace Application.Dto.CashOrder
{
    public class CashOrderSiteDto
    {
        public int SiteId { get; set; }

        public int? MerchantId { get; set; }

        public int AddressId { get; set; }

        public int CityId { get; set; }

        public int CitCarrierId { get; set; }

        public int CashCenterId { get; set; }

        public string ContractNumber { get; set; }

        public int SettlementTypeId { get; set; }

        public string CitCode { get; set; }

        public string SiteName { get; set; }

        public string SiteDescription { get; set; }
        public bool IsActive { get; set; }

        public int StatusId { get; set; }

        public int LastChangedById { get; set; }

        public bool IsApproved { get; set; }

        public bool ApprovalRequiredFlag { get; set; }
        public bool ReviewRequiredFlag { get; set; }

    }
}