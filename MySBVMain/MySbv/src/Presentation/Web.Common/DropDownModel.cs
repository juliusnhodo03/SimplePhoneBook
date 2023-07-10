namespace Web.Common
{
    /// <summary>
    /// Drop Down Model Class
    /// </summary>
	public class DropDownModel
	{
		public int Id { get; set; }
		public string Name { get; set; }
		public string Text { get; set; }
		public string Value { get; set; } 
		public object Tag { get; set; }
	}

    /// <summary>
    /// Report DropDown Model
    /// </summary>
	public class ReportDropDownModel
	{
		public string Id { get; set; }
		public string Name { get; set; }
	}
}