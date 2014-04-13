using System;
using UnityEngine;

namespace Map
{
	public abstract class Item
	{
		private string _description;
		
		private string _id;
		
		private Container myLocation;
		
		public Container GetLocation()
		{
			return myLocation;
		}
		
		public void SetLocation(Container container)
		{
			
		}
		
		public virtual bool usable
		{
			get
			{
				return false;
			}
		}
		
		public abstract bool Same(Item other);
		
		public virtual ItemUseResult Use(out string result) { result = "Nothing happens."; return ItemUseResult.DoNothing; }
		
		public virtual string description
		{
			get
			{
				return _description;
			}
		}
		
		public string id
		{
			get
			{
				return _id;
			}
		}
		
		public virtual bool isStatic 
		{
			get
			{
				return false;
			}
		}
		
		public Item(Container myLocation)
		{
			this.myLocation = myLocation;
		}
		
		
		
		public void Remove()
		{
			if(myLocation != null)
			{
				Debug.Log(myLocation + " removed item " + this);
				
				myLocation.RemoveItem(this);
			}
		}
		
		public abstract ItemUseResult UseOn(Item other, out string resultText);
		
			
		public Item (Container container, string id) : this(container)
		{
			_id = id;
			
		}
		
		public Item (Container container, string id, string description) : this (container, id)
		{
			_description = description;
		}
	}
}

