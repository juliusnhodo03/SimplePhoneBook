namespace Vault.Integration.DataContracts
{
    public interface IMessageLabel
    {
        string Label { get; set; }

        string MessageId { get; set; }
    }
}