namespace Application.Dto.VaultPayment
{
    public class PaymentResponseDto
    {
        public PaymentResponseDto()
        {
            ErrorEncountered = true;
        }

        public decimal AvailableFunds { get; set; }
        public int VaultPartialPaymentId { get; set; }
        public string ResponseMessage { get; set; }
        public bool ErrorEncountered { get; set; } 
    }
}