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
    // vvv in TESTING phase vvv
    //[SerializeField] GameObject components;  // this object will be the item that drops from the enemy

    [Header("-- Enemy Vision --")]
    [SerializeField] int lineOfSight;
    private float angleToPlayer;
    bool inSight;

    Vector3 playerDir;

    void Start()
    {
        HPOrig = HP;
        colorOrig = model.material.color;
    }

    void Update()
    {
        if (inSight && !forceFieldEngaged)
        {
            LineOfSight();
        }

    }

    void LineOfSight()
    {
        agent.SetDestination(gameManager.instance.player.transform.position);
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
            //add components
            gameManager.instance.componentsCurrent += HPOrig;
            gameManager.instance.componentsTotal += HPOrig;

            //Destroy active force field if applicable
            if (gameManager.instance.forceFieldActive && gameManager.instance.forceFieldMaker == this.gameObject)
            {
                Destroy(gameManager.instance.forceField);
                gameManager.instance.forceFieldMaker = null;
                gameManager.instance.forceField = null;
                gameManager.instance.forceFieldActive = false;
            }

            Destroy(gameObject);
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
        }
    }

    public void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            inSight = true;
        }
    }

    public void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            inSight = false;
        }
    }

    IEnumerator dmgFlash()
    {
        model.material.color = Color.red;
        yield return new WaitForSeconds(0.2f);
        model.material.color = colorOrig;
    }
}


