using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuManager : MonoBehaviour
{
    [SerializeField] InputField playerNameInput;

    static string playerName;
    public int highScore;
    public void Awake()
    {
        DontDestroyOnLoad(this.gameObject);
    }
    public void LoadGameScene()
    {
        SceneManager.LoadScene("main");
    }
    public void GetPlayerNameInput()
    {
        playerName = playerNameInput.text;
        PlayerData playerData = new PlayerData(playerName);
        Debug.Log(playerData._playerName);
    }

    [System.Serializable]
    public class PlayerData
    {
        public string _playerName { get; private set; }
        public int _highScore { get; set; }

        public PlayerData(string playerName)
        {
            _playerName = playerName;
            _highScore = 0;
        }
    }
    public void SaveData(PlayerData playerData)
    {
        string json = JsonUtility.ToJson(playerData);

        File.WriteAllText(Application.persistentDataPath + "/savefile.json", json);
    }

    public PlayerData LoadData()
    {
        string path = Application.persistentDataPath + "/savefile.json";
        if (File.Exists(path))
        {
            Debug.Log("Player Data Loaded");
            string json = File.ReadAllText(path);
            PlayerData playerData = JsonUtility.FromJson<PlayerData>(json);
            highScore = playerData._highScore;
            return playerData;
        }
        else
        {
            return null;
        }
    }

}
