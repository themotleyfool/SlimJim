using System.IO;
using NUnit.Framework;
using SlimJim.Infrastructure;
using SlimJim.Model;

namespace SlimJim.Test.Infrastructure
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
}