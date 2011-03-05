using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Linq;
using NUnit.Framework;

namespace SlimJim.Test
{
	[TestFixture]
	public class CsProjReaderTests
	{
		private FileInfo file;

		[Test]
		public void ReadsFileContentsIntoObject()
		{
			CsProj project = GetProject("Simple");

			Assert.That(project.Guid, Is.EqualTo("{4A37C916-5AA3-4C12-B7A8-E5F878A5CDBA}"));
			Assert.That(project.AssemblyName, Is.EqualTo("MyProject"));
			Assert.That(project.Path, Is.EqualTo(file.FullName));
			Assert.That(project.ReferencedAssemblyNames, Is.EqualTo(new[]
			                                                       	{
																							"System",
																							"System.Core",
																							"System.Xml.Linq",
																							"System.Data.DataSetExtensions",
																							"Microsoft.CSharp",
																							"System.Data",
																							"System.Xml"
			                                                       	}));
			Assert.That(project.ReferencedProjectGuids, Is.EqualTo(new[]
			                                                      	{
																							"{99036BB6-4F97-4FCC-AF6C-0345A5089099}",
																							"{69036BB3-4F97-4F9C-AF2C-0349A5049060}"
			                                                      	}));
		}

		[Test]
		public void TakesOnlyNameOfFullyQualifiedAssemblyName()
		{
			CsProj project = GetProject("FQAssemblyName");

			Assert.That(project.ReferencedAssemblyNames, Contains.Item("NHibernate"));
		}

		private CsProj GetProject(string fileName)
		{
			file = SampleFiles.SampleFileHelper.GetCsProjFile(fileName);
			var reader = new CsProjReader();
			return reader.Read(file);
		}
	}

	public class CsProjReader
	{
		private static readonly XNamespace Ns = "http://schemas.microsoft.com/developer/msbuild/2003";

		public CsProj Read(FileInfo csProjFile)
		{
			var csProj = new CsProj
			             	{
			             		Path = csProjFile.FullName
			             	};

			XElement xml = LoadXml(csProjFile);
			XElement properties = xml.Element(Ns + "PropertyGroup");
			csProj.Guid = properties.Element(Ns + "ProjectGuid").Value;
			csProj.AssemblyName = properties.Element(Ns + "AssemblyName").Value;	
			csProj.ReferencedAssemblyNames = ReadReferencedAssemblyNames(xml);
			csProj.ReferencedProjectGuids = ReadReferencedProjectGuids(xml);

			return csProj;
		}

		private XElement LoadXml(FileInfo csProjFile)
		{
			XElement xml;
			using (StreamReader reader = csProjFile.OpenText())
			{
				xml = XElement.Load(reader);
			}
			return xml;
		}

		private List<string> ReadReferencedAssemblyNames(XElement xml)
		{
			List<string> rawAssemblyNames = (from r in xml.DescendantsAndSelf(Ns + "Reference")
			                                   select r.Attribute("Include").Value).ToList();
			List<string> unQualifiedAssemblyNames = rawAssemblyNames.ConvertAll(n => UnQualify(n));
			return unQualifiedAssemblyNames;
		}

		private string UnQualify(string name)
		{
			if (!name.Contains(",")) return name;

			return name.Substring(0, name.IndexOf(","));
		}

		private List<string> ReadReferencedProjectGuids(XElement xml)
		{
			return (from pr in xml.DescendantsAndSelf(Ns + "ProjectReference")
			        select pr.Element(Ns + "Project").Value).ToList();
		}
	}
}