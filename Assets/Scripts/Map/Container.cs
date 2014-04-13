using System;
using System.Collections.Generic;

namespace Map
{
	public class Container
	{
		private List<Item> items;
		
		private List<Container> subContainers;
		
		public Container ()
		{
			items = new List<Item>();
			
			subContainers = new List<Container>();
		}
		
		public void RemoveItem(Item item)
		{
			items.Remove(item);
		}
		
		public void AddItem(Item item)
		{
			item.SetLocation(this);
			
			items.Add(item);
		}
		
		public List<Item> GetItems()
		{
			List<Item> items = new List<Item>();
			
			items.AddRange(this.items);
			
			foreach(Container container in subContainers)
			{
				items.AddRange(container.GetItems());
			}
			
			return items;
		}
		
		public void AddContainer(Container container)
		{
			subContainers.Add(container);
		}
	}
}

