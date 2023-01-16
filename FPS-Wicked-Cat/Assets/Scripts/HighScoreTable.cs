using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Scripting;

public class HighScoreTable : MonoBehaviour
{
    [Header("------ Score board Container ------")]
    private Transform entryContainer;
    private Transform entryTemplate;
    private List<HighscoreEntryData> highscoreEntrylist;
    private List<Transform> highscoreEntryTransformList;

    public TextMeshProUGUI userName;
    public TextMeshProUGUI score;
    public TextMeshProUGUI enemyKillCount;
    public TextMeshProUGUI timeSurvived;

    private void Awake()
    //public void HighScoreTableEntry()
    {

        entryContainer = transform.Find("EntryContainer");
        entryTemplate = entryContainer.Find("EntryTemp");

        entryContainer.gameObject.SetActive(false);

        //PlayerPrefs.DeleteAll();

        highscoreEntrylist = new List<HighscoreEntryData>()
        {
            new HighscoreEntryData() {name = "AGT", score = 4443223, enemykillCount = 12, timeSurvived = "23:00" },
            new HighscoreEntryData() {name = "GJT", score = 443223,  enemykillCount = 12, timeSurvived = "5:00" },
            new HighscoreEntryData() {name = "RAT", score = 43223,   enemykillCount = 12, timeSurvived = "12:00" },
            new HighscoreEntryData() {name = "JAT", score = 3223,    enemykillCount = 12, timeSurvived = "22:00" },
            new HighscoreEntryData() {name = "DJT", score = 44432,   enemykillCount = 12, timeSurvived = "14:00" },
            new HighscoreEntryData() {name = "NAT", score = 44433,   enemykillCount = 12, timeSurvived = "3:00" },
            new HighscoreEntryData() {name = "SBC", score = 444,     enemykillCount = 12, timeSurvived = "5:00" },
            new HighscoreEntryData() {name = "AMT", score = 4423,    enemykillCount = 12, timeSurvived = "8:00" },
            new HighscoreEntryData() {name = "BWT", score = 423,     enemykillCount = 12, timeSurvived = "9:00" },
            new HighscoreEntryData() {name = "JJT", score = 443,     enemykillCount = 12, timeSurvived = "6:00" },
        };

        string jsonString = PlayerPrefs.GetString("highscoreTable");
        HighScore highscores = JsonUtility.FromJson<HighScore>(jsonString);
        

        highscoreEntryTransformList = new List<Transform>();

        ////List shorting by score
        //for (int i = 0; i < highscores.highscoreEntryList.Count; i++)
        //{
        //    for (int j = i + 1 ; j < highscores.highscoreEntryList.Count; j++)
        //    {
        //        if (highscores.highscoreEntryList[j].score > highscores.highscoreEntryList[i].score)
        //        {
        //            HighscoreEntryData temp = highscores.highscoreEntryList[i];
        //            highscores.highscoreEntryList[i] = highscores.highscoreEntryList[j];
        //            highscores.highscoreEntryList[j] = temp;
        //        }
                
        //    }
        //}

        

        foreach (HighscoreEntryData highscoreEntry in highscores.highscoreEntryList)
        {
            CreateHighscoreEntryTransform(highscoreEntry, entryContainer, highscoreEntryTransformList);
        }      
    }

    private void CreateHighscoreEntryTransform(HighscoreEntryData highscoreEntry, Transform container, List<Transform> transformlist)
    {
        Transform entryTransform = Instantiate(entryTemplate, container);
        RectTransform entry = entryTransform.GetComponent<RectTransform>();
        entry.anchoredPosition = new Vector2(0, transformlist.Count);
        entryContainer.gameObject.SetActive(true);

        // sets text to variables
        userName.text = highscoreEntry.name;
        score.text = highscoreEntry.score.ToString();
        enemyKillCount.text = highscoreEntry.score.ToString();
        timeSurvived.text = highscoreEntry.score.ToString();

        transformlist.Add(entryTransform);
    }

    private void AddHighscoreEntry(string name, int score,int enemykillCount , string timeSurvived)
    {

        HighscoreEntryData highscoreEntry = new HighscoreEntryData { name = name , score = score , enemykillCount = enemykillCount , timeSurvived = timeSurvived };

        string jsonString = PlayerPrefs.GetString("highscoreTable");
        HighScore highscores = JsonUtility.FromJson<HighScore>(jsonString);

        string json = JsonUtility.ToJson(highscores);
        PlayerPrefs.SetString("highscoreTable", json);
        PlayerPrefs.Save();

        if (highscores == null)
        {
            highscores = new HighScore();
        }
        if (highscores.highscoreEntryList == null)
        {
            highscores.highscoreEntryList = new List<HighscoreEntryData>();
        }

    }

    private class HighScore
    {
        public List<HighscoreEntryData> highscoreEntryList;
    }

    [System.Serializable]
    private class HighscoreEntryData
    {
        public string name;
        public int score;
        public int enemykillCount;
        public string timeSurvived;
    }

}
