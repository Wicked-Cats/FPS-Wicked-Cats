using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class messageAppear : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public GameObject _rules;

    // Start is called before the first frame update
    void Start()
    {
        _rules.SetActive(false);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        _rules.SetActive(true);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        _rules.SetActive(false);
    }
}
