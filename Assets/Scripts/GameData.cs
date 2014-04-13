using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GameData : MonoBehaviour
{
	public TextAsset names;
	
	private static GameData instance;
	
	private Dictionary<string, List<string>> nameLists;
	
	void Awake()
	{
		instance = this;
		
		ParseNames();
		
	}
	
	public static string GetNameFor(string id)
	{
		return instance.ParseName(id);
	}
	
	private string ParseName(string id)
	{
		string name = nameLists[id].GetRandomElement();
		
		
		while(name.Contains("["))
		{
			Debug.Log (name);
			
			int partStart = name.IndexOf("[");
			int partEnd = name.IndexOf("]") - partStart -1;
			
			
			string part = name.Substring(partStart+1, partEnd);
			
			Debug.Log(part);
			
			name = name.Replace("[" + part + "]", nameLists[part].GetRandomElement());
		}
		
		Debug.Log (name);
		
		return name;
	}
	
	private void ParseNames()
	{
		nameLists = new Dictionary<string, List<string>>();
		
		string[] lines = names.text.Split(new char[] {'\n'}, System.StringSplitOptions.RemoveEmptyEntries);
		
		string currentType = null;
		
		foreach(string line in lines)
		{
			if(line.StartsWith("#type:"))
			{
				currentType = line.Substring(line.IndexOf(":")+1).Trim();
				
				nameLists.Add(currentType, new List<string>());
			}
			else
			{
				if(currentType != null)
				{
					nameLists[currentType].Add(line.Trim());
				}
			}
		
		}
	}
	
}

