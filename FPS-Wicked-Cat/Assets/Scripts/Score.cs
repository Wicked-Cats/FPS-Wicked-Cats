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

    //1.Need to look into logic that will check to see if a new top 10 score is reached and pull up the input window if 
    //the a new top 10 score is reached 
    //2. Need to link HighScore UI to the main menue.
    

    void Awake()
    {
        instance= this;
    }

    // need to finish players name input window 
    public void AddNewHighScorePlayerName()
    {

    }

    // not sure if i still need this function
    //public void HighScoreUpdate() 
    //{
    //    score += scoreHS;
    //}
    // called in every Enemy script TakeDamage funtion
    public void UpdateEnemyKillCount()
    {
        enemyKillCount++;
        enemyKillCount = enemyKillCountHS;
    }


    // need to wright logic to collect time when player exits the game
    public void TimeServived()
    {

    }

    // called on the Player Respawn funtion in buttonFunction
    public void UserDeathCount()
    {
        userDeath++;
        userDeath += userDeathHS;
    }

    // update the ingame score UI 
    public void AddScore(int newScore)
    {
        score += newScore;
        scoreManager.scoreText.text = "Score: " + newScore;

        if(newScore > scoreHS)
        {  newScore = scoreHS; }
    }

    // this is adding a single High score entry 
    public Score(string name, int score, int enemyKillCount, int timeSurvived , int userDeaths)
    {
        this.nameHS = name;
        this.scoreHS = score;
        this.enemyKillCountHS = enemyKillCount;
        this.timeSurvivedHS = timeSurvived;
        this.userDeathHS = userDeaths;
    }
}
