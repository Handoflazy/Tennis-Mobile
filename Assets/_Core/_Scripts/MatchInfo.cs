using System;
using UnityEngine;

[CreateAssetMenu(menuName = "GamePlay/MatchInfo", fileName = "MatchInfo")]
public class MatchInfo : ScriptableObject
{
    [SerializeField]
    private bool won;
    [SerializeField]
    private string scoreText;

    public bool Won => won;
    public string ScoreText => scoreText;

    private void OnEnable() {
        won = PlayerPrefs.GetInt("MatchWon", 0) == 1;
        scoreText = PlayerPrefs.GetString("MatchScoreText", "");
    }

    public void SetMatchInfo(bool won, string scoreText) {
        this.won = won;
        this.scoreText = scoreText;
        PlayerPrefs.SetInt("MatchWon", won ? 1 : 0);
        PlayerPrefs.SetString("MatchScoreText", scoreText);
    }

    public void Reset() {
        SetMatchInfo(false, "");
    }
}