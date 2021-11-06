using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using DG.Tweening;

public class GameManager : MonoBehaviour
{
    [SerializeField] int ballsLeft = 3;
    public int BallsLeft { get { return ballsLeft; } }
    [SerializeField] Text ballsLeftText;

    [SerializeField] float multiplier = 1f;
    public float Multiplier { get { return multiplier; } }
    [SerializeField] Text multiplierText;

    [SerializeField] float tiltChance = 0.10f;

    [SerializeField] GameObject blackScreen;
    [SerializeField] Image blackScreenImage;
    [SerializeField] GameObject boardPanel;
    [SerializeField] GameObject gameOverScreen;
    [SerializeField] Text gameOverText;

    CameraManager cameraManager;
    PlayerControls playerControls;
    Scoreboard scoreboard;

    private void Start()
    {
        cameraManager = FindObjectOfType<CameraManager>();
        playerControls = FindObjectOfType<PlayerControls>();
        scoreboard = FindObjectOfType<Scoreboard>();

        blackScreen.SetActive(true);
        blackScreenImage.DOFade(0, 3f).SetEase(Ease.Linear).OnComplete(() =>
        {
            blackScreen.SetActive(false);
        });

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
        gameOverText.text = "";
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
        cameraManager.Tilt(direction);

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

    public void RestartLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    IEnumerator RemoveWelcomeMessage()
    {
        yield return new WaitForSeconds(2f);

        scoreboard.UpdateText();
    }
}
