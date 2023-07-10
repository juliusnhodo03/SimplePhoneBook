namespace Nedbank.Integration.FileUtilities
{
    public enum FileType
    {
        TRANSACTION = 'I',
        DISALLOW = 'I',
        NACK = 'N',
        ACK = 'A',
        SECOND_ACK = 'H',
        DUPLICATE = 'D',
        UNPAIDS = 'O',
        NAEDOS = 'R'
    }
}