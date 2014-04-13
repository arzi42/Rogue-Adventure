using UnityEngine;
using System.Collections;

using Events;

public class UIEventWindow : UITextbox
{
	private string[] options;

	private StoryEvent myEvent;

	public delegate void OptionSelected(StoryEvent storyEvent, int option);

	private OptionSelected optionSelected;

	public UIEventWindow(Vector2 position, Vector2 size, StoryEvent myEvent, OptionSelected optionSelected) : base(position, size, "Event", myEvent.RunEvent(), UIContainerStyle.Window)
	{
		this.myEvent = myEvent;

		this.optionSelected = optionSelected;

		options = myEvent.GetOptions();
	}
	
	protected override void DoWindow (int id)
	{
		base.DoWindow (id);
		
		int height = options.Length * 45 + 50;
		
		for(int i = 0; i < options.Length; i++)
		{
			float y = size.y - height + i * 45;
			
			Rect rect = new Rect(20, y, size.x - 40, 40);
			
			if(GUI.Button(rect, options[i]))
			{
				if(optionSelected != null)
				{
					optionSelected(myEvent, i);

					RemoveParent();
				}
			}
		}
	}
	
}

