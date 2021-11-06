using UnityEngine;

public class Ball : MonoBehaviour
{
    [SerializeField] Vector3 resetPosition;
    [SerializeField] float launchForce = 2400f;

    [SerializeField] AudioClip sndPlunger;
    [SerializeField] AudioClip sndCollision;

    Rigidbody rb;
    AudioSource audioSource;

    GameManager gameManager;

    bool isWaitingForLaunch;
    public bool IsWaitingForLaunch { get { return isWaitingForLaunch; } }

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        audioSource = GetComponent<AudioSource>();

        gameManager = FindObjectOfType<GameManager>();

        isWaitingForLaunch = true;

        Freeze();
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.name == "WallFront")
        {
            gameManager.LostBall();

            if (gameManager.BallsLeft > 0)
            {
                Reset();
            }
            else
            {
                Destroy(gameObject);
            }
        }

        // PlayCollisionSound(); Mmmmhhh...
    }

    void Freeze()
    {
        rb.constraints = RigidbodyConstraints.FreezeAll;
    }

    void Unfreeze()
    {
        rb.constraints = RigidbodyConstraints.None;
    }

    private void Reset()
    {
        // Reset ball position
        transform.position = resetPosition;

        // Stop the ball
        rb.velocity = Vector3.zero;

        // Freeze the ball
        Freeze();

        // Waiting for new launch
        isWaitingForLaunch = true;
    }

    public void Launch()
    {
        // Unfreeze the ball
        Unfreeze();

        // Add (to fix) "vertical" force to launch the ball by simulating plunger
        rb.AddForce(new Vector3(0f, 0f, launchForce));

        // Launched
        isWaitingForLaunch = false;

        // Play plunger sound
        if (audioSource.isPlaying) audioSource.Stop();
        audioSource.PlayOneShot(sndPlunger);
    }

    private void PlayCollisionSound()
    {
        // If audio source is playing stop it
        if (audioSource.isPlaying) audioSource.Stop();

        // Play the collision sound
        audioSource.PlayOneShot(sndCollision);
    }

    public void Tilt(Vector3 direction)
    {
        rb.AddForce(direction, ForceMode.Impulse);
    }
}
