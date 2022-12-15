using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class explosion : MonoBehaviour
{
    [SerializeField] protected float playerPushBackAmount;
    [SerializeField] protected bool push;
    [SerializeField] protected float despawnTimer;

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
        else
        {
            aud.PlayOneShot(audEnemyExplosion, 0);
        }

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
