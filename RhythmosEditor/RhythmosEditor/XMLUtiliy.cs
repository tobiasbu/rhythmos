using UnityEngine;
using System.Xml;

namespace RhythmosEditor
{
	internal static class XMLUtility {
		
		static public string GetPathByPlatform() {
			#if UNITY_EDITOR
			return Application.dataPath;
			#elif UNITY_ANDROID
			return Application.persistentDataPath;
			#elif UNITY_IPHONE
			return GetiPhoneDocumentsPath();
			#else
			return Application.dataPath;
			#endif
		}
		
		static public XmlNode CreateNodeByName( ref XmlDocument xmlDoc,string name, string innerText)
		{
			XmlNode node = xmlDoc.CreateElement(name);
			node.InnerText = innerText;
			return node;
		}
	}
}
