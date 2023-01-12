using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;


public class ScoreUI : MonoBehaviour
{
    [SerializeField] TextMeshPro scoreValue;

    public void SetScoreValue(int score)
    {
        scoreValue.text = score.ToString();
    }
}
