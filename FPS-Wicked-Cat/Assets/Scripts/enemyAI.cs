using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class enemyAI : MonoBehaviour, IDamage
{
    [Header("-- Components --")]
    [SerializeField] Renderer model;
    [SerializeField] NavMeshAgent agent;

    [Header("-- Enemy Stats")]
    [SerializeField] int HP;
    private int HPOrig;

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
    

    // Start is called before the first frame update
    void Start()
    {
        HPOrig = HP;
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
        else
        {
            agent.SetDestination(gameManager.instance.player.transform.position);
        }
    }

    void LineOfSight()
    {
        angleToPlayer = Vector3.Angle(gameManager.instance.player.transform.position - shootPos.position, transform.forward);
        RaycastHit hit;

        if (Physics.Raycast(shootPos.position, gameManager.instance.player.transform.position, out hit))
        {
            if (hit.collider.CompareTag("Player") && angleToPlayer <= lineOfSight)
            {
                agent.SetDestination(gameManager.instance.player.transform.position);

                if (!isShooting) // so if he sees us he starts to shoot
                {
                    StartCoroutine(shoot());
                }
            }

            if (agent.remainingDistance <= agent.stoppingDistance)
            {
                transform.LookAt(gameManager.instance.player.transform.position);
            }
        }
       
    }

    public void takeDamage(int damage)
    {
        HP -= damage;
        StartCoroutine(dmgFlash());
        transform.LookAt(gameManager.instance.player.transform.position);
        agent.SetDestination(gameManager.instance.player.transform.position);

        if (HP <= 0)
        {
            gameManager.instance.componentsCurrent += HPOrig;
            gameManager.instance.componentsTotal += HPOrig;
            Destroy(gameObject);
        }
    }

    public void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            inSight = true;
            isPatrolling= false;
        }
    }

    public void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            inSight = false;
            isPatrolling= true;
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
        agent.stoppingDistance = stopDistOrig;
        isWaiting = false;
    }

    IEnumerator dmgFlash()
    {
        model.material.color = Color.red;
        yield return new WaitForSeconds(0.2f);
        model.material.color = Color.white;
    }

}
