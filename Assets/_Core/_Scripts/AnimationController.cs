using _Core._Scripts;
using UnityEngine;
using UnityEngine.UI;
using UnityServiceLocator;

public class AnimationController: MonoBehaviour {
    
    public Animator anim;
    public Transform head;
    public Transform lookAt;
    public ParticleSystem moveParticles;
    public Transform ball;
    public Animator indicator;
    public Animator rangeCircle;
    public Image indicatorFill;
    public Transform opponent;
    public Transform ballPosition;
    
    [HideInInspector]
    public Animator shootTip;
    public Transform arrowHolder;
    public Animator playerArrow;
    
    
    Vector3 startPos;
    Vector3 startVelocity;
    float startTime;
    float powerbarSpeed;
	
    bool right;
    bool moving;
    
    float lastDist;
    float delta;
    
    bool max;
	
    float lastFillAmount;
	
    Vector3 target;
	
    bool canShoot;
	
    float rotation;
	
    bool shoot = true;

    private GameManager gameManager;

    private void Start() {
        ServiceLocator.ForSceneOf(this).Get(out gameManager);
    }
    public void ToggleRangeCircle(bool show) {
        rangeCircle.SetBool(AnimConst.ShowParam, show);
    }
    public void StopMoveParticles() {
		moveParticles.Stop();
	}
}