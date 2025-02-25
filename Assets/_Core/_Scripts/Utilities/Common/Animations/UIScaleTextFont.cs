using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace SVS.UI
{
    public class UIScaleTextFont : MonoBehaviour
    {
        [SerializeField]
        private TextMeshProUGUI text;
        [SerializeField]
        private float fontAnimationSize = 80;
        [SerializeField]
        private float fontAnimationTime = 0.3f;
        private float baseFontSize;

        public bool PlayOnWake = true;
        private void Awake()
        {
            baseFontSize = text.fontSize;
            if(PlayOnWake)
            {
                PlayAnimation();
            }
        }
        public void PlayAnimation()
        {
            StopAllCoroutines();
            text.fontSize = baseFontSize;
            StartCoroutine(AnimateText(fontAnimationTime));


        }
        IEnumerator AnimateText(float animationTime)
        {
            float time = 0;
            float delta = fontAnimationSize - baseFontSize;
            while(time<animationTime)
            {
                time += Time.deltaTime;
                text.fontSize = Mathf.Lerp(baseFontSize, fontAnimationSize, time/animationTime);
                yield return null;
            }
            text.fontSize = baseFontSize;

        }
        
    }
}