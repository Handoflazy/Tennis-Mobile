
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
    [SerializeField] OpponentVariable opponent;
    
    [SerializeField] Transform head;
    [SerializeField] ParticleSystem moveEffect;
    [SerializeField] Animator rangeCircle;
    [SerializeField] private GameObject racket;
    [SerializeField] private GameObject brokenRacket;
    
    
    private float rotation;
    private Transform lookAt;
    private CharacterData data;

    private void Start() {
        if(!transform.root.GetComponent<Player>().isActiveAndEnabled) {
            this.enabled = false;
            return;
        }
        ServiceLocator.For(this).Get(out DataMediator dataMediator);
        data = dataMediator.CharacterData;
        
        ToggleRangeCircle(false);
        moveEffect.Stop();
        lookAt = opponent?.Value?.transform;
    }
    private void LateUpdate() {
        if(lookAt)
            head.LookAt(lookAt.position);
    }

    public void UpdateLocomotionAnim(bool moving, bool right) {
        if(!animator.IsAnimationPlaying(AnimConst.HIT_RIGHT_STATE)&&!animator.IsAnimationPlaying(AnimConst.SERVE_STATE)&&!animator.IsAnimationPlaying(AnimConst.POWER_SERVE_STATE)) {
            animator.PlayAnimation(moving ? (right ? AnimConst.RUN_RIGHT_STATE : AnimConst.RUN_LEFT_STATE) : AnimConst.IDLE_STATE, kCrossFade);
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
        if(rangeCircle.GetBool(AnimConst.SHOW_PARAM) == active) return;
        rangeCircle.SetBool(AnimConst.SHOW_PARAM, active);
    }
    
    public void PlayAnimation(string animationStateName) {
        animator.PlayAnimation(animationStateName, kCrossFade);
    }

    public void PlayBonusEffect() {
        racket.gameObject.SetActive(false);
        brokenRacket.gameObject.SetActive(true);
    }
    
}