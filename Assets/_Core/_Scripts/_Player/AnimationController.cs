
using _Core._Scripts.Utilities.Extensions;
using DG.Tweening;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Serialization;
using UnityServiceLocator;
using Utilities.Extensions;

public class AnimationController: MonoBehaviour {
    
    [SerializeField] private Animator animator;
    [SerializeField] private float kCrossFade = 0.1f;
    [SerializeField] Transform arrowHolder;
    [SerializeField] Animator arrowAnimator;
    
    [SerializeField] Transform head;
    [SerializeField] Transform lookAt;
    [SerializeField] ParticleSystem moveEffect;
    [SerializeField] Animator rangeCircle;
    [SerializeField] private GameObject racket;
    [SerializeField] private GameObject brokenRacket;
    
    
    private float rotation;
    private PlayerData data;

    private void Start() {
        ServiceLocator.For(this).Get(out DataMediator dataMediator);
        data = dataMediator.PlayerData;
        
        ToggleRangeCircle(false);
        moveEffect.Stop();
    }
    private void LateUpdate() {
        if(lookAt)
            head.LookAt(lookAt.position);
    }

    public void UpdateLocomotionAnim(bool moving, bool right) {
        if(!animator.IsAnimationPlaying(AnimConst.HitRightState)&&!animator.IsAnimationPlaying(AnimConst.ServeState)&&!animator.IsAnimationPlaying(AnimConst.PowerServeState)) {
            animator.PlayAnimation(moving ? (right ? AnimConst.RunRightState : AnimConst.RunLeftState) : AnimConst.IdleState, kCrossFade);
        }
        if (!moving) {
            rotation = 180;
            if (moveEffect.isPlaying) moveEffect.Stop();
        }
        else {
            rotation = right ? -90 : 91;
            if (!moveEffect.isPlaying) moveEffect.Play();
        }
        if (Mathf.Abs(rotation - transform.eulerAngles.y) >= 5) {
            Vector3 rot = transform.eulerAngles.With(y: rotation);
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.Euler(rot), Time.deltaTime * data.TurnSpeed);
        }
        
    }
    public void SetArrowDirection(Vector3 direction) {
        arrowAnimator.Play("Arrow pop up");
        arrowHolder.LookAt(direction);
    }
    public void ToggleRangeCircle(bool active){
        if(rangeCircle.GetBool(AnimConst.ShowParam) == active) return;
        rangeCircle.SetBool(AnimConst.ShowParam, active);
    }
    
    public void PlayAnimation(string animationStateName) {
        animator.PlayAnimation(animationStateName, kCrossFade);
    }

    public void PlayBonusEffect() {
        racket.gameObject.SetActive(false);
        brokenRacket.gameObject.SetActive(true);
    }
    
}