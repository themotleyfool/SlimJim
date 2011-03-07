using System.Collections.Generic;
using System.IO;
using NUnit.Framework;
using SlimJim.Infrastructure;
using SlimJim.Test.SampleFiles;

namespace SlimJim.Test.Infrastructure
{
	[TestFixture]
	public class ProjectFileFinderTests
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
			projectFiles = finder.FindAllProjectFiles(Path.Combine(SampleFileSystemPath, @"MyProject\"));

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
			finder.IgnorePaths("Theirs");
			projectFiles = finder.FindAllProjectFiles(SampleFileSystemPath);

			AssertFilesMatching(new[]
				{
					@"MyProject\MyProject.csproj",
					@"Ours\OurProject1\OurProject1.csproj",
					@"Ours\OurProject2\OurProject2.csproj"
				});
		}

		[Test]
		public void IgnoresRelativePathWithDifferentCase()
		{
			finder.IgnorePaths("ThEiRs");
			projectFiles = finder.FindAllProjectFiles(SampleFileSystemPath);

			AssertFilesMatching(new[]
				{
					@"MyProject\MyProject.csproj",
					@"Ours\OurProject1\OurProject1.csproj",
					@"Ours\OurProject2\OurProject2.csproj"
				});
		}

		private void AssertFilesMatching(string[] expectedPaths)
		{
			Assert.That(projectFiles.ConvertAll(file => file.FullName.Replace(SampleFileSystemPath, "")), Is.EqualTo(expectedPaths));
		}
	}
}