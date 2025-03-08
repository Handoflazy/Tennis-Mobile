using System;
using System.Collections;
using _Core._Scripts;
using _Core._Scripts.Utilities.Extensions;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;
using UnityServiceLocator;
using Utilities.Extensions;

public class UIManager : MonoBehaviour
{
    [SerializeField]
    private CanvasGroup startPanelCanvasGroup;

    [SerializeField]
    private GameObject availableIcon;

    [SerializeField]
    private GameObject gamePanel;

    [SerializeField]
    private Animator swipeLabel;

    [SerializeField]
    private Animator countdownServe;

    [SerializeField]
    private GameObject scoreMatchPanel;

    [SerializeField] private Animator transition;

    [SerializeField]
    private GameObject audioLine;

    [SerializeField]
    private GameObject vibrateLine;

    [SerializeField]
    private Animator pausePanel;
    private bool useHapticFeedback; //TODO: Implement haptic feedback, make with ScriptableVariable

    private const float AVAILABLE_ICON_SCALE = 1.5f;
    private const float AVAILABLE_ICON_DURATION = 0.4f;
    private const float START_PANEL_FADE_DURATION = 0.25f;
    private const float SCORE_MATCH_PANEL_SCALE_DURATION = 1f;
    private const float SCORE_MATCH_PANEL_HIDE_DURATION = 0.8f;
    private const float FREEZE_WAIT_TIME = 1f / 3f;

    private void Awake()
    {
        
        ServiceLocator.ForSceneOf(this).Register(this);
    }

    private void Start()
    {
        
        AnimateAvailableIcon();
        pausePanel.gameObject.SetActive(false);
        scoreMatchPanel.SetActive(false);
        SetAudio(false);
    }

    private void AnimateAvailableIcon() {
        if(PlayerPrefs.GetInt(SaveConst.DIAMONDS,0) >= 20) {
            availableIcon.SetActive(true);
            availableIcon.transform.DOScale(AVAILABLE_ICON_SCALE, AVAILABLE_ICON_DURATION).SetLoops(-1, LoopType.Yoyo);
        } else {
            availableIcon.SetActive(false);
        }
    }

    public void SetAudio(bool change)
    {
        int audio = PlayerPrefs.GetInt("Audio");
        if (change)
        {
            audio = audio == 0 ? 1 : 0;
            PlayerPrefs.SetInt("Audio", audio);
        }
        audioLine.SetActive(audio == 1);
        AudioListener.volume = audio == 0 ? 1 : 0;
    }
    public void FadeOutTransition()
    {
        transition.SetTrigger(AnimConst.TRANSITION_PARAM);
    }
    public void FadeInTransition()
    {
        transition.Play("Fade in");
    }

    public void HideStartPanel()
    {
        startPanelCanvasGroup.DOFade(0, START_PANEL_FADE_DURATION).OnComplete(() => startPanelCanvasGroup.gameObject.SetActive(false));
    }

    public void ToggleGamePanel(bool value)
    {
        gamePanel.gameObject.SetActive(value);
    }

    public IEnumerator StartCountdown()
    {
        countdownServe.PlayAnimation("Countdown serve play");
        yield return new WaitForSeconds(countdownServe.GetAnimationClipLength("Countdown serve play"));
    }

    public IEnumerator ShowScoreMatch()
    {
        scoreMatchPanel.SetActive(true);
        scoreMatchPanel.transform.localScale = Vector3.one.With(y: 0);
        yield return scoreMatchPanel.transform.DOScaleY(1, SCORE_MATCH_PANEL_SCALE_DURATION).SetEase(Ease.OutBack)
            .OnComplete(() => scoreMatchPanel.transform.DOScaleY(0, SCORE_MATCH_PANEL_HIDE_DURATION).SetEase(Ease.InBack))
            .WaitForCompletion();
        scoreMatchPanel.gameObject.SetActive(false);
    }

    public void Pause()
    {
        var toggle = !pausePanel.GetBool(AnimConst.SHOW_PARAM);
        pausePanel.gameObject.SetActive(toggle);
        pausePanel.SetBool(AnimConst.SHOW_PARAM, toggle);
        StartCoroutine(Freeze(pausePanel.GetBool(AnimConst.SHOW_PARAM)));
    }

    private IEnumerator Freeze(bool freeze)
    {
        if (freeze)
        {
            yield return new WaitForSeconds(FREEZE_WAIT_TIME);
            Time.timeScale = 0;
        }
        else
        {
            Time.timeScale = 1;
        }
    }

    public void SetHaptic(bool change)
    {
        int haptic = PlayerPrefs.GetInt("Haptic");
        if (change)
        {
            haptic = haptic == 0 ? 1 : 0;
            PlayerPrefs.SetInt("Haptic", haptic);
        }
        vibrateLine.SetActive(haptic == 1);
        useHapticFeedback = haptic == 0;
    }
}