using System;
using System.Collections;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Random = UnityEngine.Random;
using SceneReference = Eflatun.SceneReference.SceneReference;


[Serializable]
public class TournamentTitle{
    public string name;
    public Color textColor;
}
public class DanceScene : MonoBehaviour
{
    [SerializeField] SceneReference nextScene;
    [SerializeField] Renderer[] renderers;
    [SerializeField] Material winMat;
    [SerializeField] Material loseMat;
    [SerializeField] float duration;
    
    [SerializeField] Transform[] characters;
    [SerializeField] RuntimeAnimatorController idle;
    [SerializeField] RenderTexture[] characterTextures;
    [SerializeField] RawImage[] images;
	
    [SerializeField] Color winFog;
    [SerializeField] Color loseFog;
	
    [SerializeField] Text title;
    [SerializeField] Animator titleAnim;
	
	
    [HideInInspector]
    public Animator player;
	
    [HideInInspector]
    public Animator opponent;
	
    [SerializeField] TextMeshProUGUI score;
    [SerializeField] Image bar;
	
    [SerializeField] AudioSource audioSource;
    [SerializeField] AudioClip winClip;
    [SerializeField] AudioClip loseClip;
    
	
    [SerializeField] CanvasGroup mainPanel;
    [SerializeField] GameObject tournamentInfo;
	
    [SerializeField] TextMeshProUGUI tournamentTitle;
    [SerializeField] TournamentTitle[] tournamentNames;
    [SerializeField] TextMeshProUGUI tournamentSmallLabel;
	
    [SerializeField] Transform playerIndicator;
    [SerializeField] Transform[] targets;
    [SerializeField] float indicatorSpeed;
	
    [SerializeField] float revealDelay;
    [SerializeField] float tournamentPanelDuration;
	
    [SerializeField] Animator transition;
	
    [SerializeField] CanvasGroup tournamentGroup;
    
    [SerializeField] GameObject playerPrefab;
    [SerializeField] GameObject opponentPrefab;
    [SerializeField] private MatchInfo info;
	
    Vector3 indicatorTarget;
    bool moveIndicator;
	
    bool won = true;
    bool wonTournament;
    
    
    void Awake(){
#if UNITY_ADS
		TryAd();
#endif
	    tournamentInfo.SetActive(false);
		
	    if(info == null)
		    return;
		
	    won = info.Won;
	    score.text = info.ScoreText;
	    info.Reset();
    }

    private void Start() {
	    bar.fillAmount = 0;
	    if(won) {
		    RenderSettings.fogColor = winFog;
		    if(Camera.main != null) Camera.main.backgroundColor = winFog;
		    title.text = "Winner";
		    player.SetInteger("Type", Random.Range(1, 8));
		    int tournament = PlayerPrefs.GetInt("Tournament");
		    int tournamentMatch = PlayerPrefs.GetInt("Tournament Match Number",0);
		    if(tournamentMatch == 0)
			    SetCharacterData();
		    LoadCharacters();
		    
		    int tournamentRange = tournament % tournamentNames.Length;
			
		    tournamentTitle.text = tournamentNames[tournamentRange].name;
		    tournamentTitle.color = tournamentNames[tournamentRange].textColor;
		    
		    Color shadowColor = tournamentNames[tournamentRange].textColor;
		    shadowColor.a = 0.5f;
		    tournamentTitle.gameObject.GetComponent<Shadow>().effectColor = shadowColor;
		    tournamentSmallLabel.text = "Tournament #" + (tournament + 1);
		    
		    PlayerPrefs.SetInt("Match", PlayerPrefs.GetInt("Match") + 1);
		    PlayerPrefs.SetInt("Tournament Match Number", PlayerPrefs.GetInt("Tournament Match Number") + 1);
			
		    if(PlayerPrefs.GetInt("Tournament Match Number") == 3){
			    PlayerPrefs.SetInt("Tournament Match Number", 0);
			    PlayerPrefs.SetInt("Tournament", tournament + 1);
				
			    wonTournament = true;
		    }
			
		    audioSource.PlayOneShot(winClip);
	    } else {
		    RenderSettings.fogColor = loseFog;
		    Camera.main.backgroundColor = loseFog;
			
		    title.text = "Keep trying!";
			
		    opponent.SetInteger("Type", Random.Range(1, 8));
			
		    int tournamentMatchNumber = PlayerPrefs.GetInt("Tournament Match Number");
		    PlayerPrefs.SetInt("Tournament Match Number", 0);
			
		    PlayerPrefs.SetInt("Match", PlayerPrefs.GetInt("Match") - tournamentMatchNumber);
			
		    audioSource.PlayOneShot(loseClip);
	    }
    }

    private void Update() {
	    bar.fillAmount += Time.deltaTime/duration;
		
	    if((Input.GetMouseButtonDown(0) || bar.fillAmount >= 1f) && !tournamentInfo.activeSelf)
		    StartCoroutine(Continue());
		
	    if(moveIndicator){
		    playerIndicator.position = Vector3.MoveTowards(playerIndicator.position, indicatorTarget, Time.deltaTime * indicatorSpeed);
			
		    if(Input.GetMouseButtonDown(0))
			    StartCoroutine(LoadGameScene());
	    }
    }
#if UNITY_ADS
	void TryAd(){
		AdManager adManager = GameObject.FindObjectOfType<AdManager>();
		
		if(adManager == null)
			return;
		
		adManager.Interstitial();
	}
#endif
    private void SetCharacterData() {
	    PlayerPrefs.SetInt("Middle Layer Player Opponent", Random.Range(0, 2) + 2);
	    PlayerPrefs.SetInt("Middle Layer Top Character", Random.Range(0, 2) + 4);
	    PlayerPrefs.SetInt("Middle Layer Bottom Character", Random.Range(0, 2) + 6);
	    PlayerPrefs.SetInt("Middle Layer Winner", Random.Range(0, 2));
	    
	    int match = PlayerPrefs.GetInt("Match");
	    PlayerPrefs.SetInt("Opponent 1", match);
	    
	    int randomCharacter = Random.Range(0, 200);
	    int middleOpponent = PlayerPrefs.GetInt("Middle Layer Player Opponent");
	    PlayerPrefs.SetInt("Opponent 2", middleOpponent == 2 ? match + 1 : randomCharacter);
	    PlayerPrefs.SetInt("Opponent 3", middleOpponent == 3 ? match + 1 : randomCharacter);
	    int middleLayerWinnerTop = PlayerPrefs.GetInt("Middle Layer Winner") == 0 ? PlayerPrefs.GetInt("Middle Layer Top Character") : PlayerPrefs.GetInt("Middle Layer Bottom Character");
	    
	    for(int i = 4; i < 8; i++){
		    PlayerPrefs.SetInt("Opponent " + i, Random.Range(0, 200));
	    }
	    PlayerPrefs.SetInt("Opponent " + middleLayerWinnerTop, match + 2);
	    
    }
    void LoadCharacters(){
	    GameObject playerCharacter = Instantiate(playerPrefab, characters[0].position, characters[0].rotation);
	    playerCharacter.GetComponent<Player>().enabled = false;
	    playerCharacter.GetComponent<Animator>().runtimeAnimatorController = idle;
	    
	    for(int i = 1; i < 8; i++){
		    int index = PlayerPrefs.GetInt("Opponent " + i);
			
		    GameObject opponentCharacter = Instantiate(opponentPrefab, characters[i].position, characters[i].rotation);
		    //opponentCharacter.GetComponent<ModifyOutfit>().Initialize(index);
		    opponentCharacter.GetComponent<Opponent>().enabled = false;
		    opponentCharacter.GetComponent<Animator>().runtimeAnimatorController = idle;
	    }
	}
	IEnumerator AssignTextures(int tournamentMatch){
		int middleLayerTop = PlayerPrefs.GetInt("Middle Layer Top Character");
		int middleLayerBottom = PlayerPrefs.GetInt("Middle Layer Bottom Character");
		
		int targetIndex = tournamentMatch > 0 ? 2 : 1;
		indicatorTarget = targets[targetIndex].position;
		playerIndicator.position = targets[targetIndex - 1].position;
		
		if(wonTournament)
			playerIndicator.position = targets[2].position;
		
		float delay = tournamentMatch > 0 ? 0f : revealDelay;
		
		yield return new WaitForSeconds(delay * 2f);
		images[0].texture = characterTextures[0];
			
		yield return new WaitForSeconds(delay);
		images[1].texture = characterTextures[PlayerPrefs.GetInt("Middle Layer Player Opponent")];
			
		yield return new WaitForSeconds(delay);
		images[2].texture = characterTextures[middleLayerTop];
			
		yield return new WaitForSeconds(delay);
		images[3].texture = characterTextures[middleLayerBottom];
		
		if(tournamentMatch > 0){
			//player character image
			yield return new WaitForSeconds(revealDelay);
			images[4].texture = characterTextures[0];
			
			yield return new WaitForSeconds(revealDelay);
			int bottomMostTexture = PlayerPrefs.GetInt("Middle Layer Winner") == 0 ? middleLayerTop : middleLayerBottom;
			images[5].texture = characterTextures[bottomMostTexture];
		}
	}
	IEnumerator Continue(){
		if(!won){			
			SceneManager.LoadScene(nextScene.Path);
			
			yield break;
		}
		
		tournamentInfo.SetActive(true);
		mainPanel.DOFade(0,.3f);
		
		if(!wonTournament){
			StartCoroutine(AssignTextures(PlayerPrefs.GetInt("Tournament Match Number") - 1));
			
			yield return new WaitForSeconds(revealDelay * 5f);
		
			moveIndicator = true;
		}
		else{
			tournamentGroup.alpha = 0;
		}
	}
	IEnumerator LoadGameScene(){
		transition.SetTrigger("Transition");
		
		yield return new WaitForSeconds(1f/4f);
		
		SceneManager.LoadScene(nextScene.Path);
	}
    
}