using System;
using System.Collections;
using _Core._Scripts.Ads;
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
	
    [SerializeField] Text tournamentTitle;
    [SerializeField] TournamentTitle[] tournamentNames;
    [SerializeField] TextMeshProUGUI tournamentSmallLabel;
	
    [SerializeField] Transform playerIndicator;
    [SerializeField] Transform[] targets;
    [SerializeField] float indicatorSpeed;
	
    [SerializeField] float revealDelay;
    [SerializeField] float tournamentPanelDuration;
	
    [SerializeField] Animator transition;
	
    [SerializeField] CanvasGroup tournamentGroup;
    [SerializeField] CanvasGroup cupGroup;
    [SerializeField] GameObject nextButton;
    
    [SerializeField] GameObject playerPrefab;
    [SerializeField] GameObject opponentPrefab;
    [SerializeField] private MatchInfo info;
	
    Vector3 indicatorTarget;
    bool moveIndicator;
	
    bool won = true;
    bool wonTournament;
    
    
    void Awake(){
	    tournamentInfo.SetActive(false);
	    nextButton.SetActive(false);
		
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
		    title.text = TextConst.WINNER;
		    player.SetInteger(AnimConst.TYPE_PARAM, Random.Range(1, 8));
		    int tournament = PlayerPrefs.GetInt(SaveConst.TOURNAMENT);
		    int tournamentMatch = PlayerPrefs.GetInt(SaveConst.TOURNAMENT_MATCH_NUMBER,0);
		    if(tournamentMatch == 0)
			    SetCharacterData();
		    LoadCharacters();
		    
		    int tournamentRange = tournament % tournamentNames.Length;
			
		    tournamentTitle.text = tournamentNames[tournamentRange].name;
		    tournamentTitle.color = tournamentNames[tournamentRange].textColor;
		    
		    Color shadowColor = tournamentNames[tournamentRange].textColor;
		    shadowColor.a = 0.5f;
		    tournamentTitle.gameObject.GetComponent<Shadow>().effectColor = shadowColor;
		    tournamentSmallLabel.text = TextConst.TOURNAMENT + (tournament + 1);
		    
		    PlayerPrefs.SetInt(SaveConst.MATCH, PlayerPrefs.GetInt(SaveConst.MATCH) + 1);
		    PlayerPrefs.SetInt(SaveConst.TOURNAMENT_MATCH_NUMBER, PlayerPrefs.GetInt(SaveConst.TOURNAMENT_MATCH_NUMBER) + 1);
			
		    if(PlayerPrefs.GetInt(SaveConst.TOURNAMENT_MATCH_NUMBER) == 3){
			    PlayerPrefs.SetInt(SaveConst.TOURNAMENT_MATCH_NUMBER, 0);
			    PlayerPrefs.SetInt(SaveConst.TOURNAMENT, tournament + 1);
				
			    wonTournament = true;
		    }
			
		    audioSource.PlayOneShot(winClip);
	    } else {
		    RenderSettings.fogColor = loseFog;
		    Camera.main.backgroundColor = loseFog;
			
		    title.text = TextConst.KEEP_TRYING;
			
		    opponent.SetInteger(AnimConst.TYPE_PARAM, Random.Range(1, 8));
			
		    int tournamentMatchNumber = PlayerPrefs.GetInt(SaveConst.TOURNAMENT_MATCH_NUMBER);
		    PlayerPrefs.SetInt(SaveConst.TOURNAMENT_MATCH_NUMBER, 0);
			
		    PlayerPrefs.SetInt(SaveConst.MATCH, PlayerPrefs.GetInt(SaveConst.MATCH) - tournamentMatchNumber);
			
		    audioSource.PlayOneShot(loseClip);
	    }
	    titleAnim.SetBool(AnimConst.WON_PARAM, won);
		
	    foreach(Renderer rend in renderers){
		    rend.material = won ? winMat : loseMat;
	    }
    }

    private void Update() {
	    bar.fillAmount += Time.deltaTime/duration;

	    if((Input.GetMouseButtonDown(0) || bar.fillAmount >= 1f) && !tournamentInfo.activeSelf) {
		    StartCoroutine(Continue());
	    }
	    if(moveIndicator){
		    playerIndicator.position = Vector3.MoveTowards(playerIndicator.position, indicatorTarget, Time.deltaTime * indicatorSpeed);
			
		    if(Input.GetMouseButtonDown(0))
			    StartCoroutine(LoadGameScene());
	    }
    }
    IEnumerator TryAd() {
	    bool wait = true;
	    AdsManager.Instance.interstitialAds.OnAdClosed += () => {
		    wait = false;
	    };
	    AdsManager.Instance.interstitialAds.ShowInterstitialAd();
	    while (wait) {
		    yield return null;
	    }
    }
    private void SetCharacterData() {
	    
	    PlayerPrefs.SetInt(SaveConst.MIDDLE_LAYER_PLAYER_OPPONENT, Random.Range(0, 2) + 2);
	    PlayerPrefs.SetInt(SaveConst.MIDDLE_LAYER_TOP_CHARACTER, Random.Range(0, 2) + 4);
	    PlayerPrefs.SetInt(SaveConst.MIDDLE_LAYER_BOTTOM_CHARACTER, Random.Range(0, 2) + 6);
	    PlayerPrefs.SetInt(SaveConst.MIDDLE_LAYER_WINNER, Random.Range(0, 2));
	    
	    int match = PlayerPrefs.GetInt(SaveConst.MATCH);
	    PlayerPrefs.SetInt(SaveConst.OPPONENT_1, match);
	    
	    int randomCharacter = Random.Range(0, 200);
	    int middleOpponent = PlayerPrefs.GetInt(SaveConst.MIDDLE_LAYER_PLAYER_OPPONENT);
	    PlayerPrefs.SetInt(SaveConst.OPPONENT_2, middleOpponent == 2 ? match + 1 : randomCharacter);
	    PlayerPrefs.SetInt(SaveConst.OPPONENT_3, middleOpponent == 3 ? match + 1 : randomCharacter);
	    int middleLayerWinnerTop = PlayerPrefs.GetInt(SaveConst.MIDDLE_LAYER_WINNER) == 0 ? PlayerPrefs.GetInt(SaveConst.MIDDLE_LAYER_TOP_CHARACTER) : PlayerPrefs.GetInt(SaveConst.MIDDLE_LAYER_BOTTOM_CHARACTER);
	    
	    for(int i = 4; i < 8; i++){
		    PlayerPrefs.SetInt("Opponent " + i, Random.Range(0, 200));
	    }
	    PlayerPrefs.SetInt("Opponent " + middleLayerWinnerTop, match + 2);
	    
    }
    private void LoadCharacters(){
	    GameObject playerCharacter = Instantiate(playerPrefab, characters[0].position, characters[0].rotation);
	    playerCharacter.GetComponent<Player>().enabled = false;
	    playerCharacter.GetComponent<AnimationController>().enabled = false;
	    playerCharacter.GetComponent<Animator>().runtimeAnimatorController = idle;
	    
	    for(int i = 1; i < 8; i++){
		    int index = PlayerPrefs.GetInt("Opponent " + i);
			
		    GameObject opponentCharacter = Instantiate(opponentPrefab, characters[i].position, characters[i].rotation);
		    opponentCharacter.GetComponent<ModifyOutfit>().Initialize(index);
		    opponentCharacter.GetComponent<Opponent>().enabled = false;
		    opponentCharacter.GetComponent<Animator>().runtimeAnimatorController = idle;
	    }
	}
	IEnumerator AssignTextures(int tournamentMatch){
		int middleLayerTop = PlayerPrefs.GetInt(SaveConst.MIDDLE_LAYER_TOP_CHARACTER);
		int middleLayerBottom = PlayerPrefs.GetInt(SaveConst.MIDDLE_LAYER_BOTTOM_CHARACTER);
		
		int targetIndex = tournamentMatch > 0 ? 2 : 1;
		indicatorTarget = targets[targetIndex].position;
		playerIndicator.position = targets[targetIndex - 1].position;
		
		if(wonTournament)
			playerIndicator.position = targets[2].position;
		
		float delay = tournamentMatch > 0 ? 0f : revealDelay;
		
		yield return new WaitForSeconds(delay * 2f);
		images[0].texture = characterTextures[0];
			
		yield return new WaitForSeconds(delay);
		images[1].texture = characterTextures[PlayerPrefs.GetInt(SaveConst.MIDDLE_LAYER_PLAYER_OPPONENT)];
			
		yield return new WaitForSeconds(delay);
		images[2].texture = characterTextures[middleLayerTop];
			
		yield return new WaitForSeconds(delay);
		images[3].texture = characterTextures[middleLayerBottom];
		
		if(tournamentMatch > 0){
			//player character image
			yield return new WaitForSeconds(revealDelay);
			images[4].texture = characterTextures[0];
			
			yield return new WaitForSeconds(revealDelay);
			int bottomMostTexture = PlayerPrefs.GetInt(SaveConst.MIDDLE_LAYER_WINNER) == 0 ? middleLayerTop : middleLayerBottom;
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
			StartCoroutine(AssignTextures(PlayerPrefs.GetInt(SaveConst.TOURNAMENT_MATCH_NUMBER) - 1));
			
			yield return new WaitForSeconds(revealDelay * 5f);
		
			moveIndicator = true;
		}
		else{
			tournamentGroup.alpha = 0;
			cupGroup.alpha = 1;
			nextButton.SetActive(true);
		}
	}
	IEnumerator LoadGameScene(){
		StartCoroutine(TryAd());
		transition.SetTrigger(AnimConst.TRANSITION_PARAM);
		
		yield return new WaitForSeconds(1f/4f);
		
		SceneManager.LoadScene(nextScene.Path);
	}
    public void NextTournament() {
	    StartCoroutine(LoadGameScene());
    }
}