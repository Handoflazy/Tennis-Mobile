using System;
using UnityEngine;

public class FaceCamera : MonoBehaviour {
  
    Camera mainCamera;

    private void Start() {
        mainCamera = Camera.main;
    }

    void Update(){
        //face the direction of the main camera
        transform.LookAt(2 * mainCamera.transform.position - transform.position);
    }
}