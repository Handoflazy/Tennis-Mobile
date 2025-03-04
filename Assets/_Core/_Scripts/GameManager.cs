using System;
using System.Collections;
using System.Threading.Tasks;
using _Core._Scripts.Utilities.Extensions;
using DG.Tweening;
using Eflatun.SceneReference;
using Obvious.Soap;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityServiceLocator;
using Utilities.Extensions;
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
        [SerializeField]
        UIManager uiManager;
        
        [SerializeField] private Transform spawnPos;
        [SerializeField] private Transform opponentSpawnPos;
        [SerializeField] private Animator countDown;
        [SerializeField] private Transform scoreCamTarget;
        [SerializeField] private ScriptableEventNoParam spawnPowerUp;
        [SerializeField] private BallVariable ballVariable;
        
        [SerializeField] private GameObject[] confetti;
        [SerializeField] TextMeshProUGUI playerPointsLabel;
        [SerializeField] TextMeshProUGUI opponentPointsLabel;
        [SerializeField] GameObject matchPoint;
        
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
	
	
        bool resetting;
        bool playerServe;
        public int bonusDiamonds;
        [SerializeField] private GameObject scoreTexts;
        [SerializeField] private GameObject matchLabel;

        private void Awake() {
            ServiceLocator.ForSceneOf(this).Register(this);
            canvas.SetActive(true);
            pausePanel.gameObject.SetActive(false);
        }

        private void Start() {
            uiManager.HideGamePanel();
            matchPoint.gameObject.SetActive(false);
            foreach(GameObject conf in confetti){
                conf.SetActive(false);
            }
            
            SetColorScheme();
            SetAudio(false);
            
            if(bonus){
                diamondsLabel.gameObject.SetActive(false);
            }
            else{
                characterAvailableIcon.SetActive(PlayerPrefs.GetInt("Diamonds") >= 20);
            }

        }

        public async void SetUpGamePlay() {
            ServiceLocator.ForSceneOf(this).Get(out player);
            ServiceLocator.ForSceneOf(this).Get(out opponent);
            
            playerPoints = 0;
            opponentPoints = 0;
            playerPointsLabel.text = "0";
            opponentPointsLabel.text = "0";
            playerServe = true;
            await Task.Yield();
            player.SetServe(true);
        }
        public void StartGame() {
            uiManager.HideStartPanel();
            uiManager.ShowGamePanel();
            if(scoreTexts != null){
                scoreTexts.SetActive(true);
                matchLabel.SetActive(false);
            }
        }
        
        
        public void SetAudio(bool change) {
            int audio = PlayerPrefs.GetInt("Audio");
            if(change){
                audio = audio == 0 ? 1 : 0;
			
                PlayerPrefs.SetInt("Audio", audio);
            }
            audioLine.SetActive(audio == 1);
            AudioListener.volume = audio == 0 ? 1 : 0;
        }

        public void Serve() {
            ballVariable.Value = Instantiate(ballPrefab, spawnPos.position, ballPrefab.transform.rotation)
                .GetComponent<Ball>();
        }

        public void CourtTriggered(bool net) {
            HandlePoint(ballVariable.Value.GetLastHit(), !net);
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
            if(bonus){
                StartCoroutine(BonusDone());
                yield break;
            }
            player.Reset();
            opponent.Reset();
            
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
                playerPointsLabel.rectTransform.DOScaleY(1.2f, 0.25f).SetEase(Ease.OutBack).OnComplete(() => playerPointsLabel.rectTransform.DOScaleY(1f, 0.25f));
                audioManager.PlayScorePoint();
            }
            else
            {
                opponentPointsLabel.rectTransform.DOScaleY(1.2f, 0.25f).SetEase(Ease.OutBack).OnComplete(() => playerPointsLabel.rectTransform.DOScaleY(1f, 0.25f));
                audioManager.PlayLosePoint();
            }
            yield return new WaitForSeconds(1f/6f);
            opponentPointsLabel.text = "" + opponentPoints;
            playerPointsLabel.text = "" + playerPoints;
            
            spawnPowerUp.Raise();
            
            yield return new WaitForSeconds(0.25f);
            if(playerPoints >= pointsToWin){
                StartCoroutine(Done(true));
            }
            else if(opponentPoints >= pointsToWin){
                StartCoroutine(Done(false));
            }
            else if(playerPoints == pointsToWin - 1 || opponentPoints == pointsToWin - 1){
                yield return new WaitForSeconds(0.5f);
			
                matchPoint.gameObject.SetActive(true);
                matchPoint.transform.localScale = Vector3.one.With(y: 0);
                yield return matchPoint.transform.DOScaleY(1, 1f).SetEase(Ease.OutBack)
                    .OnComplete(() => matchPoint.transform.DOScaleY(0, 0.8f).SetEase(Ease.InBack))
                    .WaitForCompletion();
                matchPoint.gameObject.SetActive(false);
                audioManager.PlayMatchPoint();
                
                //yield return new WaitForSeconds(0.5f);
            }
            yield return new WaitForSeconds(1f);
            foreach(GameObject conf in confetti){
                conf.SetActive(false);
            }
            if(!playerServeOnly){
                if(playerServe){
                    player.SetServe(true);
                }
                else{
                    StartCoroutine(OpponentServe());
                }
		
                playerServe = !playerServe;
            }
            else{
                player.SetServe(true);
            }
            resetting = false;
        }
        public IEnumerator Done(bool wonMatch){
            transition.SetTrigger(AnimConst.TransitionParam);
            cameraZoom.SetTrigger(AnimConst.ZoomParam);
		
            yield return new WaitForSeconds(1f/4f);
			
            GameObject matchInfo = new ("MatchInfo", typeof(MatchInfo));
            MatchInfo info = matchInfo.GetOrAdd<MatchInfo>();
		
            info.won = wonMatch;
            info.scoreText = playerPoints + " - " + opponentPoints;
		
            DontDestroyOnLoad(matchInfo);

            SceneManager.LoadScene(winScene.Path);
        }
        // ReSharper disable Unity.PerformanceAnalysis
        IEnumerator OpponentServe(){
            countDown.PlayAnimation("Countdown serve play");
            yield return new WaitForSeconds(3f);
		
            StartCoroutine(opponent.JustHit());
		
            opponent.anim.PlayAnimation(AnimConst.ServeState);
		
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
            bonusDiamonds++;
            audioManager.PlayMatchPoint();
            
            if(!diamondsLabel.gameObject.activeSelf)
                diamondsLabel.gameObject.SetActive(true);
            int max = 3 + PlayerPrefs.GetInt("Bonus max");
            
            if(bonusDiamonds >= max){
                resetting = true;
                diamondsLabel.gameObject.SetActive(false);
                
                StartCoroutine(BonusDone());
            }
        }

        IEnumerator BonusDone() {
            PlayerPrefs.SetInt("Diamonds", PlayerPrefs.GetInt("Diamonds") + bonusDiamonds);
            bonuspopupLabel.text = "+" + bonusDiamonds;
            if(PlayerPrefs.GetInt("Bonus max") < maxBonusTargets - 3)
                PlayerPrefs.SetInt("Bonus max", PlayerPrefs.GetInt("Bonus max") + 1);
            bonuspopup.SetTrigger("Play");
            yield return new WaitForSeconds(1f);
            yield return new WaitForSeconds(1f/4f);
		
            SceneManager.LoadScene(0);
        }
        public void Pause() {
            pausePanel.gameObject.SetActive(!pausePanel.gameObject.activeSelf);
            pausePanel.Play("Pause panel fade in");
            StartCoroutine(Freeze(pausePanel.gameObject.activeSelf));
        }
        IEnumerator Freeze(bool freeze){
            if(freeze){
                yield return new WaitForSeconds(1f/3f);
			
                Time.timeScale = 0;
            }
            else{
                Time.timeScale = 1;
            }
        }
        public void SetHaptic(bool change){
            int haptic = PlayerPrefs.GetInt("Haptic");
		
            if(change){
                haptic = haptic == 0 ? 1 : 0;
			
                PlayerPrefs.SetInt("Haptic", haptic);
            }
		
            vibrateLine.SetActive(haptic == 1);
		
            useHapticFeedback = haptic == 0;
        }
    }
}