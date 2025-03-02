using System;
using System.Collections;
using System.Collections.Generic;
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
	public UIManager UIManager;
	public GameObject scoreTexts;
	public GameObject matchLabel;
	public CameraMovement cameraMovement;
	
	public bool playerOnly;
    
	
	[SerializeField,Required] Player playerPrefab;
	[SerializeField,Required] Opponent opponentPrefab;

	private void Awake() {
		if(playerPrefab == null || opponentPrefab == null){
			Debug.LogWarning("No player/opponent prefab in resources");
		}
		else{
			Player newPlayer = Instantiate(playerPrefab.gameObject, playerPosition.position, playerPosition.rotation).GetComponent<Player>();
			ServiceLocator.ForSceneOf(this).Get(out GameManager manager);
			manager.SetPlayer(newPlayer);
			
			if(!playerOnly){
				Opponent newOpponent = Instantiate(opponentPrefab, opponentPosition.position, opponentPosition.rotation).GetComponent<Opponent>();
				manager.SetOpponent(newOpponent);
			
				newOpponent.player = newPlayer;
				newOpponent.lookAt = newPlayer.transform;

				newPlayer.SetOpponent(newOpponent);
			}
			
			Opponent op = FindObjectOfType<Opponent>();
			newPlayer.lookAt = op.transform;
			
			if(playerOnly){
				newPlayer.SetOpponent(op);
				
				op.lookAt = newPlayer.transform;
				op.player = newPlayer;
			}
			
			cameraMovement.camTarget = newPlayer.transform;
			
			AssignPlayerReferences(newPlayer);
		}
	}
	
	void AssignPlayerReferences(Player player){
		player.comboLabel = comboLabel;
		player.comboNumberLabel = comboNumberLabel;
		player.swipeLabel = swipeLabel;
		player.uiManager = UIManager;
		player.scoreTexts = scoreTexts;
		player.matchLabel = matchLabel;
		player.cameraMovement = cameraMovement;
	}
}
