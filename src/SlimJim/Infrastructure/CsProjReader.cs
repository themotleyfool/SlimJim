using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Linq;
using SlimJim.Model;

namespace SlimJim.Infrastructure
{
	public class CsProjReader
	{
		private static readonly XNamespace Ns = "http://schemas.microsoft.com/developer/msbuild/2003";

		public virtual CsProj Read(FileInfo csProjFile)
		{
			var xml = LoadXml(csProjFile);
			var properties = xml.Element(Ns + "PropertyGroup");

			if (properties == null) return null;

			var assemblyName = properties.Element(Ns + "AssemblyName");

			if (assemblyName == null) return null;

			return new CsProj
			{
				Path = csProjFile.FullName,
				AssemblyName = assemblyName.Value,
				Guid = properties.Element(Ns + "ProjectGuid").ValueOrDefault()?.ToUpper(),
				TargetFrameworkVersion = properties.Element(Ns + "TargetFrameworkVersion").ValueOrDefault(),
				ReferencedAssemblyNames = ReadReferencedAssemblyNames(xml),
				ReferencedProjectGuids = ReadReferencedProjectGuids(xml),
				UsesMSBuildPackageRestore = FindImportedNuGetTargets(xml)
			};
		}

		private XElement LoadXml(FileInfo csProjFile)
		{
			XElement xml;
			using (var reader = csProjFile.OpenText())
			{
				xml = XElement.Load(reader);
			}
			return xml;
		}

		private List<string> ReadReferencedAssemblyNames(XElement xml)
		{
			var rawAssemblyNames = (from r in xml.DescendantsAndSelf(Ns + "Reference")
											 where r.Parent.Name == Ns + "ItemGroup"
											 select r.Attribute("Include").Value).ToList();
			var unQualifiedAssemblyNames = rawAssemblyNames.ConvertAll(UnQualify);
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
					  select pr.Element(Ns + "Project").Value?.ToUpper()).ToList();
		}

		private bool FindImportedNuGetTargets(XElement xml)
		{
			var importPaths = (from import in xml.DescendantsAndSelf(Ns + "Import")
					select import.Attribute("Project").Value);
			return importPaths.Any(p => p.EndsWith(@"\.nuget\nuget.targets", StringComparison.InvariantCultureIgnoreCase));
		}


	}
}