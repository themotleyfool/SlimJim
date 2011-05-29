using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace SlimJim.Model
{
	public class SlnGenerationOptions
	{
		private const string DefaultSolutionName = "SlimJim";
		private string solutionName;
		private string slnOutputPath;

	    public SlnGenerationOptions(string workingDirectory)
		{
			ProjectsRootDirectory = workingDirectory;
			AdditionalSearchPaths = new List<string>();
			TargetProjectNames = new List<string>();
			VisualStudioVersion = VisualStudioVersion.VS2010;
		}

	    public string ProjectsRootDirectory { get; set; }
	    public List<string> AdditionalSearchPaths { get; private set; }
	    public ICollection<string> TargetProjectNames { get; private set; }
	    public VisualStudioVersion VisualStudioVersion { get; set; }
	    public bool IncludeEfferentAssemblyReferences { get; set; }

	    public string SlnOutputPath
		{
			get { return slnOutputPath ?? ProjectsRootDirectory; }
			set { slnOutputPath = value; }
		}

	    public string SolutionName
		{
			get
			{
				if (string.IsNullOrEmpty(solutionName))
				{
					if (TargetProjectNames.Count == 1)
					{
						return TargetProjectNames.First();
					}
					
					if (TargetProjectNames.Count > 1)
					{
						return string.Join("_", TargetProjectNames);
					}
					
					if (!string.IsNullOrEmpty(ProjectsRootDirectory))
					{
						return GetLastSegmentNameOfProjectsRootDirectory();
					}

					return DefaultSolutionName;
				}

				return solutionName;
			}
			set { solutionName = value; }
		}

	    private string GetLastSegmentNameOfProjectsRootDirectory()
		{
			MatchCollection matches = Regex.Matches(ProjectsRootDirectory, @"([^\\:]+)\\?");
			string lastSegment = DefaultSolutionName;
			foreach (Match match in matches)
			{
				string segmentName = match.Groups[1].Value;
				if (segmentName != "")
				{
					lastSegment = segmentName;
				}
			}

			return lastSegment;
		}

	    public SlnGenerationMode Mode
		{
			get 
			{ 
				return TargetProjectNames.Count == 0 
					? SlnGenerationMode.FullGraph 
					: SlnGenerationMode.PartialGraph; 
			}
		}
	}
}