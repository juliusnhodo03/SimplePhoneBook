namespace Application.Modules.FailedVaultRequest
{
	public class UpdateMessages
	{
		public static string Success = @"Vault Request updated successfully.";
		public static string Approved = @"Vault Request approved successfully.";
		public static string Failed = @"Failed to save Vault Request.";
		public static string Error = @"An Error has occured while updating Vault Request! ";
		public static string NoChanges = @"You did not make any changes! ";
		public static string FailedApproval = @"Failed to approve Vault Request!";
		public static string Declined = @"Vault Request approval has been declined!";
		public static string CantReleaseCitWithTransactions = @"Please update other Transactions attached to this CIT message first!";
	}
}