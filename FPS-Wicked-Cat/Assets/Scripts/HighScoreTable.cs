using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Scripting;

public class HighScoreTable : MonoBehaviour
{
    [Header("------ Score board Container ------")]
    private Transform entryContainer;
    [SerializeField] Transform entryTemplate;
    private HighscoreEntryData[] highscoreArray;
    private List<Transform> highscoreEntryTransformList;

    public TextMeshProUGUI userName;
    public TextMeshProUGUI score;
    public TextMeshProUGUI enemyKillCount;
    public TextMeshProUGUI timeSurvived;

    private void Awake()
    //public void HighScoreTableEntry()
    {

        entryContainer = transform.Find("EntryContainer");
        //entryTemplate = entryContainer.Find("EntryTemp");

        entryContainer.gameObject.SetActive(false);

        //highscoreEntryTransformList.Clear();
        
        highscoreArray = new HighscoreEntryData[10]
        {
            new HighscoreEntryData( "AGT",  4443223,  12,  "23:00") ,
            new HighscoreEntryData( "GJT", 443223, 12,  "5:00") ,
            new HighscoreEntryData("RAT",  43223,  12,  "12:00") ,
            new HighscoreEntryData("JAT", 3223,  12, "22:00"), 
            new HighscoreEntryData( "DJT",  44432,  12,  "14:00" ) ,
            new HighscoreEntryData(  "NAT",  44433, 12,  "3:00" ) ,
            new HighscoreEntryData("SBC", 444, 12, "5:00") ,
            new HighscoreEntryData("AMT", 4423, 12, "8:00"), 
            new HighscoreEntryData("BWT", 423, 12, "9:00") ,
            new HighscoreEntryData("JJT", 443, 12, "6:00") ,
        };

        //string jsonString = PlayerPrefs.GetString("highscoreTable");
        //HighScore highscores = JsonUtility.FromJson<HighScore>(jsonString);
        

    }

    private void Start()
    {

        
        highscoreEntryTransformList = new List<Transform>();
        

        //List shorting by score
        for (int i = 0; i < highscoreArray.Length; i++)
        {
            for (int j = i + 1; j < highscoreArray.Length; j++)
            {
                if (highscoreArray[j].score > highscoreArray[i].score)
                {
                    HighscoreEntryData temp = highscoreArray[i];
                    highscoreArray[i] = highscoreArray[j];
                    highscoreArray[j] = temp;
                }
            }
        }

        List<Transform> templist = new List<Transform>();
        

        foreach (HighscoreEntryData highscoreEntry in highscoreArray)
        {            
            CreateHighscoreEntryTransform(highscoreEntry, entryContainer, highscoreEntryTransformList);
        }
        //highscoreEntryTransformList = templist;
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
        enemyKillCount.text = highscoreEntry.enemykillCount.ToString();
        timeSurvived.text = highscoreEntry.timeSurvived.ToString();

        transformlist.Add(entryTransform);
    }

    private void AddHighscoreEntry(string name, int score,int enemykillCount , string timeSurvived)
    {

        //HighscoreEntryData highscoreEntry = new HighscoreEntryData { name = name , score = score , enemykillCount = enemykillCount , timeSurvived = timeSurvived };

        //string jsonString = PlayerPrefs.GetString("highscoreTable");
        //HighScore highscores = JsonUtility.FromJson<HighScore>(jsonString);

        //string json = JsonUtility.ToJson(highscores);
        //PlayerPrefs.SetString("highscoreTable", json);
        //PlayerPrefs.Save();

        //if (highscores == null)
        //{
        //    highscores = new HighScore();
        //}
        //if (highscores.highscoreEntryList == null)
        //{
        //    highscores.highscoreEntryList = new List<HighscoreEntryData>();
        //}

    }


    private class HighScore
    {
        public List<HighscoreEntryData> highscoreEntryList;
    }

    [System.Serializable]
    public class HighscoreEntryData
    {
        public string name;
        public int score;
        public int enemykillCount;
        public string timeSurvived;

        public HighscoreEntryData(string _name, int _score, int _enemykillCount, string _timeSurvived)
        {
            name = _name;
            score = _score;
            enemykillCount = _enemykillCount;
            timeSurvived = _timeSurvived;
        }
    }

}
