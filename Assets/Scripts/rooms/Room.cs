using NUnit.Framework;
using System.Collections.Generic;
using Unity.VisualScripting;
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

    public List <string> items;

    public List<string> originalItems;


} 
