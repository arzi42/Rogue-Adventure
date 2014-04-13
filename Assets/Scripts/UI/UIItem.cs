using UnityEngine;
using System.Collections;

using Map;

public class UIItem : UIContainer
{
	private Item item;
	
	private int count = 1;
	
	private UIInventory myInventory;
	
	public Item GetItem()
	{
		return item;
	}
	
	public override bool draggable {
		get 
		{
			return !item.isStatic;
		}
	}
	
	protected override bool alwaysOnTop {
		get {
			return true;
		}
	}
	
	public void AddCount(int amount)
	{
		count += amount;
	}
	
	public void SetToInventory(UIInventory inventory)
	{
		Debug.Log ("Item picked up, inventory = " + inventory);
		myInventory = inventory;
	}
	
	public override UIElement StartDrag ()
	{
		Debug.Log ("Start Drag: item");
		
		return base.StartDrag ();
	}
	
	public UIItem(Vector2 position, Vector2 size, Item item) : base(position, size, "Item", UIContainerStyle.Box)
	{
		this.item = item;
		
		customStyle = "Item";
	}
	
	public override void RemoveParent ()
	{
		base.RemoveParent ();
		
		if(myInventory != null)
		{
			myInventory.ItemRemoved(this);
		}
	}
	
	public void Consumed()
	{
		count --;
		
		if(count == 0)
		{
			RemoveParent();
		}
	}
	
	public override bool Accept (UIContainer uiContainer)
	{
		UIItem uiItem = uiContainer as UIItem;
		
		if(uiItem != null)
		{
			if(myInventory != null && uiItem.GetItem ().Same(item))
			{
				return myInventory.Accept(uiContainer);
			}
			
			
			Debug.Log ("Item used, inventory = " + myInventory);
			
			string resultText;
			
			ItemUseResult result = uiItem.item.UseOn(item, out resultText);
			
			UIMessages.Show(resultText);
			
			switch(result)
			{
			case ItemUseResult.RejectItem:
				
				Debug.Log("Reject item");
				
				return false;
				
			case ItemUseResult.DestroyBothItems:
				
				Debug.Log("Destroy both");
				
				Consumed();
				
				uiItem.Consumed();
				
				return uiItem.count == 0;
	
			case ItemUseResult.DestroyThisItem:
				
				Debug.Log("Destroy this item");
				
				Consumed();
				
				//uiItem.SetParent(uiItem.myInventory);
				
				return false;
				
			case ItemUseResult.DestroyUsedItem:
				
				Debug.Log("Destroy used item");
				
				uiItem.Consumed();
				
				return uiItem.count == 0;
			}
		}
		
		return false;
	}
	
	protected override void OnDraw ()
	{
		
		GUI.skin.box.fontSize = 20;
		GUI.skin.label.fontSize = 18;
		
		if(count == 1)
		{
			title = "Item";
		}
		else
		{
			title = "Item (x" + count + ")";
		}
		
		
		base.OnDraw ();
		
		Rect rect = GetRect();
		
//		Debug.Log (parent + " " + rect);
		
		GUI.Label(rect, item.description, "ItemText");
		
		if(item.usable)
		{
			if(GUI.Button(new Rect(GetPosition().x + GetRect().width / 2 - 30, GetPosition().y + 40, 60, 30), "Use", "ShortButton"))
			{
				string result;
				item.Use(out result);
				
				UIMessages.Show(result);
			}
		}
	}
	
}

