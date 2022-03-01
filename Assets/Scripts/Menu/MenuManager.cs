using System.Collections;
using System.Collections.Generic;
using System.IO;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuManager : MonoBehaviour
{
    [SerializeField] InputField playerNameInput;
    [SerializeField] TextMeshProUGUI bestPlayerText;
    public static MenuManager Instance { get; private set; }

    public string playerName;
    public int highScore;
    public string bestPlayerName;
    public int bestPlayerHighScore;
    public void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }
    void Start()
    {
        //resets highscore if needed
        //PlayerData playerData = new PlayerData("test"); playerData._highScore = 0;SaveData(playerData);

        LoadData();
        bestPlayerText.text = "Best Score:"+bestPlayerName + "   " + bestPlayerHighScore;
    }

    public void LoadGameScene()
    {
        SceneManager.LoadScene("main");
    }
    public void GetPlayerNameInput()
    {
        playerName = playerNameInput.text;
        PlayerData playerData = new PlayerData(playerName);
    }

    [System.Serializable]
    public class PlayerData
    {
        public string _playerName;
        public int _highScore;

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

    public void LoadData()
    {
        string path = Application.persistentDataPath + "/savefile.json";
        if (File.Exists(path))
        {
            string json = File.ReadAllText(path);
            PlayerData playerData = JsonUtility.FromJson<PlayerData>(json);
            bestPlayerHighScore = playerData._highScore;
            bestPlayerName = playerData._playerName;
        }
    }

}
