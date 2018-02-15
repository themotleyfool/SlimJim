
using System.Xml;

using log4net;


namespace SlimJim
{
    using Model;


    public abstract class CsProjConverter
	{
		protected const string MSBuildXmlNamespace = "http://schemas.microsoft.com/developer/msbuild/2003";
		protected readonly ILog log;
		protected XmlNamespaceManager nsMgr;

		protected CsProjConverter()
		{
			log = LogManager.GetLogger(GetType());
		}

		protected static XmlElement CreateElementWithInnerText(XmlDocument doc, string elementName, string text)
		{
			var e = doc.CreateElement(elementName, MSBuildXmlNamespace);
			e.InnerText = text;
			return e;
		}

		protected XmlDocument LoadProject(CsProj project)
		{
			var doc = new XmlDocument();
			doc.Load(project.Path);
			nsMgr = new XmlNamespaceManager(doc.NameTable);
			nsMgr.AddNamespace("msb", MSBuildXmlNamespace);
			return doc;
		}
	}
}