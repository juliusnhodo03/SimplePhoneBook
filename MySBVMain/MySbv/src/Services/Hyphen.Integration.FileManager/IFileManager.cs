using System.Collections.Generic;
using System.IO;

namespace Hyphen.Integration.FileManager
{
    public interface IFileManager
    {
        List<MemoryStream> ReadFile(string path);
    }
}