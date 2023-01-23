using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    private ScoreData scoreData;
    [Header("----- Scoring System -----")]
    public TextMeshProUGUI scoreText;// in game score UI

    private void Awake()
    {
        //loads json file form playerPrefs when the High score UI is opened and adds default

        var json = PlayerPrefs.GetString("scores", "{}");
        scoreData = JsonUtility.FromJson<ScoreData>(json);
    }
    // sorts the array when there is a new entry
    public IEnumerable<Score> GetHighScores()
    {
        return scoreData.scores.OrderByDescending(x => x.score);
    }
    // globle variable that is called in the enemy scripts to to collected variables
    public void AddScore(Score score)
    {
        scoreData.scores.Add(score);
    }

    // calls the saved funtion when game is closed
    private void OnDestroy()
    {
        SaveScore();
    }

    //save function
    public void SaveScore()
    {
        var json = JsonUtility.ToJson(scoreData);
        PlayerPrefs.SetString("scores", json);
    }
}