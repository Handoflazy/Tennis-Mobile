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
    [HideInInspector]
    public bool inactive;
    private bool playerHit;
    private GameManager gameManager;
    
    private readonly int pausedHash = Animator.StringToHash("Paused");

    private void Start() {
        flames.SetActive(false);
        ServiceLocator.ForSceneOf(this).Get(out gameManager);
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
    public bool GetLastHit() {
        return playerHit;
    }
    public void SetKinematic(bool value) {
        rb.isKinematic = value;
        circle.SetBool(pausedHash, value);
    }

    public Vector3 Velocity {
        get => rb.velocity;
        set => rb.velocity = value;
    }

    private void Out() {
        Instantiate(wrongSlideEffect,transform.position - Vector3.up*offset,wrongSlideEffect.transform.rotation);
        gameManager.Out();
    }

    void Grown() => transform.DOScale(0.7F, 0.2F);

    public void Flames() {
        GetComponent<MeshRenderer>().material = flameMat;
        flames.SetActive(true);
        circle.gameObject.SetActive(false);
        Grown();
    }
    public bool IsFlame() {
        return flames.activeSelf;
    }
}
