using System.Collections.Generic;
using UnityEngine;

public class NavagationManager : MonoBehaviour
{
    public static NavagationManager instance;

    public Room startingRoom;
    public Room currentRoom;
    public Exit toKeyNorth;
    public List<Room> rooms; //Will allow nav manager to have access to all rooms


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
        //Unpack();
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
            //onRestart.Invoke(); //Calling my restart event to happen
            //currentRoom = startingRoom; //Puts player back to starting point
            //Unpack();
            GameRestart();
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

    public void LoadRooms(Room room)
    {
        currentRoom = room;
        Unpack();
    }

    public void GameRestart()
    {
        onRestart.Invoke(); //Calling my restart event to happen
        //^ Point to a function
        currentRoom = startingRoom; //Puts player back to starting point
        toKeyNorth.is_hidden = true;

        Unpack();

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
        foreach (string i in currentRoom.items)
        {
            if (i == item)
            {
                isFound = true;
                if (item == "orb")
                {
                    toKeyNorth.is_hidden = false;
                }
            }

        }
            if (isFound)
            {
                currentRoom.items.Remove(item);
                currentRoom.description = "There is a subtle glow that remains where the blue orb used to be";
            }

        return isFound;//item not found in room
    }

    public Room GetRoomByName(string name)
    {
        foreach(Room aroom in rooms)
        {
            if (aroom.name == name)
            {
                return aroom;
            }
        }
        return null;
    }

}
