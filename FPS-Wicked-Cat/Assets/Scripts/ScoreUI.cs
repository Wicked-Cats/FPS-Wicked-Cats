using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreUI : MonoBehaviour
{
    [SerializeField] Text scoreValue;

    public void SetScoreValue(int score)
    {
        scoreValue.text = score.ToString();
    }
}
