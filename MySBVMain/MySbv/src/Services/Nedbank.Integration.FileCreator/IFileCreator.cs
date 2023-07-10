using Domain.Data.Nedbank.Model;

namespace Nedbank.Integration.FileCreator
{
    public interface IFileCreator
    {
        /// <summary>
        /// Initializes the batch file properties.
        /// 01 – Transaction instructions.
        /// 02 – Disallow instructions.
        /// </summary>
        /// <param name="fileType"></param>
        NedbankBatchFile BatchInitialiser(string fileType);


        /// <summary>
        /// Generate a batch file by passing in initialized batch.
        /// </summary>
        /// <param name="batchFile"></param>
        void GenerateBatchFile(ref NedbankBatchFile batchFile);
    }
}