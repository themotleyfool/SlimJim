using System;
using System.Linq;

namespace SlimJim.Model
{
	public sealed class VisualStudioVersion
	{
		private static readonly VisualStudioVersion vs2008 = new VisualStudioVersion("2008", "10.00", "9.0");
		private static readonly VisualStudioVersion vs2010 = new VisualStudioVersion("2010", "11.00", "10.0");
		private static readonly VisualStudioVersion vs2012 = new VisualStudioVersion("2012", "12.00", "12.0");

		private VisualStudioVersion(string year, string slnFileVersionNumber, string pathVersionNumber)
		{
			Year = year;
			SlnFileVersionNumber = slnFileVersionNumber;
			PathVersionNumber = pathVersionNumber;
		}

		public string Year { get; private set; }
		public string SlnFileVersionNumber { get; private set; }
		public string PathVersionNumber { get; private set; }

		public static VisualStudioVersion VS2008
		{
			get { return vs2008; }
		}

		public static VisualStudioVersion VS2010
		{
			get { return vs2010; }
		}

		public static VisualStudioVersion VS2012
		{
			get { return vs2012; }
		}

		public static VisualStudioVersion ParseVersionString(string versionNumber)
		{
			var versions = new[] { vs2008, vs2010, vs2012 };

			return versions.FirstOrDefault(v => versionNumber.Contains(v.Year)) ?? VS2012;
		}

		public override string ToString()
		{
			return string.Format("{0} ({1})", Year, SlnFileVersionNumber);
		}
	}
}