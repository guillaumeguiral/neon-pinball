using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Panel : MonoBehaviour
{
    [SerializeField] float multiplierBonus = 0.1f;
    [SerializeField] float minMagnitudeToTrigger = 10f;
    [SerializeField] float delayBeforeReactivate = 20f;

    [SerializeField] AudioClip sndCollision;

    Animator animator;
    BoxCollider boxCollider;
    AudioSource audioSource;

    GameManager gameManager;

    private void Start()
    {
        animator = GetComponent<Animator>();
        boxCollider = GetComponent<BoxCollider>();
        audioSource = GetComponent<AudioSource>();

        gameManager = FindObjectOfType<GameManager>();
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.impulse.magnitude > minMagnitudeToTrigger)
        {
            PlayCollisionSound();

            StartCoroutine(DeactivateCollider());

            animator.SetBool("isTouched", true);

            gameManager.IncreaseMultiplier(multiplierBonus);

            StartCoroutine(RectivatePanel());
        }
    }

    private void PlayCollisionSound()
    {
        // If audio source is playing stop it
        if (audioSource.isPlaying) audioSource.Stop();

        // Play collision sound
        audioSource.PlayOneShot(sndCollision);
    }

    IEnumerator DeactivateCollider()
    {
        yield return new WaitForSeconds(0.2f);

        boxCollider.enabled = false;
    }

    IEnumerator RectivatePanel()
    {
        yield return new WaitForSeconds(delayBeforeReactivate);

        animator.SetBool("isTouched", false);
        boxCollider.enabled = true;
    }
}
