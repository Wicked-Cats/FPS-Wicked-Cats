using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerBullet : MonoBehaviour
{
    [SerializeField] Rigidbody rb;

    public int damage;
    [SerializeField] int speed;
    [SerializeField] int despawnTimer;
    [SerializeField] GameObject explosion;


    void Start()
    {
        rb.velocity = Camera.main.transform.forward * speed;
        transform.rotation = Camera.main.transform.rotation;
        damage = gameManager.instance.playerScript.damage + gameManager.instance.playerScript.shootDamage;
        Destroy(gameObject, despawnTimer);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy") && other is CapsuleCollider)
        {
            if (other.GetComponent<IDamage>() != null)
            {
                other.GetComponent<IDamage>().takeDamage(damage);
            }
            if (explosion != null)
            {
                Instantiate(explosion, transform.position, transform.rotation);
            }
            Destroy(gameObject);
        }
        else if (other is MeshCollider)
        {
            if (explosion != null)
            {
                Instantiate(explosion, transform.position, transform.rotation);
            }
            Destroy(gameObject);
        }
    }
}