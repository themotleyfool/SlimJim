using System.Collections.Generic;

namespace SlimJim.Model
{
	public class SlnBuilder
	{
		private readonly List<CsProj> projectsList;
		private readonly Sln builtSln;

		public SlnBuilder(List<CsProj> projectsList)
		{
			this.projectsList = projectsList;
			builtSln = new Sln();
		}

		public virtual Sln BuildPartialGraphSln(string rootAssemblyName)
		{
			builtSln.Name = rootAssemblyName;

			CsProj rootProject = AddAssemblySubtree(rootAssemblyName);

			AddAfferentReferencesToProject(rootProject);

			return builtSln;
		}

		private void AddAfferentReferencesToProject(CsProj project)
		{
			if (project != null)
			{
				List<CsProj> afferentAssemblyReferences = projectsList.FindAll(
					csp => csp.ReferencedAssemblyNames.Contains(project.AssemblyName));

				AddAfferentReferences(afferentAssemblyReferences);

				List<CsProj> afferentProjectReferences =
					projectsList.FindAll(csp => csp.ReferencedProjectGuids.Contains(project.Guid));

				AddAfferentReferences(afferentProjectReferences);
			}
		}

		private void AddAfferentReferences(List<CsProj> afferentReferences)
		{
			foreach (CsProj assemblyReference in afferentReferences)
			{
				AddProjectAndReferences(assemblyReference);
			}
		}

		private CsProj AddAssemblySubtree(string assemblyName)
		{
			CsProj project = FindProjectByAssemblyName(assemblyName);

			AddProjectAndReferences(project);

			return project;
		}

		private void AddProjectSubtree(string projectGuid)
		{
			CsProj referencedProject = FindProjectByProjectGuid(projectGuid);

			AddProjectAndReferences(referencedProject);
		}

		private void AddProjectAndReferences(CsProj project)
		{
			if (project != null)
			{
				builtSln.AddProject(project);

				foreach (string projectGuid in project.ReferencedProjectGuids)
				{
					AddProjectSubtree(projectGuid);
				}
			}
		}

		private CsProj FindProjectByProjectGuid(string projectGuid)
		{
			return projectsList.Find(csp => csp.Guid == projectGuid);
		}

		private CsProj FindProjectByAssemblyName(string assemblyName)
		{
			return projectsList.Find(csp => csp.AssemblyName == assemblyName);
		}
	}
}