using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainManager : MonoBehaviour
{
    public Brick BrickPrefab;
    public int LineCount = 6;
    public Rigidbody Ball;
    public float ballSpeed=2.0f;

    public Text ScoreText;
    public Text BestScoreText;

    public GameObject GameOverText;
    public List<Brick> pooledObjects;

    private bool m_Started = false;
    private int m_Points;
    
    private bool m_GameOver = false;

    
    // Start is called before the first frame update
    void Start()
    {
        CreateBricks();//create bricks and pooling them after start game
    }

    private void CreateBricks()
    {
        BestScoreText.text = "BestScore:" + MenuManager.Instance.bestPlayerName + "  " + MenuManager.Instance.bestPlayerHighScore;
        const float step = 0.6f;
        int perLine = Mathf.FloorToInt(4.0f / step);

        int[] pointCountArray = new[] { 1, 1, 2, 2, 5, 5 };
        for (int i = 0; i < LineCount; ++i)
        {
            for (int x = 0; x < perLine; ++x)
            {
                Vector3 position = new Vector3(-1.5f + step * x, 2.5f + i * 0.3f, 0);
                var brick = Instantiate(BrickPrefab, position, Quaternion.identity);
                pooledObjects.Add(brick);
                brick.PointValue = pointCountArray[i];
                brick.onDestroyed.AddListener(AddPoint);
            }
        }
    }

    private void Update()
    {
        Playing();//player's gameplay and gameover
    }
    private void FixedUpdate()
    {
        CreateNewBricks();//calling bricks from pool after all of them destroyed 
    }
    private void CreateNewBricks()
    {
        if (m_Points % 96 == 0&&m_Points!=0)
        {
            ballSpeed += 2.0f;
            for (int i = 0; i < pooledObjects.Count; i++)
            {
                pooledObjects[i].SetActive();
            }
        }
    }

    private void Playing()
    {
        if (!m_Started)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                m_Started = true;
                float randomDirection = Random.Range(-1.0f, 1.0f);
                Vector3 forceDir = new Vector3(randomDirection, 1, 0);
                forceDir.Normalize();

                Ball.transform.SetParent(null);
                Ball.AddForce(forceDir * ballSpeed, ForceMode.VelocityChange);
            }
        }
        else if (m_GameOver)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
            }
        }
    }

    private void AddPoint(int point)
    {
        m_Points += point;
        ScoreText.text = $"Score : {m_Points}";
    }
    public void GameOver()
    {
        SaveHighScore();
        LoadHighScore();
        BestScoreText.text = "BestScore:" + MenuManager.Instance.bestPlayerName + "  " + MenuManager.Instance.bestPlayerHighScore;
        m_GameOver = true;
        GameOverText.SetActive(true);
    }
    private void SaveHighScore()
    {
        if (MenuManager.Instance.bestPlayerHighScore < m_Points)
        {
            MenuManager.PlayerData playerData = new MenuManager.PlayerData(MenuManager.Instance.playerName);
            playerData._highScore = m_Points;
            MenuManager.Instance.SaveData(playerData);
            MenuManager.Instance.LoadData();
            Debug.Log("Player Data Saved:" + playerData._highScore);
        }
    }
    private void LoadHighScore()
    {
        MenuManager.Instance.LoadData();
    }
}
