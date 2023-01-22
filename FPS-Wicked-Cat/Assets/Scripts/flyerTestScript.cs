using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class flyerTestScript : MonoBehaviour, IDamage
{
    [SerializeField] GameObject forceField;
    [SerializeField] bool forceFieldEngaged;

    [Header("-- Components --")]
    [SerializeField] Renderer model;
    [SerializeField] NavMeshAgent agent;
    [SerializeField] MeshRenderer meshRenderer1;
    private Color colorOrig;
    [SerializeField] Material baseMaterial;
    [SerializeField] Material dissolveMaterial;
    [SerializeField] GameObject body;

    [Header("-- Enemy Stats")]
    [SerializeField] float HP;
    private float HPOrig;

    [Header("-- Enemy Vision --")]
    [SerializeField] int lineOfSight;

    [Header("-- Item Drops --")]
    [SerializeField] GameObject itemDrop;

    [Header("-- Effects --")]
    [SerializeField] float dissolveSpeed;

    bool isPathed;
    bool teleporting;
    float timeForDissolve;
    bool teleportCycle;


    void Start()
    {
        HP = HP + gameManager.instance.timeDamageIncrease;
        HPOrig = HP;
        colorOrig = model.material.color;
        meshRenderer1 = body.GetComponent<MeshRenderer>();
    }

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

        if (!forceFieldEngaged)
        {
           LineOfSight();
        }
    }

    void LineOfSight()
    {
        if (!isPathed && agent.isOnOffMeshLink == false )//&& !teleporting)
        {
            StartCoroutine(path());
        }

        transform.LookAt(gameManager.instance.player.transform.position);

        float xDif = gameManager.instance.player.transform.position.x - this.transform.position.x;
        float zDif = gameManager.instance.player.transform.position.z - this.transform.position.z;

        if (!gameManager.instance.forceFieldActive)
        {
            if (xDif < 3 && xDif > -3)
            {
                if (zDif < 3 && zDif > -3)
                {
                    forceFieldEngaged = true;
                    gameManager.instance.forceFieldActive = true;
                    gameManager.instance.forceFieldMaker = this.gameObject;
                    Vector3 forceSpawnPos = this.transform.position;
                    forceSpawnPos.y = forceSpawnPos.y -3;
                    Instantiate(forceField, forceSpawnPos, this.transform.rotation);
                    gameManager.instance.forceField = GameObject.FindGameObjectWithTag("Force Field");
                }
            }
        }
    }

    public void takeDamage(int damage)
    {
        int rand = Random.Range(1, 100);
        if (rand <= gameManager.instance.critChance)
        {
            HP -= (float)damage * gameManager.instance.critDamageMulti;
        }
        else
        {
            HP -= damage;
        }

        StartCoroutine(dmgFlash(rand));

        if (HP <= 0)
        {
            //Destroy active force field if applicable
            if (gameManager.instance.forceFieldActive && gameManager.instance.forceFieldMaker == this.gameObject)
            {
                Destroy(gameManager.instance.forceField);
                gameManager.instance.forceFieldMaker = null;
                gameManager.instance.forceField = null;
                gameManager.instance.forceFieldActive = false;
            }

            Instantiate(itemDrop, transform.position, transform.rotation);

            Destroy(gameObject);
        }
    }

    IEnumerator dmgFlash(int rand)
    {
        if (rand <= gameManager.instance.critChance)
        {
            model.material.color = Color.yellow;
        }
        else
        {
            model.material.color = Color.red;
        }
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


