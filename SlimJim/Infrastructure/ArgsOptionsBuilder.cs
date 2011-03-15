namespace SlimJim.Infrastructure
{
	public class ArgsOptionsBuilder
	{
		private SlnGenerationOptions options;

		public static SlnGenerationOptions BuildOptions(string[] args, string workingDirectory)
		{
			var builder = new ArgsOptionsBuilder();
			return builder.Build(args, workingDirectory);
		}

		private SlnGenerationOptions Build(string[] args, string workingDirectory)
		{
			options = new SlnGenerationOptions
				{
					ProjectsRootDirectory = workingDirectory
				};

			ProcessSwitches(args);

			return options;
		}

		private void ProcessSwitches(string[] args)
		{
			foreach (string arg in args)
			{
				if (!arg.StartsWith("/"))
				{
					options.TargetProjectName = arg;
				}
				else
				{
					ProcessSwitch(arg);
				}
			}
		}

		private void ProcessSwitch(string arg)
		{
			string command = arg.Substring(1, 1);
			string value = arg.Substring(3);

			switch (command)
			{
				case "d":
					options.ProjectsRootDirectory = value;
					break;
				case "p":
					options.TargetProjectName = value;
					break;
				case "a":
					options.AdditionalSearchPaths.Add(value);
					break;
				case "o":
					options.SlnOutputPath = value;
					break;
			}
		}
	}
}