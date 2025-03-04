using UnityEngine;

public class PowerUpBallSpawner : MonoBehaviour
{
    public PowerUpBall powerUpPrefab;
    public float range;
    public int chance;
        
    public void RandomSpawn(){
        if(Random.Range(0, chance) != 0)
            return;
		    
        Vector3 position = transform.position + Vector3.right * Random.Range(-range, range);
		    
        Instantiate(powerUpPrefab, position, powerUpPrefab.transform.rotation)
            .GetComponent<PowerUpBall>().SetSpawner(this);
    }
}