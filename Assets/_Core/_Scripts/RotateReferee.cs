using System;
using _Core._Scripts;
using UnityEngine;
using UnityServiceLocator;
using Utilities.Extensions;

public class RotateReferee : MonoBehaviour
{
    [SerializeField] BallVariable ball;

    void Update() {
        LookAtBall();
    }

    void LookAtBall() {
        if(ball.Value == null)
            return;
        transform.LookAt(ball.Value.transform.position.With(y: transform.position.y));
    }
}