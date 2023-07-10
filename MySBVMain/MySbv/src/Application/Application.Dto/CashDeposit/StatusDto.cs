using System.Collections.ObjectModel;

namespace Application.Dto.CashDeposit
{
    public class StatusDto
    {
        public int StatusId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }

        public virtual Collection<CashDepositDto> CashDeposits { get; set; }
    }
}