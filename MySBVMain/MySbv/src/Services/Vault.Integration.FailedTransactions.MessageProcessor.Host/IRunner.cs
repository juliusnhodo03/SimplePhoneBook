namespace Vault.Integration.FailedTransactions.MessageProcessor.Host
{
	public interface IRunner
	{
		/// <summary>
		/// Pools the GPT and the CASH CONNECT Queues for
		/// messages and process them
		/// </summary>
		void Run();
	}
}