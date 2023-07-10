using System;

namespace Domain.Logging
{
    public interface ILogManager
    {
        ILogger GetLogger(Type type);
    }
}