using UnityEngine;
using System.Collections.Generic;

public static class ExtensionMethods
{
	public static string Capitalize(this string str)
	{
		return char.ToUpper(str[0]) + str.Substring(1);
	}
	
	public static T GetRandomElement<T>(this List<T> list) 
	{
		return list[Random.Range(0, list.Count)];
	}
	
	public static T PopRandomElement<T>(this List<T> list) 
	{
		object item = list[Random.Range(0, list.Count)];
		
		list.Remove((T)item);
		
		return (T) item;
	}
	
	public static T FindComponent<T>(this Transform transform) where T: Component
	{
		
		Component target = transform.GetComponent<T>();
		
		if(target == null)
		{
			foreach(Transform child in transform)
			{
				
				target = child.FindComponent<T>();
				
				if(target != null)
					return (T) target;
			}
			
			
		}
		
		return (T) target;
		
	}
	
	public static T[] FindComponents<T>(this Transform transform) where T: Component
	{
		
		List<T> targets = new List<T>();
		
		Component target = transform.GetComponent<T>();
		
		if(target != null)
		{
			targets.Add((T)target);
		}
		
		foreach(Transform child in transform)
		{
			
			targets.AddRange(child.FindComponents<T>());
			
			
		}
			
		return targets.ToArray();
		
	}
	
	
	
	public static Transform FindByName(this Transform transform, string name)
	{
		if(transform.name.Contains(name))
			 return transform;
		
		return RecursiveFind(transform, name);
	}
	
	private static Transform RecursiveFind(Transform transform, string name)
	{	
		//Debug.Log (transform.name);
		foreach(Transform child in transform)
		{
			if(child.name.Contains(name))
			{
				return child;
			} else
			{
				Transform t = RecursiveFind(child, name);
				if(t != null)
					return t;
			}
		}
		
		return null;
	}
	
	public static void ChangeLayersRecursively(this GameObject gameObject, string fromLayer, string toLayer)
	{
		
		if(LayerMask.LayerToName(gameObject.layer) == fromLayer)
			gameObject.layer = LayerMask.NameToLayer(toLayer);
		
		foreach(Transform child in gameObject.transform)
		{
			child.gameObject.ChangeLayersRecursively(fromLayer, toLayer);
		}
		
	}
	
	
	public static void PlayAnimationInChildren(this Transform transform, string animationName, string[] randomEndings = null)
	{
		
		string ending = "";
		
		if(randomEndings != null)
		{
			ending = randomEndings[Random.Range(0, randomEndings.Length)];	
		}
		
		foreach(Transform child in transform)
		{
			if(child.animation != null)
			{
				string randomizedName = child.name + "_" + animationName + ending;
				
				if(child.animation.isPlaying)
					child.animation.Rewind();
				
				child.animation.Play(randomizedName);
				
			}
			
		}
		
	}
}
	
