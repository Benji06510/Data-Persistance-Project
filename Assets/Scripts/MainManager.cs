using System.Collections;
using System.Collections.Generic;
using System.IO;
using TreeEditor;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainManager : MonoBehaviour
{
    public Brick BrickPrefab;
    public int LineCount = 6;
    public Rigidbody Ball;

    public Text ScoreText;
    public Text HighScoreText;
    public GameObject GameOverText;
    
    private bool m_Started = false;
    public static int m_Points;
    
    private bool m_GameOver = false;

    [SerializeField] public static int brickCount = 0;

    // Start is called before the first frame update
    void Start()
    {
        
        ShowHighScore();
        FillField();

    }

    private void Update()
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
                Ball.AddForce(forceDir * 2.0f, ForceMode.VelocityChange);
            }
        }
        else if (m_GameOver)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
                m_Points = 0;
                ShowHighScore();

            }
        }
        CompareScore();
        if(brickCount == 0)
        {
            FillField();
        }
    }

    void AddPoint(int point)
    {
        m_Points += point;
        ScoreText.text = $"Score : {m_Points}";
    }

    public void GameOver()
    {
        m_GameOver = true;
        GameOverText.SetActive(true);

        if (m_Points > ScoreManager.highScoreScore)
        {
            HighScoreText.text = "Best score : " + ScoreManager.userName + " : " + m_Points;
            ScoreManager.Instance.SaveHighScore(m_Points);
        }
    }

    private void ShowHighScore()
    {
        if (ScoreManager.Instance != null)
        {
            if (ScoreManager.hasHighScore)
            {
                ScoreManager.Instance.LoadHighScore();
                HighScoreText.text = "Best score : " + ScoreManager.highScore;
            }
        }
    }

    private void CompareScore()
    {
        if(m_Points > ScoreManager.highScoreScore)
        {
            HighScoreText.text = "Best score : " + ScoreManager.userName + " : " + m_Points;
        }
        else
        {
            ShowHighScore();
        }
    }

    private void FillField()
    {
        const float step = 0.6f;
        int perLine = Mathf.FloorToInt(4.0f / step);

        int[] pointCountArray = new[] { 1, 1, 2, 2, 5, 5 };
        for (int i = 0; i < LineCount; ++i)
        {
            for (int x = 0; x < perLine; ++x)
            {
                Vector3 position = new Vector3(-1.5f + step * x, 2.5f + i * 0.3f, 0);
                var brick = Instantiate(BrickPrefab, position, Quaternion.identity);
                brick.PointValue = pointCountArray[i];
                brick.onDestroyed.AddListener(AddPoint);
                brickCount++;
            }
        }
    }
    
}
