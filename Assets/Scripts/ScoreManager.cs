using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Rendering;


public class ScoreManager : MonoBehaviour
{
    public static ScoreManager Instance;
    public static string highScore;
    public static int highScoreScore;
    public static int lastScore;
    public static string userName;
    public static bool hasHighScore;

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }
        
        Instance = this;
        DontDestroyOnLoad(gameObject);

        LoadHighScore();
    }   

    [System.Serializable]
    class SaveData
    {
        public string player;
        public int score;
    }
    public void LoadHighScore()
    {
        string path = Application.persistentDataPath + "/savefile.json";

        if (File.Exists(path))
        {
            string json = File.ReadAllText(path);
            SaveData data = JsonUtility.FromJson<SaveData>(json);

            highScore = data.player + " : " + data.score;
            hasHighScore = true;
            highScoreScore = data.score;

            // Debuging
            Debug.Log("Debug du high score dans 'LoadHighScore : " + highScore);
        }
        else
        {
            hasHighScore= false;
        }
    }

    public void SaveHighScore(int points)
    {
        SaveData data = new SaveData();
        data.player = userName;
        data.score = points;

        string json = JsonUtility.ToJson(data);

        File.WriteAllText(Application.persistentDataPath + "/savefile.json", json);
    }
}
