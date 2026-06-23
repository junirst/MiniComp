using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCollison : MonoBehaviour
{
    private AnimatorController animatorController;

    private void Start()
    {
        animatorController = GetComponent<AnimatorController>();
        CubeGameManager.Instance.onPlay.AddListener(ActivatePlayer);
    }
    private void ActivatePlayer () 
    {
        gameObject.SetActive(true);
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Obstacle")
        {
            AudioManager.Instance?.PlayHitSfx();
            
            if (animatorController != null)
            {
                animatorController.SetAnimationState(AnimatorController.AnimationState.Dead);
            }
            
            gameObject.SetActive(false);
            CubeGameManager.Instance.GameOver();
        }
    }
}

