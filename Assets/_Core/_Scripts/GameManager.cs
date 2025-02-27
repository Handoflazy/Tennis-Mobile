using System;
using Eflatun.SceneReference;
using TMPro;
using UnityEngine;
using UnityServiceLocator;

namespace _Core._Scripts
{
    public class ColorScheme {
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
        }

        public void FireBall() {
            Debug.Log("FireBall");
        }

        public void Out() {
            Debug.Log("Out");
        }
    }
}