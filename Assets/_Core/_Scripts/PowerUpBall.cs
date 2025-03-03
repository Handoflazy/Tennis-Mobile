using UnityEngine;

public class PowerUpBall : MonoBehaviour
{
    public PowerUpBallSpawner spawner;
    private Player player;

    private void Start() {
        player = FindObjectOfType<Player>();
    }

    public void SetSpawner(PowerUpBallSpawner spawner){
        this.spawner = spawner;
    }

    private void OnTriggerEnter(Collider other) {
        Ball ball = other.gameObject.GetComponent<Ball>();
		
        if(ball == null || !ball.GetLastHit())
            return;
        player.ComboDone(ball);
        spawner.RandomSpawn();
		
        Destroy(gameObject);   
    }
}