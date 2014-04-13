using System;

namespace Map
{
	public class Tool: Item
	{
		private int type;
		
		public Tool (Container location, string id, string description, int type) : base (location, id, description)
		{
			this.type = type;
		}
		
		public override bool Same (Item other)
		{
			if(other is Tool)
			{
				return (other as Tool).type == type;
			}
			
			return false;
		}
		
		public override ItemUseResult UseOn (Item other, out string resultText)
		{
			//UIMessages.Show(this + " used on " + other);
			
			Lock otherLock = other as Lock;
			
			resultText = "These items can't be used together.";
			
			if(otherLock != null && otherLock.Open(type, out resultText))
			{
				return ItemUseResult.DestroyThisItem;
			}
			
			return ItemUseResult.RejectItem;
		}
	}
}

