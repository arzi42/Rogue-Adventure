using UnityEngine;
using System.Collections;

public class UIContainer : UIElement
{
	protected Vector2 size;
	
	protected string title;
	
	private static int currentId;
	
	private UIContainerStyle myStyle;
	
	private int id;
	
	protected string customStyle;
	
	public UIContainerStyle GetStyle()
	{
		return myStyle;
	}
	public virtual bool draggable
	{
		get
		{
			return false;
		}
	}
	
	public virtual UIElement StartDrag()
	{
		return parent;
	}
	
	public virtual bool Accept(UIContainer uiContainer)
	{
		return false;
	}
	
	public Vector2 GetSize()
	{
		return size;
	}
	
	public UIContainer GetElementAt(Vector2 position)
	{
//		Debug.Log (this +": " + GetScreenRect() + " " + position);
		
		if(GetScreenRect().Contains(position))
		{
//			Debug.Log (this);
			UIContainer caught = null;
			foreach(UIElement child in children)
			{
				if(child is UIContainer)
				{
					caught = (child as UIContainer).GetElementAt(position);
					
					if(caught != null)
					{
						return caught;
					}
				}
				
			}
			
			return this;
		}
		
		return null;
	}
	
	public UIContainer(Vector2 position, Vector2 size, string title, UIContainerStyle style = UIContainerStyle.Empty) : base(position)
	{
		this.size = size;
		this.title = title;
		
		myStyle = style;
		
		id = currentId ++;
	}
	
	protected Rect GetScreenRect()
	{
		return new Rect(GetScreenPosition().x, GetScreenPosition().y, size.x, size.y);
	}
	
	protected Rect GetRect(bool world = false)
	{
		bool local = false;
		
		if(!world && parent != null && parent is UIContainer && (parent as UIContainer).myStyle == UIContainerStyle.Window)
		{
			local = true;
		}
			
		return new Rect(GetPosition(local).x, GetPosition(local).y, size.x, size.y);
	}
	
	protected override bool drawChildrenInWindow 
	{
		get 
		{
			return myStyle == UIContainerStyle.Window;
		}
	}
	protected override void OnDraw ()
	{
		//GUI.Box(GetRect(), title);
		
		//Debug.Log(title + ": " + GetRect() + " " + id);
		
		switch(myStyle)
		{
		case UIContainerStyle.Box:
			
			if(customStyle != null)
			{
				GUI.Box(GetRect(), title, customStyle);
			}
			else
			{
				GUI.Box(GetRect(), title);
			}
			DoWindow(id);
			break;
		case UIContainerStyle.Window:
			GUI.Window(id, GetRect(), DoWindow, title);	
			break;
		}
		
		
	}
	
	
	
}

