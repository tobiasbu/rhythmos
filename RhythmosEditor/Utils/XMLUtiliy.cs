using System.Xml;

namespace RhythmosEditor.Utils
{
	internal static class XMLUtility {
			
		static public XmlNode CreateNodeByName(XmlDocument xmlDoc, string name, string innerText)
		{
			XmlNode node = xmlDoc.CreateElement(name);
			node.InnerText = innerText;
			return node;
		}
	}
}
