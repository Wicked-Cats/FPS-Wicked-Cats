using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cogPickup : MonoBehaviour
{
    [SerializeField] GameObject cog;
    [SerializeField] public bool isHealthPack;
    [SerializeField] Rigidbody rb;
    Vector3 rot;
    Vector3 pushBack;
    bool beingPulled;

    private void Start()
    {
        rb = this.GetComponent<Rigidbody>();
    }
    private void Update()
    {
        transform.Rotate(0f, 0.5f, 0f);
        if (beingPulled)
        {
            rb.velocity = pushBack;
        }
    }

    public void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && !(other is SphereCollider))
        {
            if (isHealthPack)
            {
                HealthPack();
            }
            else
            {
                Components();
            }
            Destroy(gameObject);
        }
    }

    void HealthPack()
    {
        gameManager.instance.playerScript.HP = gameManager.instance.playerScript.HP + (gameManager.instance.playerScript.HPOrig / 10);

        if (gameManager.instance.playerScript.HP > gameManager.instance.playerScript.HPOrig)
        {
            gameManager.instance.playerScript.HP = gameManager.instance.playerScript.HPOrig;
        }

        gameManager.instance.playerScript.updateHPBar();
        Destroy(gameObject);
    }

    void Components()
    {
        gameManager.instance.componentsCurrent += 1;
        gameManager.instance.componentsTotal += 1;
        gameManager.instance.updateComponentsDisplay();
        Destroy(gameObject);
    }

    public void PushbackSet(Vector3 _pushBack)
    {
        pushBack = _pushBack;
        beingPulled = true;
    }
}
