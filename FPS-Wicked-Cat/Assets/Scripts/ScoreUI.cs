using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ScoreUI : MonoBehaviour
{
    public RowUI rowUI;
    public ScoreManager scoreManager;

    void Start()
    {
        //scoreManager.AddScore(new Score("AAA", 4500000, 200));
        //scoreManager.AddScore(new Score("aaa", 33, 1));
        //scoreManager.AddScore(new Score("aaa", 5333, 4));
        //scoreManager.AddScore(new Score("aaa", 334545, 4));
        //scoreManager.AddScore(new Score("aaa", 33366, 4));

        var scores = scoreManager.GetHighScores().ToArray();
        for (int i = 0; i < scores.Length; i++)
        {
            if (i <= 9)
            {
                var row = Instantiate(rowUI, transform).GetComponent<RowUI>();
                row.name.text = scores[i].name;
                row.score.text = scores[i].score.ToString();
                row.enemyKillCount.text = scores[i].enemyKillCount.ToString();
            }
            else
            { }

        }
    }
}
