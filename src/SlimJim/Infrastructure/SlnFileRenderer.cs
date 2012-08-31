using System;
using System.IO;
using System.Reflection;
using SlimJim.Model;
using Antlr3.ST;

namespace SlimJim.Infrastructure
{
	public class SlnFileRenderer
	{
		private static readonly StringTemplateGroup templateGroup;
		private readonly Sln solution;

		static SlnFileRenderer()
		{
			var stream = typeof (SlnFileRenderer).Assembly.GetManifestResourceStream("SlimJim.Templates.SolutionTemplate.st");
			templateGroup = new StringTemplateGroup("SlnTemplates");
			using (stream)
			{
				templateGroup.DefineTemplate("SolutionTemplate", new StreamReader(stream).ReadToEnd());
			}
		}
		public SlnFileRenderer(Sln solution)
		{
			this.solution = solution;
		}

		public string Render()
		{
			var slnTemplate = templateGroup.GetInstanceOf("SolutionTemplate");
			slnTemplate.SetAttribute("sln", solution);
			return slnTemplate.ToString();
		}
	}
}