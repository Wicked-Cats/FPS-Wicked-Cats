using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class enemyAI : MonoBehaviour, IDamage
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
    [SerializeField] float playerFaceSpeed;
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

    Vector3 playerDir;
    bool playerInRange;


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

        Debug.DrawRay(headPos.position, playerDir);

        if (Physics.Raycast(headPos.position, playerDir, out see))
        {
            if (see.collider.CompareTag("Player") && angleToPlayer <= lineOfSight)
            {
                agent.SetDestination(gameManager.instance.player.transform.position);

                if (!isShooting) // so if he sees us he starts to shoot
                {
                    StartCoroutine(shoot());
                }
                if (agent.remainingDistance <= agent.stoppingDistance)
                {
                    FacePlayer();
                }
            }

        }
    }

    void FacePlayer()
    {
        playerDir.y = 0;
        Quaternion rot = Quaternion.LookRotation(playerDir);
        transform.rotation = Quaternion.Lerp(transform.rotation, rot, Time.deltaTime * playerFaceSpeed);
    }

    public void takeDamage(int damage)
    {
        HP -= damage;
        FacePlayer();
        agent.SetDestination(gameManager.instance.player.transform.position);
        StartCoroutine(dmgFlash());

        if (HP <= 0)
        {
            //add components
            gameManager.instance.componentsCurrent += HPOrig;
            gameManager.instance.componentsTotal += HPOrig;

            //game win condition
            if (gameManager.instance.componentsTotal >= 30)
            {
                gameManager.instance.isPaused = !gameManager.instance.isPaused;
                gameManager.instance.activeMenu = gameManager.instance.winMenu;
                gameManager.instance.activeMenu.SetActive(gameManager.instance.isPaused);
                gameManager.instance.pause();
                gameManager.instance.componentsTotal = 0;
                gameManager.instance.componentsCurrent = 0;
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
        yield return new WaitForSeconds(0.3f);
        model.material.color = colorOrig;
    }

}
