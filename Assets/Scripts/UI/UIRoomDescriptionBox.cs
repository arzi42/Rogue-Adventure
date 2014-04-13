using UnityEngine;
using System.Collections;

using Entities;
using Map;

public class UIRoomDescriptionBox : UITextbox
{
	private Adventurer adventurer;
	
	public UIRoomDescriptionBox(Vector2 position, Vector2 size, string title, Adventurer adventurer) : base(position, size, title, "", UIContainerStyle.Window)
	{
		this.adventurer = adventurer;
	}
	
	protected override void DoWindow (int id)
	{
		SetText(adventurer.GetWorldDataString() + "\n" + adventurer.GetCurrentRoom().GetDescription());
		
		base.DoWindow (id);
	}
	
	
	
	/*protected override void OnDraw ()
	{
		
		
		//GUI.skin.box.fontSize = 32;
		//GUI.skin.label.fontSize = 26;
		
		base.OnDraw();
	}*/
	
}

