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
	}
}