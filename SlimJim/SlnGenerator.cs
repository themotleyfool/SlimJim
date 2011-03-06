using System.Collections.Generic;

namespace SlimJim
{
	public class SlnGenerator
	{
		private readonly List<CsProj> projectsList;
		private readonly Sln generatedSln;

		public SlnGenerator(List<CsProj> projectsList)
		{
			this.projectsList = projectsList;
			generatedSln = new Sln();
		}

		public Sln GeneratePartialGraphSln(string rootAssemblyName)
		{
			generatedSln.Name = rootAssemblyName;

			CsProj rootProject = AddAssemblySubtree(rootAssemblyName);

			AddAfferentReferencesToProject(rootProject);

			return generatedSln;
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
				generatedSln.AddProject(project);

				foreach (string referencedAssemblyName in project.ReferencedAssemblyNames)
				{
					AddAssemblySubtree(referencedAssemblyName);
				}

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