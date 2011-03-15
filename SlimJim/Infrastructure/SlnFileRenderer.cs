using System;
using System.IO;
using System.Reflection;
using SlimJim.Model;
using Antlr3.ST;

namespace SlimJim.Infrastructure
{
	public class SlnFileRenderer
	{
		private readonly Sln solution;

		public SlnFileRenderer(Sln solution)
		{
			this.solution = solution;
		}

		public string Render()
		{
			string templatesPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Templates\\");
			var templateGroup = new StringTemplateGroup("SlnTemplates", templatesPath);
			StringTemplate slnTemplate = templateGroup.GetInstanceOf("SolutionTemplate");
			slnTemplate.SetAttribute("sln", solution);
			return Environment.NewLine + slnTemplate + Environment.NewLine;
		}
	}
}