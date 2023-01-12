using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HighScore : MonoBehaviour
{
    int score;

    private void Start()
    {
        SetLatestScore();
    }

    private void SetLatestScore()
    {
        score = PlayerPrefs.GetInt("HighScore", 0);
    }

    private void SaveScore(int score)
    {
        PlayerPrefs.SetInt("HighScore", score);
    }

    public void SetScoreIfGreater(int newScore)
    {
        if (score > newScore)
        {
            score = newScore;
            SaveScore(newScore);
        }
    }
}
