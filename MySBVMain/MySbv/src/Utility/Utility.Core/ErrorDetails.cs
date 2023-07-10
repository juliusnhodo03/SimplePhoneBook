using System;

namespace Utility.Core
{
    public class ErrorDetails
    {
        public bool HasError { get; set; }
        public string ErrorMessage { get; set; }
        public string StackTrace { get; set; }
        public string MethodName { get; set; }
        public DateTime ExecuteDateAndTime { get; set; }
        public Type ExceptionType { get; set; }
    }
}