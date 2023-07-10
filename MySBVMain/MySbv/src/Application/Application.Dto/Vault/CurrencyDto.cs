
namespace Application.Dto.Vault
{
	public class CurrencyDto
	{
		public int CurrencyId { get; set; }
		public int CountryId { get; set; }
		public string Code { get; set; }
		public string Symbol { get; set; }
		public string Name { get; set; }
		public string Description { get; set; }
		public string LookUpKey { get; set; }
	}
}
