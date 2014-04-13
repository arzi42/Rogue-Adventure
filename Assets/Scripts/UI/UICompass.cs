using UnityEngine;
using Entities;
using Map;

using System.Collections;
using System.Collections.Generic;

public class UICompass : UIContainer
{
	private Adventurer adventurer;
	
	private Vector2 buttonSize;
	
	public delegate void RoomChanged(Room room);
	
	private RoomChanged roomChanged;
	
	public UICompass(Vector2 position, Vector2 size, string title, Adventurer adventurer, RoomChanged roomChanged) : base(position, size, title, UIContainerStyle.Window)
	{
		this.adventurer = adventurer;
		
		this.roomChanged = roomChanged;
		
		buttonSize = new Vector2(size.x * 0.33f, size.y * 0.20f);
		
		
	}
	
	protected override void DoWindow (int id)
	{
		//GUI.skin.box.fontSize = 26;
		
		base.DoWindow(id);
		
		Room room = adventurer.GetCurrentRoom();
		
		Dictionary<Direction, Door> currentExits = room.GetExits();
		
		Vector2 compassPosition = size * 0.5f;
		
		compassPosition.y += 15;
		
		foreach(Direction direction in currentExits.Keys)
		{
			
			Point directionPoint = direction.ToPoint();
			
			Rect buttonRect = new Rect(	compassPosition.x +(buttonSize.x * 0.75f) * directionPoint.x, 
										compassPosition.y +(buttonSize.y * 0.75f) * directionPoint.y, 
										buttonSize.x, buttonSize.y);
			
			buttonRect.x -= buttonSize.x / 2;
			buttonRect.y -= buttonSize.y / 2;
			
			if(currentExits[direction].Accessible())
			{
				if(GUI.Button(buttonRect, direction.ToString(), "ShortButton"))
				{
					adventurer.TravelTo(direction);
					
					UIMessages.Show("Went " + direction);
					
					if(roomChanged != null)
					{
						roomChanged(adventurer.GetCurrentRoom());
					}
				}
			}
			else 
			{
				GUI.Label(buttonRect, "", "WaxSeal");
			}
			
		}
	}
	
}

