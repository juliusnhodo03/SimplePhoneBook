namespace Application.Dto.CashOrder
{
    public class DenominationDto
    {
        public int DenominationId { get; set; }
        public int CountryId { get; set; }
        public string DenominationName { get; set; }
        public string DenominationTypeDescription { get; set; }
        public int DenominationTypeId { get; set; }
        public int ValueInCents { get; set; }
    }
}