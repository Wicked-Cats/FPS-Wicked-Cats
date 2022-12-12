using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class explosion : MonoBehaviour
{
    [SerializeField] int pushBackAmount;
    [SerializeField] bool push;
    private void OnTriggerEnter(Collider other)
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
