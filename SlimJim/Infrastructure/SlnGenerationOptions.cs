using System;
using System.Collections.Generic;

namespace SlimJim.Infrastructure
{
	public class SlnGenerationOptions
	{
		public SlnGenerationOptions()
		{
			AdditionalSearchPaths = new List<string>();
		}

		public string ProjectsRootDirectory { get; set; }
		public string TargetProjectName { get; set; }
		public List<string> AdditionalSearchPaths { get; set; }
		public string SlnOutputPath { get; set; }

		public SlnGenerationMode Mode
		{
			get 
			{ 
				return string.IsNullOrEmpty(TargetProjectName) 
					? SlnGenerationMode.Full 
					: SlnGenerationMode.Partial; 
			}
		}
	}
}