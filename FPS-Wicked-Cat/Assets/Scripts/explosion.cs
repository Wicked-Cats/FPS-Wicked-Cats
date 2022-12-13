using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class explosion : MonoBehaviour
{
    [SerializeField] float pushBackAmount;
    [SerializeField] bool push;
    [SerializeField] float despawnTimer;


    private void Start()
    {
        Destroy(gameObject, despawnTimer);
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player") || other.CompareTag("Enemy"))
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
