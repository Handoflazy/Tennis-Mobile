using System;
using System.Collections;
using _Core._Scripts;
using _Core._Scripts.Utilities.Extensions;
using DG.Tweening;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;
using UnityServiceLocator;
using Utilities.Extensions;

public class UIManager : MonoBehaviour
{
    [FoldoutGroup("Start Screen"), SerializeField, Required]
    private CanvasGroup startPanelCanvasGroup;

    [FoldoutGroup("Start Screen"), SerializeField, Required]
    private GameObject availableIcon;

    [FoldoutGroup("Game Panel"), SerializeField, Required]
    private GameObject gamePanel;

    [FoldoutGroup("Game Panel"), SerializeField, Required]
    private Animator swipeLabel;

    [FoldoutGroup("Game Panel"), SerializeField, Required]
    private Animator countdownServe;

    [FoldoutGroup("Game Panel"), SerializeField, Required]
    private GameObject scoreMatchPanel;

    [SerializeField] private Animator transition;

    [FoldoutGroup("Pause Panel"), SerializeField, Required]
    private GameObject audioLine;

    [FoldoutGroup("Pause Panel"), SerializeField, Required]
    private GameObject vibrateLine;

    [FoldoutGroup("Pause Panel"), SerializeField, Required]
    private Animator pausePanel;

    [Header("Bonus scene only")]
    public Animator bonuspopup;
    public TextMeshProUGUI bonuspopupLabel;
    public TextMeshProUGUI diamondsLabel;
    public Animator diamondLabelAnim;

    private bool useHapticFeedback; //TODO: Implement haptic feedback, make with ScriptableVariable

    private const float AvailableIconScale = 1.5f;
    private const float AvailableIconDuration = 0.4f;
    private const float StartPanelFadeDuration = 0.25f;
    private const float ScoreMatchPanelScaleDuration = 1f;
    private const float ScoreMatchPanelHideDuration = 0.8f;
    private const float FreezeWaitTime = 1f / 3f;

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

    private void AnimateAvailableIcon()
    {
        availableIcon.transform.DOScale(AvailableIconScale, AvailableIconDuration).SetLoops(-1, LoopType.Yoyo);
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

    public void ToggleBonus(bool value)
    {
        if (value)
        {
            diamondsLabel.gameObject.SetActive(false);
        }
        else
        {
            availableIcon.SetActive(PlayerPrefs.GetInt("Diamonds") >= 20);
        }
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
        startPanelCanvasGroup.DOFade(0, StartPanelFadeDuration).OnComplete(() => startPanelCanvasGroup.gameObject.SetActive(false));
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
        yield return scoreMatchPanel.transform.DOScaleY(1, ScoreMatchPanelScaleDuration).SetEase(Ease.OutBack)
            .OnComplete(() => scoreMatchPanel.transform.DOScaleY(0, ScoreMatchPanelHideDuration).SetEase(Ease.InBack))
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
            yield return new WaitForSeconds(FreezeWaitTime);
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