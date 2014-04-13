using UnityEngine;
using System.Collections;

namespace Events
{
	public class NPC
	{
	
		private string type;
		private string name;
		private string id;
		
		public string GetName()
		{
			return name;
		}
		
		public NPC(string id, string type)
		{
			this.id = id; 
			this.type = type;
			this.name = GameData.GetNameFor(type).Capitalize();
		}
		
		public string GetVariable(string variable)
		{
			if(variable == "name")
			{
				return name;
			}
			
			if(variable == "type")
			{
				return type;
			}
			
			return null;
		}		
	}
}

