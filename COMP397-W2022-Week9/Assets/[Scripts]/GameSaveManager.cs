using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameSaveManager : MonoBehaviour
{
    public Transform player;

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
        string playerPosition = JsonUtility.ToJson(player.position);
        string playerRotation = JsonUtility.ToJson(player.rotation.eulerAngles);
        PlayerPrefs.SetString("PlayerPosition", playerPosition);
        PlayerPrefs.SetString("PlayerRotation", playerRotation);
        PlayerPrefs.Save();
        Debug.Log("Game data saved!");
    }

    void LoadGame()
    {
        if (PlayerPrefs.HasKey("PlayerPosition"))
        {
            player.gameObject.GetComponent<CharacterController>().enabled = false;
            player.position = JsonUtility.FromJson<Vector3>(PlayerPrefs.GetString("PlayerPosition"));
            player.rotation = Quaternion.Euler(JsonUtility.FromJson<Vector3>(PlayerPrefs.GetString("PlayerRotation")));                    
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
        PlayerPrefs.DeleteAll();
        Debug.Log("Data reset complete");
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
