using System;
using System.Collections.Generic;

namespace SlimJim.Model
{
	public class Sln
	{
		public Sln()
		{
			Projects = new List<CsProj>();
			Guid = System.Guid.NewGuid().ToString("N");
		}

		public string Name { get; set; }
		public string Guid { get; set; }
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