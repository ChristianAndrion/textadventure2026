using UnityEngine;

[CreateAssetMenu(fileName = "Room", menuName = "Text/Room")]
public class Room : ScriptableObject
{
    public string roomName;
    [TextArea]
    public string description;
    public Exit[] exits;

    //Another way we can add pickup items
    //public bool hasKey;
    //public bool hasOrb;

    public string[] items;
} 
