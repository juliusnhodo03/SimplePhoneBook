namespace Domain.Data.Core
{
    public interface IIdentity
    {
        string Name { get; set; }
        string Description { get; set; }
        string LookUpKey { get; set; }
    }
}