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
    // vvv in TESTING phase vvv
    //[SerializeField] GameObject components;  // this object will be the item that drops from the enemy

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


    Vector3 playerDir;
    float stopDistOrig;

    // Start is called before the first frame update
    void Start()
    {
        HPOrig = HP;
        colorOrig = model.material.color;
    }

    // Update is called once per frame
    void Update()
    {
        if (inSight)
        {
            agent.stoppingDistance = stopDistOrig;
            LineOfSight();
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
        HP -= damage;
        agent.SetDestination(gameManager.instance.player.transform.position);

        StartCoroutine(dmgFlash());

        if (HP <= 0)
        {
            //add components
            gameManager.instance.componentsCurrent += HPOrig;
            gameManager.instance.componentsTotal += HPOrig;

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

    // vvv in TESTING phase vvv
    //private void ItemDrop()
    //{
    //    Instantiate(components, transform.parent);
    //    gameManager.instance.componentsCurrent += HPOrig;
    //    gameManager.instance.componentsTotal += HPOrig;
    //}
}
