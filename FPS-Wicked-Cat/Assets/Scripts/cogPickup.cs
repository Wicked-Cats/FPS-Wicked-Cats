using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cogPickup : MonoBehaviour
{
    [SerializeField] GameObject cog;

    public void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            
        }
    }
}
