using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HighScore : MonoBehaviour
{
    int highScore;

    private void Start()
    {
        // calls for current High score on game start
        SetLatestHightScore();
    }

    // pulls most resent High Score form local game file and sets default
    private void SetLatestHightScore()
    {
        highScore = PlayerPrefs.GetInt("HighScore", 0);
    }

    // save current High Score to local game file
    private void SaveScore(int score)
    {
        PlayerPrefs.SetInt("HighScore", score);
    }

    // Changes High Score when new score is reached and saves to local game file
    public void SetScoreIfGreater(int newScore)
    {
        if (highScore > newScore)
        {
            highScore = newScore;
            SaveScore(newScore);
        }
    }
}
