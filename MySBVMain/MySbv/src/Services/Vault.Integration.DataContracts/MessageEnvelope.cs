
namespace Vault.Integration.DataContracts
{
    public class MessageEnvelope<T> : IMessageLabel
    {
        public string Label { get; set; }
        public string MessageId { get; set; }
        public T MessageObject { get; set; }
    }
}
