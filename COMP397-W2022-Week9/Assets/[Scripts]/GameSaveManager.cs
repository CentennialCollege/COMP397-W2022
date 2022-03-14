using System.Collections;
using System.Collections.Generic;
using System;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class GameSaveManager : MonoBehaviour
{
    public Transform player;
    public PlayerDataScriptableObject playerDataSO;

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.K))
        {
            SaveGame();
        }

        if (Input.GetKeyDown(KeyCode.L))
        {
            LoadGame();
        }
    }

    void SaveGame()
    {
        BinaryFormatter bf = new BinaryFormatter();
        FileStream file = File.Create(Application.persistentDataPath + "/MySaveData.dat");
        PlayerData data = new PlayerData();
        data.position = new[] { player.position.x, player.position.y, player.position.z};
        data.rotation = new[] { player.rotation.eulerAngles.x, player.rotation.eulerAngles.y, player.rotation.eulerAngles.z };
        bf.Serialize(file, data);
        file.Close();
        Debug.Log("Game data saved!");
        playerDataSO.position = player.position;
        playerDataSO.rotation = player.rotation;
        playerDataSO.health = 50;
    }

    void LoadGame()
    {
        if (File.Exists(Application.persistentDataPath + "/MySaveData.dat"))
        {
            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Open(Application.persistentDataPath + "/MySaveData.dat", FileMode.Open);
            PlayerData data = (PlayerData)bf.Deserialize(file);
            file.Close();
            player.gameObject.GetComponent<CharacterController>().enabled = false;
            player.position = new Vector3(data.position[0], data.position[1], data.position[2]);
            player.rotation = Quaternion.Euler(data.rotation[0], data.rotation[1], data.rotation[2]);
            player.gameObject.GetComponent<CharacterController>().enabled = true;
            Debug.Log("Game data loaded!");
        }
        else
        {
            Debug.LogError("There is no save data!");
        }
    }

    void ResetData()
    {
        if (File.Exists(Application.persistentDataPath + "/MySaveData.dat"))
        {
            File.Delete(Application.persistentDataPath + "/MySaveData.dat");
            Debug.Log("Data reset complete!");
        }
        else
        {
            Debug.LogError("No save data to delete.");
        }
    }


    public void OnSaveButton_Pressed()
    {
        SaveGame();
    }

    public void OnLoadButton_Pressed()
    {
        LoadGame();
    }
}
