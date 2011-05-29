
Microsoft Visual Studio Solution File, Format Version $sln.Version.SlnFileVersionNumber$
# Visual Studio $sln.Version.Year$

$sln.Projects:{ p | Project("$sln.Guid$") = "$p.AssemblyName$", "$p.Path$", "$p.Guid$"
EndProject};separator="\r\n"$

Global
$if(sln.Projects)$
	GlobalSection(SolutionConfigurationPlatforms) = preSolution
		Debug|Any CPU = Debug|Any CPU
		Release|Any CPU = Release|Any CPU
	EndGlobalSection
	GlobalSection(ProjectConfigurationPlatforms) = postSolution
	$sln.Projects:{ p |
	$p.Guid$.Debug|Any CPU.ActiveCfg = Debug|Any CPU
	$p.Guid$.Debug|Any CPU.Build.0 = Debug|Any CPU
	$p.Guid$.Release|Any CPU.ActiveCfg = Release|Any CPU
	$p.Guid$.Release|Any CPU.Build.0 = Release|Any CPU};separator="\r\n"$
	EndGlobalSection
	$endif$
	GlobalSection(SolutionProperties) = preSolution
		HideSolutionNode = FALSE
	EndGlobalSection
EndGlobal
