using NUnit.Framework;

namespace SlimJim.Test
{
	[TestFixture]
	public class HintPathConverterTests
	{
		[Test]
		[TestCase(@"c:\projects\solutions\packages", @"c:\projects\myproj\src\MyProj\MyProj.csproj", @"..\..\..\solutions\packages")]
		public void CalculateRelativePathToSlimjimPackages(string solutionPath, string csProjPath, string expectedResult)
		{
			var actual = HintPathConverter.CalculateRelativePathToSlimjimPackages(solutionPath, csProjPath);

			Assert.That(actual, Is.EqualTo(expectedResult));
		}
	}
}