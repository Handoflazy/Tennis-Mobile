using System;
using System.Collections;
using DG.Tweening;
using Eflatun.SceneReference;
using Obvious.Soap;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityServiceLocator;
using Random = UnityEngine.Random;

namespace _Core._Scripts
{
    [Serializable]
    public struct ColorScheme
    {
        public Color floor;
        public Color background;
        public Color court;
    }

    public class GameManager : MonoBehaviour
    {
        [SerializeField] private SceneReference winScene;
        [SerializeField] private ScoreUIManager scoreUI;
        [SerializeField] private UIManager uiManager;
        [SerializeField] private BallFactory factory;
        [SerializeField] private MatchInfo matchInfo;
    
        [Header("SOAP")]
        [SerializeField] private PlayerVariable player;
        [SerializeField] private OpponentVariable opponent;
        [SerializeField] private ScriptableEventNoParam spawnPowerUp;
        [SerializeField] private BallVariable ballVariable;
        [SerializeField] private ScriptableEventNoParam CameraZoom;

        [Header("Game Settings")]
        [SerializeField] private bool playerServeOnly;
        [SerializeField] private int pointsToWin = 3;
        [SerializeField] private int maxBonusTargets;

        [Space(15)]
        [SerializeField] private Transform playerSpawnPos;
        [SerializeField] private Transform opponentSpawnPos;
        [SerializeField] private Transform scoreCamTarget;

        [FoldoutGroup("Stadium"), SerializeField] private ColorScheme[] colorSchemes;
        [FoldoutGroup("Stadium"), SerializeField] private Material floor;
        [FoldoutGroup("Stadium"), SerializeField] private Material stadium;
        [FoldoutGroup("Stadium"), SerializeField] private Material court;
        

        private CameraMovement cam;
        private AudioManager audioManager;
        public GameEffectManager gameEffectManager;
        public VisitorSpawner visitors;

        private int playerPoints;
        private int opponentPoints;
        private bool resetting;
        private bool playerServe;
        public int bonusDiamonds;

        private void Awake()
        {
            ServiceLocator.ForSceneOf(this).Register(this);
        }

        private void Start()
        {
            ServiceLocator.ForSceneOf(this).Get(out visitors);
            ServiceLocator.ForSceneOf(this).Get(out cam);
            ServiceLocator.ForSceneOf(this).Get(out audioManager);
            uiManager.ToggleGamePanel(false);
            

            SetColorScheme();
        }

        public void SetUpGamePlay()
        {
            playerPoints = 0;
            opponentPoints = 0;
            StartCoroutine(scoreUI.UpdatePlayerScore(playerPoints));
            StartCoroutine(scoreUI.UpdateOpponentScore(opponentPoints));
            
            int tournament = PlayerPrefs.GetInt(SaveConst.TOURNAMENT) + 1;
            scoreUI.ShowMatchLabel("Tournament #" + tournament);
            playerServe = false;
            player.Value.SetServe(true);
        }

        public void StartGame()
        {
            uiManager.HideStartPanel();
            uiManager.ToggleGamePanel(true);
            scoreUI.HideMatchLabel();
        }

        public void Serve()
        {
            ballVariable.Value = factory.Create();
            ballVariable.Value.transform.position = playerSpawnPos.position;
        }

        public void CourtTriggered(bool net)
        {
            if(ballVariable.Value)
                HandlePoint(ballVariable.Value.GetLastHit(), !net);
        }

        private void HandlePoint(bool lastHit, bool winIfLastHit)
        {
            if (lastHit == winIfLastHit)
            {
                WinPoint();
            }
            else
            {
                LosePoint();
            }
        }

        private void WinPoint()
        {
            playerPoints++;
            if (!resetting)
                StartCoroutine(CheckAndReset(true));
        }

        private void LosePoint()
        {
            opponentPoints++;
            if (!resetting)
                StartCoroutine(CheckAndReset(false));
        }

        private IEnumerator CheckAndReset(bool wonPoint)
        {
            resetting = true;
            player.Value.Reset();
            opponent.Value.Reset();

            yield return new WaitForSeconds(0.75f);
            cam.SwitchTargetTemp(scoreCamTarget, 1.5f, 0.5f);

            yield return new WaitForSeconds(0.5f);

            if (wonPoint)
            {
                visitors.Cheer();
                if(!gameEffectManager) ServiceLocator.ForSceneOf(this).Get(out gameEffectManager);
                yield return StartCoroutine(gameEffectManager.PlayConfetti());
                yield return scoreUI.UpdatePlayerScore(playerPoints);
                audioManager.PlayScorePoint();
            }
            else
            {
                visitors.Disbelief();
                yield return scoreUI.UpdateOpponentScore(opponentPoints);
                audioManager.PlayLosePoint();
            }
            spawnPowerUp.Raise();

            yield return new WaitForSeconds(0.25f);
            if (playerPoints >= pointsToWin)
            {
                StartCoroutine(Done(true));
            }
            else if (opponentPoints >= pointsToWin)
            {
                StartCoroutine(Done(false));
            }
            else if (playerPoints == pointsToWin - 1 || opponentPoints == pointsToWin - 1)
            {
                yield return new WaitForSeconds(0.5f);
                yield return StartCoroutine(uiManager.ShowScoreMatch());
                audioManager.PlayMatchPoint();
            }
            yield return new WaitForSeconds(1f);
            if (!playerServeOnly)
            {
                if (playerServe)
                {
                    player.Value.SetServe(true);
                }
                else
                {
                    StartCoroutine(OpponentServe());
                }

                playerServe = !playerServe;
            }
            else
            {
                player.Value.SetServe(true);
            }
            resetting = false;
        }

        private IEnumerator Done(bool wonMatch)
        {
            uiManager.ShowTransition();
            CameraZoom.Raise();

            yield return new WaitForSeconds(0.25f);
            
            matchInfo.SetMatchInfo(wonMatch,playerPoints + " - " + opponentPoints);
            yield return new WaitForSeconds(1f);

            SceneManager.LoadScene(winScene.Path);
        }

        private IEnumerator OpponentServe()
        {
            yield return StartCoroutine(uiManager.StartCountdown());
            StartCoroutine(opponent.Value.JustHit());
            opponent.Value.PlayerServeAnimation();

            yield return new WaitForSeconds(0.28f);
            Serve();
            opponent.Value.HitBall(true, opponentSpawnPos);
        }

        private void SetColorScheme()
        {
            int random = Random.Range(0, colorSchemes.Length);

            floor.color = colorSchemes[random].floor;
            stadium.color = colorSchemes[random].background;
            court.color = colorSchemes[random].court;
        }

        public void FireBall()
        {
            WinPoint();
            StartCoroutine(cam.Shake(0.2f, 1.2f));
        }

        public void Out()
        {
            LosePoint();
        }

        public void AddBonus()
        {
            bonusDiamonds++;
            audioManager.PlayMatchPoint();
            
            
        }
        private void OnDestroy() {
            DOTween.KillAll();
        }
    }
    
}