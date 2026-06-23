using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    public static ScoreManager Instance { get; private set; }

    [SerializeField] private int basePoints = 10;
    [SerializeField] private float comboWindow = 3f;

    private int score;
    private int combo;
    private float timeSinceLastFood;

    public int Score => score;
    public int Combo => combo;
    public int HighScore { get; set; }

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

    private void Update()
    {
        if (combo <= 1) return;

        timeSinceLastFood += Time.deltaTime;
        if (timeSinceLastFood >= comboWindow)
        {
            combo = 1;
            timeSinceLastFood = 0f;
        }
    }

    public void OnFoodEaten()
    {
        if (timeSinceLastFood < comboWindow && timeSinceLastFood > 0f)
        {
            combo++;
        }
        else
        {
            combo = 1;
        }

        score += basePoints * combo;
        timeSinceLastFood = 0f;
    }

    public void SaveHighScore()
    {
        PlayerPrefs.SetInt(HighScoreKey, HighScore);
        PlayerPrefs.Save();
    }

    public void LoadHighScore()
    {
        HighScore = PlayerPrefs.GetInt(HighScoreKey, 0);
    }

    public void Reset()
    {
        score = 0;
        combo = 1;
        timeSinceLastFood = 0f;
    }
}
