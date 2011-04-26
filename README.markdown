SlimJim
=======

SlimJim is a Visual Studio solution file generator.

SlimJim is meant to be invoked from the command line. It will search a given folder (recursively) for .csproj files, and include them in a new solution file if:
1 The name mathes one of the target project files.
2 The project has a dependency on one or more of the target project files.
3 One of the target project files has a dependency on it.
4 Any project to be included in the solution has a project reference to it. 

Invocation
----------

* `/r:path-to-projects-root`              Required; indicates the directory which contains the root project that you want to build a solution file for.
* `/t:target-project-name`                Required; include one or more target projects in your solution. All dependencies will be included for them.
* `/a:path-to-include-in-search`          Optional; specify one or more directories to search for projects that depend on or are depdended on by the target projects.
* `/o:destination-path-for-solution-file` Optional; specify a directory other than the working directory to write the generated solution file.
* `/v:visual-studio-version-year`         Optional; supported versions: 2008, 2010 (default).
* `/n:solution-name`                      Optional; alternate name for generated solution file. If omitted, defaults to target project name (/t:).


Upcoming features
-----------------

* .slim file in your projects root directory can contain details for generation of one or more solution files. This will allow for simple invocation, e.g. "C:\MyProjects>slimjim".
* Generate solution file for entire project graph.

Known issues 
------------
* If SlnOutputPath does not exist, an exception is raised.
* All paths must be absolute. Relative paths for additional search paths, projects root, and solution output path will be supported soon.
* No usage instructions
* No error handling
