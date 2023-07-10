using System.Collections.Generic;
using System.IO;
using Domain.Data.Hyphen.Model;

namespace Hyphen.Integration.Facs
{
    public interface IFacs
    {
        void CreateBatchFile(ref BatchFile file);
        void ReadResponseFile(List<MemoryStream> files);
    }
}