using System;
using _Core._Scripts;
using UnityEngine;
using UnityServiceLocator;
using Utilities.Extensions;

public class RotateReferee : MonoBehaviour
{
    GameManager gameManager;
    private Transform ball;
    private void Start() {
        ServiceLocator.ForSceneOf(this).Get(out gameManager);
    }

    void Update() {
        if(ball == null && gameManager.ballScript != null) {
            {
                ball = gameManager.ballScript.transform;
            }
        }
        LookAtBall();
    }

    void LookAtBall() {
        if(ball == null)
            return;
        transform.LookAt(ball.transform.position.With(y: transform.position.y));
    }
}