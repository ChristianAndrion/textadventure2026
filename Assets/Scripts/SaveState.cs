using System.Collections.Generic;
using UnityEngine;

[System.Serializable]

public class SaveState
{
    public string currentRoom;
    public List<string> inventory;
    public List<string> pickedUpItems;
}
