using Application.Dto.CashDeposit;

namespace Application.Dto.CashProcessing
{
    public class CashProccessingModel
    {
        public bool IsVaultDeposit { get; set; }
        public int? UserTypeId { get; set; }
        public CashProcessingDto CashProcessing { get; set; }
        public VaultContainerDto VaultProcessing { get; set; }  
    }
}