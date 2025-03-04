using System;
using System.Collections;
using System.Collections.Generic;
using _Core._Scripts;
using _Core._Scripts.Utilities.Extensions;
using UnityEngine.UI;
using UnityEngine;
using UnityServiceLocator;
using Utilities.Extensions;
using Random = UnityEngine.Random;

public class Opponent : MonoBehaviour {
	
	public float speed;
	public float turnSpeed;
	public Animator anim;
	public Transform head;
	public Transform lookAt;
	public ParticleSystem moveParticles;
	public BallVariable ball;
	public Animator arrow;

	public float force;
	public float upForce;
	public Player player;
	public float moveRange;
	public Transform servePoint;
	[SerializeField] SoundPlayer soundPlayer;
	
	public bool notRotating;
	
	//directly follow ball rather then using the target position
	public bool followBall;
	
	Vector3 target;
	
	bool right;
	bool moving;
	
	bool justHit;
	
	float lastDist;
	Vector3 serveStart;
	
	float rotation;

	private void Awake() {
	    ServiceLocator.ForSceneOf(this).Register(this);
	}

	void Start() {
	    ServiceLocator.ForSceneOf(this).Get(out player);
		moveParticles.Stop();
		serveStart = servePoint.position;
    }
    void Update(){
		//check if character should move
		Move();
    }
	
	void LateUpdate(){
		//look at the opponent
		head.LookAt(lookAt.position);
		
		//keep the serve position in the same place after moving bones
		Vector3 pos = serveStart;
		pos.x = transform.position.x;
		servePoint.position = pos;
	}
	
	void Move(){
		//either move to the target, or directly follow the ball x position
		if(target == Vector3.zero)
			return;
		
		float dist = Vector3.Distance(transform.position, target);
		moving = dist > 0.1f;
		
		if(moving){
			if(!followBall || ball == null){
				transform.position = Vector3.MoveTowards(transform.position, target, Time.deltaTime * speed);
			}
			else{
				Vector3 pos = transform.position;
				bool move = ball.Value.GetLastHit();
				Vector3 ballTarget = move ? new Vector3(ball.Value.gameObject.transform.position.x, pos.y, pos.z) : pos;
				transform.position = Vector3.MoveTowards(transform.position, ballTarget, Time.deltaTime * speed);
			}

			right = target.x > transform.position.x;
		}
		if(!moving){
			rotation = 0;
			
			if(moveParticles.isPlaying)
				moveParticles.Stop();
		}
		else{
			if(right){
				rotation = 91;
			}
			else{
				rotation = -90;
			}
			
			if(!moveParticles.isPlaying)
				moveParticles.Play();
		}
		
		//update the animator value to display the correct animation
		if(!anim.IsAnimationPlaying(AnimConst.ServeState)&&anim.GetCurrentAnimatorStateInfo(0).normalizedTime>0.6f)
			anim.PlayAnimation(moving ? (right ? AnimConst.RunRightState : AnimConst.RunLeftState) : AnimConst.IdleState);
		if(notRotating)
			return;
		
		//rotate character based on movement
		Vector3 rot = transform.eulerAngles;
		rot.y = rotation;
		
		transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.Euler(rot), Time.deltaTime * turnSpeed);
	}
	
	void OnTriggerEnter(Collider other){
		//shoot the ball on trigger enter (when ball enters the opponent box)
		if(!other.gameObject.CompareTag("Ball") || ball.Value.inactive || justHit)
			return;
		
		float xDistance = Mathf.Abs(transform.position.x - other.gameObject.transform.position.x);
		
		if(xDistance > 1.3f){
			if(Random.Range(0, 2) == 0)
				anim.PlayAnimation(AnimConst.HitRightState);
			return;
		}
		
		StartCoroutine(JustHit());
		
		anim.PlayAnimation(AnimConst.HitRightState);
		HitBall(false, null);
	}
	
	//shoot the ball in a random direction and update the ball and the player
	public void HitBall(bool noFlame, Transform spawnPosition){
		soundPlayer.PlayHitBallSound();
		
		Vector3 random = new Vector3(Random.Range(-moveRange, moveRange), 0, player.gameObject.transform.position.z);
		
		
		if(ball.Value.IsFlame() && !noFlame)
			return;
			
		ball.Value.SetLastHit(false);
		
		Vector3 direction = (random - transform.position).normalized;
		
		player.SetTargetPosition(random);
		
		ball.Value.transform.position = spawnPosition == null ? servePoint.position : spawnPosition.position;
		
		ball.Value.Velocity = direction * force + Vector3.up * upForce;
	}
	
	//make sure we don't hit twice at the same time
	public IEnumerator JustHit(){
		justHit = true;
		
		yield return new WaitForSeconds(1f);
		
		justHit = false;
	}
	
	//set move target
	public void SetTarget(Vector3 pos){
		target = pos;
	}
	
	public void Reset() {
		SetTarget(transform.position.With(x:0));
	}
}