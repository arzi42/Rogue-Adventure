using System;

namespace Map
{
	public enum Direction
	{
		North, South, East, West
		
	}
	
	public static class DirectionEnumExtensions
	{
		public static Point ToPoint(this Direction direction)
		{
			switch(direction)
			{
				case Direction.North:
					return new Point(0, -1);
				case Direction.South:
					return new Point(0, 1);
				case Direction.East:
					return new Point(1, 0);
				case Direction.West:
					return new Point(-1, 0);
			}
			
			return new Point(0,0);
		}
		
		public static Direction Opposite(this Direction direction)
		{
			switch(direction)
			{
				case Direction.North:
					return Direction.South;
				case Direction.South:
					return Direction.North;
				case Direction.East:
					return Direction.West;
				case Direction.West:
					return Direction.East;
			}
			
			return direction;
		}
	}
	
}

