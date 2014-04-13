using System;
using System.Collections.Generic;

using UnityEngine;

namespace Map
{
	public abstract class Grid<T>
	{
		private T[,] grid;
		
		private Dictionary<T, Point> allElements;
		
		protected delegate void OnIterate(T element, int x, int y);
		
		public Grid(): this(new Point(0, 0)) {}
		
		public int width
		{
			get
			{
				return grid.GetLength(0);
			}
		}
		
		public Direction GetDirectionBetween(Point p1, Point p2)
		{
			int x = p1.x - p2.x;
			int y = p1.y - p2.y;
			
			if(x == 0)
			{
				if(y < 0)
				{
					return Direction.South;
				}
				else
				{
					return Direction.North;
				}
			}
			else
			{
				if(x < 0)
				{
					return Direction.East;
				}
				else
				{
					return Direction.West;
				}
			}
		}
		
		public T Get(Point p)
		{
			
			return grid[p.x, p.y];
		}
		
		public int height
		{
			get
			{
				return grid.GetLength(1);
			}
		}
		
		
		
		protected T PopRandomElement()
		{
			T element = new List<T>(allElements.Keys).GetRandomElement();
			
			allElements.Remove(element);
			
			return element;
		}
		
		protected Point GetPositionOf(T element)
		{
			return allElements[element];
		}
		
		protected List<T> GetAll()
		{
			return new List<T>(allElements.Keys);
		}
		
		public List<Point> GetPathBetween(Point startPosition, Point endPosition)
		{
			Point current = startPosition.Copy();
			
			List <Point> path = new List<Point>();
			
			path.Add(startPosition);
			
//			Debug.Log ("START = " + startPosition + " END = " + endPosition);
			
			int count = 0;
			
			while(current != endPosition && count++ < 100)
			{
				int x = current.x < endPosition.x ? current.x +1 : (current.x > endPosition.x ? current.x -1 : current.x);
				int y = current.y;
				
				if(x == current.x)
				{
					y = current.y = current.y < endPosition.y ? current.y +1 : (current.y > endPosition.y ? current.y -1 : current.y);
				}
				
				current = new Point(x, y);
				
//				Debug.Log("PATH[" + count + "]: " + current);
				
				path.Add(current.Copy());
			}
			
			return path;
		}
		
		public bool IsWithinBounds(Point p)
		{
			if(p.x < 0 || p.y < 0 || p.x >= width || p.y >= height)
			{
				return false;
			}
			
			return true;
		}
		
		public List<T> GetAdjacents(Point p)
		{
			List<T> adjacents = new List<T>();
			
			for(int x = -1; x < 2; x++)
			{
				for(int y = -1; y < 2; y++)
				{
					if((x == 0 && y == 0) || (x != 0 && y != 0))
					{
						continue;
					}
					
					Point point = new Point(p.x + x, p.y + y);
					
					if(IsWithinBounds(point))
					{
						adjacents.Add(Get (point));
					}
					
				}
			}
			
			return adjacents;
		}
		
		public T GetRandomAdjacent(Point p)
		{
			return GetAdjacents(p).GetRandomElement();
		}
		
		protected void IterateAll(OnIterate iterateAction)
		{
			for(int x = 0; x < width; x++)
			{
				for(int y = 0; y < height; y++)
				{
					iterateAction(grid[x, y], x, y);
				}
			}
		}
		
		public Grid(Point size)
		{
			grid = new T[size.x, size.y];
			
			allElements = new Dictionary<T, Point>();
			
			for(int x = 0; x < size.x; x++)
			{
				for(int y = 0; y < size.y; y++)
				{
					T element = Create(x, y);
					
					grid[x, y] = element;
					
					allElements.Add(element, new Point(x, y));
				}
			}
		}
		
		public T[,] GetGrid()
		{
			return grid;
		}
		
		protected abstract T Create(int x, int y);
		
	}
}

