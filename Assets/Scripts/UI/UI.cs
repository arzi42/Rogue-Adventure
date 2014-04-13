using UnityEngine;
using System.Collections;
using System.Collections.Generic;

using Map;
using Entities;
using Events;

public class UI : MonoBehaviour 
{
	public Material roomMaterial;
	
	public Texture2D background;
	
	private Adventurer adventurer;
	
	private UIContainer uiBase;
	
	private UITextbox roomDescription;
	
	private List<UIItem> currentRoomItems;
	
	private UIContainer dragged;
	
	private UIElement draggedOldParent;
	
	private UIMessages messages;
	
	private UICompass compass;
	
	private Vector2 dragOffset;
	
	private Vector2 draggedOriginalPosition;
	private Vector2 draggedOriginalPositionLocal;
	
	private UIInventory inventory;
	
	public GUISkin skin;
	
	public TextAsset intro;
	
	// Use this for initialization
	void Start () 
	{
		
		Dungeon dungeon = Dungeon.CreateInstance<Dungeon>();
		
		dungeon.AddIntroEvent(intro.text);
		
		Area[,] areas = dungeon.GetAreas(0);
		
		for(int x = 0; x < areas.GetLength(0); x++)
		{
			for(int y = 0; y < areas.GetLength(1); y++)
			{
				CreateArea(x, y, areas[x, y]);
			}
		}
		
		currentRoomItems = new List<UIItem>();
		
		adventurer = new Adventurer(dungeon);
		
		SetupUI();
		
		roomChanged = true;
		
		//RoomEntered(adventurer.GetCurrentRoom());
	}
	
	private void UpdateRoomItems(Room room)
	{
		foreach(UIItem item in roomDescription.GetChildrenOf<UIItem>())
		{
			Debug.Log("Removing item");
			item.RemoveParent();
		}
		
		currentRoomItems.Clear();
		
		int count = 0;
		foreach(Item item in room.GetItems())
		{
			if(item != null)
			{
				Debug.Log ("new item: " + item.description);
				
				float x = 40 + (145 * count);
				
				count ++;
				
				UIItem uiItem = new UIItem(new Vector2(x, 320), new Vector2(100, 70), item);
				
				
				uiItem.SetParent(roomDescription);
			}
		}
	}
	
	private void SetupUI()
	{
		uiBase = new UIContainer(new Vector2(0, 0), new Vector2(Screen.width, Screen.height), "");
		
		
		
		compass = new UICompass(new Vector2(15, Screen.height - 300), new Vector2(300, 300), "Compass", adventurer, RoomChanged);
		
		compass.SetParent(uiBase);
		
		
		
		//roomDescription = new UITextbox(new Vector2(0, 0), new Vector2 (350, 510), "Current Room", "");
		roomDescription = new UIRoomDescriptionBox(new Vector2(Screen.width - 550, 0), new Vector2 (550, Screen.height), "Current Room", adventurer);
		
		roomDescription.SetParent(uiBase);
		
		messages = new UIMessages(new Vector2(40, 230), new Vector2(470, 70));
		
		messages.SetParent(roomDescription);
		
		inventory = new UIInventory(new Vector2(40, 400), new Vector2(470, 250), "Inventory", adventurer);
		
		inventory.SetParent(roomDescription);
		
		//UITextbox testBox = new UITextbox(new Vector2(15, 15), new Vector2(350, 510), "TEST", "TEST");
		
		//testBox.SetParent(uiBase);
	}
	
	private void CreateArea(int areaX, int areaY, Area area)
	{
		Room[,] rooms = area.GetGrid();
		
		for(int x = 0; x < rooms.GetLength(0); x++)
		{
			for(int y = 0; y < rooms.GetLength(1); y++)
			{
				if(rooms[x, y].connected)
				{
					GameObject roomCube = GameObject.CreatePrimitive(PrimitiveType.Cube);
					
					roomCube.transform.position = new Vector3((areaX * area.width) + x, 0, (areaY * area.height + y) * -1);
					
					roomCube.transform.localScale = Vector3.one * 0.9f;
					
					roomCube.renderer.material = roomMaterial;
					
					rooms[x, y].debugCube = roomCube;
					
					roomCube.renderer.material.color = rooms[x, y].hilightColor;
					
				}
			}
		}
		
		foreach(Room room in rooms)
		{
			if(room.connected)
			{
				LineRenderer line = room.debugCube.AddComponent<LineRenderer>();
				
				line.SetWidth(0.1f, 0.1f);
				
				int count = 0;
				foreach(Door exit in room.GetExits().Values)
				{
//					Debug.Log (exit);
					Room other = exit.GetDestination(room);
					
					if(other != null && other.debugCube != null)
					{
					line.SetVertexCount(count + 2);
					
					line.SetPosition(count, room.debugCube.transform.position);
					line.SetPosition(count + 1, other.debugCube.transform.position);
					
					count += 2;
					}
				}
			}
		}
		
	}
	
	private string ParseRoomData(string data)
	{
		// All the important data are in brackets, parse while any brackets are found.
		
		GUISkin skin = GUI.skin;
		
		GUIStyle style = skin.label;
		
		
		
		while(data.Contains("["))
		{
			int startIndex = data.IndexOf("[");
			
			int endIndex = data.IndexOf("]");
			
			Debug.Log (data + " " + startIndex + " " + endIndex);
			
			string item = data.Substring(startIndex+1, endIndex - startIndex-1);
			
			Debug.Log (item);
			
			data = data.Replace("[" + item + "]", "");
		}
		
		return data;
	}
	
	private void DrawItem(Vector2 position, Item item)
	{
		Rect boxRect = new Rect(position.x, position.y, 200, 75);
		
		GUI.Box(boxRect, "");
		
		Rect labelRect = new Rect(position.x + 15, position.y + 10, 170, 40);
		
		GUI.Label(labelRect, item.description);
	}
	
	private bool roomChanged;
	
	private void RoomChanged(Room room)
	{
		roomChanged = true;
		
		
	}

	private void OptionSelected(StoryEvent storyEvent, int option)
	{
		storyEvent.SelectOption(option, adventurer);

		compass.visible = true;
		roomDescription.visible = true;
	}
	
	void Update()
	{
		if(roomChanged)
		{
			Room room = adventurer.GetCurrentRoom();
			UpdateRoomItems(room);
			
			
			StoryEvent storyEvent = room.GetEvent ();
			
			if(storyEvent != null)
			{
				
				UIEventWindow eventWindow = new UIEventWindow(new Vector2(Screen.width / 2  - 200, Screen.height / 2 - 300), new Vector2(400, 600), storyEvent, OptionSelected);
				
				eventWindow.SetParent(uiBase);
				
				compass.visible = false;
				roomDescription.visible = false;
			}
			
			
			roomChanged = false;
		}
		
		Vector2 mousePos = Input.mousePosition;
		
		mousePos.y = Screen.height - mousePos.y;
		
		if(Input.GetMouseButtonDown(0))
		{
			UIContainer clicked = uiBase.GetElementAt(mousePos);
			
			if(clicked != null && clicked.draggable)
			{
				draggedOldParent = clicked.StartDrag();
				
				dragged = clicked;
				
				dragOffset = dragged.GetScreenPosition() - mousePos;
				
				dragged.SetParent(roomDescription);
				
				Debug.Log(dragOffset);
				
				draggedOriginalPosition = dragged.GetPosition(true);
				
				
			}
		}
		
		if(Input.GetMouseButtonUp(0))
		{
			if(dragged != null)
			{
				UIContainer draggedOnto = uiBase.GetElementAt(mousePos);
				
				if(draggedOnto.Accept(dragged))
				{
					dragged.SetParent(draggedOnto);
					
				}
				else
				{
					
					
					Debug.Log ("Setting " + dragged + " parent to " + draggedOldParent + " " + draggedOriginalPosition);
					dragged.SetParent(draggedOldParent);
					
					dragged.SetPosition(draggedOriginalPosition);
				}
			
				dragged = null;
			}
		}
		
		if(dragged != null)
		{
			//Debug.Log(mousePos + dragOffset - draggedOldParent.GetPosition());
			Vector2 parentOffset = Vector2.zero;
			
			dragged.SetPosition(mousePos + dragOffset - roomDescription.GetPosition());
		}
	}

	void OnGUI()
	{
		GUI.skin = skin;
		
		GUI.DrawTexture(new Rect(0, 0, background.width, background.height), background);
		
		uiBase.Draw();
		
		//GUI.Window(100, new Rect(15, 15, 350, 510), TestWindow, "");
		
		
		/*Room room = adventurer.GetCurrentRoom();
		
		Dictionary<Direction, Door> currentExits = room.GetExits();
		
		Rect roomDescriptionLabel = new Rect(10, 10, Screen.width - 20, 200);
		
		string description = ParseRoomData(room.GetDescription());
		
		GUI.Label(roomDescriptionLabel, description);
		
		Vector2 compassPosition = new Vector2(120, Screen.height - 100);
		
		Vector2 itemPosition = new Vector2(20, 220);
		
		foreach(Item item in room.GetItems())
		{
			DrawItem(itemPosition, item);
			
			itemPosition.x += 210;
		}
		
		foreach(Direction direction in currentExits.Keys)
		{
			
			Point directionPoint = direction.ToPoint();
			
			Rect buttonRect = new Rect(compassPosition.x + 100 * directionPoint.x, compassPosition.y + 60 * directionPoint.y, 100, 30);
			
			if(currentExits[direction].Accessible())
			{
				if(GUI.Button(buttonRect, direction.ToString()))
				{
					adventurer.TravelTo(direction);
				}
			}
			else 
			{
				GUI.Label(buttonRect, direction + " (Blocked");
			}
			
		}
		*/
	}
}
