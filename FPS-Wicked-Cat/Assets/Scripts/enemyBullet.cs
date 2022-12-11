using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class enemyBullet : MonoBehaviour
{
    [SerializeField] Rigidbody rb;

    public int damage;
    [SerializeField] int speed;
    [SerializeField] int despawnTimer;

    // Start is called before the first frame update
    void Start()
    {
        if (CompareTag("Enemy Bullet"))
        {
            rb.velocity = transform.forward * speed;
        }

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
    }
}