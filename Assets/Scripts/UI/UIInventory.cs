using UnityEngine;
using System.Collections;
using System.Collections.Generic;

using Entities;

public class UIInventory : UIContainer
{
	private List<UIItem> items;
	
	private Adventurer adventurer;
	
	public UIInventory(Vector2 position, Vector2 size, string title, Adventurer adventurer) : base(position, size, title, UIContainerStyle.Box)
	{
		items = new List<UIItem>();
		
		this.adventurer = adventurer;
	}
	
	public void ItemRemoved(UIItem item)
	{
		Debug.Log ("Item removed");
		items.Remove(item);
		
		ArrangeItems();
	}
	
	private void ArrangeItems()
	{
		for(int i = 0; i < items.Count; i++)
		{
			UIItem item = items[i];
			
			Vector2 pos = new Vector2(15 + (item.GetSize().x + 15) * (i % 2), 45 + (i / 2) * (item.GetSize().y+20));
			
			item.SetPosition(pos);
		}
	}
	public override bool Accept (UIContainer uiContainer)
	{
		UIItem item = uiContainer as UIItem;
		if(item != null && !items.Contains(item))
		{	
			adventurer.Pickup(item.GetItem());
			
			UIMessages.Show("Picked up " + item.GetItem().description.ToLower());
			
			UIItem same = null;
			
			foreach(UIItem oldItem in items)
			{
				if(oldItem.GetItem().Same(item.GetItem()))
				{
					same = oldItem;
					break;
				}
			}
			
			if(same == null)
			{
				item.SetToInventory(this);
			
				items.Add(item);
			}
			else
			{
				item.RemoveParent();
				
				same.AddCount(1);
			}
			
			ArrangeItems();
			
			return true;
		}
		
		return false;
	}
	
	
}

