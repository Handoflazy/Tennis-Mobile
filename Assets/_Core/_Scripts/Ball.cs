using System;
using _Core._Scripts;
using DG.Tweening;
using UnityEngine;
using UnityServiceLocator;

public class Ball : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Rigidbody rb;
    [SerializeField] private GameObject ballEffect;
    [SerializeField] private float offset;
    [SerializeField] private Animator circle;
    [SerializeField] private GameObject flames;
    [Header("Prefabs")]
    [SerializeField] private Material flameMat;
    [SerializeField] private GameObject brokenFloor;
    [SerializeField] private GameObject wrongSlideEffect;

    private bool inactive;
    private bool playerHit;
    private GameManager gameManager;
    
    private int pausedHash = Animator.StringToHash("Paused");

    private void Start() {
        flames.SetActive(false);
        ServiceLocator.ForSceneOf(this).Get<GameManager>(out gameManager);
    }

    private void Update() {
        if(circle.GetBool(pausedHash) != rb.isKinematic)
            circle.SetBool(pausedHash, rb.isKinematic);
    }

    private void OnCollisionEnter(Collision other) {
        if(!other.gameObject.CompareTag("Ground"))
            return;
        if(flames.activeSelf) {
            Instantiate(brokenFloor,transform.position - Vector3.up*4*offset,brokenFloor.transform.rotation);
            gameManager.FireBall();
            Destroy(gameObject);
        }
        if(inactive)
            return;
        if(playerHit&& transform.position.z>3.75f) {
            Out();
            return;
        }
        Instantiate(ballEffect,transform.position - Vector3.up*offset,ballEffect.transform.rotation);

    }

    public void SetLastHit(bool player) {
        playerHit = player;
    }

    private void Out() {
        Instantiate(wrongSlideEffect,transform.position - Vector3.up*offset,wrongSlideEffect.transform.rotation);
        gameManager.Out();
    }

    void Grown() {
        transform.DOScale(0.7F, 0.2F);
    }

    public void Flames() {
        flames.SetActive(true);
        GetComponent<MeshRenderer>().material = flameMat;
        circle.gameObject.SetActive(false);
        Grown();
    }
}
