using _Core._Scripts;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;
using UnityServiceLocator;

public class LoadCharacters : MonoBehaviour
{
    public Transform playerPosition;
    public Transform opponentPosition;
    
    public Animator comboLabel;
	public TextMeshProUGUI comboNumberLabel;
	public Animator swipeLabel;
	public GameObject scoreTexts;
	public GameObject matchLabel;
	public bool playerOnly;
    
	
	[SerializeField,Required] Player playerPrefab;
	[SerializeField,Required] Opponent opponentPrefab;
	

	private void Start() {
		SpawnCharacter();
	}
	public void SpawnCharacter() {
		if(playerPrefab == null || opponentPrefab == null){
			Debug.LogWarning("No player/opponent prefab in resources");
		}
		else{
			Player newPlayer = Instantiate(playerPrefab.gameObject, playerPosition.position, playerPosition.rotation).GetComponent<Player>();
			
			if(!playerOnly){
				Opponent newOpponent = Instantiate(opponentPrefab, opponentPosition.position, opponentPosition.rotation).GetComponent<Opponent>();
				newOpponent.player = newPlayer;
				newOpponent.lookAt = newPlayer.transform;
				
			}
			if(playerOnly){
				ServiceLocator.ForSceneOf(this).Get(out Opponent op);
				op.lookAt = newPlayer.transform;
				op.player = newPlayer;
			}
			ServiceLocator.ForSceneOf(this).Get(out CameraMovement cam);
			cam.SetTarget(newPlayer.transform);
			AssignPlayerReferences(newPlayer);
			
			ServiceLocator.ForSceneOf(this).Get(out GameManager gm);
			gm.SetUpGamePlay();
		}
	}

	void AssignPlayerReferences(Player player){
		player.comboLabel = comboLabel;
		player.comboNumberLabel = comboNumberLabel;
		player.swipeLabel = swipeLabel;
		player.matchLabel = matchLabel;
	}
}
