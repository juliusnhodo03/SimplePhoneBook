using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using Domain.Serializer;
using Utility.Core;

namespace Application.Modules.XmlManipulation
{
	public class XmlFile : IXmlFile
	{
		#region Fields

		private readonly ISerializer _serializer;

		#endregion

		#region Constructor

		/// <summary>
		///     Class Connstructor
		/// </summary>
		/// <param name="serializer"></param>
		public XmlFile(ISerializer serializer)
		{
			_serializer = serializer;
		}

		#endregion

		#region XML Manipulation

		/// <summary>
		///     Compares 2 Xml Files and get the differences
		///     The files must share the same structure or (XSD)
		/// </summary>
		/// <param name="dbXml"></param>
		/// <param name="editedXml"></param>
		/// <returns></returns>
		public List<XmlComparisonResult> Compare(string dbXml, string editedXml)
		{
			// create Xml files
			XDocument baseXml = XDocument.Parse(dbXml);
			XDocument comparedWithXml = XDocument.Parse(editedXml);
			XElement root = baseXml.Root;

			// get differences
			var elements = from dbNode in baseXml.Element(root.Name).Descendants()
				join xmlNode in comparedWithXml.Element(root.Name).Descendants()
				on dbNode.Name.LocalName equals xmlNode.Name.LocalName
				select new
				{
					dbNode,
					xmlNode
				};

			var xmlComparisonResults = new List<XmlComparisonResult>();

			foreach (var element in elements)
			{
				// convert values to upper case
				element.dbNode.Value = element.dbNode.Value.Trim().ToUpper();
				element.xmlNode.Value = element.xmlNode.Value.Trim().ToUpper();

				// run comparison
				XmlComparisonResult node = GetComaprison(element.dbNode, element.xmlNode);

				// check if node is not null
				if (node == null) continue;

				// add a change
				xmlComparisonResults.Add(node);
			}

			// no duplicates
			return xmlComparisonResults.Distinct().ToList();
		}


		/// <summary>
		///     Compares 2 related nodes
		///     If differences are found returns the result otherwise null
		/// </summary>
		/// <param name="dbNode"></param>
		/// <param name="xmlNode"></param>
		/// <returns></returns>
		private XmlComparisonResult GetComaprison(XElement dbNode, XElement xmlNode)
		{
			// check if nodes are equal in value
			bool isEqual = XNode.DeepEquals(dbNode, xmlNode);

			// confirm no null references
			if (isEqual) return null;

			// create comparison result
			var comparisonResult = new XmlComparisonResult
			{
				NodeName = dbNode.Name.LocalName,
				OldValue = dbNode.Value,
				NewValue = xmlNode.Value
			};

			// response to requester
			return comparisonResult;
		}

		#endregion
	}
}