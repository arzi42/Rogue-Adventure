using System;

namespace Map
{
	public class Key: Item
	{
		private int type;
		
		public override bool Same (Item other)
		{
			if(other is Key)
			{
				return (other as Key).type == type;
			}
			
			return false;
		}
		
		public int GetKeyType()
		{
			return type;
		}
		
		public Key (Container container, string id, string description, int type) : base(container, id, description)
		{
			this.type = type;
		}
		
		public override ItemUseResult UseOn (Item other, out string resultText)
		{
			Lock lockItem = other as Lock;
			
			resultText = "This item cannot be used that way.";
			
			if(lockItem != null && lockItem.Open(type, out resultText))
			{
				
				return ItemUseResult.DestroyBothItems;
			}
			
			return ItemUseResult.RejectItem;
		}
		
	}
}

