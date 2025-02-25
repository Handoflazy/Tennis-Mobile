using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HooverAnimation : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField]
    private float distance = 0.2f;
    [SerializeField]
    private float duration;
    private Tweener tweener;
    public Ease animationEase;
    public bool PlayOnAwake = false;
    private void Start()
    {
        if (PlayOnAwake)
        {
            PlayerAnimation();
        }
    }
    public void PlayerAnimation()
    {
        tweener = transform.DOMoveY(transform.position.y+ distance, duration).SetEase(animationEase).SetLoops(-1, LoopType.Yoyo);
    }
    private void OnDestroy()
    {
        if (tweener != null) // Check if tweener exists before stopping
        {
            DOTween.Kill(transform);
        }
    }
    // Update is called once per frame

}
