using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cogPickup : MonoBehaviour
{
    [SerializeField] GameObject cog;

    Vector3 rot;

    private void Update()
    {
        transform.Rotate(0f, 0.5f, 0f);
    }  
    
    public void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            
        }
    }
    

}
