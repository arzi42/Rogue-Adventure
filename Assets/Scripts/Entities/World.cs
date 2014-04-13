using System;

namespace Entities
{
	public class World
	{
		// In minutes, 24 * 60 minutes = 1 day
		private int time;
	
		public string GetTimeString()
		{
			int hours = time / 60;
			int minutes = time % 60;
			
			bool pm = hours >= 12;
			
			if(hours == 0)
			{
				hours = 12;
			}
			else if(hours > 12)
			{
				hours -= 12;
			}
			
			return hours + " hours, " + minutes + " minutes " + (pm ? "PM":"AM");
			
		}
		
		public void TickClock(int minutes)
		{
			this.time += minutes;
		}
		
		public World ()
		{
			// Start the game at 1 PM
			
			time = 13 * 60;
		}
		
		
	}
}

