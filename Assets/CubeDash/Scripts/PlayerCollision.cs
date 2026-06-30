using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCollison : MonoBehaviour
{
    private AnimatorController animatorController;
    private PlayerMovement playerMovement;
    private Rigidbody2D playerRigidbody;
    private Collider2D playerCollider;
    private bool isDying = false;

    private void Start()
    {
        animatorController = GetComponent<AnimatorController>();
        playerMovement = GetComponent<PlayerMovement>();
        playerRigidbody = GetComponent<Rigidbody2D>();
        playerCollider = GetComponent<Collider2D>();
        CubeGameManager.Instance.onPlay.AddListener(ActivatePlayer);
    }
    private void ActivatePlayer () 
    {
        isDying = false;

        if (animatorController != null)
        {
            animatorController.ResetAnimationState(AnimatorController.AnimationState.Idle);
        }

        if (playerMovement != null)
        {
            playerMovement.enabled = true;
        }

        if (playerRigidbody != null)
        {
            playerRigidbody.simulated = true;
            playerRigidbody.velocity = Vector2.zero;
        }

        if (playerCollider != null)
        {
            playerCollider.enabled = true;
        }

        gameObject.SetActive(true);
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (isDying || collision.gameObject.tag != "Obstacle")
        {
            return;
        }

        isDying = true;
        AudioManager.Instance?.PlayHitSfx();

        if (playerMovement != null)
        {
            playerMovement.enabled = false;
        }

        if (playerRigidbody != null)
        {
            playerRigidbody.velocity = Vector2.zero;
            playerRigidbody.simulated = false;
        }

        if (playerCollider != null)
        {
            playerCollider.enabled = false;
        }

        if (animatorController != null)
        {
            animatorController.LockAnimationState(AnimatorController.AnimationState.Dead);
        }

        StartCoroutine(HandleDeath());
        CubeGameManager.Instance.GameOver();
    }

    private IEnumerator HandleDeath()
    {
        float deathDuration = 0.4f;

        if (animatorController != null)
        {
            deathDuration = animatorController.GetAnimationDuration(AnimatorController.AnimationState.Dead);
        }

        yield return new WaitForSeconds(deathDuration);
        gameObject.SetActive(false);
    }
}

