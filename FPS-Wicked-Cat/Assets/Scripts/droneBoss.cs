using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class droneBoss : MonoBehaviour , IDamage
{
    [Header("-- Components --")]
    [SerializeField] Renderer model;
    [SerializeField] NavMeshAgent agent;
    public Color colorOrig;
    [SerializeField] Material baseMaterial;
    [SerializeField] Material dissolveMaterial;

    [Header("-- Enemy Stats")]
    [SerializeField] int HP;
    private int HPOrig;
    [SerializeField] Transform headPos;

    [Header("-- Enemy Vision --")]
    [SerializeField] int lineOfSight;
    private float angleToPlayer;

    [Header("-- Enemy Gun Stats --")]
    [SerializeField] float shootRate1;
    [SerializeField] float shootRate2;
    [SerializeField] GameObject bullet1;
    [SerializeField] GameObject bullet2;
    [SerializeField] Transform shootPos1;
    [SerializeField] Transform shootPos2;
    [SerializeField] Transform shootPos3;
    [SerializeField] Transform shootPos4;
    [SerializeField] bool isShooting1;
    [SerializeField] bool isShooting2;

    [Header("-- Item Drops --")]
    [SerializeField] GameObject[] itemDrop;

    [Header("-- Movment --")]
    [SerializeField] float teleportInterval;
    [SerializeField] float dissolveSpeed;
    [SerializeField] int emergencyTeleportInterval;


    [Header("----- Scoring System -----")]
    [SerializeField] int score;


    Vector3 playerDir;
    bool isTeleporting;
    bool teleportCycle;
    float timeForDissolve;
    bool baseTeleportTimer;
    bool emergencyTeleport;
    int playerInSightTick;
    bool timerActive;
    bool firstTeleport;

    // Start is called before the first frame update
    void Start()
    {
        HPOrig = HP;
        colorOrig = model.material.color;
        score = HPOrig;
    }

    // Update is called once per frame
    void Update()
    {
        if (!isTeleporting)
        {
            LineOfSight();
            if (!firstTeleport)
            {
                StartCoroutine(teleportTimer());
                firstTeleport = true;
            }
        }

        if (baseTeleportTimer || isTeleporting)
        {
            teleport();
        }

        if (emergencyTeleport && !isTeleporting)
        {
            teleport();
            emergencyTeleport = false;
        }
    }

    void LineOfSight()
    {
        playerDir = gameManager.instance.enemyAimPoint.transform.position - headPos.position;
        angleToPlayer = Vector3.Angle(playerDir, transform.forward);

        RaycastHit see;
        transform.LookAt(gameManager.instance.player.transform.position);

        if (Physics.Raycast(headPos.position, playerDir, out see))
        {
            if (see.collider.CompareTag("Player") && angleToPlayer <= lineOfSight)
            {
                playerInSightTick = 0;
                if (!isShooting1) // so if he sees us he starts to shoot
                {
                    StartCoroutine(shoot1());
                }
                if (!isShooting2)
                {
                    StartCoroutine(shoot2());
                }
            }
            else if (!isTeleporting)
            {
                playerInSightTick++;
            }
            if (playerInSightTick == emergencyTeleportInterval)
            {
                emergencyTeleport = true;
                playerInSightTick = 0;
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
            gameManager.instance.scoreTotal += score;
            gameManager.instance.killcount++;
            // item drop
            GameObject drop = itemDrop[Random.Range(0, itemDrop.Length - 1)];
            cogPickup cog = drop.GetComponent<cogPickup>();
            if (cog.isHealthPack)
            {
                Instantiate(drop, transform.position, transform.rotation);
            }
            else
            {
                for (int i = 0; i < HPOrig; i++)
                {
                    Transform item = transform;
                    item.position = new Vector3(item.position.x + Random.Range(-0.75f, 0.75f), item.position.y, item.position.z - Random.Range(-0.75f, 0.75f));
                    Instantiate(drop, item.position, transform.rotation);
                }
            }

            
            Destroy(gameObject);
        }
    }

    IEnumerator shoot1()
    {
        isShooting1 = true;
        Instantiate(bullet1, shootPos1.position, transform.rotation);
        Instantiate(bullet1, shootPos2.position, transform.rotation);
        yield return new WaitForSeconds(shootRate1);
        isShooting1 = false;
    }

    IEnumerator shoot2()
    {
        isShooting2 = true;
        Instantiate(bullet2, shootPos3.position, transform.rotation);
        Instantiate(bullet2, shootPos4.position, transform.rotation);
        yield return new WaitForSeconds(shootRate2);
        isShooting2 = false;
    }

    
    IEnumerator dmgFlash()
    {
        model.material.color = Color.red;
        yield return new WaitForSeconds(0.2f);
        model.material.color = colorOrig;
    }

    IEnumerator teleportTimer()
    {
        baseTeleportTimer = false;
        timerActive = true;
        yield return new WaitForSeconds(teleportInterval);
        baseTeleportTimer = true;
        timerActive = false;
    }

    void teleport()
    {
        if (!isTeleporting)
        {
            model.material = dissolveMaterial;
            timeForDissolve = 0.01f;
            isTeleporting = true;
        }
        if (isTeleporting)
        {

            if (dissolveMaterial.GetFloat("_Cutoff") > 0.9f)
            {
                ////implement movement
                int xChange = Random.Range(-30, 30);
                int zChange = Random.Range(-30, 30);

                Vector3 targetPos = new Vector3(gameManager.instance.player.transform.position.x + xChange,
                    gameManager.instance.player.transform.position.y,
                    gameManager.instance.player.transform.position.z + zChange);

                NavMeshHit hit;
                if (NavMesh.SamplePosition(targetPos, out hit, 10f, 1))
                {
                    agent.transform.position = hit.position;
                    teleportCycle = true;
                }
                ///movement end
            }
            else if (dissolveMaterial.GetFloat("_Cutoff") < 0.05f && teleportCycle)
            {
                model.material = baseMaterial;
                isTeleporting = false;
                if (!timerActive)
                {
                    StartCoroutine(teleportTimer());
                }
                teleportCycle = false;
            }
        }
        dissolveMaterial.SetFloat("_Cutoff", Mathf.Sin(timeForDissolve * dissolveSpeed));
        timeForDissolve += Time.deltaTime;
    }
}