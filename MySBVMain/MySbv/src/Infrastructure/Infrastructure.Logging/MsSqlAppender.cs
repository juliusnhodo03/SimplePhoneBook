using System.Configuration;
using log4net.Appender;

namespace Infrastructure.Logging
{
    public class MsSqlAppender : AdoNetAppender
    {
        public new string ConnectionString
        {
            get { return base.ConnectionString; }
            set
            {
                base.ConnectionString = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
            }
        }
    }
}