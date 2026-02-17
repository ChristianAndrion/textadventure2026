using System.Collections.Generic;
using UnityEngine;

public class NavagationManager : MonoBehaviour
{
    public static NavagationManager instance;

    public Room startingRoom;
    public Room currentRoom;

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

        foreach(Exit e in currentRoom.exits)
        {
            description += "\n" + e.description;
            exitRooms.Add(e.direction.ToString(), e.room);
        }

        InputManager.instance.UpdateStory(description);
    }

    public bool SwitchRooms(string direction)
    {
        if(exitRooms.ContainsKey(direction))
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
}
