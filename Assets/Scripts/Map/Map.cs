using UnityEngine;
using System.Collections;

namespace Map
{
	public class Dungeon : ScriptableObject
	{
		private Floor[] floors;
		
		private int numberOfFloors = 1;
		
		private Point areasPerFloor = new Point(2, 2);
		
		private int currentFloor;
		
		public void AddIntroEvent(string source)
		{
			floors[0].GetEntranceRoom().SetEvent(source);
		}
		
		void OnEnable()
		{
			if(floors == null)
			{
				floors = new Floor[numberOfFloors];
				
				for(int i = 0; i < numberOfFloors; i++)
				{
					floors[i] = new Floor(areasPerFloor);
				}
			}
		}
		
		public Room GetFirstRoom()
		{
			return floors[0].GetEntranceRoom();
		}
		
		public Area[,] GetAreas(int floor)
		{
			return floors[floor].GetGrid();
		}
		
		
	}
}

