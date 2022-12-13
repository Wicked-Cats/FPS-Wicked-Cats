using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]

public class gunObjects : ScriptableObject
{
    public string gunName;
    public int shootDamage;
    public float shootRate;
    public int shootDistance;
    public GameObject bulletModel;
    public GameObject gunModel;
    public AudioClip gunShot;
}
