using Domain.Logging;

namespace Infrastructure.Logging
{
    public static class GenericLoggingExtensions
    {
        public static ILogger Log<T>(this T thing) where T : class 
        {
            ILogger log = LogManager.GetLogger<T>();
            return log;
        }
    }
}