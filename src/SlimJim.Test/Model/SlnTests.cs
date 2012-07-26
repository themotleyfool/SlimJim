using System;
using System.Linq;
using NUnit.Framework;
using SlimJim.Model;

namespace SlimJim.Test.Model
{
	[TestFixture]
	public class SlnTests
	{
		[Test]
		public void VersionDefaultsTo2010()
		{
			Assert.That(new Sln("sln").Version, Is.EqualTo(VisualStudioVersion.VS2010));
		}

		[Test]
		public void GuidFormatIncludesCurlyBraces()
		{
			Assert.That(new Sln("sample").Guid, Is.StringStarting("{"));
			Assert.That(new Sln("sample").Guid, Is.StringEnding("}"));
		}

		[Test]
		public void CreatesNoSolutionFoldersForSimpleProjectStructure()
		{
			var sln = new Sln("sample") {ProjectsRootDirectory = "Fake/Example"};
			sln.AddProjects(new CsProj {Path = "Fake/Example/ProjectA/ProjectA.csproj"});
			sln.AddProjects(new CsProj {Path = "Fake/Example/ProjectB/ProjectB.csproj"});

			Assert.That(sln.Folders, Is.Null, "Folders");
		}

		[Test]
		public void CreatesSolutionFoldersForNestedProjectStructure()
		{
			var sln = new Sln("sample") { ProjectsRootDirectory = "Fake/Example" };
			sln.AddProjects(new CsProj { Path = "Fake/Example/ModuleA/ProjectA/ProjectA.csproj" });
			sln.AddProjects(new CsProj { Path = "Fake/Example/ModuleA/ProjectB/ProjectB.csproj" });

			var folder = sln.Folders.FirstOrDefault();

			Assert.That(folder.FolderName, Is.EqualTo("ModuleA"));
			Assert.That(folder.ContentGuids.Count, Is.EqualTo(2));
		}

		[Test]
		public void CreatesNestedSolutionFolders()
		{
			var sln = new Sln("sample") { ProjectsRootDirectory = "Fake/Example" };
			var proj = new CsProj { Path = "Fake/Example/Grouping1/ModuleA/ProjectA/ProjectA.csproj", Guid = Guid.NewGuid().ToString("B")};
			sln.AddProjects(proj);

			var child = sln.Folders.First(f => f.FolderName == "ModuleA");
			var parent = sln.Folders.First(f => f.FolderName == "Grouping1");

			Assert.That(parent.ContentGuids, Is.EqualTo(new[] {child.Guid}));
			Assert.That(child.ContentGuids, Is.EqualTo(new[] { proj.Guid }));
		}

		[Test]
		public void HandlesTrailingSlashOnRootDirectory()
		{
			var sln = new Sln("sample") { ProjectsRootDirectory = "Fake/Example/" };
			var proj = new CsProj { Path = "Fake/Example/ModuleA/ProjectA/ProjectA.csproj", Guid = Guid.NewGuid().ToString("B") };
			sln.AddProjects(proj);

			Assert.That(sln.Folders.FirstOrDefault(f => f.FolderName == "ModuleA"), Is.Not.Null, "Folders");
		}
	}
}