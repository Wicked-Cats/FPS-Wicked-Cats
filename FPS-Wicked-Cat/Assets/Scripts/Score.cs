using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[Serializable]
public class Score
{
    public static Score instance;
    private ScoreManager scoreManager;

    public int score = 0;
    public string name;
    
    public int enemyKillCount;
    public int timeSurvived;
    private int userDeath = 0;

    private string nameHS;
    private int scoreHS = 0;
    private int enemyKillCountHS = 0;
    private int timeSurvivedHS = 0;
    private int userDeathHS;
     
    void Awake()
    {
        instance= this;
    }

    public void AddNewHighScorePlayerName()
    {

    }
    
    public void HighScoreUpdate() 
    {
        score += scoreHS;
    }
    // called in every Enemy script TakeDamage funtion
    public void UpdateEnemyKillCount()
    {
        enemyKillCount++;
        enemyKillCount = enemyKillCountHS;
    }

    public void TimeServived()
    {

    }
    // called on the Player Respawn funtion in buttonFunction
    public void UserDeathCount()
    {
        userDeath++;
        userDeath += userDeathHS;
    }

    public void AddScore(int scoreHS)
    {
        score += scoreHS;
        scoreManager.scoreText.text = "Score: " + scoreHS;
    }
    public Score(string name, int score, int enemyKillCount, int timeSurvived , int userDeaths)
    {
        this.nameHS = name;
        this.scoreHS = score;
        this.enemyKillCountHS = enemyKillCount;
        this.timeSurvivedHS = timeSurvived;
        this.userDeathHS = userDeaths;
    }
}
