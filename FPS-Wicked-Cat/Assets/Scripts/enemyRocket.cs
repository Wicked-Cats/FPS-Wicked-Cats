using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class enemyRocket : explosion
{
    [SerializeField] int damageAmount;
    [SerializeField] int radius;
    [SerializeField] int force;
    [SerializeField] GameObject hitEffect;

    private new void Start()
    {
        StartCoroutine(damage());
        Destroy(gameObject);
    }

    private new void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            base.OnTriggerStay(other);
        }
    }

    IEnumerator damage()
    {
        Vector3 distance = gameManager.instance.player.transform.position - transform.position;
        if (distance.magnitude < radius)
        {
            gameManager.instance.playerScript.takeDamage(damageAmount);
        }
        Instantiate(hitEffect, transform.position, transform.rotation);
        yield return new WaitForSeconds(1);

    }
}
