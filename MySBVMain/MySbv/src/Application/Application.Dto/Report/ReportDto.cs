using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Dto.Report
{
	public class ReportDto
	{
		public int ReportId { get; set; }

		[Display(Name = "Report Name")]
		public string Name { get; set; }
		public string Description { get; set; }
		public string LookUpKey { get; set; }
		public string Path { get; set; }
		public int? UserTypeId { get; set; }
	}
}
