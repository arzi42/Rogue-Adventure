using UnityEngine;
using System;
using System.Collections.Generic;

namespace Map
{
	public class Floor: Grid<Area>
	{
		private Area[,] areas;
		
		private Area entranceArea;
		
		public Room GetEntranceRoom()
		{
			return entranceArea.GetNexus();
		}
		
		private void AddKey(List<Tool> toolsAlreadyAdded, List<Area> connectedAreas, Item key)
		{		
			if(key is Tool)
			{
				Tool keyAsTool = key as Tool;
				
				foreach(Tool tool in toolsAlreadyAdded)
				{
					// Same tool already planted, don't plant any more
					if(keyAsTool.Same(tool))
					{
						return;
					}
				}
				
				toolsAlreadyAdded.Add(keyAsTool);
				
			}
			
			
			connectedAreas.GetRandomElement().AddItem(key);
			
		}
		
		public Floor (Point size): base(size) 
		{
//			Debug.Log("Floor constructor");
			
			List<Point> criticalPath = GetPathBetween(PopRandomElement().position, PopRandomElement().position);
			
			entranceArea = Get(criticalPath[0]);
			
			Area previous = null;
			
//			Debug.Log ("Critical path length = " + criticalPath.Count);
			
			List<Area> connectedAreas = new List<Area>();
			
			List<Tool> toolsAlreadyAdded = new List<Tool>();
			
			// Connect critical path areas
			
			foreach(Point point in criticalPath)
			{
				Area area = Get (point);
				
				area.RandomizeNexus();
				
				Debug.Log("Area: "+ area.position);
				
				
				
				if(previous != null)
				{
					Item key = previous.Connect(area, GetDirectionBetween(previous.position, area.position));
					
					// Randomize the key to any of the previous connected areas
					
					AddKey(toolsAlreadyAdded, connectedAreas, key);
					
				}
				
				previous = area;
				
				connectedAreas.Add(area);
				
				
				
				
			}
			
			// Connect other areas
			
			
			
			foreach(Area area in GetAll())
			{
				area.RandomizeNexus();
				
				foreach(Area other in GetAdjacents(area.position))
				{
					Direction direction = GetDirectionBetween(area.position, other.position);
				
					Item key = area.Connect(other, direction);
					
					AddKey(toolsAlreadyAdded, connectedAreas, key);
				}
				
				connectedAreas.Add(area);
			}
			
			// Make paths from nexus to exits in areas
			
			foreach(Area area in connectedAreas)
			{
//				Debug.Log ("Making path to area " + area.position);
				area.ConnectNexusToConnectionRooms();
			}
			
			// Create dark (inaccessible) rooms and connect remaining rooms to nexus
			
			foreach(Area area in connectedAreas)
			{
//				Debug.Log ("Connecting remaining rooms " + area.position);
				
				area.DarkenRooms();
				
				area.ConnectRemainingRoomsToNexus();
				
				area.PlotKeys();
			}
			
			
		}
		
		protected override Area Create (int x, int y)
		{
//			Debug.Log("Creating area " + x + ", " + y);
			
			return new Area(new Point(4, 4), new Point(x, y));
		}
	}
}

