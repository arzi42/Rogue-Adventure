
using UnityEngine;

namespace Map
{
	public class Lock: Item
	{
		public bool locked;
		
		public override bool Same (Item other)
		{
			if(other is Lock)
			{
				return (other as Lock).type == type;
			}
			
			return false;
		}
		
		private string[] lockSuccessResults = new string[]
		{
			"The key fits the lock. The door is now open.",
			"The crystal and the barrier disappear in a flash.",
			"You pick your way through the crumble.",
			"You saw the trunk in half.",
			"You hear a sound in the distance."
		};
		
		private string[] lockFailResults = new string[]
		{
			"It doesn't seem to be the right key.",
			"The barrier doesn't react to that.",
			"That doesn't move the rockpile.",
			"The tree trunk can't be cut with that",
			"The stone door is impervious to that."
		};
		
		
		private string[] lockDescriptions = new string[] 
		{ 
			"A locked door",
			"A magical barrier",
			"A rock pile",
			"A tree trunk",
			"A stone door with no visible handle"
		};
		
		private string[] keyDescriptions = new string[]
		{
			"A rusty key",
			"A glowing crystal",
			"A pick axe",
			"A saw",
			"A button in the wall."
		};
		
		public override bool isStatic {
			get 
			{
				return true;
			}
		}
		
		private int type;
		
		public Item GetKey(Container location)
		{
			if(type < 2)
			{
				return new Key(location, "KEY#" + type, keyDescriptions[type], type);
			}
			else if(type == 4)
			{
				RoomObject button = new RoomObject(location, "BUTTON#" + type, keyDescriptions[type], RoomObjectType.DoorOpeningButton, this);
				
				button.SetSubType(type);
				
				return button;
			}
			else
			{
				return new Tool(location, "TOOL#" + type, keyDescriptions[type], type);
			}
		}
		
		public override string description
		{
			get
			{
				return lockDescriptions[type];
			}
		}
		
		public override ItemUseResult UseOn (Item other, out string resultText)
		{
			resultText = "This item cannot be used that way";
			
			return ItemUseResult.RejectItem;
		}
		
		public bool Open(int keyType, out string resultText)
		{
			if(keyType == type)
			{
				resultText = lockSuccessResults[type];
				
				locked = false;
				
				Remove();
				
				return true;
			}
			
			resultText = lockFailResults[type];
			
			return false;
		}
		
		public Lock (Container myLocation) : base(myLocation)
		{
			type = Random.Range(0, lockDescriptions.Length);
			
			locked = true;
			
		}
	}
}

