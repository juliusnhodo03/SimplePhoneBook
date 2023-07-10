namespace Nedbank.Integration.Response.Reader
{
    public interface IResponseReader
    {
        /// <summary>
        ///     Get Response Files from Nedbank.
        /// </summary>
        /// <param name="path"></param>
        void ReadFiles(string path);
    }
}