using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cogPickup : MonoBehaviour
{
    [SerializeField] GameObject cog;
    [SerializeField] public bool isHealthPack;
    Vector3 rot;


    private void Update()
    {
        transform.Rotate(0f, 0.5f, 0f);
    }

    public void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (isHealthPack)
            {
                HealthPack();
            }
            else
            {
                Components();
            }
            Destroy(gameObject);
        }
    }

    void HealthPack()
    {
        gameManager.instance.playerScript.HP += 2;

        if (gameManager.instance.playerScript.HP > gameManager.instance.playerScript.HPOrig)
        {
            gameManager.instance.playerScript.HP = gameManager.instance.playerScript.HPOrig;

        }

        gameManager.instance.playerScript.updateHPBar();
        Destroy(gameObject);
    }

    void Components()
    {
        gameManager.instance.componentsCurrent += 1;
        gameManager.instance.componentsTotal += 1;

        Destroy(gameObject);
    }

}
