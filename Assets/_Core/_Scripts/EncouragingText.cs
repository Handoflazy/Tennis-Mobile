using System;
using System.Collections;
using System.Linq;
using TMPro;
using UnityEngine;
using Random = UnityEngine.Random;

[Serializable]
public class TextState
{
    public string[] texts;
    public Color color;
    public float min;
}
public class EncouragingText : MonoBehaviour
{
    public TextState[] states;
    public Animator anim;
    public TextMeshProUGUI label; 
    float duration;


    private void Awake() {
        duration = anim.runtimeAnimatorController.animationClips
            .FirstOrDefault(clip => clip.name == AnimConst.EncouragingTextPopup)!.length;
    }

    public void ShowText(float fillAmount) {
        TextState state = states.FirstOrDefault(s => fillAmount > s.min && 
                                                     (Array.IndexOf(states, s) == states.Length - 1 
                                                                            || fillAmount <= states[Array.IndexOf(states, s) + 1].min));
        if(state != null) {
            Show(state);
        }
    }
    void Show(TextState state){
        label.color = state.color;
        label.text = state.texts[Random.Range(0, state.texts.Length)];
		anim.Play(AnimConst.EncouragingTextPopup);
        
    }
}