using System;
using System.IO;
using System.Linq;
using System.Xml;
using System.Xml.XPath;
using SlimJim.Model;

namespace SlimJim
{
	public class HintPathConverter : CsProjConverter
	{
		private const string NuGetPackagesDirectoryName = @"packages\";
		private enum Mode { Convert, Restore }

		public void ConvertHintPaths(Sln solution, SlnGenerationOptions options)
		{
			var packagesDir = Path.Combine(options.SlnOutputPath, "packages");
			foreach (var project in solution.Projects)
			{
				ConvertProjectHintPaths(project, packagesDir);
			}
		}

		public void RestoreHintPaths(Sln solution, SlnGenerationOptions options)
		{
			var packagesDir = Path.Combine(options.SlnOutputPath, "packages");
			foreach (var project in solution.Projects)
			{
				ConvertProjectHintPaths(project, packagesDir, Mode.Restore);
			}
		}

		private void ConvertProjectHintPaths(CsProj project, string packagesDir, Mode mode = Mode.Convert)
		{
			var doc = LoadProject(project);
			var nav = doc.CreateNavigator();

			var nugetHintPaths = nav.Select("//msb:ItemGroup/msb:Reference/msb:HintPath", nsMgr)
				.Cast<XPathNavigator>()
				.Where(e => e.Value.Contains(NuGetPackagesDirectoryName))
				.ToList();

			if (!nugetHintPaths.Any()) return;

			var firstIndex = nugetHintPaths.First().Value.IndexOf(NuGetPackagesDirectoryName, StringComparison.InvariantCultureIgnoreCase);

			if (nugetHintPaths.Any(e => e.Value.IndexOf(NuGetPackagesDirectoryName, StringComparison.InvariantCultureIgnoreCase) != firstIndex))
			{
				log.WarnFormat("Project {0} does not have consistent HintPath values for nuget packages. Skipping.", project.Path);
				return;
			}

			log.InfoFormat("Converting nuget hint paths in project {0}.", project.Path);

			var originalPackageDir = nugetHintPaths.First().Value.Substring(0, firstIndex + NuGetPackagesDirectoryName.Length);
			string relativePackagesDir;

			if (mode == Mode.Convert)
			{
				relativePackagesDir = CalculateRelativePathToSlimjimPackages(packagesDir, project.Path);
			}
			else
			{
				var e = doc.SelectSingleNode("//msb:PropertyGroup/msb:SlimJimOriginalPackageDir", nsMgr);
				if (e == null)
				{
					log.WarnFormat("Not restoring hint paths to project {0} because it does not have property SlimJimOriginalPackageDir defined.", project.Path);
					return;
				}
				relativePackagesDir = e.InnerText;
				e.ParentNode.RemoveChild(e);
			}

			foreach (var hintPath in nugetHintPaths)
			{
				var modifiedHintPath = Path.Combine(relativePackagesDir, hintPath.Value.Substring(firstIndex + NuGetPackagesDirectoryName.Length));
				log.DebugFormat("Change hint path from {0} to {1}", hintPath.Value, modifiedHintPath);
				var element = CreateElementWithInnerText(doc, "HintPath", modifiedHintPath);
				hintPath.ReplaceSelf(element.CreateNavigator());
			}

			if (mode == Mode.Convert)
			{
				var firstPropertyGroup = doc.SelectSingleNode("//msb:PropertyGroup[1]", nsMgr);
				if (firstPropertyGroup.SelectSingleNode("msb:SlimJimOriginalPackageDir", nsMgr) == null)
				{
					firstPropertyGroup.AppendChild(CreateElementWithInnerText(doc, "SlimJimOriginalPackageDir",
						originalPackageDir));
				}
			}

			doc.Save(project.Path);
		}

		public static string CalculateRelativePathToSlimjimPackages(string packagesPath, string csProjPath)
		{
			var relativeUri = new Uri(csProjPath).MakeRelativeUri(new Uri(packagesPath)).OriginalString;
			return relativeUri.Replace(Path.AltDirectorySeparatorChar, Path.DirectorySeparatorChar);
		}
	}
}