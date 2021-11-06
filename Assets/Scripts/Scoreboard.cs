using UnityEngine;
using UnityEngine.UI;

public class Scoreboard : MonoBehaviour
{
    [SerializeField] int score = 0;
    public int Score { get { return score; } }

    [SerializeField] Text scoreText;

    GameManager gameManager;

    private void Start()
    {
        gameManager = FindObjectOfType<GameManager>();
    }

    public void AddToScore(int amount)
    {
        score += Mathf.RoundToInt(amount * gameManager.Multiplier);
        
        UpdateText();
    }

    public void UpdateText()
    {
        scoreText.text = "Score: " + score;
    }
}
