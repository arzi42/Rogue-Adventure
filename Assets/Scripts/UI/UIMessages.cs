using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class UIMessages : UIContainer
{
	private List<string> messages;
	
	private static UIMessages instance;
	
	private static int maxLines = 3;
	
	public static void Show(string message)
	{
		instance.messages.Add(message);
		
		if(instance.messages.Count > maxLines)
		{
			instance.messages.RemoveAt(0);
		}
	}
	
	public UIMessages(Vector2 position, Vector2 size) : base(position, size, "", UIContainerStyle.Empty)
	{
		messages = new List<string>();
		
		instance = this;
	}
	
	protected override void OnDraw ()
	{
		GUI.Label(GetRect(false), string.Join("\n", messages.ToArray()), "OutlineText");
	}
	
}

