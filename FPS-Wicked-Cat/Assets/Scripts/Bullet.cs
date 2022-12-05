using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField] Rigidbody rb;

    [SerializeField] int damage;
    [SerializeField] int speed;
    [SerializeField] int despawnTimer;

    // Start is called before the first frame update
    void Start()
    {
        rb.velocity = Camera.main.transform.forward * speed;
        Destroy(gameObject, despawnTimer);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            if (other.GetComponent<IDamage>() != null)
            {
                other.GetComponent<IDamage>().takeDamage(damage);
            }
        }
        else if (other.CompareTag("Player"))
        {
            gameManager.instance.playerScript.takeDamage(damage);
        }
        Destroy(gameObject);
    }
}