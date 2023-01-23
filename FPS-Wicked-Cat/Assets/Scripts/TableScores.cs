using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TableScores : MonoBehaviour
{
    private Transform entryContainer;
    private Transform entryTemplate;
    private List<Transform> highscoreEntryTransformList;

    public void SetUpTable()
    {
        entryContainer = transform.Find("entryContainer");
        entryTemplate = entryContainer.Find("entryTemplate");

        entryTemplate.gameObject.SetActive(false);

        //AddHighScoreEntry(1000, "AAA");

        string jsonString = PlayerPrefs.GetString("highscoreTable");
        HighScores highscores = JsonUtility.FromJson<HighScores>(jsonString);

        for( int x = 0; x < highscores.highscoreEntryList.Count; x++)
        {
            for(int i = x + 1; i < highscores.highscoreEntryList.Count; i++)
            {
                if(highscores.highscoreEntryList[i].score > highscores.highscoreEntryList[x].score)
                {
                    HighscoreEntry temp = highscores.highscoreEntryList[x];
                    highscores.highscoreEntryList[x] = highscores.highscoreEntryList[i];
                    highscores.highscoreEntryList[i] = temp;
                }
            }
        }

        highscoreEntryTransformList = new List<Transform>();
        foreach (HighscoreEntry highscoreEntry in highscores.highscoreEntryList)
        {
            CreateEntryTransform(highscoreEntry, entryContainer, highscoreEntryTransformList);
        }
    }

    private void CreateEntryTransform(HighscoreEntry highscoreEntry, Transform container, List<Transform> transformList)
    {
        float templateHeight = 20f;
        Transform entryTransform = Instantiate(entryTemplate, container);
        RectTransform entryRectTransform = entryTransform.GetComponent<RectTransform>();
        entryRectTransform.anchoredPosition = new Vector2(0, -templateHeight * transformList.Count);
        entryTransform.gameObject.SetActive(true);


        int rank = transformList.Count + 1;
        string rankString;
        switch (rank)
        {
            default:
                rankString = rank + "TH";
                break;
            case 1:
                rankString = rank + "ST";
                break;
            case 2:
                rankString = rank + "ND";
                break;
            case 3:
                rankString = rank + "RD";
                break;
        }

        entryTransform.Find("posText").GetComponent<Text>().text = rankString;

        int score = highscoreEntry.score;

        entryTransform.Find("scoreText").GetComponent<Text>().text = score.ToString();

        string name = highscoreEntry.name;
        entryTransform.Find("nameText").GetComponent<Text>().text = name;
        transformList.Add(entryTransform);

        int killed = highscoreEntry.killed;

        entryTransform.Find("killedText").GetComponent<Text>().text = killed.ToString();

        float survived = highscoreEntry.time;
        float minutes = Mathf.FloorToInt(survived / 60);
        float seconds = Mathf.FloorToInt(survived % 60);

        string time = string.Format("{0:00}:{1:00}", minutes, seconds);

        entryTransform.Find("timeText").GetComponent<Text>().text = time;
    }

    public void AddHighScoreEntry(int score, string name, int killed, float time)
    {
        HighscoreEntry highscoreEntry = new HighscoreEntry { score = score, name = name, killed = killed, time = time};

        string jsonString = PlayerPrefs.GetString("highscoreTable");
        HighScores highscores = JsonUtility.FromJson<HighScores>(jsonString);

        highscores.highscoreEntryList.Add(highscoreEntry);

        string json = JsonUtility.ToJson(highscores);
        PlayerPrefs.SetString("highscoreTable", json);
        PlayerPrefs.Save();
    }

    private class HighScores
    {
        public List<HighscoreEntry> highscoreEntryList;
    }

    [System.Serializable]
    private class HighscoreEntry
    {
        public int score;
        public string name;
        public int killed;
        public float time;
    }
}
