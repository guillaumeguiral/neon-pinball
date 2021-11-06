using UnityEngine;
using DG.Tweening;

public class Bumper : MonoBehaviour
{
    [SerializeField] MeshRenderer meshRenderer;
    Color initialColor;

    [SerializeField] Animator myAnimator;

    [SerializeField] AudioClip[] sndBumps;
    AudioSource audioSource;

    [SerializeField] int hitScore;

    Scoreboard scoreboard;

    void Start()
    {
        initialColor = meshRenderer.material.color;

        audioSource = GetComponent<AudioSource>();

        scoreboard = FindObjectOfType<Scoreboard>();
    }

    private void OnCollisionEnter(Collision collision)
    {
        // Animate the bumper (color and mesh animation)
        AnimateBumper();

        // Repule the ball
        RepulseBall(collision);
        
        // Play bumper sound
        PlayBumperSound();

        // Add hit score to scoreboard
        scoreboard.AddToScore(hitScore);
    }

    private void AnimateBumper()
    {
        // Reset bumper initial color
        meshRenderer.material.color = initialColor;

        // Add color small transition
        meshRenderer.material.DOColor(Color.red, 0.2f).From();

        // Trigger animation of the bumper
        myAnimator.SetTrigger("isBumping");
    }

    private void RepulseBall(Collision collision)
    {
        // Get direction from the ball to the bumper
        Vector3 direction = (transform.position - collision.transform.position).normalized;

        // Prevent from going to the moon
        direction.y = 0;

        // "Repulse" the ball in the opposite direction of the bumper
        collision.rigidbody.AddForce(-direction * 30f, ForceMode.Impulse);
    }

    private void PlayBumperSound()
    {
        // Get random bumper sound from the list
        int randomSoundIndex = Random.Range(0, sndBumps.Length - 1);

        // If audio source is playing stop it
        if (audioSource.isPlaying) audioSource.Stop();

        // Finally, play the random bumper sound
        audioSource.PlayOneShot(sndBumps[randomSoundIndex]);
    }
}
