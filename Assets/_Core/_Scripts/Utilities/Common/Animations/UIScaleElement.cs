using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIScaleElement : MonoBehaviour
{

    private Sequence sequence;

    [SerializeField]
    private RectTransform element;
    [SerializeField]
    private float animationEndScale;
    [SerializeField]
    private float animationTime;

    private Vector3 baseScale, endScale;

    [SerializeField]
    private bool playConstantly = false;
    [SerializeField]
    private int loopTime = 2;

    [SerializeField]
    private bool PlayOnWake = false;

    private void Start()
    {
        baseScale = element.localScale;
        endScale = Vector3.one * animationEndScale;
        if(PlayOnWake)
        {
            PlayAnimation();
        }
       
    }
    public void PlayAnimation()
    {
        if (playConstantly)
        {
            sequence = DOTween.Sequence().Append(element.DOScale(baseScale, animationTime).SetEase(Ease.InFlash))
                .Append(element.DOScale(endScale, animationTime));
            sequence.SetLoops(-1, LoopType.Yoyo);
        

        }
        else
        {
            sequence = DOTween.Sequence().Append(element.DOScale(baseScale, animationTime).SetEase(Ease.InFlash))
                .Append(element.DOScale(endScale, animationTime));
            sequence.SetLoops(loopTime, LoopType.Yoyo);
           
        }
        sequence.Play();

    }
    private void OnDestroy()
    {
        if(sequence != null)
        {
            sequence.Kill();
        }
    }
}
