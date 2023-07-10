using System.Collections.Generic;
using Domain.Data.Model;

namespace Application.Modules.Reporting
{
	public interface IReportingValidation
	{
		string GetReportPath(int reportId);
		string GetReportName(int reportId);
	}
}