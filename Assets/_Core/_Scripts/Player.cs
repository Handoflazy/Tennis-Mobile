using System;
using System.Collections;
using _Core._Scripts;
using DG.Tweening;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;
using UnityServiceLocator;
using Utilities.Extensions;
using Utilities.ImprovedTimers;
using Random = UnityEngine.Random;

public class Player : MonoBehaviour
{
    [SerializeField] private PlayerData data;
    [SerializeField] private PlayerSoundPlayer soundPlayer;
    [SerializeField] private UIManager uiManager;
    [SerializeField] private float K_crossFade = 0.1f;
    [SerializeField] private CameraMovement cameraMovement;

    public Animator anim;
    public Transform head;
    public Transform lookAt;
    public ParticleSystem moveParticles;
    public Ball ball;
    public Animator indicator;
    public Animator rangeCircle;
    public Image indicatorFill;
    public Transform opponent;
    public Transform ballPosition;

    [HideInInspector] public Animator comboLabel;
    [HideInInspector] public Text comboNumberLabel;
    [HideInInspector] public Animator swipeLabel;

    private bool shoot;
    private bool canShoot;
    private float rotation;
    private float powerBarSpeed;
    [SerializeField] private bool showBar;
    private Vector3 targetPosition;
    private float lastFillAmount;
    private Vector3 startPos;
    private Vector3 startVelocity;
    [SerializeField] private float maxDragTime;
    [SerializeField] private float dragDistance;

    private CountdownTimer dragTimer;
    private bool max;
    public bool right;
    public bool moving;
    private GameManager gameManager;
    private float delta;
    private string currentAnimState;

    private void Start()
    {
        ServiceLocator.ForSceneOf(this).Get(out gameManager);
        uiManager.HideGamePanel();
        rangeCircle.SetBool(AnimConst.ShowParam, false);
        moveParticles.Stop();

        powerBarSpeed = Random.Range(data.PowerbarSpeedMin, data.PowerbarSpeedMax);
        dragTimer = new CountdownTimer(maxDragTime);
        dragTimer.OnTimerStop += ResetShot;
    }

    private void Update()
    {
        Move();
        HandlePowerBar();
        if (!ball && !showBar) return;
        CheckBallRange();
        if (rangeCircle.GetBool(AnimConst.ShowParam)) Shoot();
    }

    private void LateUpdate()
    {
        //head.LookAt(lookAt.position);
    }

    private void CheckBallRange()
    {
        float zDistance = ball == null ? 0 : Mathf.Abs(transform.position.z - ball.gameObject.transform.position.z);
        if (showBar || (zDistance < data.BallRange && !ball.inactive))
        {
            CanHitBall();
        }
        else if (rangeCircle.GetBool(AnimConst.ShowParam))
        {
            rangeCircle.SetBool(AnimConst.ShowParam, false);
        }
    }

    private void CanHitBall()
    {
        if (rangeCircle.GetBool("Show")) return;
        rangeCircle.SetBool("Show", true);
    }

    private void Move()
    {
        if (targetPosition == Vector3.zero) return;
        float dist = Vector3.Distance(transform.position, targetPosition);
        moving = dist > 0.1f && !rangeCircle.GetBool(AnimConst.ShowParam);

        if (moving)
        {
            transform.position = Vector3.MoveTowards(transform.position, targetPosition, Time.deltaTime * data.Speed);
            right = targetPosition.x < transform.position.x;
        }

        if (!moving)
        {
            rotation = 180;
            if (moveParticles.isPlaying) moveParticles.Stop();
        }
        else
        {
            rotation = right ? -90 : 91;
            if (!moveParticles.isPlaying) moveParticles.Play();
        }

        PlayAnimation(moving ? (right ? AnimConst.RunRightState : AnimConst.RunLeftState) : AnimConst.IdleState);

        if (Math.Abs(rotation - transform.eulerAngles.y) >= 5)
        {
            Vector3 rot = transform.eulerAngles.With(y: rotation);
            transform.DORotate(rot, 10).SetEase(Ease.InSine);
        }
    }

    private void Shoot()
    {
        Vector3 currentPos = Input.mousePosition;
        if (Input.GetMouseButtonDown(0) && shoot)
        {
            canShoot = true;
            lastFillAmount = showBar ? indicatorFill.fillAmount : -1;
            startPos = currentPos;
            dragTimer.Start();
            if (ball)
            {
                startVelocity = ball.Velocity;
                ball.SetKinematic(true);
            }
        }
        else if (Input.GetMouseButton(0) && canShoot)
        {
            if (Vector3.Distance(startPos, currentPos) > dragDistance) Hit(currentPos);
        }
        else if (Input.GetMouseButtonUp(0) && canShoot)
        {
            ResetShot();
        }
    }

    private void ResetShot()
    {
        canShoot = false;
        if (lastFillAmount < 0)
        {
            ball.SetKinematic(false);
            ball.Velocity = startVelocity;
        }
    }

    private IEnumerator PowerBarMax()
    {
        max = true;
        indicator.SetTrigger(AnimConst.MaxParam);
        yield return new WaitForSeconds(0.3f);
        max = false;
    }

    private void PlayAnimation(string stateAnim)
    {
        if (currentAnimState == stateAnim) return;
        currentAnimState = stateAnim;
        anim.CrossFade(stateAnim, K_crossFade);
    }

    private void HandlePowerBar()
    {
        if (showBar)
        {
            if (!indicator.GetBool(AnimConst.ActiveParam))
            {
                indicator.SetBool(AnimConst.ActiveParam, true);
                indicatorFill.fillAmount = 0;
                delta = 1f;
                //cameraMovement.Zoom(true);
            }

            float fillSpeed = indicatorFill.fillAmount < 0.2f ? 0.2f : indicatorFill.fillAmount;
            if (indicatorFill.fillAmount > 0.95f && delta > 0) fillSpeed /= data.PowerbarMaxSlowdown;
            if (indicatorFill.fillAmount > 0.8f && !max) StartCoroutine(PowerBarMax());

            indicatorFill.fillAmount += Time.deltaTime * powerBarSpeed * delta * fillSpeed;
            if ((delta < 0 && indicatorFill.fillAmount < 0.03f) || (delta > 0 && indicatorFill.fillAmount > 0.99f)) delta *= -1f;
        }
        else
        {
            if (indicator.GetBool(AnimConst.ActiveParam)) indicator.SetBool(AnimConst.ActiveParam, false);
        }
    }

    private void Hit(Vector3 currentPos) {
        // Implement hit logic here
    }
}