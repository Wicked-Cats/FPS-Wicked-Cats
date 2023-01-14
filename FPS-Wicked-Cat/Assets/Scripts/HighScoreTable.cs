using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HighScoreTable : MonoBehaviour
{
    [Header("------ Score board Container ------")]
    private Transform entryContainer;
    private Transform entryTemplate;

    public void HighScoreEntry()
    {
       entryContainer= transform.Find("EntryContainer");
        entryTemplate = entryContainer.Find("EntryTemp");
         
        entryContainer.gameObject.SetActive(false);

        //float temlateheight = 20f;

        for (int i = 0; i < 10; i++)
        {
            Transform entrytransform = Instantiate(entryTemplate,entryContainer);
            RectTransform entry = entrytransform.GetComponent<RectTransform>();
            //entry.anchoredPosition = new Vector2(0,  i);
            entryContainer.gameObject.SetActive(true);
        }
    }

}
    