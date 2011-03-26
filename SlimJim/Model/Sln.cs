using System.Collections.Generic;

namespace SlimJim.Model
{
	public class Sln
	{
		public Sln(string name)
			:this(name, System.Guid.NewGuid().ToString("N"))
		{
		}

		public Sln(string name, string guid)
		{
			Name = name;
			Guid = guid;
			Projects = new List<CsProj>();
			Version = VisualStudioVersion.VS2010;
		}

		public string Name { get; private set; }
		public string Guid { get; private set; }
		public VisualStudioVersion Version { get; set; }
		public List<CsProj> Projects { get; private set; }

		public void AddProjects(params CsProj[] csProjs)
		{
			foreach (CsProj proj in csProjs)
			{
				if (!Projects.Contains(proj))
				{
					Projects.Add(proj);
				}
			}
		}
	}
}