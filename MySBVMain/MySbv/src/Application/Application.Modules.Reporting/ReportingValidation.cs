using Application.Modules.Common;

namespace Application.Modules.Reporting
{
    public class ReportingValidation : IReportingValidation
    {
        #region Fields

        private readonly ILookup _lookup;

        #endregion

        #region Constructor

		public ReportingValidation(ILookup lookup)
		{
		    _lookup = lookup;
		}

        #endregion

        #region IReport Validation

        public string GetReportPath(int reportId)
		{
			return _lookup.GetReportById(reportId).Path;
		}

		public string GetReportName(int reportId)
		{
            return _lookup.GetReportById(reportId).Name;
		}

        #endregion
        
    }
}