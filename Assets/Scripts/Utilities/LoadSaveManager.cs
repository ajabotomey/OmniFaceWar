using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class LoadSaveManager : MonoBehaviour
{
    public static LoadSaveManager instance;
    
    private PlayerData playerData;

    // Start is called before the first frame update
    void Start()
    {
        if (instance == null)
            instance = this;
    }

    public void Load(string saveName)
    {
        // Use BinaryReader
        playerData = new PlayerData();

        string filePath = Application.persistentDataPath + "/" + saveName + ".kws";

        // Load the player data
        using (BinaryReader reader = new BinaryReader(File.Open(filePath, FileMode.Open))) 
        {
            float x = reader.ReadSingle();
            float y = reader.ReadSingle();
            playerData.Position = new Vector2(x, y);

            playerData.CurrentLevel = reader.ReadString();
        }
    }

    public void Save(string saveName)
    {
        string filePath = Application.persistentDataPath + "/" + saveName + ".kws";

        // Retrieve player data

        // Use BinaryWriter
        using (BinaryWriter writer = new BinaryWriter(File.Open(filePath, FileMode.Create))) {
            // Have to separate the position data as BinaryWriter only supports primitive types
            writer.Write(playerData.Position.x);
            writer.Write(playerData.Position.y);
            writer.Write(playerData.CurrentLevel);
        }
    }

    public void SetCurrentPosition(Vector2 position)
    {
        playerData.Position = position;
    }

    public void SetCurrentLevel(string level)
    {
        playerData.CurrentLevel = level;
    }
}


[System.Serializable]
public class PlayerData 
{
    public Vector2 Position {get; set;}
    public string CurrentLevel {get; set;}

    public PlayerData()
    {
        Position = Vector2.zero;
        CurrentLevel = "";
    }

    public PlayerData(Vector2 position, string currentLevel)
    {
        this.Position = position;
        this.CurrentLevel = currentLevel;
    }
}