namespace Utility.Core
{
    public class MethodResult<T>
    {
        #region Fields

        private readonly MethodStatus _appStatus;
        private readonly ErrorDetails _errorDetails;
        private readonly string _message;
        private readonly T _resultEntity;

        #endregion

        #region Constructor

        public MethodResult(MethodStatus appStatus, T resultEntity = default(T), string message = "",
            object tag = null,
            ErrorDetails errorDetails = null)
        {
            _resultEntity = resultEntity;
            _message = message;
            _appStatus = appStatus;
            Tag = tag;
            _errorDetails = errorDetails;
        }

        #endregion

        #region Properties

        public string Message
        {
            get { return _message; }
        }

        public T EntityResult
        {
            get { return _resultEntity; }
        }

        public MethodStatus Status
        {
            get { return _appStatus; }
        }

        public object Tag { get; set; }

        public ErrorDetails ErrorDetails
        {
            get { return _errorDetails; }
        }

        #endregion
    }
}