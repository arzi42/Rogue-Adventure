using System;
using System.Collections.Generic;
using UnityEngine;

using Entities;

namespace Events
{
	public class StoryEvent
	{
		private string text;
		
		private List<StoryOption> options;
		
		private Dictionary<string, NPC> npcs;

		public bool once;
		
		public StoryEvent (string source)
		{
			options = new List<StoryOption>();
			
			npcs = new Dictionary<string, NPC>();
			
			ParseFromSource(source);
		}

		public void SelectOption(int index, Adventurer adventurer)
		{
			options[index].Selected(adventurer);
		}
		
		public string[] GetOptions()
		{
			string[] optionStrings = new string[options.Count];
			
			for(int i = 0; i < options.Count; i++)
			{
				optionStrings[i] = options[i].GetText();
			}
			
			return optionStrings;
			
		}
		
		private void ParseFromSource(string source)
		{
			const int TEXT = 1, OPTION = 2;
			
			int mode = 0;
			
			Stack<int> previousModes = new Stack<int>();
			
			string optionString = "";
			StoryOptionType optionType = StoryOptionType.AddTrait;
			string[] optionParameters = null;
			
			foreach(string line in source.Split('\n'))
			{
				if(line.Trim().StartsWith("#"))
				{
					string command = line.Trim().Substring(1);
					
					string[] parameters = null;
					
					if(command.Contains(":"))
					{
						string[] parts = command.Split(':');
						
						command = parts[0].Trim();
						
						parameters = parts[1].Split(',');
						
						for(int i = 0; i < parameters.Length; i++)
						{
							parameters[i] = parameters[i].Trim();
						}
					}
					
					if(command == "text")
					{
						previousModes.Push(mode);
						
						mode = TEXT;
					}
					if(command == "option")
					{
						previousModes.Push(mode);
						
						mode = OPTION;
						
						optionString = "";
						
					}
					
					if(command == "add_trait")
					{
						optionType = StoryOptionType.AddTrait;
						
						optionParameters = parameters;
					}
					
					if(command == "end")
					{
						if(mode == OPTION)
						{
							StoryOption option = new StoryOption(optionString, optionType, optionParameters);
							
							
							
							options.Add(option);
						}
						
						mode = previousModes.Pop();
					}
					
					if(command == "npc")
					{
						npcs.Add(parameters[0], new NPC(parameters[0], parameters[1]));
					}

					if(command == "once")
					{
						once = true;
					}
				}
				else
				{
					switch(mode)
					{
					case TEXT:
						text += ParseLine(line) + "\n";
						break;
					case OPTION:
						optionString += line + "\n";
						break;
					}
				}
			}
				
				
			
		}
		
		private string ParseLine(string line)
		{
			string parsedString = "";
			
			int mode = 0;
			
			int PARSE_VARIABLE = 1;
			
			string currentVariable = "";
			
			string referencedEntity = null;
			
			for(int i = 0; i < line.Length; i++)
			{
				char character = line[i];
				
				if(mode == PARSE_VARIABLE)
				{
					if(Char.IsLetterOrDigit(character))
					{
						currentVariable += character;
					}
					else if(character == '.' && i < line.Length -1 && Char.IsLetterOrDigit(line[i+1]))
					{
						referencedEntity = currentVariable;
						
						currentVariable = "";
					}
					else 
					{
						if(referencedEntity != null)
						{
							Debug.Log(referencedEntity);
							parsedString += npcs[referencedEntity].GetVariable(currentVariable) + character;
						}
						else
						{
							
						}
						
						currentVariable = "";
						referencedEntity = null;
						
						mode = 0;
					}
					
					
				}
				else
				{
					if(character == '$')
					{
						mode = PARSE_VARIABLE;
					}
					else
					{
						parsedString += character;
					}
				}
			}
			
			
			return parsedString;
		}
		
		public string RunEvent()
		{
			return text;
		}
	}
}

