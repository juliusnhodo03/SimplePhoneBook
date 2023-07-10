using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.IO;
using System.Linq;
using System.Text;
using Domain.Data.Model;
using Domain.Data.Nedbank.Model;
using Domain.Repository;
using Infrastructure.Logging;

namespace Nedbank.Integration.FileUtilities
{
    [Export(typeof (IFileUtility))]
    [PartCreationPolicy(CreationPolicy.NonShared)]
    public class FileUtility : IFileUtility
    {
        #region Fields

        private readonly NedbankClientProfile _clientProfile;

        private readonly IRepository _repository;

        #endregion

        #region Constructor

        [ImportingConstructor]
        public FileUtility(IRepository repository)
        {
            _repository = repository;
            _clientProfile =
                _repository.Query<NedbankClientProfile>(e => e.LookupKey == "CLIENT_PROFILE_NUMBER").FirstOrDefault();
        }

        #endregion

        #region IFileUtility

        /// <summary>
        /// Convert MemoryStream to string[] array
        /// </summary>
        /// <param name="file"></param>
        public string[] ConvertToArray(MemoryStream file) 
        {
            string respons = Encoding.ASCII.GetString(file.ToArray());
            string[] lines = respons.Split(new[] { "\r\n", "\n" }, StringSplitOptions.None);

            // Remove the last line as it contains blank
            IEnumerable<string> finalLines = lines.Where(a => !string.IsNullOrEmpty(a));

            return finalLines.ToArray();
        }


        /// <summary>
        ///     Gets a Filename start characters.
        ///     Extensions are not included.
        /// </summary>
        /// <param name="fileType"></param>
        public string GetFilePrefix(FileType fileType)
        {
            // get Client-Prefix as a file identifier.
            // Such that MySBV picks only MySBV files.
            string client = _clientProfile.Prefix;

            // Get Environment on which MySBV is connected to Nedbank
            string server = GetEnvironmentFileStartLetter();

            // file name
            return string.Format("{0}NGG00.CDPACK.{1}{2}", server, client, Convert.ToChar(fileType));
        }


        /// <summary>
        ///     gets a File name, given a file type.
        /// </summary>
        /// <param name="fileType"></param>
        /// <param name="batchCount"></param>
        public string GetFilename(FileType fileType, string batchCount)
        {
            // Create file prefix.
            string fileStartName = GetFilePrefix(fileType);

            // File name
            return string.Format("{0}{1}.D0.SQ320", fileStartName, batchCount);
        }


        /// <summary>
        ///     Get Environment on which MySBV is connected to Nedbank.
        ///     1.  UAT or
        ///     2.  PRODUCTION
        /// </summary>
        public string GetEnvironmentFileStartLetter()
        {
            string exception =
                string.Format(@"Exception On Method {0}: Cannot get Environment on which Nedbank is Running!",
                    "[GET ENVIRONMENT PREFIX => FILE-UTILITY]");

            try
            {
                SystemConfiguration server =
                    _repository.Query<SystemConfiguration>(e => e.LookUpKey == "NEDBANK_SERVER_ENVIRONMENT")
                        .FirstOrDefault();

                string prefix;

                switch (server.Value)
                {
                    case "UAT":
                        prefix = "Q";
                        break;
                    case "PROD":
                        prefix = "P";
                        break;
                    default:
                        this.Log().Fatal("Cannot find Configuration Lookupkey = [NEDBANK_SERVER_ENVIRONMENT] => [GetEnvironmentFileStartLetter]");
                        throw new ArgumentNullException(exception);
                }
                return prefix;
            }
            catch (Exception e)
            {
                this.Log().Fatal(exception, e);
                throw;
            }
        }


        /// <summary>
        ///     gets date by Format.
        /// </summary>
        /// <param name="format"></param>
        public string GetDate(Format format)
        {
            DateTime date = DateTime.Now;
            var dateBuilder = new StringBuilder();

            switch (format)
            {
                case Format.YYYYMMDD:
                    dateBuilder.Append(date.Year.ToString().PadLeft(4, '0'))
                        .Append(date.Month.ToString().PadLeft(2, '0'))
                        .Append(date.Day.ToString().PadLeft(2, '0'));
                    break;
                case Format.YYMMDD:
                    dateBuilder.Append(date.Year.ToString().Substring(2, 2))
                        .Append(date.Month.ToString().PadLeft(2, '0'))
                        .Append(date.Day.ToString().PadLeft(2, '0'));
                    break;
            }
            return dateBuilder.ToString();
        }


        /// <summary>
        ///     Get batch number by current date
        /// </summary>
        /// <param name="format"></param>
        public string GetBatchNumber(Format format)
        {
            string batchDate = GetDate(format);

            // Get previously created batches on this current Date
            // and get the maximum "Batch Count"
            List<NedbankBatchFile> batches = _repository.All<NedbankBatchFile>()
                .Where(e => e.BatchDate.Equals(batchDate)).ToList();

            List<int> batchCounts = batches.Select(batch => Convert.ToInt32(batch.BatchCount)).ToList();

            int batchCount = batchCounts.OrderByDescending(e => e).FirstOrDefault();

            if (batchCounts.Count > 0)
            {
                batchCount++;
            }
            else
            {
                batchCount = 1;
            }

            var batchNumber = new StringBuilder();
            batchNumber.Append("NB-");
            batchNumber.Append(batchDate);
            batchNumber.Append("-");
            batchNumber.Append(batchCount.ToString().PadLeft(3, '0'));

            return batchNumber.ToString();
        }
        


        /// <summary>
        ///     Gets the reponse file path from Nedbank
        /// </summary>
        public string GetResponseFilePath()
        {
            SystemConfiguration path = _repository.Query<SystemConfiguration>(a => a.LookUpKey == "NEDBANK_PICKUP_PATH")
                .FirstOrDefault();

            if (path != null && !string.IsNullOrEmpty(path.Value))
            {
                return path.Value;
            }
            throw new ArgumentNullException(string.Format("LookUpKey Not Found [{0}]", "NEDBANK_PICKUP_PATH"));
        }
        #endregion
    }
}