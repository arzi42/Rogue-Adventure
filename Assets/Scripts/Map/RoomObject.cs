using System;

namespace Map
{
	public class RoomObject: Item
	{
		private RoomObjectType type;
		
		private Item target;
		
		private int subType;
		
		private bool disabled;
		
		public override bool Same (Item other)
		{
			if(other is RoomObject)
			{
				RoomObject otherObject = other as RoomObject;
				
				return otherObject.type == type && otherObject.subType == subType;
			}
			
			return false;
		}
		
		
		public RoomObject (Container location, string id, string description, RoomObjectType type, Item target): base(location, id, description)
		{
			this.type = type;
			
			this.target = target;
		}
		
		public void SetSubType(int subType)
		{
			this.subType = subType;
		}
		
		public override bool isStatic {
			get 
			{
				return true;
			}
		}
		
		public override bool usable {
			get 
			{
				return !disabled;
			}
		}
		
		public override ItemUseResult UseOn (Item other, out string resultText)
		{
			resultText = "These items cannot be used together.";
			
			return ItemUseResult.RejectItem;
			
		}
		
		public override ItemUseResult Use (out string result)
		{
			switch(type)
			{
			case RoomObjectType.DoorOpeningButton:
				
				Lock targetLock = target as Lock;
				
				targetLock.Open(subType, out result);
				
				disabled = true;
				
				return ItemUseResult.DoNothing;
			}
			
			result = "Nothing happens.";
			
			return ItemUseResult.DoNothing;
		}
	}
}

