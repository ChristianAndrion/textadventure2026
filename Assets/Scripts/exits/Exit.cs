using Unity.VisualScripting;
using UnityEngine;

[CreateAssetMenu(fileName = "Exit", menuName = "Text/Exit")]
public class Exit : ScriptableObject
{
    public enum Direction { north, south, east, west };
    
    public Direction direction;

    [TextArea]
    public string description;

    public Room room; //Room that the exit is attatched to

    public bool is_locked;
    public bool is_hidden;
}
