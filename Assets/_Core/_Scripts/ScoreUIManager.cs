using System;
using System.Collections;
using DG.Tweening;
using TMPro;
using UnityEngine;

public class ScoreUIManager : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI matchLabel;
    [SerializeField] private TextMeshProUGUI playerScore;
    [SerializeField] private TextMeshProUGUI opponentScore;
    [SerializeField] private GameObject ScorePanel;
    
    

    public void ShowMatchLabel(string text) {
        matchLabel.text = text;
        matchLabel.rectTransform.DOScale(Vector3.one, 0.5f).SetEase(Ease.OutBack);
        ScorePanel.SetActive(false);
    }
    public void HideMatchLabel() {
        matchLabel.rectTransform.DOScale(Vector3.zero, 0.5f).SetEase(Ease.InBack);
        ScorePanel.SetActive(true);
    }
    
    public IEnumerator UpdatePlayerScore(int score) {
        yield return playerScore.rectTransform.DOScaleY(1.2f, 0.25f).SetEase(Ease.OutBack)
            .OnComplete(() => playerScore.rectTransform.DOScaleY(1f, 0.25f)).WaitForCompletion();
        playerScore.text = score.ToString();
      
    }
    
    public IEnumerator UpdateOpponentScore(int score) {
        yield return opponentScore.rectTransform.DOScaleY(1.2f, 0.25f).SetEase(Ease.OutBack)
            .OnComplete(() => opponentScore.rectTransform.DOScaleY(1f, 0.25f)).WaitForCompletion();
        opponentScore.text = score.ToString();
    }
}