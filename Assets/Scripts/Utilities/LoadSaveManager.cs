//using Microsoft.Xbox;
using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;
using Zenject;

public class LoadSaveManager : MonoBehaviour
{
    public static LoadSaveManager instance;

    [Inject] private WeaponController weaponControl;
    
    private PlayerData playerData;

    // Start is called before the first frame update
    void Start()
    {
        if (instance == null)
            instance = this;

        playerData = new PlayerData();

        DontDestroyOnLoad(gameObject);
    }

#region Standalone PC Functions

    public void StandalonePCLoad(string saveName)
    {
        // Use BinaryReader

        string filePath = Application.persistentDataPath + "/" + saveName + ".kws";

        // Load the player data
        using (BinaryReader reader = new BinaryReader(File.Open(filePath, FileMode.Open))) 
        {
            playerData.LastSaved = DateTime.Parse(reader.ReadString());
            float x = reader.ReadSingle();
            float y = reader.ReadSingle();
            playerData.Position = new Vector2(x, y);

            playerData.CurrentLevel = reader.ReadString();
            playerData.CurrentWeapon = (WeaponSelect)reader.ReadInt32();
            playerData.TimePlayed = DateTime.Parse(reader.ReadString());
            playerData.BindingsJson = reader.ReadString();
        }
    }

    public void StandalonePCSave(string saveName)
    {
        string filePath = Application.persistentDataPath + "/" + saveName + ".kws";

        SaveCurrentData();

        // Use BinaryWriter
        using (BinaryWriter writer = new BinaryWriter(File.Open(filePath, FileMode.Create))) {
            // Have to separate the position data as BinaryWriter only supports primitive types
            writer.Write(playerData.LastSaved.ToString());
            writer.Write(playerData.Position.x);
            writer.Write(playerData.Position.y);
            writer.Write(playerData.CurrentLevel);
            writer.Write((int)playerData.CurrentWeapon);
            writer.Write(playerData.TimePlayed.ToString());
            writer.Write(playerData.BindingsJson);
        }
    }

#endregion

#region Steam PC Functions

#endregion

#region Xbox Functions

    public void XboxLoad()
    {
        //Gdk.Helpers.OnGameSaveLoaded += OnGameSaveLoaded;
        //Gdk.Helpers.LoadSaveData();
    }

    /*private void OnGameSaveLoaded(object sender, GameSaveLoadedArgs saveData)
    {
        BinaryFormatter formatter = new BinaryFormatter();
        using (MemoryStream memoryStream = new MemoryStream(saveData.data))
        {
            object playerSaveDataObj = formatter.Deserialize(memoryStream);
            playerData = playerSaveDataObj as PlayerData;
        }
    }*/

    public void XboxSave()
    {
        SaveCurrentData();

        BinaryFormatter formatter = new BinaryFormatter();
        using (MemoryStream memoryStream = new MemoryStream())
        {
            formatter.Serialize(memoryStream, playerData);
            //Gdk.Helpers.Save(memoryStream.ToArray());
        }
    }

#endregion

#region PlayStation Functions

#endregion

#region Nintendo Functions

#endregion

    public void SetCurrentPosition(Vector2 position)
    {
        playerData.Position = position;
    }

    public void SetCurrentLevel(string level)
    {
        playerData.CurrentLevel = level;
    }

    public void SetCurrentWeapon(WeaponSelect weapon)
    {
        playerData.CurrentWeapon = weapon;
    }

    public void SaveCurrentData()
    {
        playerData.CurrentWeapon = weaponControl.GetCurrentWeaponSelect();
        
        playerData.LastSaved = DateTime.Now;

        float seconds = Time.timeSinceLevelLoad;
        playerData.TimePlayed += TimeSpan.FromSeconds(seconds);
    }

    public void SetBindingsJson(string json)
    {
        playerData.BindingsJson = json;
    }

    public string GetBindingsJson()
    {
        return playerData.BindingsJson;
    }
}


[System.Serializable]
public class PlayerData 
{
    public Vector2 Position {get; set;}
    public string CurrentLevel {get; set;}
    public WeaponSelect CurrentWeapon {get; set;}
    public DateTime LastSaved {get; set;}
    public DateTime TimePlayed {get; set;}
    public string BindingsJson {get; set;}

    public PlayerData()
    {
        Position = Vector2.zero;
        CurrentLevel = "";
        CurrentWeapon = WeaponSelect.PISTOL;
    }

    public PlayerData(Vector2 position, string currentLevel, WeaponSelect currentWeapon)
    {
        this.Position = position;
        this.CurrentLevel = currentLevel;
        this.CurrentWeapon = currentWeapon;
    }
}