using System;

namespace Map
{
	public class Door: Container
	{
		private Room destination1;
		private Room destination2;
		
		private Lock myLock;
		
		public Lock GetLock()
		{
			return myLock;
		}
		
		public Room GetDestination(Room fromRoom)
		{
			if(fromRoom == destination1)
				return destination2;
			
			if(fromRoom == destination2)
				return destination1;
			
			return null;
		}
		
		public string GetDescription()
		{
			if(myLock != null)
			{
				return myLock.description;
			}
			
			return "";
		}
		
		public bool Accessible()
		{
			return myLock == null || !myLock.locked;
		}
		
		public bool LeadsTo(Room room)
		{
			return room == destination1 || room == destination2;
		}
		
		public Room GoThrough(Room fromRoom)
		{
			Room destination = GetDestination(fromRoom);
			
			destination.Entered();
			
			return destination;
		}
		
		public Door (Room destination1, Room destination2, bool locked)
		{
			this.destination1 = destination1;
			this.destination2 = destination2;
			
			if(locked)
			{
				myLock = new Lock(this);
				
				AddItem(myLock);
			}
			
		}
		
		
		
	}
}

