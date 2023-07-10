using System.Collections.Generic;
using Utility.Core;

namespace Application.Modules.XmlManipulation
{
	public interface IXmlFile
	{
		List<XmlComparisonResult> Compare(string dbXml, string editedXml);
	}
}