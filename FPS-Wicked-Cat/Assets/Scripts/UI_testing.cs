using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_testing : MonoBehaviour
{
    [SerializeField] private UI_InputWindow inputWindow;
    private Button _button;
    private void Start()
    {
        
    }
    void btnClick()
    {
        Debug.Log("Button Pressed"); 
    }
}
