
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
    
    public bool inactive;
    private bool playerHit;
    private GameManager gameManager;
    private GameEffectManager gameEffectManager;
    
    Vector3 lastVelocity;
    
    private readonly int pausedHash = Animator.StringToHash("Paused");

    private void Start() {
        flames.SetActive(false);
        ServiceLocator.ForSceneOf(this).Get(out gameManager);
    }

    private void OnCollisionEnter(Collision other) {
        if(gameEffectManager == null)
            ServiceLocator.ForSceneOf(this).Get(out gameEffectManager);
        if(!other.gameObject.CompareTag("Ground"))
            return;
        if(flames.activeSelf) {
            gameEffectManager.BrokenFloor(transform.position - Vector3.up*offset*4);
            gameManager.FireBall();
            Destroy(gameObject);
        }
        if(inactive)
            return;
        if(playerHit&& transform.position.z>3.75f) {
            Out();
            return;
        }
        gameEffectManager.BallEffect(transform.position - Vector3.up*offset);

    }

    public void SetLastHit(bool player) {
        playerHit = player;
    }
    public bool GetLastHit() {
        return playerHit;
    }
    public void Frozen(bool value, bool applyLastVelocity = false) {
        if(value) {
            lastVelocity = rb.velocity;
        }
        else if(applyLastVelocity) {
            rb.velocity = lastVelocity;
        }
        rb.isKinematic = value;
        circle.SetBool(pausedHash, value);
    }

    public Vector3 Velocity {
        get => rb.velocity;
        set => rb.velocity = value;
    }

    private void Out() {
        gameEffectManager.BallEffect(transform.position - Vector3.up*offset);
        gameManager.Out();
        Destroy(gameObject);
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
