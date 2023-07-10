namespace Nedbank.Integration.FileManager
{
    public interface IFileManager
    {
        /// <summary>
        ///     Archive file.
        /// </summary>
        /// <param name="file"></param>
        void ArchiveFile(string file);
    }
}