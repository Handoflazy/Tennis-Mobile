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
    
	[SerializeField] PlayerVariable player;
	[SerializeField] OpponentVariable opponent;
	
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
			player.Value = Instantiate(playerPrefab.gameObject, playerPosition.position, playerPosition.rotation).GetComponent<Player>();
			
			if(!playerOnly){
				opponent.Value = Instantiate(opponentPrefab, opponentPosition.position, opponentPosition.rotation).GetComponent<Opponent>();
			}
			ServiceLocator.ForSceneOf(this).Get(out CameraMovement cam);
			cam.SetTarget(player.Value.transform);
			AssignPlayerReferences(player.Value);
			
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
