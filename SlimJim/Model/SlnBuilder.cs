using System.Collections.Generic;

namespace SlimJim.Model
{
	public class SlnBuilder
	{
		private readonly List<CsProj> projectsList;
		private Sln builtSln;
		private static SlnBuilder overriddenBuilder;

		public SlnBuilder(List<CsProj> projectsList)
		{
			this.projectsList = projectsList;
		}

		public virtual Sln BuildPartialGraphSln(SlnGenerationOptions options)
		{
			builtSln = new Sln(options.SolutionName)
				{
					Version = options.VisualStudioVersion
				};

			foreach (string targetProjectName in options.TargetProjectNames)
			{
				CsProj rootProject = AddAssemblySubtree(targetProjectName);

				AddAfferentReferencesToProject(rootProject);
			}

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
				builtSln.AddProjects(project);

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

		public static SlnBuilder GetSlnBuilder(List<CsProj> projects)
		{
			return overriddenBuilder ?? new SlnBuilder(projects);
		}

		public static void OverrideDefaultBuilder(SlnBuilder slnBuilder)
		{
			overriddenBuilder = slnBuilder;
		}
	}
}