
namespace Hyphen.Integration.Reconciliation.FileProcessor
{
    public interface IFileManager
    {
        /// <summary>
        ///     Reads Response Files from Nedbank.
        /// </summary>
        /// <param name="path"></param>
        void ReadFiles(string path);
    }
}