using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public abstract class UIElement
{
	protected UIElement parent;
	
	private Vector2 position;
	
	protected List<UIElement> children;
	
	private bool _visible;
	
	public bool visible
	{
		get
		{
			return _visible;
		}
		set
		{
			_visible = value;
		}
	}
	
	public UIElement(Vector2 position)
	{
		this.position = position;
		
		children = new List<UIElement>();
		
		_visible = true;
	}
	
	
	
	public void SetParent(UIElement parent)
	{
		if(this.parent != null)
		{
			this.parent.children.Remove(this);
		}
		
		this.parent = parent;
		
		parent.children.Add(this);
	}
	
	public void SetPosition(Vector2 position)
	{
		this.position = position;
	}
	
	public Vector2 GetScreenPosition()
	{
		Vector2 position = this.position;
		
		if(parent != null)
		{
			position += parent.GetScreenPosition();
		}
		
		return position;
	}
	
	public Vector2 GetPosition(bool local = false)
	{
		Vector2 position = this.position;
		
		if(parent != null && !local)
		{
			if(!(parent is UIContainer) || (parent as UIContainer).GetStyle() != UIContainerStyle.Window)
			{
				position += parent.GetPosition();
			}
		}
		
		return position;
	}
	
	public List<T> GetChildrenOf<T>() where T: UIElement
	{
		List<T> neededChildren = new List<T>();
		
		foreach(UIElement child in children)
		{
			if(child.GetType() == typeof(T))
			{
				neededChildren.Add((T) child);
			}
		}
		
		return neededChildren;
	}
	
	protected virtual bool drawChildrenInWindow
	{
		get
		{
			return false;
		}
	}
	
	protected virtual bool alwaysOnTop
	{
		get
		{
			return false;
		}
	}
	
	private void DrawChildren()
	{
		foreach(UIElement child in children)
		{
			if(!child.alwaysOnTop)
			{
				child.Draw();
			}
		}
		
		foreach(UIElement child in children)
		{
			if(child.alwaysOnTop)
			{
				child.Draw();
			}
		}
	}
	
	public void Draw()
	{
		if(_visible)
		{
			OnDraw();
			
			if(!drawChildrenInWindow)
			{
				DrawChildren();
			}
		}
	}

	protected virtual void DoWindow(int id) 
	{
		if(_visible)
		{
			if(drawChildrenInWindow)
			{
				DrawChildren();
			}
		}
	}
	
	public virtual void RemoveParent()
	{
		if(parent != null)
		{
			parent.children.Remove(this);
		
			parent = null;
		}
	}
	
	protected abstract void OnDraw();
	
}

