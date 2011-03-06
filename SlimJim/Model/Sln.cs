using System.Collections.Generic;

namespace SlimJim.Model
{
	public class Sln
	{
		public Sln()
		{
			Projects = new List<CsProj>();
		}

		public string Name { get; set; }
		public List<CsProj> Projects { get; set; }

		public void AddProject(CsProj csProj)
		{
			if (!Projects.Contains(csProj))
			{
				Projects.Add(csProj);
			}
		}
	}
}