using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class explosion : MonoBehaviour
{
    [SerializeField] float pushBackAmount;
    [SerializeField] bool push;
    [SerializeField] float despawnTimer;

    [Header("-- Audio --")]
    [SerializeField] AudioSource aud;
    [SerializeField] AudioClip audEnemyExplosion;
    [Range(0, 1)][SerializeField] float audEnemyExplosionVol;


    private void Start()
    {
        if (push)
        {
            aud.PlayOneShot(audEnemyExplosion, audEnemyExplosionVol);
        }

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
