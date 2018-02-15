using System.Collections.Generic;
using System.IO;
using NUnit.Framework;
using SlimJim.Infrastructure;
using SlimJim.Test.SampleFiles;
using System.Linq;

namespace SlimJim.Test.Infrastructure
{
	[TestFixture]
	public class ProjectFileFinderTests : TestBase
	{
		private static readonly string SampleFileSystemPath = SampleFileHelper.GetSampleFileSystemPath();
		private ProjectFileFinder finder;
		private List<FileInfo> projectFiles;

		[SetUp]
		public void BeforeEach()
		{
			finder = new ProjectFileFinder();
		}

		[Test]
		public void FindsOneProjectInFolderWithCsproj()
		{
			projectFiles = finder.FindAllProjectFiles(Path.Combine(SampleFileSystemPath, @"MyProject"));

			AssertFilesMatching(new[]
				{
					@"MyProject\MyProject.csproj"
				});
		}

		[Test]
		public void ReturnsFileInfosForEachProjectInFileSystem()
		{
			projectFiles = finder.FindAllProjectFiles(SampleFileSystemPath);

			AssertFilesMatching(new[]
				{
					@"MyProject\MyProject.csproj",
					@"Ours\OurProject1\OurProject1.csproj",
					@"Ours\OurProject2\OurProject2.csproj",
					@"Theirs\TheirProject1\TheirProject1.csproj",
					@"Theirs\TheirProject2\TheirProject2.csproj",
					@"Theirs\TheirProject3\TheirProject3.csproj",
				});
		}

		[Test]
		public void IgnoresRelativePath()
		{
			finder.IgnorePatterns("Their");
			projectFiles = finder.FindAllProjectFiles(SampleFileSystemPath);

			AssertFilesMatching(new[]
				{
					@"MyProject\MyProject.csproj",
					@"Ours\OurProject1\OurProject1.csproj",
					@"Ours\OurProject2\OurProject2.csproj"
				});
		}

		[Test]
		public void IgnoresFileName()
		{
			finder.IgnorePatterns("TheirProject3.csproj");
			projectFiles = finder.FindAllProjectFiles(SampleFileSystemPath);

			AssertFilesMatching(new[]
				{
					@"MyProject\MyProject.csproj",
					@"Ours\OurProject1\OurProject1.csproj",
					@"Ours\OurProject2\OurProject2.csproj",
   					@"Theirs\TheirProject1\TheirProject1.csproj",
					@"Theirs\TheirProject2\TheirProject2.csproj",
				});
		}

		[Test]
		public void IgnoresRelativePathWithDifferentCase()
		{
			finder.IgnorePatterns("ThEiR");
			projectFiles = finder.FindAllProjectFiles(SampleFileSystemPath);

			AssertFilesMatching(new[]
				{
					@"MyProject\MyProject.csproj",
					@"Ours\OurProject1\OurProject1.csproj",
					@"Ours\OurProject2\OurProject2.csproj"
				});
		}

		[Test]
		public void IgnoresCertainFoldersByDefault()
		{
			Assert.That(finder.PathIsIgnored(new DirectoryInfo(Path.Combine(WorkingDirectory, ".svn"))), Is.True, ".svn folders ignored");
			Assert.That(finder.PathIsIgnored(new DirectoryInfo(Path.Combine(WorkingDirectory, "doo.svn.wop"))), Is.False, "don't ignore folders with .svn in the name");
			Assert.That(finder.PathIsIgnored(new DirectoryInfo(Path.Combine(WorkingDirectory, ".hg"))), Is.True, ".hg folders ignored");
			Assert.That(finder.PathIsIgnored(new DirectoryInfo(Path.Combine(WorkingDirectory, "doo.hg.wop"))), Is.False, "don't ignore folders with .hg in the name");
			Assert.That(finder.PathIsIgnored(new DirectoryInfo(Path.Combine(WorkingDirectory, ".git"))), Is.True, ".git folders ignored");
			Assert.That(finder.PathIsIgnored(new DirectoryInfo(Path.Combine(WorkingDirectory, "doo.git.wop"))), Is.False, "don't ignore folders with .git in the name");
			Assert.That(finder.PathIsIgnored(new DirectoryInfo(Path.Combine(WorkingDirectory, "bin"))), Is.True, "bin folders ignored");
			Assert.That(finder.PathIsIgnored(new DirectoryInfo(Path.Combine(WorkingDirectory, "obing"))), Is.False, "don't ignore folders with bin in the name");
			Assert.That(finder.PathIsIgnored(new DirectoryInfo(Path.Combine(WorkingDirectory, "obj"))), Is.True, "obj folders ignored");
			Assert.That(finder.PathIsIgnored(new DirectoryInfo(Path.Combine(WorkingDirectory, "blobjee"))), Is.False, "don't ignore folders with obj in the name");
		}

		[Test]
		public void IgnoresReSharperFolders()
		{
			Assert.That(finder.PathIsIgnored(new DirectoryInfo(Path.Combine(WorkingDirectory, "_ReSharper.Something"))), Is.True);
			Assert.That(finder.PathIsIgnored(new DirectoryInfo(Path.Combine(WorkingDirectory, "ReSharper"))), Is.True);
			Assert.That(finder.PathIsIgnored(new DirectoryInfo(Path.Combine(WorkingDirectory, "___ReSharper___"))), Is.True);
		}

		private void AssertFilesMatching(string[] expectedPaths)
		{
			expectedPaths = expectedPaths.Select(p => p.Replace ('\\', Path.DirectorySeparatorChar)).ToArray ();
			Assert.That(projectFiles.ConvertAll(file => file.FullName.Replace(SampleFileSystemPath, "")), Is.EqualTo(expectedPaths));
		}
	}
}