using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    public static ScoreManager Instance { get; private set; }

    [SerializeField] private int basePoints = 10;

    private int score;
    private int highScore;

    public int Score => score;
    public int HighScore
    {
        get => highScore;
        set => highScore = value;
    }

    private const string HighScoreKey = "SnakeHighScore";

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        LoadHighScore();
    }

    public void OnFoodEaten()
    {
        score += basePoints;
    }

    public void SaveHighScore()
    {
        PlayerPrefs.SetInt(HighScoreKey, highScore);
        PlayerPrefs.Save();
    }

    public void LoadHighScore()
    {
        highScore = PlayerPrefs.GetInt(HighScoreKey, 0);
    }

    public void Reset()
    {
        score = 0;
    }
}
