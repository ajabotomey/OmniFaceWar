using System.Collections;
using System.Collections.Generic;
using System.IO;
using System;
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
    }

    public void Load(string saveName)
    {
        // Use BinaryReader
        playerData = new PlayerData();

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
        }
    }

    public void Save(string saveName)
    {
        string filePath = Application.persistentDataPath + "/" + saveName + ".kws";

        playerData.CurrentWeapon = weaponControl.GetCurrentWeaponSelect();
        
        playerData.LastSaved = DateTime.Now;

        float seconds = Time.timeSinceLevelLoad;
        playerData.TimePlayed += TimeSpan.FromSeconds(seconds);

        // Use BinaryWriter
        using (BinaryWriter writer = new BinaryWriter(File.Open(filePath, FileMode.Create))) {
            // Have to separate the position data as BinaryWriter only supports primitive types
            //writer.Write(playerData.LastSaved.ToShortDateString() + " " + playerData.LastSaved.ToShortTimeString());
            writer.Write(playerData.LastSaved.ToString());
            writer.Write(playerData.Position.x);
            writer.Write(playerData.Position.y);
            writer.Write(playerData.CurrentLevel);
            writer.Write((int)playerData.CurrentWeapon);
            writer.Write(playerData.TimePlayed.ToString());
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

    public void SetCurrentWeapon(WeaponSelect weapon)
    {
        playerData.CurrentWeapon = weapon;
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