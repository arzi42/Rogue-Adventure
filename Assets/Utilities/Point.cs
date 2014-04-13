using UnityEngine;
using System;

[System.Serializable]
public class Point
{
	public int x, y;
	
	public Point (int x, int y)
	{
		this.x = x;
		this.y = y;
	}
	
	public int sqrLength
	{
		get
		{
			return x * x + y * y;
		}
	}
	
	public float length
	{
		get
		{
			return Mathf.Sqrt(sqrLength);
		}
	}
		
	
	public override bool Equals (object obj)
	{
		if(!(obj is Point))
		{
			return false;
		}
		
		Point point = (Point) obj;
		
		if(point.x == x && point.y == y) return true;
		
		return false;
	}
	
	public bool Equals(Point p)
	{
		
		if(p == null)
			return false;
		
		return (x == p.x) && (y == p.y);
		
	}
	
	public void Unitify()
	{
		
		if(x != 0)
			x = (int) Mathf.Sign(x);
		
		if(y != 0)
			y = (int) Mathf.Sign(y);
		
	}
	
	public Point Copy()
	{
		return new Point(x, y);
	}
	
	public static Point operator+(Point p1, Point p2)
	{
		return new Point(p1.x + p2.x, p1.y + p2.y);
	}
	public static Point operator-(Point p1, Point p2)
	{
		return new Point(p1.x - p2.x, p1.y - p2.y);
	}
	
	public override int GetHashCode ()
	{
		return string.Format ("[Point: x={0}, y={1}]", x, y).GetHashCode();
	}
	
	public override string ToString ()
	{
		return string.Format ("[Point: x={0}, y={1}]", x, y);
	}
	
	public static bool operator ==(Point a, Point b)
	{
		
		if(System.Object.ReferenceEquals(a, b))
			return true;
		
		if((object)a == null || (object)b == null)
			return false;
		
		return (a.x == b.x && a.y == b.y);
	}
	
	public static bool operator !=(Point a, Point b)
	{
		
		return !(a == b);
		
	}
}

