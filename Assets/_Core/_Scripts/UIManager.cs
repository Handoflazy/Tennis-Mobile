using System;
using _Core._Scripts;
using DG.Tweening;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Serialization;
using UnityServiceLocator;

public class UIManager : MonoBehaviour
{
    [FoldoutGroup("Start Screen")]
    [SerializeField, Required] CanvasGroup startPanelCanvasGroup;
    [FoldoutGroup("Start Screen")]
    [SerializeField, Required] private GameObject availableIcon;
    
    [FoldoutGroup("Game Panel")]
    [SerializeField, Required] GameObject gamePanel;
    [FoldoutGroup("Game Panel")]
    [SerializeField, Required] private Animator SwipeLabel;
    [FoldoutGroup("Game Panel")]
    [SerializeField, Required] private Animator CountdownServe;
    [SerializeField] private Animator transition;
    private void Awake() {
        ServiceLocator.ForSceneOf(this).Register(this);
    }

    private void Start() {
        availableIcon.transform.DOScale(1.5f,0.4f).SetLoops(-1,LoopType.Yoyo);
        
    }

    public void HideStartPanel() {
        startPanelCanvasGroup.DOFade(0,0.25f).OnComplete(()=>startPanelCanvasGroup.gameObject.SetActive(false));
    }

    public void HideGamePanel() {
        gamePanel.gameObject.SetActive(false);
    }

    public void ShowGamePanel() {
        gamePanel.gameObject.SetActive(true);
    }
}