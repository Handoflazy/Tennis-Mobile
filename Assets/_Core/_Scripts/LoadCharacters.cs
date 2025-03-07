using TMPro;
using UnityEngine;
using UnityServiceLocator;

public class LoadCharacters : MonoBehaviour
{
	[SerializeField]  Transform playerPosition;
	[SerializeField]  Transform opponentPosition;
	[SerializeField]  bool playerOnly;
    
	[SerializeField] PlayerVariable player;
	[SerializeField] OpponentVariable opponent;
	
	[SerializeField] Player playerPrefab;
	[SerializeField] Opponent opponentPrefab;
	

	private void Start() {
		SpawnCharacter();
	}
	public void SpawnCharacter() {
		if(playerPrefab == null || opponentPrefab == null){
			Debug.LogWarning("No player/opponent prefab in resources");
		}
		else{
			player.Value = Instantiate(playerPrefab.gameObject, playerPosition.position, playerPosition.rotation).GetComponent<Player>();
			
			if(!playerOnly){
				opponent.Value = Instantiate(opponentPrefab, opponentPosition.position, opponentPosition.rotation).GetComponent<Opponent>();
			}
			ServiceLocator.ForSceneOf(this).Get(out CameraMovement cam);
			cam.SetTarget(player.Value.transform);
			
			ServiceLocator.ForSceneOf(this).Get(out GameManager gm);
			gm.SetUpGamePlay();
		}
	}
}
