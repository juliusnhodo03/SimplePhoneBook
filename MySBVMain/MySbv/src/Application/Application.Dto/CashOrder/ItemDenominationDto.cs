namespace Application.Dto.CashOrder
{
    public class ItemDenominationDto
    {
        public int DenominationId { get; set; }
        public string DenominationName { get; set; }
        public string DenominationType { get; set; }

        public int ValueInCents { get; set; }

        public int Count { get; set; }

        public decimal Value { get; set; }

        public int CashOrderContainerDropItemId { get; set; }
    }
}