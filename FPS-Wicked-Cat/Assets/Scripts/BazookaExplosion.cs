using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BazookaExplosion : explosion
{
    [SerializeField] int damageAmount;
    [SerializeField] int radius;
    [SerializeField] int force;
    [SerializeField] GameObject[] enemies;
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
        enemies = GameObject.FindGameObjectsWithTag("Enemy");

        foreach (GameObject enemy in enemies)
        {
            Vector3 distance = enemy.transform.position - transform.position;
            if (distance.magnitude < radius)
            {
                enemy.GetComponent<IDamage>().takeDamage(damageAmount);
            }
        }
        Instantiate(hitEffect, transform.position, transform.rotation);
        yield return new WaitForSeconds(1);
        
    }
}