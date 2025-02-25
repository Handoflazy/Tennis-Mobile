using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UIShakeAnimation : MonoBehaviour
{
    [SerializeField]
    private RectTransform element;
    [Header("Shake Animation Settings")]
    public float shakeTime = 0.5f, shakeStrength=20, randomness = 90;
    public int vibrato = 90;
    public bool fadeOut = true;
    public float delayBetweenShakes = 3;

    [Space(10)]

    Sequence sequence;
    public bool PlayOnWake = true;
    private void OnEnable()
    {
        DOTween.KillAll();
        PlayAnimation();
    }
    private void Start()
    {
        sequence = DOTween.Sequence().Append(element.DOShakeRotation(shakeTime, shakeStrength, vibrato, randomness, fadeOut));
        sequence.SetLoops(-1,LoopType.Restart);
        sequence.AppendInterval(delayBetweenShakes);

        if (PlayOnWake)
        {
            PlayAnimation();
        }
    }
    public void PlayAnimation()=>sequence.Play();
}
