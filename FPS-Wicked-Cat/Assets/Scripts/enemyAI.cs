using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class enemyAI : MonoBehaviour, IDamage
{
    private Score score;
    private gameManager gameManager;
    [Header("-- Components --")]
    [SerializeField] Renderer model;
    [SerializeField] SkinnedMeshRenderer meshRenderer1;
    [SerializeField] SkinnedMeshRenderer meshRenderer2;
    [SerializeField] NavMeshAgent agent;
    [SerializeField] Animator anim;
    public Color colorOrig;
    [SerializeField] Material baseMaterial;
    [SerializeField] Material dissolveMaterial;
    [SerializeField] GameObject body;
    [SerializeField] GameObject[] legs;

    [Header("-- Enemy Stats")]
    [SerializeField] int HP;
    private int HPOrig;
    [SerializeField] Transform headPos;

    [Header("-- Enemy Vision --")]
    [SerializeField] int lineOfSight;
    [SerializeField] float playerFaceSpeed;
    [SerializeField] float extraShotRange;
    private float angleToPlayer;

    [Header("-- Enemy Gun Stats --")]
    [SerializeField] float shootRate;
    [SerializeField] GameObject bullet;
    [SerializeField] Transform shootPos;
    [SerializeField] bool isShooting;

    [Header("-- Item Drops --")]
    [SerializeField] GameObject[] itemDrop;

    [Header("-- Effects --")]
    [SerializeField] GameObject explosion;
    [SerializeField] GameObject fireVaccum;
    [SerializeField] float dissolveSpeed;

    [Header("----- Scoring System -----")]
    public int scoreValue;
    public int killCount;



    Vector3 playerDir;
    bool imDead;
    bool isPathed;
    bool teleporting;
    float timeForDissolve;
    bool teleportCycle;

    void Start()
    {
        HP = HP + gameManager.instance.timeDamageIncrease;
        HPOrig = HP;
        colorOrig = model.material.color;
        meshRenderer1 = body.GetComponent<SkinnedMeshRenderer>();
        gameManager = GameObject.Find("Game Manager").GetComponent<gameManager>();
    }

    void Update()
    {
        if (agent.isOnOffMeshLink || teleporting)
        {
            if (!teleporting)
            {
                agent.isStopped = true;
                meshRenderer1.material = dissolveMaterial;
                foreach (GameObject x in legs)
                {

                    meshRenderer2 = x.GetComponent<SkinnedMeshRenderer>();
                    meshRenderer2.material = dissolveMaterial;
                }
                timeForDissolve = 0.01f;
                teleporting = true;
            }
            if (teleporting)
            {

                if (dissolveMaterial.GetFloat("_Cutoff") > 0.9f)
                {
                    agent.CompleteOffMeshLink();
                    teleportCycle = true;
                }
                else if (dissolveMaterial.GetFloat("_Cutoff") < 0.05f && teleportCycle)
                {
                    meshRenderer1.material = baseMaterial;
                    foreach (GameObject x in legs)
                    {

                        meshRenderer2 = x.GetComponent<SkinnedMeshRenderer>();
                        meshRenderer2.material = baseMaterial;
                    }
                    agent.isStopped = false;
                    teleporting = false;
                    teleportCycle = false;
                }
            }
            dissolveMaterial.SetFloat("_Cutoff", Mathf.Sin(timeForDissolve * dissolveSpeed));
            timeForDissolve += Time.deltaTime;
        }

        if (HP > 0)
        {
            anim.SetFloat("Speed", agent.velocity.normalized.magnitude);
            LineOfSight();
        }
    }

    //checks if player is in enemies view and start shooting when 
    void LineOfSight()
    {
        if (!isPathed && agent.isOnOffMeshLink == false && !teleporting)
        {
            StartCoroutine(path());
        }


        playerDir = gameManager.instance.enemyAimPoint.transform.position - headPos.position;
        angleToPlayer = Vector3.Angle(playerDir, transform.forward);

        RaycastHit see;

        Debug.DrawRay(headPos.position, playerDir);

        if (Physics.Raycast(headPos.position, playerDir, out see))
        {
            if (see.collider.CompareTag("Player") && angleToPlayer <= lineOfSight)
            {

                if (!isShooting && angleToPlayer <= lineOfSight / 3 && playerDir.magnitude <= agent.stoppingDistance + extraShotRange && !teleporting) // so if he sees us and we are mostly in front of him he starts to shoot
                {
                    StartCoroutine(shoot());
                }


            }
            FacePlayer();

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
        if (!imDead && !teleporting)
        {
            //damages enemy and gives feedback to player
            HP -= damage;
            StartCoroutine(dmgFlash());

            //check if enemy has died
            if (HP <= 0)
            {
                StartCoroutine(death());
                imDead = true;

                score.UpdateEnemyKillCount();
                score.AddScore(scoreValue);

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

            }
            //makes enemy look to players last position in response to the damage
            FacePlayer();
        }
    }

    IEnumerator shoot()
    {
        isShooting = true;
        //anim.SetTrigger("Shoot");
        Instantiate(bullet, shootPos.position, transform.rotation);
        yield return new WaitForSeconds(shootRate);
        isShooting = false;
    }

    IEnumerator dmgFlash()
    {
        model.material.color = Color.red;
        yield return new WaitForSeconds(0.3f);
        model.material.color = colorOrig;
    }

    IEnumerator death()
    {
        agent.SetDestination(transform.position);
        agent.isStopped = true;
        // creates fire vaccum
        Instantiate(fireVaccum, transform.position, transform.rotation);
        anim.SetBool("Death", true);

        yield return new WaitForSeconds(2f);

        // creates EXPLOSION!!!!!
        Instantiate(explosion, transform.position, transform.rotation);

        Destroy(gameObject);
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
