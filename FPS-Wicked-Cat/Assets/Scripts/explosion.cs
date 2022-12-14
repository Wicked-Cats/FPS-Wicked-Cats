using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class explosion : MonoBehaviour
{
    [SerializeField] protected float playerPushBackAmount;
    [SerializeField] protected bool push;
    [SerializeField] protected float despawnTimer;


    private void Start()
    {
        Destroy(gameObject, despawnTimer);
    }

    protected void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (push)
            {
                gameManager.instance.playerScript.PushBackInput((other.transform.position - transform.position) * playerPushBackAmount);
            }
            else
            {
                gameManager.instance.playerScript.PushBackInput((transform.position - other.transform.position) * playerPushBackAmount);
            }
        }
    }
}
