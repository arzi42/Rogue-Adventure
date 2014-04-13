using UnityEngine;
using System.Collections;

public class UITextbox : UIContainer
{
	private string text;
	
	public UITextbox(Vector2 position, Vector2 size, string title, string text, UIContainerStyle style = UIContainerStyle.Window) : base(position, size, title, style)
	{
		this.text = text;
	}	
	
	public void SetText(string text)
	{
		this.text = text;
	}
	
	
	
	protected override void DoWindow (int id)	
	{
		base.DoWindow(id);
		
		Rect rect = new Rect(50, 100, size.x - 100, size.y);
		
		/*rect.x += 15;
		rect.width -= 30;
		
		rect.y += 35;
		rect.height -= 50;*/
		
		GUI.Label(rect, text, "PlainText");
	}
	
}

