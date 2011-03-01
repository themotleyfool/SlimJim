using System.IO;
using SlimJim;
using NUnit.Framework;

namespace SlimJim.Test.FullDirectoryGeneration
{
	[TestFixture]
	public class GeneratesBlankSolutionFileForEmptyDirectory
	{
		[Test]
		public void FileNameDefaultsToRootFolderName()
		{
			var gen = new SolutionFileGenerator(@"C:\TestFolder");

			FileInfo fileHandle = gen.GenerateSolutionFile();

			Assert.That(fileHandle.FullName, Is.EqualTo(@"C:\TestFolder\TestFolder.sln"));
		}
	}
}
