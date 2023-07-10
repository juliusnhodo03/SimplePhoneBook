using System.IO;

namespace Nedbank.Integration.FileUtilities
{
    public interface IFileUtility
    {
        /// <summary>
        /// Convert MemoryStream to string[] array
        /// </summary>
        /// <param name="file"></param>
        string[] ConvertToArray(MemoryStream file);

        /// <summary>
        ///     Gets a Filename start characters.
        ///     Extensions are not included.
        /// </summary>
        /// <param name="fileType"></param>
        string GetFilePrefix(FileType fileType);

        /// <summary>
        /// gets a File name, given a file type.
        /// </summary>
        /// <param name="fileType"></param>
        /// <param name="batchCount"></param>
        string GetFilename(FileType fileType, string batchCount);

        /// <summary>
        ///     Get Environment on which MySBV is connected to Nedbank.
        ///     1.  UAT or
        ///     2.  PRODUCTION
        /// </summary>
        string GetEnvironmentFileStartLetter();

        /// <summary>
        /// gets date by format
        /// </summary>
        /// <param name="format"></param>
        string GetDate(Format format);

        /// <summary>
        /// Get batch number by current date
        /// </summary>
        /// <param name="format"></param>
        string GetBatchNumber(Format format);

        /// <summary>
        ///     Gets the reponse file path from Nedbank
        /// </summary>
        string GetResponseFilePath();
    }
}