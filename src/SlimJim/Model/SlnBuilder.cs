using System;
using System.Collections.Generic;
using System.Linq;
using log4net;

namespace SlimJim.Model
{
	public class SlnBuilder
	{
		private static readonly ILog Log = LogManager.GetLogger(typeof(SlnFileGenerator));

		private readonly List<CsProj> projectsList;
		private Sln builtSln;
		private static SlnBuilder overriddenBuilder;
		private SlnGenerationOptions options;

		public SlnBuilder(List<CsProj> projectsList)
		{
			this.projectsList = projectsList;
		}

		public virtual Sln BuildSln(SlnGenerationOptions options)
		{
			this.options = options;
			builtSln = new Sln(options.SolutionName)
				{
					Version = options.VisualStudioVersion,
					ProjectsRootDirectory = options.ProjectsRootDirectory
				};

			AddProjectsToSln(options);

			return builtSln;
		}

		private void AddProjectsToSln(SlnGenerationOptions options)
		{
			if (options.Mode == SlnGenerationMode.PartialGraph)
			{
				AddPartialProjectGraphToSln(options);
			}
			else
			{
				AddAllProjectsToSln();
			}
		}

		private void AddPartialProjectGraphToSln(SlnGenerationOptions options)
		{
			Log.Info("Building partial graph solution for target projects: " + string.Join(", ", options.TargetProjectNames));

			foreach (string targetProjectName in options.TargetProjectNames)
			{
				CsProj rootProject = AddAssemblySubtree(targetProjectName);

				if (rootProject == null)
				{
					Log.WarnFormat("Project {0} not found.", targetProjectName);
				}

				if (options.SkipAfferentAssemblyReferences == false)
				{
					AddAfferentReferencesToProject(rootProject);
				}
			}
		}

		private void AddAllProjectsToSln()
		{
			Log.Info("Building full graph solution.");

			projectsList.ForEach(AddProject);
		}

		private CsProj AddAssemblySubtree(string assemblyName, string targetFrameworkVersion = "")
		{
			CsProj project = FindProjectByAssemblyName(assemblyName, targetFrameworkVersion);

			AddProjectAndReferences(project);

			return project;
		}

		private CsProj FindProjectByAssemblyName(string assemblyName, string targetFrameworkVersion)
		{
			var matches = projectsList.Where(p => p.AssemblyName == assemblyName).ToList();


			if (matches.Count <= 1)
			{
				var single = matches.SingleOrDefault();
				if (single != null) Log.InfoFormat("Found projects with AssemblyName {0}: {1}", assemblyName, single.Path);
				return single;
			}

			//TODO: filter projects that don't specify version
			if (string.IsNullOrEmpty(targetFrameworkVersion))
			{
				Log.WarnFormat("Found multiple projects with AssemblyName {0} and no target framework version is specified: {1}", assemblyName, string.Join(", ", matches.Select(m => m.Path)));
				return matches.First();
			}

			var myVersion = new Version(targetFrameworkVersion.Substring(1));
			var versions = matches
							.Where(m => m.TargetFrameworkVersion != null && m.TargetFrameworkVersion.StartsWith("v"))
							.ToDictionary(m => new Version(m.TargetFrameworkVersion.Substring(1)));

			var closest = versions.Where(v => v.Key <= myVersion).OrderByDescending(v => v.Key).FirstOrDefault();

		    if (closest.Value != null)
		    {
                Log.InfoFormat("Found multiple projects with AssemblyName {0}: {1} and chose {2}", assemblyName, string.Join(", ", matches.Select(m => m.Path)), closest.Value.Path);
                return closest.Value;
		    }

            Log.WarnFormat("Found multiple projects with AssemblyName {0}: {1} and none have compatible TargetFrameworkVersion property. Choosing {2}", assemblyName, string.Join(", ", matches.Select(m => m.Path)), matches.First());
		    return matches.First();
		}

		private void AddProjectAndReferences(CsProj project)
		{
			if (project != null)
			{
				AddProject(project);

				IncludeEfferentProjectReferences(project);

				if (options.IncludeEfferentAssemblyReferences)
				{
					IncludeEfferentAssemblyReferences(project);
				}
			}
		}

		private void AddProject(CsProj project)
		{
			builtSln.AddProjects(project);
		}

		private void IncludeEfferentProjectReferences(CsProj project)
		{
			foreach (string projectGuid in project.ReferencedProjectGuids)
			{
				AddProjectSubtree(projectGuid);
			}
		}

		private void IncludeEfferentAssemblyReferences(CsProj project)
		{
			foreach (string assemblyName in project.ReferencedAssemblyNames)
			{
				AddAssemblySubtree(assemblyName, project.TargetFrameworkVersion);
			}
		}

		private void AddProjectSubtree(string projectGuid)
		{
			CsProj referencedProject = FindProjectByProjectGuid(projectGuid);

			AddProjectAndReferences(referencedProject);
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

		private CsProj FindProjectByProjectGuid(string projectGuid)
		{
			return projectsList.Find(csp => csp.Guid == projectGuid);
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
