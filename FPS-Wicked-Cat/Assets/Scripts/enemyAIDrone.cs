using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class enemyAIDrone : MonoBehaviour, IDamage
{
    [Header("-- Components --")]
    [SerializeField] Renderer model;
    [SerializeField] NavMeshAgent agent;
    [SerializeField] MeshRenderer meshRenderer1;
    public Color colorOrig;
    [SerializeField] Material baseMaterial;
    [SerializeField] Material dissolveMaterial;
    [SerializeField] GameObject body;

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

    [Header("-- Effects --")]
    [SerializeField] float dissolveSpeed;


    Vector3 playerDir;
    float stopDistOrig;
    bool isPathed;
    bool teleporting;
    float timeForDissolve;
    bool teleportCycle;

    // Start is called before the first frame update
    void Start()
    {
        HP = HP + gameManager.instance.timeDamageIncrease;
        HPOrig = HP;
        colorOrig = model.material.color;
        meshRenderer1 = body.GetComponent<MeshRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        if (agent.isOnOffMeshLink || teleporting)
        {
            if (!teleporting)
            {
                agent.isStopped = true;
                meshRenderer1.material = dissolveMaterial;
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
                    agent.isStopped = false;
                    teleporting = false;
                    teleportCycle = false;
                }
            }
            dissolveMaterial.SetFloat("_Cutoff", Mathf.Sin(timeForDissolve * dissolveSpeed));
            timeForDissolve += Time.deltaTime;
        }
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
