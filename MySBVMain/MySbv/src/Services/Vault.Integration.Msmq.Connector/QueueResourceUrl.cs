namespace Vault.Integration.Msmq.Connector
{
    /// <summary>
    /// 
    /// </summary>
	internal class QueueResourceUrl
	{
        /// <summary>
		/// The Root for Private Message Queues
        /// </summary>
		private const string BASE_URL = @".\Private$\";

		/// <summary>
		/// GPT CashDeposits Message Queue
		/// </summary>
        public const string GPT_REQUEST = BASE_URL + @"Gpt.devices.deposits";

        /// <summary>
        /// Cash Connect CashDeposits Message Queue
        /// </summary>
        public const string CASH_CONNECT_REQUEST = BASE_URL + @"CashConnect.devices.deposits";

        /// <summary>
        /// Greystone CashDeposits Message Queue
        /// </summary>
        public const string GREYSTONE_REQUEST = BASE_URL + @"Greystone.devices.deposits"; 

		/// <summary>
		/// Cash Connect Payments Message Queue
		/// </summary>
		public const string RESPONSE = BASE_URL + @"All.devices.response.messages";

		/// <summary>
		/// Confirmation Message Queue 
		/// </summary>
		public static string CONFIRMATION = BASE_URL + @"Confirmation.messages.all.devices";

		/// <summary>
		/// Confirmation Message Queue
		/// </summary>
		public static string FAILED_REQUEST = BASE_URL + @"Failed.requests.messages.all.devices";
	}
}
