using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopKeepers : MonoBehaviour
{
    public Animator shopKeeper;
  
    // Start is called before the first frame update
    void Start()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.isTrigger)
        {
            shopKeeper.SetTrigger("isAnim");
            shopKeeper.SetBool("isNear", true);
        }

    }
    private void OnTriggerExit(Collider other)
    {
        if (other.isTrigger)
        {
            shopKeeper.SetBool("isNear", false);
            shopKeeper.ResetTrigger("isAnim");
        }
    }

}
