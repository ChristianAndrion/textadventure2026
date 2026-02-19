using System.Collections.Generic;
using UnityEngine;

public class NavagationManager : MonoBehaviour
{
    public static NavagationManager instance;

    public Room startingRoom;
    public Room currentRoom;
    public Exit toKeyNorth;
    public delegate void Restart(); //Custom event
    public event Restart onRestart;

    private Dictionary<string, Room> exitRooms = new Dictionary<string, Room>();

    

    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(gameObject);

        DontDestroyOnLoad(gameObject);
    }


    private void Start()
    {
        currentRoom = startingRoom;
        Unpack();
    }

    void Unpack()
    {
        string description = currentRoom.description;

        exitRooms.Clear();//Important -- need as switch from room to room
        foreach(Exit e in currentRoom.exits)
        {
            if (!e.is_hidden)
            {
                description += "\n" + e.description;
                exitRooms.Add(e.direction.ToString(), e.room);
            } 
        }

        InputManager.instance.UpdateStory(description);
        if(currentRoom.name == "dragons")
        {
            onRestart.Invoke(); //Calling my restart event to happen
            currentRoom = startingRoom; //Puts player back to starting point
            Unpack();
        }
    }

    public bool SwitchRooms(string direction)
    {
        if(exitRooms.ContainsKey(direction))
        {
            if (GameManager.instance.inventory.Contains("key") || !getExit(direction).is_locked)
            {
                currentRoom = exitRooms[direction];
                InputManager.instance.UpdateStory("You go " + direction);
                Unpack();
                return true;
            }
            else
            {
                return false;
            }
        }
        return false;
    }

    public Exit getExit(string direction)
    {
        foreach (Exit e in currentRoom.exits)
    {
        if (e.direction.ToString() == direction)
            return e;
    }
        return null;
    }

    public bool getItem(string item)
    {
        bool isFound = false;
        foreach(string i in currentRoom.items)
    {
            if (i == item)
            {
                isFound = true;
                if(item == "orb")
                {
                    toKeyNorth.is_hidden = false;
                }
            }
                return true;
    }
        return isFound;//item not found in room
    }

}
