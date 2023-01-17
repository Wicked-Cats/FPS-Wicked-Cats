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
    private Color colorOrig;

    [Header("-- Enemy Stats")]
    [SerializeField] int HP;
    private int HPOrig;

    [Header("-- Enemy Vision --")]
    [SerializeField] int lineOfSight;

    [Header("-- Item Drops --")]
    [SerializeField] GameObject itemDrop;

    bool isPathed;


    void Start()
    {
        HPOrig = HP;
        colorOrig = model.material.color;
    }

    void Update()
    {

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
                    forceSpawnPos.y = 0;
                    Instantiate(forceField, forceSpawnPos, this.transform.rotation);
                    gameManager.instance.forceField = GameObject.FindGameObjectWithTag("Force Field");
                }
            }
        }
    }

    public void takeDamage(int damage)
    {
        HP -= damage;
        agent.SetDestination(gameManager.instance.player.transform.position);

        StartCoroutine(dmgFlash());

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


