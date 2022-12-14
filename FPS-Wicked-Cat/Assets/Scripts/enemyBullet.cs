using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class enemyBullet : MonoBehaviour
{
    [SerializeField] Rigidbody rb;

    public int damage;
    [SerializeField] float speed;
    [SerializeField] int despawnTimer;

    void Start()
    {
        if (CompareTag("Enemy Bullet"))
        {
            rb.velocity = Vector3.Normalize(gameManager.instance.enemyAimPoint.transform.position - transform.position) * speed;
        }
        damage += gameManager.instance.timeDamageIncrease;
        Destroy(gameObject, despawnTimer);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other is CapsuleCollider)
        {
            if (other.CompareTag("Player"))
            {
                gameManager.instance.playerScript.takeDamage(damage);
            }

            Destroy(gameObject);
        }
        else if (other is SphereCollider)
        {
            
        }
        else
        {
            Destroy(gameObject);
        }
    }
}