using UnityEngine;
using DG.Tweening;

public class PlayerControls : MonoBehaviour
{
    // Initial gravity : -9.81

    [SerializeField] Rigidbody rbFlipperLeft;
    [SerializeField] Rigidbody rbFlipperRight;

    [SerializeField] float initialRotation = 30f;
    [SerializeField] float activatedRotation = 10f;

    [SerializeField] int hitScore = 50;

    [SerializeField] AudioSource audioSourceFlipperLeft;
    [SerializeField] AudioSource audioSourceFlipperRight;

    [SerializeField] AudioClip sndFlipperUp;
    [SerializeField] AudioClip sndFlipperDown;

    [SerializeField] bool isControlsActivated = true;

    [SerializeField] float tiltForceMin = 2f;
    [SerializeField] float tiltForceMax = 5f;

    Ball ball;
    GameManager gameManager;
    Scoreboard scoreboard;

    private void Start()
    {
        ball = FindObjectOfType<Ball>();
        gameManager = FindObjectOfType<GameManager>();
        scoreboard = FindObjectOfType<Scoreboard>();
    }

    void Update()
    {
        ProcessFlippers();
        ProcessPlunger();

        if (gameManager.BallsLeft <= 0 && !isControlsActivated)
        {
            if (Input.anyKeyDown)
            {
                gameManager.RestartLevel();
            }
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (isControlsActivated)
        {
            scoreboard.AddToScore(hitScore);
        }
    }

    private void ProcessFlippers()
    {
        if (isControlsActivated)
        {
            ProcessFlipperLeft();
            ProcessFlipperRight();
            ProcessTilt();
        }
    }

    private void ProcessFlipperLeft()
    {
        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            rbFlipperLeft.DORotate(new Vector3(0f, -activatedRotation, 0f), 0.05f, RotateMode.Fast);

            audioSourceFlipperLeft.PlayOneShot(sndFlipperUp);
        }
        if (Input.GetKeyUp(KeyCode.LeftArrow))
        {
            rbFlipperLeft.DORotate(new Vector3(0f, initialRotation, 0f), 0.05f, RotateMode.Fast);

            audioSourceFlipperLeft.PlayOneShot(sndFlipperDown);
        }
    }

    private void ProcessFlipperRight()
    {
        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            rbFlipperRight.DORotate(new Vector3(0f, activatedRotation, 0f), 0.05f, RotateMode.Fast);

            audioSourceFlipperRight.PlayOneShot(sndFlipperUp);
        }
        if (Input.GetKeyUp(KeyCode.RightArrow))
        {
            rbFlipperRight.DORotate(new Vector3(0f, -initialRotation, 0f), 0.05f, RotateMode.Fast);

            audioSourceFlipperRight.PlayOneShot(sndFlipperDown);
        }
    }

    private void ProcessPlunger()
    {
        if (!ball.IsWaitingForLaunch || gameManager.BallsLeft <= 0) return;

        if (Input.GetKeyDown(KeyCode.DownArrow) || Input.GetKeyDown(KeyCode.Space))
        {
            ball.Launch();
        }
    }

    private void ProcessTilt()
    {
        if (ball.IsWaitingForLaunch) return;

        if (Input.GetKeyDown(KeyCode.A))
        {
            Tilt(-1f);
        }
        if (Input.GetKeyDown(KeyCode.E))
        {
            Tilt(1f);
        }
    }

    private void Tilt(float xDirection)
    {
        // Set tilt force
        Vector3 tiltForce = new Vector3(Random.Range(tiltForceMin, tiltForceMax), 0, 0) * xDirection;

        ball.Tilt(tiltForce);

        gameManager.Tilt(tiltForce);
    }

    public void ActivateControls()
    {
        isControlsActivated = true;
    }

    public void DeactivateControls()
    {
        isControlsActivated = false;
    }
}
