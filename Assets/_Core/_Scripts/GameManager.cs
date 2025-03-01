using System;
using System.Collections;
using Eflatun.SceneReference;
using TMPro;
using UnityEngine;
using UnityServiceLocator;
using Random = UnityEngine.Random;

namespace _Core._Scripts
{
    [Serializable]
    public struct ColorScheme {
        public Color floor;
        public Color background;
        public Color court;
    }
    public class GameManager : MonoBehaviour
    {
        [SerializeField] private SceneReference winScene;
        [SerializeField] private bool playerServeOnly;
        [SerializeField] private int pointsToWin = 3;
        [SerializeField] private Animator transition;
        [SerializeField] private Animator cameraZoom;
        [SerializeField] private GameObject canvas;
//[SerializeField] private VisitorSpawner visitors;
        [SerializeField] private CameraMovement cameraMovement;
        public GameObject ballPrefab;

        private Player player;
        private Opponent opponent;
        
        [SerializeField] private Transform spawnPos;
        [SerializeField] private Transform opponentSpawnPos;
        [SerializeField] private Animator countDown;
        [SerializeField] private Transform scoreCamTarget;
        
        [SerializeField] private GameObject[] confetti;
        [SerializeField] TextMeshProUGUI playerPointsLabel;
        [SerializeField] TextMeshProUGUI opponentPointsLabel;
        [SerializeField] Animator playerPointsAnim;
        [SerializeField] Animator opponentPointsAnim;
        [SerializeField] Animator matchPoint;
        
        public ColorScheme[] colorSchemes;
        public Material floor;
        public Material stadium;
        public Material court;

        [SerializeField] AudioManager audioManager;

        public GameObject audioLine;
        public GameObject vibrateLine;
        public Animator pausePanel;
        public GameObject characterAvailableIcon;
        
        [Header("Bonus scene only")]
        public bool bonus;
        public Animator bonuspopup;
        public TextMeshProUGUI bonuspopupLabel;
        public TextMeshProUGUI diamondsLabel;
        public Animator diamondLabelAnim;
        
        public int maxBonusTargets;
        bool useHapticFeedback;
	
        int playerPoints;
        int opponentPoints;
	
        Ball ballScript;
	
        bool resetting;
        bool playerServe;
        
        private void Awake() {
            ServiceLocator.ForSceneOf(this).Register(this);
            canvas.SetActive(true);
        }

        private void Start() {
            foreach(GameObject conf in confetti){
                conf.SetActive(false);
            }
            SetColorScheme();
            player.SetBar(true);
            SetAudio(false);
            
            /*if(bonus){
                diamondsLabel.gameObject.SetActive(false);
            }
            else{
                characterAvailableIcon.SetActive(PlayerPrefs.GetInt("Diamonds") >= 20);
            }*/

        }

        private void SetAudio(bool change) {
            int audio = PlayerPrefs.GetInt("Audio");
            if(change){
                audio = audio == 0 ? 1 : 0;
			
                PlayerPrefs.SetInt("Audio", audio);
            }
            audioLine.SetActive(audio == 1);
            AudioListener.volume = audio == 0 ? 1 : 0;
        }

        public void Serve() {
            ballScript = Instantiate(ballPrefab, spawnPos.position, ballPrefab.transform.rotation).GetComponent<Ball>();
            player.ball = ballScript;
            opponent.ball = ballScript;
        }

        public void CourtTriggered(bool net) {
            if (net) {
                HandlePoint(ballScript.GetLastHit(), false);
            } else {
                HandlePoint(ballScript.GetLastHit(), true);
            }
        }
        private void HandlePoint(bool lastHit, bool winIfLastHit) {
            if (lastHit == winIfLastHit) {
                WinPoint();
            } else {
                LosePoint();
            }
        }

        private void WinPoint() {
            playerPoints++;
            if(!resetting)
                StartCoroutine(CheckAndReset(true));
        }

        private void LosePoint() {
            opponentPoints++;
            if(!resetting)
                StartCoroutine(CheckAndReset(false));
        }

        IEnumerator CheckAndReset(bool wonPoint) {
            resetting = true;
            //TODO: BONUS
            player.Reset();
            opponent.Reset();
            
            if(ballScript && !ballScript.inactive)
                ballScript.inactive = true;
            yield return new WaitForSeconds(0.75f);
            cameraMovement.SwitchTargetTemp(scoreCamTarget, 1.5f, 0.5f);
            
            yield return new WaitForSeconds(0.5f);
            
            if(wonPoint)
            {
                //visitors.Cheer();
                foreach(GameObject conf in confetti){
                    conf.SetActive(true);
                    yield return new WaitForSeconds(0.15f);
                }
                playerPointsAnim.SetTrigger(AnimConst.EffectParam);
            }
            else
            {
                opponentPointsAnim.SetTrigger(AnimConst.EffectParam);
            }
            yield return new WaitForSeconds(1f/6f);
            opponentPointsLabel.text = "" + opponentPoints;
            playerPointsLabel.text = "" + playerPoints;
            
            yield return new WaitForSeconds(0.25f);
            if(playerPoints >= pointsToWin){
                StartCoroutine(Done(true));
            }
            else if(opponentPoints >= pointsToWin){
                StartCoroutine(Done(false));
            }
            else if(playerPoints == pointsToWin - 1 || opponentPoints == pointsToWin - 1){
                yield return new WaitForSeconds(0.5f);
			
                matchPoint.SetTrigger(AnimConst.ShowParam);  
                audioManager.PlaySuccess();
                
                yield return new WaitForSeconds(0.5f);
            }
            yield return new WaitForSeconds(1f);
            foreach(GameObject conf in confetti){
                conf.SetActive(false);
            }
            if(!playerServeOnly){
                if(playerServe){
                    player.SetBar(true);
                }
                else{
                    StartCoroutine(OpponentServe());
                }
		
                playerServe = !playerServe;
            }
            else{
                player.SetBar(true);
            }
        }
        public IEnumerator Done(bool wonMatch){
            transition.SetTrigger(AnimConst.TransitionParam);
            cameraZoom.SetTrigger(AnimConst.ZoomParam);
		
            yield return new WaitForSeconds(1f/4f);
			
            GameObject matchInfo = new GameObject();
            //MatchInfo info = matchInfo.AddComponent<MatchInfo>();
		
            //info.won = wonMatch;
            //info.scoreText = playerPoints + " - " + opponentPoints;
		
            DontDestroyOnLoad(matchInfo);
		
            //SceneManager.LoadScene(winScene);
        }
        IEnumerator OpponentServe(){
            countDown.SetTrigger(AnimConst.CountdownParam);
		
            yield return new WaitForSeconds(3f);
		
            StartCoroutine(opponent.JustHit());
		
            opponent.anim.SetTrigger(AnimConst.ServeParam);
		
            yield return new WaitForSeconds(0.28f);
            Serve();
            opponent.HitBall(true, opponentSpawnPos);
        }
        void SetColorScheme() {
            int random = Random.Range(0, colorSchemes.Length);
		
            floor.color = colorSchemes[random].floor;
            stadium.color = colorSchemes[random].background;
            court.color = colorSchemes[random].court;
        }
        public void FireBall() {
            WinPoint();
            StartCoroutine(cameraMovement.Shake(0.2f, 1.2f));
        }

        public void Out() {
            LosePoint();
        }

        public void AddBonus() {
            throw new NotImplementedException();
        }

        public void SetPlayer(Player player) {
            this.player = player;
        }
        public void SetOpponent(Opponent opponent) {
            this.opponent = opponent;
        }
    }
}