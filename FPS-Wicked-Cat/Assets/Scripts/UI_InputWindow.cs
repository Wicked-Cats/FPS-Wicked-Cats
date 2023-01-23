using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_InputWindow : MonoBehaviour
{
    // code is the start for the input for users name
    private void Awake()
    {
        Hide();
    }
    public void Show()
    {
        gameObject.SetActive(true);
    }

    public void Hide()
    {
        gameObject.SetActive(false);
    }
}
