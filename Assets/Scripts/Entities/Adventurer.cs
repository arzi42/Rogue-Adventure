using System;
using System.Collections.Generic;

using Map;

namespace Entities
{
	public class Adventurer
	{
		private World world;
		
		private Room currentRoom;
		
		private List<Item> inventory;

		private List<string> traits;
		
		public string GetWorldDataString()
		{
			return world.GetTimeString();
		}
		
		public void Pickup(Item item)
		{
			currentRoom.RemoveItem(item);
			
			inventory.Add(item);
		}

		public void AddTrait(string trait)
		{
			if(traits == null)
			{
				traits = new List<string>();
			}

			if(!traits.Contains(trait))
			{
				traits.Add(trait);
			}
		}
		
		public bool HasTrait(string trait)
		{
			if(traits != null)
			{
				return traits.Contains(trait);
			}

			return false;
		}

		public void RemoveTrait(string trait)
		{
			if(traits != null && traits.Contains(trait))
			{
				traits.Remove(trait);
			}
		}

		public Room GetCurrentRoom()
		{
			return currentRoom;
		}
		
		public Adventurer (Dungeon dungeon)
		{
			inventory = new List<Item>();
			
			currentRoom = dungeon.GetFirstRoom();
			
			currentRoom.Entered();
			
			world = new World();
		}
		
		public void TravelTo(Direction direction)
		{
			world.TickClock(5);
			
			currentRoom = currentRoom.TravelTo(direction);
		}
	}
}

