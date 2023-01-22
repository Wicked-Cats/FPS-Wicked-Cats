using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    private ScoreData scoreData;
    [Header("----- Scoring System -----")]
    public TextMeshProUGUI scoreText;

    private void Awake()
    {
        var json = PlayerPrefs.GetString("scores", "{}");
        scoreData = JsonUtility.FromJson<ScoreData>(json);
    }

    public IEnumerable<Score> GetHighScores()
    {
        return scoreData.scores.OrderByDescending(x => x.score);
    }

    public void AddScore(Score score)
    {
        scoreData.scores.Add(score);
    }

    private void OnDestroy()
    {
        SaveScore();
    }

    public void SaveScore()
    {
        var json = JsonUtility.ToJson(scoreData);
        PlayerPrefs.SetString("scores", json);
    }
}