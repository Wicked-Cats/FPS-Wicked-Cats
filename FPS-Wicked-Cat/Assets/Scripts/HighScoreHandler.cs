using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HighScoreHandler : MonoBehaviour
{
   
    int highscore;

    
    private void Start()
    {
        // calls for current High score on game start
        SetLatesHighScore();
    }


    // pulls most resent High Score form local game file and sets default
    private void SetLatesHighScore()
    {
        highscore = PlayerPrefs.GetInt("ScoreText", 0);
    }


    // save current High Score to local game file
    private void SaveHighscore(int score)
    {
        PlayerPrefs.SetInt("ScoreText", score);
    }


    // Changes High Score when new score is reached and saves to local game file
    public void SetHighscoreIfGreater(int newScore)
    {
        if (newScore > highscore)
        {
            highscore = newScore;
            SaveHighscore(newScore);
        }
    }
}
