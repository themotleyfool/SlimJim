using System.Collections.Generic;

namespace SlimJim.Model
{
	public class CsProj
	{
		public CsProj()
		{
			ReferencedAssemblyNames = new List<string>();
			ReferencedProjectGuids = new List<string>();
		}

		public string Guid { get; set; }
		public string Path { get; set; }
		public string AssemblyName { get; set; }
		public string TargetFrameworkVersion { get; set; }
		public List<string> ReferencedAssemblyNames { get; set; }
		public List<string> ReferencedProjectGuids { get; set; }
		public bool UsesMSBuildPackageRestore { get; set; }

		public string ProjectName
		{
			get { return System.IO.Path.GetFileNameWithoutExtension(Path); }
		}

		public void ReferencesAssemblies(params CsProj[] assemblyReferences)
		{
			foreach (CsProj reference in assemblyReferences)
			{
				if (!ReferencedAssemblyNames.Contains(reference.AssemblyName))
				{
					ReferencedAssemblyNames.Add(reference.AssemblyName);
				}
			}
		}

		public void ReferencesProjects(params CsProj[] projectReferences)
		{
			foreach (CsProj reference in projectReferences)
			{
				if (!ReferencedProjectGuids.Contains(reference.Guid))
				{
					ReferencedProjectGuids.Add(reference.Guid);
				}
			}
		}

		public override string ToString()
		{
			return base.ToString() + 
				string.Format(@" {{AssemblyName=""{0}"", Guid=""{1}"", Path=""{2}""}}", AssemblyName, Guid, Path);
		}
	}
}
