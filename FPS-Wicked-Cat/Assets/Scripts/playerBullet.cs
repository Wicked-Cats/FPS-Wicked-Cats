using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerBullet : MonoBehaviour
{
    [SerializeField] Rigidbody rb;

    public int damage;
    [SerializeField] int speed;
    [SerializeField] int despawnTimer;

    // Start is called before the first frame update
    void Start()
    {
        rb.velocity = Camera.main.transform.forward * speed;
        damage = gameManager.instance.playerScript.damage + gameManager.instance.playerScript.shootDamage;

        Destroy(gameObject, despawnTimer);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other is CapsuleCollider)
        {
            if (other.CompareTag("Enemy"))
            {
                if (other.GetComponent<IDamage>() != null)
                {
                    other.GetComponent<IDamage>().takeDamage(damage);
                }
            }
        }

        Destroy(gameObject);
    }
}
