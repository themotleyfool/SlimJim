using System;
using System.Linq;

namespace SlimJim.Model
{
	public sealed class VisualStudioVersion
	{
		private static readonly VisualStudioVersion vs2008 = new VisualStudioVersion("2008", "10.00", "9.0");
		private static readonly VisualStudioVersion vs2010 = new VisualStudioVersion("2010", "11.00", "10.0");
		private static readonly VisualStudioVersion vs2012 = new VisualStudioVersion("2012", "12.00", "12.0");
		private static readonly VisualStudioVersion vs2013 = new VisualStudioVersion("2013", "12.00", "13.0");
		private static readonly VisualStudioVersion vs2015 = new VisualStudioVersion("2015", "12.00", "14.0");
        private static readonly VisualStudioVersion vs2017 = new VisualStudioVersion("2017", "12.00", "15.0");

        private string _year;

        private VisualStudioVersion(string year, string slnFileVersionNumber, string pathVersionNumber)
		{
			Year = year;
			SlnFileVersionNumber = slnFileVersionNumber;
            PathVersionNumber = pathVersionNumber;
		}


        /// <summary>
        /// Starting with Visual Studio 2015, the year, but the version is used in the solution header:
        ///  Example VS2017 solution header:
        ///  # Visual Studio 15
        ///  VisualStudioVersion = 15.0.26430.15
        ///  MinimumVisualStudioVersion = 10.0.40219.1
        /// </summary>
        public string Year
        {
            get
            {
                if (_year == "2015" || _year == "2017")
                {
                    return PathVersionNumber.Substring(0, 2);
                }

                return _year;
            }
            private set
            {
                _year = value;
            }
        }

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

        public static VisualStudioVersion VS2013
        {
            get { return vs2013; }
        }

        public static VisualStudioVersion VS2015
        {
            get { return vs2015; }
        }

        public static VisualStudioVersion VS2017
        {
            get { return vs2017; }
        }


        public static VisualStudioVersion ParseVersionString(string versionNumber)
		{
			var versions = new[] { vs2008, vs2010, vs2012, vs2013, vs2015, vs2017 };

			return versions.FirstOrDefault(v => versionNumber.Contains(v.Year)) ?? VS2017;
		}

		public override string ToString()
		{
			return string.Format("{0} ({1})", Year, SlnFileVersionNumber);
		}
	}
}