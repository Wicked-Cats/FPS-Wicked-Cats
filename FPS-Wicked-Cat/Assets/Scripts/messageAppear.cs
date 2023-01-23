using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class messageAppear : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public GameObject _rules;
    [SerializeField] bool isHighscore;

    // Start is called before the first frame update
    void Start()
    {
        _rules.SetActive(false);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        _rules.SetActive(true);
        if(isHighscore)
        {
            gameManager.instance.tableScores.SetUpTable();
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        _rules.SetActive(false);
    }

    public void OnPointerEnterManual()
    {
        _rules.SetActive(true);
    }

    public void OnPointerExitManual()
    {
        _rules.SetActive(false);
    }

}
