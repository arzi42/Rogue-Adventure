using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace Map
{
	public class Area: Grid<Room>
	{
		public Point position;
		
		private Point nexus;
		
		private List<Point> connectionRooms;
		
		private List<Item> keys;
		
		private List<Room> possibleKeyRooms;
		
		private string[] roomDescriptions = new string[] 
		{
			"This is a plain room with rock walls.",
			"The musty old room smells dank.",
			"The room is lit by sunlight bursting in from a small crack in the ceiling.",
			"Small streams of water tricke down the walls, cooling the air.",
			"There are remnants of an old camp in this rocky cavern.",
			"An outlandish, worn statue adorns this ancient hall.",
			"Walls of this room are filled with a blanket of moss.",
			"Broken pillars lay on the ground of this room."
		};
		
		private string GetRandomDescription()
		{
			return roomDescriptions[Random.Range(0, roomDescriptions.Length)];
		}
		
		public Area(Point size, Point position): base(size) 
		{
			Debug.Log ("Area Position = " + position);
			this.position = position;
			
			connectionRooms = new List<Point>();
			
			keys = new List<Item>();
			
			possibleKeyRooms = new List<Room>();
		}
		
		
		public override int GetHashCode ()
		{
			return position.GetHashCode ();
		}
		
		protected override Room Create (int x, int y)
		{
			return new Room(this, new Point(x, y), GetRandomDescription());
		}
		
		/* 
		 * Nexus is the central room every room in the area is connected to. In the start and end areas this is the start / goal room.
		 * 
		 * It cannot be on any of the edges.
		 * 
		 */
		
		public void RandomizeNexus()
		{
			int x = Random.Range(1, width - 1);
			int y = Random.Range(1, height - 1);
			
			nexus = new Point(x, y);
			
			Get(nexus).Highlight(new Color(0, 0, 1, 0.5f));
			
			
		}
		
		/*private Room ConnectedTo(Area other, Point atPosition, Direction direction, Room otherRoom, Door door)
		{
			
			
			return room;
		}*/
		
		private Point GetPointAtEdge(Point pos, Direction direction)
		{
			Point position = new Point(0,0);
			
			switch(direction)
			{
				case Direction.North:
					position = new Point(pos.x < 0 ? Random.Range(0, width) : pos.x, 0);
					break;
				case Direction.South:
					position = new Point(pos.x < 0 ? Random.Range(0, width) : pos.x, height-1);
					break;
				case Direction.East:
					position = new Point(width -1, pos.y < 0 ? Random.Range(0, height) : pos.y);
					break;
				case Direction.West:
					position = new Point(0, pos.y < 0 ? Random.Range(0, height) : pos.y);
					break;
			}
			
			return position;
		}
		
		public Room GetNexus()
		{
			return Get (nexus);
		}
		
		public void ConnectNexusToConnectionRooms()
		{
			Debug.Log(connectionRooms.Count);
			
			foreach(Point connection in connectionRooms)
			{
				List<Point> path = GetPathBetween(nexus, connection);
				
				Point previous = null;
				
				foreach(Point p in path)
				{
//					Debug.Log ("Critical Path: " + p);
					Room room = Get (p);
					
					if(nexus != p)
					{
						room.Highlight(new Color(0.5f, 0.5f, 0, 0.5f));
					}
					
					
					if(previous != null)
					{
						Get(previous).ConnectTo(room);
					}
					
					previous = p;
				}
			}
		}
		
		
		
		public void DarkenRooms()
		{
			int darkRoomCount = 2;
			
			List<Room> unconnectedRooms = new List<Room>();
			
			
			
			foreach(Room room in GetAll())
			{
				if(!room.connected)
				{
					unconnectedRooms.Add(room);
				}
			}
			
			//Debug.Log ("Area " + position + " has " + unconnectedRooms.Count + " unconnected rooms.");
			
			for(int i = 0; i < darkRoomCount && unconnectedRooms.Count > 0;i++)
			{
				Room room = unconnectedRooms.PopRandomElement();
				//Debug.Log ("Darkening " + room);
				
				room.SetDark();
			}
		}
		
		public void PlotKeys()
		{
			while(keys.Count > 0)
			{
				Room room = possibleKeyRooms.PopRandomElement();
				
				room.AddItem(keys.PopRandomElement());
				
				room.Highlight(new Color(1, 1, 1, 0.5f));
			}
		}
		
		public void ConnectRemainingRoomsToNexus()
		{
			IterateAll(ConnectRoomToNexus);
		}
		
		private void ConnectRoomToNexus(Room room, int x, int y)
		{
			if(room.connected || room.dark)
			{
				return;
			}
			
			possibleKeyRooms.Add(room);
			
			List<Room> adjacents = GetAdjacents(new Point(x, y));
			
			List<Room> connectedAdjacents = new List<Room>();
			
			foreach(Room adjacent in adjacents)
			{
				if(adjacent.connected)
				{
					connectedAdjacents.Add(adjacent);
				}
			}
			
			if(connectedAdjacents.Count > 0)
			{
				Room adjacent = connectedAdjacents.GetRandomElement();
				
				room.ConnectTo(adjacent);
				
			}
			else
			{
				List<Point> path = GetPathBetween(new Point(x, y), nexus);
				
				Room previous = null;
				
				foreach(Point p in path)
				{
					Room next = Get (p);
					
					if(previous != null)
					{
						bool wasConnected = next.connected;
						
						previous.ConnectTo(next);
						
						if(wasConnected)
						{
							break;
						}
						else
						{
							possibleKeyRooms.Add(next);
						}
					}
					
					previous = next;
				}
				
				/*int nx = x < nexus.x ? 1 : (x > nexus.x ? -1 : 0);
				
				int ny = 0;
				
				if(nx == 0)
				{
					ny = y < nexus.y ? 1 : (y > nexus.y ? -1 : 0);
				}
				
				Point pointTowardsNexus = new Point(x + nx, y + ny);
				
				Room adjacent = Get(pointTowardsNexus);
				
				room.ConnectTo(adjacent);*/
			}
		}
		
		public void AddItem(Item key)
		{
			keys.Add(key);
		}
		
		public Item Connect(Area other, Direction direction)
		{
			
			Point position = GetPointAtEdge(new Point(-1, -1), direction);
			
			Room room = Get (position);
			
			Point otherPosition = other.GetPointAtEdge(position, direction.Opposite());
			
			Room otherRoom = other.Get (otherPosition);
			
			// Area connections are always locked
			
			Door door = new Door(room, otherRoom, true);
			
			// Get the key to the door
			
			Item key = door.GetLock().GetKey(null);
			
			room.AddConnection(direction, otherRoom, door);
			otherRoom.AddConnection(direction.Opposite(), room, door);
			
			other.connectionRooms.Add(otherPosition);
			connectionRooms.Add(position);
			
			return key;
		
		}
	}
}
