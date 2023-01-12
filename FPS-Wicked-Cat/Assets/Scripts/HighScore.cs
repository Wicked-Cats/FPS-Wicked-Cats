using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewBehaviourScript : MonoBehaviour
{
    int highScore;

    private void Start()
    {
        SetLatestScore();
    }

    private void SetLatestScore()
    {
        highScore = PlayerPrefs.GetInt("HighScore", 0);
    }

    private void SaveScore(int score)
    {
        PlayerPrefs.SetInt("HighScore", score);
    }

    public void SetScoreIfGreater(int score)
    {
        if (highScore > score)
        {
            highScore = score;
            SaveScore(score);
        }
    }
}
