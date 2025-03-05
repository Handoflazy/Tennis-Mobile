using System.Collections;
using _Core._Scripts.Utilities.Extensions;
using UnityEngine;
using UnityServiceLocator;
using Utilities.Extensions;
using Random = UnityEngine.Random;

public class Opponent : MonoBehaviour
{
    [Header("Elements")]
    [SerializeField] private PlayerVariable player;
    [SerializeField] private SoundPlayer soundPlayer;
    [SerializeField] private CharacterData data;
    [SerializeField] private BallVariable ball;

    [Header("For Animation")]
    [SerializeField] private Transform servePoint;
    [SerializeField] private Animator anim;
    [SerializeField] private Transform head;
    [SerializeField] private Transform lookAt;
    [SerializeField] private ParticleSystem moveParticles;
    [SerializeField] private Animator arrow;
    [Header("Settings")]
    [SerializeField] private bool followBall;

    private Vector3 target;
    private Vector3 serveStart;
    private bool notRotating;
    private bool right;
    private bool moving;
    private bool justHit;
    private float lastDist;
    private float rotation;

    private const float MoveThreshold = 0.1f;
    private const float HitWaitTime = 1f;
    private const float MaxXDistance = 1.3f;
    private const float RotationRight = 91f;
    private const float RotationLeft = -90f;
    private void Start()
    {
        lookAt = player.Value.transform;
        moveParticles.Stop();
        serveStart = servePoint.position;
    }

    private void Update() => Move();

    public void PlayerServeAnimation() {
        anim.PlayAnimation(AnimConst.ServeState);
    }
    private void LateUpdate()
    {
        head.LookAt(lookAt.position);
        UpdateServePointPosition();
    }

    private void Move()
    {
        if (target == Vector3.zero) return;

        float dist = Vector3.Distance(transform.position, target);
        moving = dist > MoveThreshold;

        if (moving)
        {
            MoveTowardsTarget();
            right = transform.InverseTransformPoint(target).x > 0;
        }

        UpdateRotationAndParticles();
        UpdateAnimator();
    }

    private void MoveTowardsTarget() {
        if (!followBall || ball == null) {
            transform.position = Vector3.MoveTowards(transform.position, target, Time.deltaTime * data.Speed);
        }
        else {
            Vector3 pos = transform.position;
            bool move = ball.Value.GetLastHit();
            Vector3 ballTarget = move ? new Vector3(ball.Value.gameObject.transform.position.x, pos.y, pos.z) : pos;
            transform.position = Vector3.MoveTowards(transform.position, ballTarget, Time.deltaTime * data.Speed);
        }
    }

    private void UpdateRotationAndParticles() {
        if (!moving) {
            rotation = 0;
            if (moveParticles.isPlaying) moveParticles.Stop();
        }
        else {
            rotation = right ? RotationRight : RotationLeft;
            if (!moveParticles.isPlaying) moveParticles.Play();
        }

        if (!notRotating) {
            Vector3 rot = transform.eulerAngles;
            rot.y = rotation;
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.Euler(rot), Time.deltaTime * data.TurnSpeed);
        }
    }

    private void UpdateAnimator()
    {
        if (!anim.IsAnimationPlaying(AnimConst.ServeState) && anim.GetCurrentAnimatorStateInfo(0).normalizedTime > 0.6f)
        {
            anim.PlayAnimation(moving ? (right ? AnimConst.RunRightState : AnimConst.RunLeftState) : AnimConst.IdleState);
        }
    }

    private void UpdateServePointPosition() => servePoint.position = serveStart.With(x:transform.position.x );

    private void OnTriggerEnter(Collider other)
    {
        if (!other.gameObject.CompareTag("Ball") || ball.Value.inactive || justHit) return;

        float xDistance = Mathf.Abs(transform.position.x - other.gameObject.transform.position.x);

        if (xDistance > MaxXDistance)
        {
            if (Random.Range(0, 2) == 0)
                anim.PlayAnimation(AnimConst.HitRightState);
            return;
        }

        StartCoroutine(JustHit());
        anim.PlayAnimation(AnimConst.HitRightState);
        HitBall(false, null);
    }

    public void HitBall(bool noFlame, Transform spawnPosition)
    {
        soundPlayer.PlayHitBallSound();

        Vector3 random = new Vector3(Random.Range(-data.MoveRange, data.MoveRange), 0, player.Value.gameObject.transform.position.z);

        if (ball.Value.IsFlame() && !noFlame) return;

        ball.Value.SetLastHit(false);

        Vector3 direction = (random - transform.position).normalized;
        player.Value.SetTargetPosition(random);

        ball.Value.transform.position = spawnPosition == null ? servePoint.position : spawnPosition.position;
        ball.Value.Velocity = direction * data.Force + Vector3.up * data.UpForce;
    }

    public IEnumerator JustHit()
    {
        justHit = true;
        yield return new WaitForSeconds(HitWaitTime);
        justHit = false;
    }

    public void SetTargetPosition(Vector3 pos) => target = pos;

    public void Reset() => SetTargetPosition(transform.position.With(x: 0));
}