using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class explosion : MonoBehaviour
{
    [SerializeField] float pushBackAmount;
    [SerializeField] bool push;
    [SerializeField] float despawnTimer;
    [SerializeField] bool isFireVaccum;
    ParticleSystem mySystem;


    private void Start()
    {
        mySystem = GetComponent<ParticleSystem>();
        mySystem.Stop();
        var main = mySystem.main;
        if(isFireVaccum)
        {
        main.duration = 2f;
        }
        mySystem.Play();
        Destroy(gameObject, despawnTimer);
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (push)
            {
                gameManager.instance.playerScript.PushBackInput((other.transform.position - transform.position) * pushBackAmount);
            }
            else
            {
                gameManager.instance.playerScript.PushBackInput((transform.position - other.transform.position) * pushBackAmount);

            }
        }

    }
}
