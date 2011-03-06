using System.Collections.Generic;
using SlimJim.Model;

namespace SlimJim.Infrastructure
{
	public class SolutionFileInfo
	{
		public SolutionFileInfo(string slnFilePath, string slnFileContents)
		{
			FullText = slnFileContents;
			FullName = slnFilePath;
			CsProjs = new List<CsProj>();
		}

		public string FullName { get; private set; }
		public string FullText { get; set; }

		public List<CsProj> CsProjs { get; set; }
	}
}