using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;
using UnityEngine.Analytics;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    public List<string> inventory = new List<string>();
    public List<string> pickedUpItems = new List<string>();

    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(gameObject);

        DontDestroyOnLoad(gameObject);
    }

    void Start()
    {
        NavagationManager.instance.onRestart += ResetGame; //Notice no ()
        Load();
    }

    public void Save()
    {
        SaveState gameState = new SaveState();
        gameState.currentRoom = NavagationManager.instance.currentRoom.name;
        gameState.inventory = inventory;
        gameState.pickedUpItems = pickedUpItems;

        BinaryFormatter bf = new BinaryFormatter();
        FileStream aFile = File.Create(Application.persistentDataPath + "/player.save");
        //Debug.Log((Application.persistentDataPath));
        bf.Serialize(aFile, gameState);
        aFile.Close();
    }

    void Load()
    {
        if (File.Exists(Application.persistentDataPath + "/player.save")) //Yippee it exists, load that thing
        {
            BinaryFormatter bf = new BinaryFormatter();
            FileStream aFile = File.Open(Application.persistentDataPath + "/player.save", FileMode.Open);
            //Debug.Log((Application.persistentDataPath));
            SaveState gameState = (SaveState) bf.Deserialize(aFile);
            aFile.Close();

            Room room = NavagationManager.instance.GetRoomByName(gameState.currentRoom);
            if (room != null)
            {
                NavagationManager.instance.LoadRooms(room);
            }
            if (gameState.inventory != null)
            {
                inventory = gameState.inventory;
            }
            if (gameState.pickedUpItems != null)
            {
                pickedUpItems = gameState.pickedUpItems;
                NavagationManager.instance.UpdateRooms(pickedUpItems);
            }
        }
        else //new player
            NavagationManager.instance.GameRestart();
    }

    void ResetGame()
    {
        inventory.Clear();
        pickedUpItems.Clear();
        if (File.Exists(Application.persistentDataPath + "/player.save"))
        {
            File.Delete(Application.persistentDataPath + "/player.save");
        }
    }



}
