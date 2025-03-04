using System;
using System.Collections;
using System.Linq;
using DG.Tweening;
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
public class PlayerUI : MonoBehaviour
{
    [SerializeField] Indicator indicator;
    [SerializeField] private GameObject diamond;
    public TextState[] states;
    public Animator anim;
    public TextMeshProUGUI label;


    private void Start() {
        diamond.SetActive(false);
    }

    public void ShowIndicator(float speed) {
        indicator.Show(speed);
    }
    public void HideIndicator() {
        indicator.Hide();
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
    public void PlayDiamondEffect() {
        diamond.SetActive(true);
        diamond.transform.DOScale(1.2f, 0.2f)
            .OnComplete(() => diamond.transform.DOScale(0.1f, 0.5f));
        diamond.transform.DOLocalMoveY(0.43f, 0.1f).OnComplete(() => diamond.transform.DOLocalMoveY(1.6f, 0.3f));
    }
}