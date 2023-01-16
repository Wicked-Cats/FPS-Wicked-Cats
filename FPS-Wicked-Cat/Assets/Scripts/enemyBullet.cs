using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class enemyBullet : MonoBehaviour
{
    [SerializeField] Rigidbody rb;

    public int damage;
    [SerializeField] float speed;
    [SerializeField] int despawnTimer;
    [SerializeField] bool homing;
    [SerializeField] float homingAccuracy;
    [SerializeField] GameObject explosion;

    bool aimDelay;

    void Start()
    {
        if (CompareTag("Enemy Bullet"))
        {
            rb.velocity = Vector3.Normalize(gameManager.instance.enemyAimPoint.transform.position - transform.position) * speed;
        }
        damage += gameManager.instance.timeDamageIncrease;
        Destroy(gameObject, despawnTimer);
    }

    private void Update()
    {
        if (homing)
        {
            float angleToPlayer = Vector3.Angle(gameManager.instance.enemyAimPoint.transform.position - transform.position, transform.forward);
            if (angleToPlayer <= 1.25f)
            {
                rb.velocity = Vector3.Lerp(rb.velocity, Vector3.Normalize(gameManager.instance.enemyAimPoint.transform.position - transform.position) * speed, homingAccuracy * 1000f);
                transform.forward = Vector3.Normalize(rb.velocity);
            }

        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other is CapsuleCollider && (other.CompareTag("Player")))
        {
            gameManager.instance.playerScript.takeDamage(damage);

            if (explosion != null)
            {
                Instantiate(explosion, transform.position, transform.rotation);
            }

            Destroy(gameObject);
        }
        else if (!(other is SphereCollider) && !(other.CompareTag("Enemy")))
        {
            if (explosion != null)
            {
                Instantiate(explosion, transform.position, transform.rotation);
            }

            Destroy(gameObject);
        }


    }
}