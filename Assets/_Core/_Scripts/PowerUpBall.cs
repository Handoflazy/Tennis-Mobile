using DG.Tweening;
using UnityEngine;

public class PowerUpBall : MonoBehaviour
{
    [SerializeField] private PlayerVariable player;

    private void Start() {
        transform.DOScale(40f, 1f).SetLoops(-1, LoopType.Restart);
    }

    private void OnTriggerEnter(Collider other) {
        Ball ball = other.gameObject.GetComponent<Ball>();
		
        if(ball == null || !ball.GetLastHit())
            return;
        player.Value.ComboDone(ball);
		
        Destroy(gameObject);   
    }

    private void OnDestroy() {
        transform.DOKill();
    }
}