using System.Collections;
using _Core._Scripts;
using Obvious.Soap;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;
using UnityServiceLocator;
using Utilities.Extensions;
using Utilities.ImprovedTimers;
using Random = UnityEngine.Random;

public class Player : MonoBehaviour
{
    [FoldoutGroup("Elements"), SerializeField] private PlayerData data;
    [FoldoutGroup("Elements"), SerializeField] private SoundPlayer soundPlayer;
    [FoldoutGroup("Elements"), SerializeField] private AnimationController anim;
    [FoldoutGroup("Elements"), SerializeField] private BallSensor sensor;
    [FoldoutGroup("Elements"), SerializeField] private PlayerUI playerUI;

    [Space(10)]
    [Header("Scriptable Variables")]
    [SerializeField] private FloatVariable indicatorValue;
    [SerializeField] private ScriptableEventVector3 ballTargetPosition;
    [SerializeField] private BallVariable ball;

    [HideInInspector] public GameObject matchLabel;
    [HideInInspector] public Animator comboLabel;
    [HideInInspector] public TextMeshProUGUI comboNumberLabel;
    [HideInInspector] public Animator swipeLabel;

    private bool shoot = true;
    private Vector3 targetPosition;
    private Vector3 startPos;
    private Vector3 startVelocity;

    private bool canShoot;
    [ShowInInspector] private bool serveShot;
    private bool right;
    private bool moving;

    private CountdownTimer dragTimer;
    private GameManager gameManager;
    private Opponent opponent;
    private CameraMovement cam;

    private const float ServeAnimWaitTime = 0.28f;
    private const float DisableShootingWaitTime = 0.7f;
    private const float ShakeDuration = 0.12f;
    private const float ShakeMagnitude = 0.5f;

    private void Awake()
    {
        ServiceLocator.ForSceneOf(this).Register(this);
    }

    private void Start() {
        ServiceLocator.ForSceneOf(this).Get(out gameManager);
        ServiceLocator.ForSceneOf(this).Get(out opponent);
        ServiceLocator.ForSceneOf(this).Get(out cam);
        dragTimer = new CountdownTimer(data.MaxDragTime);
    }

    private void Update()
    {
        Move();
        if (serveShot || (sensor.IsBallInRange && !ball.Value.inactive)) Shoot();
    }

    private void Move()
    {
        if (targetPosition == Vector3.zero) return;
        float dist = Vector3.Distance(transform.position, targetPosition);
        moving = dist > 0.1f && !sensor.IsBallInRange;
        if (moving)
        {
            transform.position = Vector3.MoveTowards(transform.position, targetPosition, Time.deltaTime * data.Speed);
            right = targetPosition.x < transform.position.x;
        }
        anim.UpdateLocomotionAnim(moving, right);
    }

    private void Shoot()
    {
        Vector3 currentPos = Input.mousePosition;
        if (Input.GetMouseButtonDown(0) && shoot)
        {
            canShoot = true;
            startPos = currentPos;
            dragTimer.Start();
            if (ball.Value)
            {
                startVelocity = ball.Value.Velocity;
                ball.Value.SetKinematic(true);
            }
        }
        else if (Input.GetMouseButton(0) && canShoot)
        {
            if (dragTimer.IsFinished) ResetShot();
            else if (Vector3.Distance(startPos, currentPos) > data.DragDistance) Hit(currentPos);
        }
        else if (Input.GetMouseButtonUp(0) && canShoot)
        {
            ResetShot();
        }
    }

    private void ResetShot()
    {
        canShoot = false;
        if (!serveShot)
        {
            ball.Value.SetKinematic(false);
            ball.Value.Velocity = startVelocity;
        }
    }

    private void Hit(Vector3 currentPos)
    {
        var random = CalculateNextPosition(currentPos);
        var direction = (random - transform.position).normalized;
        if (serveShot)
            HandleServeShot(direction);
        else
            HandleRegularShot(direction);
        if (gameManager.bonus && gameManager.bonusDiamonds >= 2 + PlayerPrefs.GetInt("Bonus max"))
        {
            anim.PlayBonusEffect();
        }
        ball.Value.SetLastHit(true);
        ballTargetPosition.Raise(random);
        if (!cam.enabled)
        {
            gameManager.StartGame();
            cam.enabled = true;
            cam.Zoom(true);
        }
        canShoot = false;
        StartCoroutine(DisableShooting());
    }

    private Vector3 CalculateNextPosition(Vector3 currentPos)
    {
        float deltaX = currentPos.x - startPos.x;
        float xPos = Mathf.Clamp(data.SwipeSensitivity * -deltaX, -data.MoveRange, data.MoveRange);
        return new Vector3(xPos, 0, opponent.gameObject.transform.position.z);
    }

    private void HandleServeShot(Vector3 direction)
    {
        gameManager.Serve();
        SetServe(false);
        StartCoroutine(ServeAnim(direction, indicatorValue > 0.8f));
    }

    private void HandleRegularShot(Vector3 direction)
    {
        anim.PlayAnimation(AnimConst.HitRightState);
        ball.Value.Velocity = direction * data.Force + Vector3.up * data.UpForce;
        soundPlayer.PlayHitBallSound();
        ball.Value.SetKinematic(false);
        StartCoroutine(cam.Shake(ShakeDuration, ShakeMagnitude));
    }

    private IEnumerator ServeAnim(Vector3 direction, bool powerShot)
    {
        anim.PlayAnimation(powerShot ? AnimConst.PowerServeState : AnimConst.ServeState);
        yield return new WaitForSeconds(ServeAnimWaitTime);
        float barForce = data.Force * 0.8f + (data.Force * indicatorValue * 0.3f);
        ball.Value.Velocity = direction * barForce + Vector3.up * data.UpForce;
        playerUI.ShowText(indicatorValue);

        if (indicatorValue > 0.95f && !gameManager.bonus)
        {
            playerUI.PlayDiamondEffect();
            PlayerPrefs.SetInt("Diamonds", PlayerPrefs.GetInt("Diamonds") + 1);
        }

        soundPlayer.PlayHitBallSound();
        ball.Value.SetKinematic(false);
        StartCoroutine(cam.Shake(ShakeDuration, ShakeMagnitude));
    }

    private IEnumerator DisableShooting()
    {
        shoot = false;
        yield return new WaitForSeconds(DisableShootingWaitTime);
        shoot = true;
    }

    public void SetServe(bool isServing)
    {
        if (isServing)
            playerUI.ShowIndicator(Random.Range(data.PowerBarSpeedMin, data.PowerBarSpeedMax));
        else
            playerUI.HideIndicator();
        serveShot = isServing;
        cam?.Zoom(isServing);
        anim.ToggleRangeCircle(isServing);
    }

    public void SetTargetPosition(Vector3 target) => targetPosition = target;

    public void ComboDone(Ball ball)
    {
        if (ball)
        {
            ball.Flames();
            soundPlayer.PlayFireSound();
        }
    }

    public void Reset()
    {
        SetTargetPosition(transform.position.With(x: 0));
        ball.Reset();
        ComboDone(null);
    }
}
