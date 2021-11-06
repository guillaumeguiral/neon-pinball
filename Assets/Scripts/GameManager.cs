using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class GameManager : MonoBehaviour
{
    [SerializeField] int ballsLeft = 3;
    public int BallsLeft { get { return ballsLeft; } }
    [SerializeField] Text ballsLeftText;

    [SerializeField] float multiplier = 1f;
    public float Multiplier { get { return multiplier; } }
    [SerializeField] Text multiplierText;

    [SerializeField] float tiltChance = 0.25f;

    [SerializeField] GameObject boardPanel;
    [SerializeField] GameObject gameOverScreen;
    [SerializeField] Text gameOverText;

    PlayerControls playerControls;
    Scoreboard scoreboard;

    private void Start()
    {
        playerControls = FindObjectOfType<PlayerControls>();
        scoreboard = FindObjectOfType<Scoreboard>();

        StartCoroutine(RemoveWelcomeMessage());
    }

    public void LostBall()
    {
        ballsLeft--;

        ResetMultiplier();
        UpdateBallsLeftText();

        if (ballsLeft > 0)
        {
            playerControls.ActivateControls();
        }
        else
        {
            GameOver();
        }
    }

    public void WinBall()
    {
        ballsLeft++;

        UpdateBallsLeftText();
    }

    public void IncreaseMultiplier(float amount)
    {
        multiplier += amount;
        
        UpdateMultiplierText();
    }

    public void ResetMultiplier()
    {
        multiplier = 1f;

        UpdateMultiplierText();
    }

    private void GameOver()
    {
        boardPanel.SetActive(false);

        gameOverScreen.SetActive(true);
        gameOverText.DOText("FINAL SCORE: " + scoreboard.Score, 2f);

        playerControls.DeactivateControls();
    }

    void UpdateBallsLeftText()
    {
        ballsLeftText.text = "BALLS LEFT: " + ballsLeft;
    }

    void UpdateMultiplierText()
    {
        multiplierText.text = "MULTIPLIER: " + multiplier.ToString();
    }

    public void Tilt(Vector3 direction)
    {
        // Shake camera
        Camera.main.DOShakePosition(0.25f, direction / 4f);

        // Check if tilt is (randomly) detected
        bool isTiltDetected = Random.Range(0, 1f) < tiltChance;

        // If so...
        if (isTiltDetected)
        {
            // Deactivate player controls until reset
            playerControls.DeactivateControls();

            // Display TILT message
            Debug.Log("TILT");
        }
    }

    IEnumerator RemoveWelcomeMessage()
    {
        yield return new WaitForSeconds(2f);

        scoreboard.UpdateText();
    }
}
