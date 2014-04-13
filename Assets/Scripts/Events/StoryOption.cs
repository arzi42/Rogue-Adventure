using System;
using Entities;

namespace Events
{
	public class StoryOption
	{
		private string text;
		private StoryOptionType type;
		private string[] parameters;
		
		public StoryOption (string text, StoryOptionType type, string[] parameters)
		{
			this.text = text;
			this.type = type;
			this.parameters = parameters;
		}
		
		public string GetText()
		{
			return text;
		}

		public void Selected(Adventurer adventurer)
		{
			switch(type)
			{
			case StoryOptionType.AddTrait:

				foreach(string param in parameters)
				{
					adventurer.AddTrait(param);
				}

				break;
			}
		}
	}
}

