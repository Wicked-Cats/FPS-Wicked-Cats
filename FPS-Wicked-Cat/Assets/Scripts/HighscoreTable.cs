using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HighscoreTable : MonoBehaviour
{
   
    public Transform entryContainer;
    public Transform entryTemplate;

    private void Awake()
    {

        entryContainer = transform.Find("EntryContainer");
        entryTemplate = entryContainer.Find("EntryTemp");

        entryTemplate.gameObject.SetActive(false);

        float templateHeight = 10f;

        for (int i = 0; i < 10; i++)
        {
            Transform entryTransform = Instantiate(entryTemplate, entryContainer);
            RectTransform entryRect = entryTransform.GetComponent<RectTransform>();
            entryRect.anchoredPosition = new Vector2(0, -templateHeight * i);
            entryTransform.gameObject.SetActive(true);
        }
    }
}
