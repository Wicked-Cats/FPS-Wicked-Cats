using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class Score
{
    public string name;
    public int score;
    public int enemyKillCount;
    public int timeSurvived;

    public Score(string name, int score, int enemyKillCount, int timeSurvived)
    {
        this.name = name;
        this.score = score;
        this.enemyKillCount = enemyKillCount;
        this.timeSurvived = timeSurvived;
    }
}