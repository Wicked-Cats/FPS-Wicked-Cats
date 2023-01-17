using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class enemyAIDrone : MonoBehaviour, IDamage
{
    [Header("-- Components --")]
    [SerializeField] Renderer model;
    [SerializeField] NavMeshAgent agent;
    public Color colorOrig;

    [Header("-- Drone Stats")]
    [SerializeField] int HP;
    private int HPOrig;
    [SerializeField] Transform headPos;

    [Header("-- Drone Vision --")]
    [SerializeField] int lineOfSight;
    private float angleToPlayer;
    bool inSight;

    [Header("-- Drone Gun Stats --")]
    [SerializeField] int shootDmg;
    [SerializeField] float shootRate;
    [SerializeField] GameObject bullet;
    [SerializeField] Transform shootPos;
    [SerializeField] Transform shootPos2;
    [SerializeField] bool isShooting;

    [Header("-- Item Drops --")]
    [SerializeField] GameObject[] itemDrop;


    Vector3 playerDir;
    float stopDistOrig;
    bool isPathed;

    // Start is called before the first frame update
    void Start()
    {
        HPOrig = HP;
        colorOrig = model.material.color;
    }

    // Update is called once per frame
    void Update()
    {
        LineOfSight();
    }

    void LineOfSight()
    {
        if (!isPathed && agent.isOnOffMeshLink == false )//&& !teleporting)
        {
            StartCoroutine(path());
        }

        playerDir = gameManager.instance.player.transform.position - headPos.position;
        angleToPlayer = Vector3.Angle(playerDir, transform.forward);

        RaycastHit see;
        transform.LookAt(gameManager.instance.player.transform.position);

        if (Physics.Raycast(headPos.position, playerDir, out see))
        {
            if (see.collider.CompareTag("Player") && angleToPlayer <= lineOfSight)
            {
                if (!isShooting) // so if he sees us he starts to shoot
                {
                    StartCoroutine(shoot());
                }
            }

        }
    }

    public void takeDamage(int damage)
    {
        //damages enemy and gives feedback to player
        HP -= damage;
        StartCoroutine(dmgFlash());

        //check if enemy has died
        if (HP <= 0)
        {

            // item drop
            GameObject drop = itemDrop[Random.Range(0, itemDrop.Length - 1)];
            cogPickup cog = drop.GetComponent<cogPickup>();
            if (cog.isHealthPack)
            {
                Instantiate(drop, shootPos.transform.position, transform.rotation);
            }
            else
            {
                for (int i = 0; i < HPOrig; i++)
                {
                    Transform item = shootPos.transform;
                    item.position = new Vector3(item.position.x + Random.Range(-0.75f, 0.75f), item.position.y, item.position.z - Random.Range(-0.75f, 0.75f));
                    Instantiate(drop, item.position, transform.rotation);
                }
            }

            Destroy(gameObject);
        }
    }

    IEnumerator shoot()
    {
        isShooting = true;
        Instantiate(bullet, shootPos.position, transform.rotation);
        Instantiate(bullet, shootPos2.position, transform.rotation);
        yield return new WaitForSeconds(shootRate);
        isShooting = false;
    }

    IEnumerator dmgFlash()
    {
        model.material.color = Color.red;
        yield return new WaitForSeconds(0.2f);
        model.material.color = colorOrig;
    }
    IEnumerator path()
    {
        isPathed = true;
        NavMeshPath path = new NavMeshPath();
        agent.CalculatePath(gameManager.instance.player.transform.position, path);
        agent.SetPath(path);
        yield return new WaitForSeconds(1f);
        isPathed = false;
    }
}
