using System.Collections.Generic;
using UnityEditor.Search;
using UnityEngine;
using UnityEngine.UIElements.Experimental;

public class NavagationManager : MonoBehaviour
{
    public static NavagationManager instance;

    public Room startingRoom;
    public Room currentRoom;
    public Room orbRoom;
    public Room keyRoom;
    public Room swordRoom;
    public Room dragonRoom;
    public Exit toKeyNorth;
    public Exit toShieldEast;
    public Exit toSmokeyWest;
    public Exit blueDoor;
    public Exit redDoor;
    public Exit goldenDoor;
    public List<Room> rooms; //Will allow nav manager to have access to all rooms


    public delegate void Restart(); //Custom event
    public event Restart onRestart;

    private Dictionary<string, Room> exitRooms = new Dictionary<string, Room>();
    private bool dragonDead = false;

    

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
        if( currentRoom == null)
            currentRoom = startingRoom;
        //Unpack();
    }

    public void Unpack()
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
        
        if(currentRoom.roomName == "Dragon" && dragonDead == false)
        {
            //onRestart.Invoke(); //Calling my restart event to happen
            //currentRoom = startingRoom; //Puts player back to starting point
            //Unpack();
            if(GameManager.instance.inventory.Contains("SWORD") && !GameManager.instance.inventory.Contains("SHIELD"))
            {
                InputManager.instance.UpdateStory("You attempt to fight the dragon, but it burns you to a crisp");
                GameRestart();
            }
            else if (GameManager.instance.inventory.Contains("SHIELD") && !GameManager.instance.inventory.Contains("SWORD"))
            {
                InputManager.instance.UpdateStory("You block the dragons fire, but have no way to harm the dragon. The dragon swats you aside and the impact of hitting the wall kills you immediately.");
                GameRestart();
            }
            else if (GameManager.instance.inventory.Contains("SWORD") && GameManager.instance.inventory.Contains("SHIELD"))
            {
                InputManager.instance.UpdateStory("You block the dragons fire and then, with every bit of courage you have, you charge at the dragon and successfully stab it in the heart!");
                InputManager.instance.UpdateStory("Among the dragon's loot you see a GOLDEN KEY!");
                dragonDead = true;
                toShieldEast.is_hidden = false;
                toSmokeyWest.is_hidden = false;
                currentRoom.description = "The dragons corpse lays lifeless";
            }
            else
            {
                InputManager.instance.UpdateStory("The dragon burns you to a crisp immediately");
                GameRestart();
            }
                
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
        toShieldEast.is_hidden = true;
        toSmokeyWest.is_hidden = true;
        blueDoor.is_locked = true;
        redDoor.is_locked = true;
        goldenDoor.is_locked = true;
        dragonDead = false;
        orbRoom.description = "A blue ORB glows in the middle of the room";
        keyRoom.description = "A BLUE KEY is positioned on a pedestal in the middle of the room, illuminated by a crack in the ceiling";
        swordRoom.description = "Light shines down on a SWORD lodged into a rock";
        dragonRoom.description = "The dragon, sensing that you are here, awakens.";

        foreach (Room room in rooms)
        {
            if (room.originalItems != null)
            {
                room.items.Clear();
                foreach (string item in room.originalItems)
                {
                    room.items.Add(item);
                }
            }
        }

        Unpack();

    }

    public void UpdateRooms(List<string> pickedUpItems)
    {
        foreach(Room room in rooms)
        {
            foreach (string item in pickedUpItems)
            {
                if(room.originalItems.Contains(item.ToUpper()))
                {
                    room.items.Clear();
                    room.items.Add(item);
                }
            }
        }
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
            if (i.ToUpper() == item.ToUpper())
            {
                isFound = true;
                Debug.Log(item);
                if (item.Equals("ORB",System.StringComparison.OrdinalIgnoreCase))
                {
                    //Debug.Log("Orb picked up");
                    toKeyNorth.is_hidden = false;
                    currentRoom.description = "There is a subtle glow that remains where the blue ORB used to be";
                }
                else if(item.Equals("SWORD", System.StringComparison.OrdinalIgnoreCase))
                {
                    currentRoom.description = "There is a large rock with a hole in it";
                }
                else if (item.Equals("BLUE", System.StringComparison.OrdinalIgnoreCase))
                {
                    blueDoor.is_locked = false;
                    currentRoom.description = "There is an empty pedestal in the middle of the room.";
                }
                else if (item.Equals("RED", System.StringComparison.OrdinalIgnoreCase))
                {
                    redDoor.is_locked = false;
                }
                else if (item.Equals("GOLDEN", System.StringComparison.OrdinalIgnoreCase))
                {
                    goldenDoor.is_locked = false;
                }

            }

        }
            if (isFound)
            {
                currentRoom.items.Remove(item.ToUpper());
                GameManager.instance.pickedUpItems.Add(item.ToUpper());
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
