using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class enemyAIFlyers : MonoBehaviour, IDamage
{
    [Header("-- Components --")]
    [SerializeField] Renderer model;
    [SerializeField] NavMeshAgent agent;
    public Color colorOrig;

    [Header("-- Enemy Stats")]
    [SerializeField] int HP;
    private int HPOrig;
    [SerializeField] Transform headPos;

    [Header("-- Enemy Vision --")]
    private bool isPatrolling = true;
    [SerializeField] int lineOfSight;
    private float angleToPlayer;
    bool inSight;

    [Header("-- Enemy Gun Stats --")]
    [SerializeField] int shootDmg;
    [SerializeField] float shootRate;
    [SerializeField] GameObject bullet;
    [SerializeField] Transform shootPos;
    [SerializeField] bool isShooting;

    [Header("-- Patrol Points --")]
    [SerializeField] GameObject[] patrolPoints;
    private float stopDistOrig;
    private int pointMovement;
    bool isWaiting;

    [Header("-- Item Drops --")]
    [SerializeField] GameObject[] itemDrop;


    Vector3 playerDir;


    // Start is called before the first frame update
    void Start()
    {
        HPOrig = HP;
        colorOrig = model.material.color;
        stopDistOrig = agent.stoppingDistance;
    }

    // Update is called once per frame
    void Update()
    {
        if (inSight)
        {
            agent.stoppingDistance = stopDistOrig;
            LineOfSight();
        }
        else
        {
            AiMovement();
        }

    }

    void AiMovement()
    {

        if (isPatrolling)
        {
            if (!isWaiting)
            {
                StartCoroutine(changePoint(patrolPoints[pointMovement]));

                if (pointMovement != patrolPoints.Length - 1)
                {
                    pointMovement++;
                }
                else
                {
                    pointMovement = 0;
                }
            }
        }


    }

    void LineOfSight()
    {
        playerDir = gameManager.instance.player.transform.position - headPos.position;
        angleToPlayer = Vector3.Angle(playerDir, transform.forward);

        RaycastHit see;

        if (Physics.Raycast(headPos.position, playerDir, out see))
        {
            Debug.DrawRay(headPos.position, playerDir);
            if (see.collider.CompareTag("Player") && angleToPlayer <= lineOfSight)
            {
                agent.SetDestination(gameManager.instance.player.transform.position);
                transform.LookAt(gameManager.instance.player.transform.position);
                if (!isShooting) // so if he sees us he starts to shoot
                {
                    StartCoroutine(shoot());
                }

                if (agent.remainingDistance <= agent.stoppingDistance)
                {
                    transform.LookAt(gameManager.instance.player.transform.position);
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

    public void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            inSight = true;
            isPatrolling = false;
        }
    }

    public void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            inSight = false;
            isPatrolling = true;
        }
    }

    IEnumerator shoot()
    {
        isShooting = true;
        Instantiate(bullet, shootPos.position, transform.rotation);
        yield return new WaitForSeconds(shootRate);
        isShooting = false;
    }

    IEnumerator changePoint(GameObject point)
    {
        isWaiting = true;
        agent.stoppingDistance = 0;
        agent.SetDestination(point.transform.position);
        yield return new WaitForSeconds(10f);
        isWaiting = false;
    }

    IEnumerator dmgFlash()
    {
        model.material.color = Color.red;
        yield return new WaitForSeconds(0.2f);
        model.material.color = colorOrig;
    }

    // vvv in TESTING phase vvv
    //private void ItemDrop()
    //{
    //    Instantiate(components, transform.parent);
    //    gameManager.instance.componentsCurrent += HPOrig;
    //    gameManager.instance.componentsTotal += HPOrig;
    //}
}
