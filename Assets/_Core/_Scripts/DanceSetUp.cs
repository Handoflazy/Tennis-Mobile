using UnityEngine;

public class DanceSetUp: MonoBehaviour {
    public Transform playerPosition;
    public Transform opponentPosition;
	
    public RuntimeAnimatorController danceController;
	
    [SerializeField] Player playerPrefab;
    [SerializeField] Opponent opponentPrefab;
    
    void Awake(){
		
        if(playerPrefab == null || opponentPrefab == null){
            Debug.LogWarning("No player/opponent prefab in resources");
			
            return;
        }
		
        GameObject newOpponent = Instantiate(opponentPrefab.gameObject, opponentPosition.position, opponentPosition.rotation);
        newOpponent.GetComponent<Opponent>().enabled = false;
			
        GameObject newPlayer = Instantiate(playerPrefab.gameObject, playerPosition.position, playerPosition.rotation);
        newPlayer.GetComponent<Player>().enabled = false;
        newPlayer.GetComponentInChildren<AnimationController>().enabled = false;
		
        newOpponent.GetComponent<Animator>().runtimeAnimatorController = danceController;
        newPlayer.GetComponent<Animator>().runtimeAnimatorController = danceController;
		
        DanceScene danceScene = FindObjectOfType<DanceScene>();
        danceScene.player = newPlayer.GetComponent<Animator>();
        danceScene.opponent = newOpponent.GetComponent<Animator>();
		
        newPlayer.GetComponentInChildren<ParticleSystem>().Stop();
        newOpponent.GetComponentInChildren<ParticleSystem>().Stop();
    }
}