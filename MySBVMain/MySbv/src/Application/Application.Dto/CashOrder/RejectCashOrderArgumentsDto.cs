namespace Application.Dto.CashOrder
{
    public class RejectCashOrderArgumentsDto
    {
        public int Id { get; set; }
        public string PreviousComments { get; set; }
        public string CurrentComments { get; set; }
    }
}