using System;
using UnityEngine;
using System.Collections.Generic;

using Events;

namespace Map
{
	public class Room: Container
	{
		private bool _dark;
		
		public bool dark
		{
			get
			{
				return _dark;
			}
			
		}
		
		private Dictionary<Direction, Door> exits;
		
		public Color hilightColor = new Color(0, 1, 0, 0.5f);
		
		private Area myArea;
		
		private Point position;
		
		public GameObject debugCube;
		
		private string description;
		
		private StoryEvent roomEvent;
		
		public void SetEvent(string source)
		{
			roomEvent = new StoryEvent(source);
		}
		
		public StoryEvent GetEvent()
		{
			StoryEvent storyEvent = roomEvent;

			if(roomEvent != null && roomEvent.once)
			{
				roomEvent = null;
			}

			return storyEvent;
		}

		public string GetDescription()
		{
			string fullDescription = description;
			
			foreach(Direction exit in exits.Keys)
			{
				Door door = exits[exit];
				
				if(!door.Accessible())
				{
					fullDescription += "\n" +door.GetDescription() + " blocks your way " + exit.ToString().ToLower() + ".";	
				}
			}
			
			return fullDescription;
		}
		
		public void SetDark()
		{
			_dark = true;
		}
		
		public void Entered()
		{
			debugCube.renderer.material.color = Color.red;
			
			Debug.Log("My area: " + myArea.position);
		}
		
		public Room TravelTo(Direction direction)
		{
			debugCube.renderer.material.color = hilightColor;
			
			return exits[direction].GoThrough(this);
		}
		
		public void Highlight(Color color)
		{
			hilightColor = color;
		}
		
		public void ConnectTo(Room other)
		{
			
			Direction direction = GetDirectionTo(other);
			
//			Debug.Log ("Connecting " + position + " to " + direction + " and " + other.position + " to " + direction.Opposite());
			
			Door door = new Door(this, other, false);
			
			AddConnection(direction, other, door);
			
			other.AddConnection(direction.Opposite(), this, door);
			
		}
		
		
		
		public Door AddConnection(Direction direction, Room other, Door door)
		{
			if(exits.ContainsKey(direction) && !exits[direction].LeadsTo(other))
			{
				Debug.LogError("Two exits leading to different rooms created!");
			}
			
			if(!exits.ContainsKey(direction))
			{
				if(door == null)
				{
					Debug.LogError("Door = null!");
				}
				
				exits.Add(direction, door);
				
				AddContainer(door);
				
				return door;
			}
			else
			{
				Debug.LogWarning("Room " + position + " in area " + myArea.position + " tried to be connected twice in " + direction);
			}
			
			return null;
		}
		
		public bool connected
		{
			get
			{
				return exits.Count > 0;
			}
		}
		
		
		
		public Room (Area area, Point position, string description) : base()
		{
			exits = new Dictionary<Direction, Door>();
			
			this.description = description;
			
			myArea = area;
			
			this.position = position;
			
		}
		
		public Direction GetDirectionTo(Room other)
		{
			return myArea.GetDirectionBetween(position, other.position);
		}
		
		public Dictionary<Direction, Door> GetExits()
		{
			return exits;
		}
		
		public override string ToString ()
		{
			return position.ToString();
		}
			
	}
}

