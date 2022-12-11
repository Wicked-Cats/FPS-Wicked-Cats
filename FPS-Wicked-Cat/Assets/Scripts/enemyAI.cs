using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class enemyAI : MonoBehaviour, IDamage
{
    [Header("-- Components --")]
    [SerializeField] Renderer model;
    [SerializeField] NavMeshAgent agent;
    [SerializeField] Animator anim;
    public Color colorOrig;

    [Header("-- Enemy Stats")]
    [SerializeField] int HP;
    private int HPOrig;
    [SerializeField] Transform headPos;

    [Header("-- Enemy Vision --")]
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

    [Header("-- Item Drops --")]
    [SerializeField] GameObject[] itemDrop;
    

    Vector3 playerDir;
    private float stopDistOrig;


    void Start()
    {
        HPOrig = HP;
        colorOrig = model.material.color;
        stopDistOrig = agent.stoppingDistance;
    }

    void Update()
    {
        if (inSight)
        {
            anim.SetFloat("Speed", agent.velocity.normalized.magnitude);
            agent.stoppingDistance = stopDistOrig;
            LineOfSight();
        }



    }

    //checks if player is in enemies view and start shooting when 
    void LineOfSight()
    {
        agent.SetDestination(gameManager.instance.player.transform.position);
        playerDir = gameManager.instance.player.transform.position - headPos.position;
        angleToPlayer = Vector3.Angle(playerDir, transform.forward);

        RaycastHit see;

        Debug.DrawRay(headPos.position, playerDir);

        if (Physics.Raycast(headPos.position, playerDir, out see))
        {
            if (see.collider.CompareTag("Player") && angleToPlayer <= lineOfSight)
            {

                if (!isShooting && angleToPlayer <= lineOfSight / 3) // so if he sees us and we are mostly in front of him he starts to shoot
                {
                    StartCoroutine(shoot());
                }

                //turns enemy towards player if he gets to close
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
        //damages enemy and gives feedback to player
        HP -= damage;
        StartCoroutine(dmgFlash());

        //makes enemy go to players last position in response to the damage
        FacePlayer();
        agent.SetDestination(gameManager.instance.player.transform.position);

        //check if enemy has died
        if (HP <= 0)
        {
            
            // item drop
            GameObject drop = itemDrop[Random.Range(0, itemDrop.Length-1)];
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
                    item.position = new Vector3(item.position.x +Random.Range(-0.75f, 0.75f), item.position.y, item.position.z - Random.Range(-0.75f, 0.75f));
                    Instantiate(drop, item.position, transform.rotation);
                }
            }

            //add components
            //gameManager.instance.componentsCurrent += HPOrig;
            //gameManager.instance.componentsTotal += HPOrig;

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

    //code for player entering enemies sense range
    public void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            inSight = true;
        }

    }

    //code for player leaving enemy sense range
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
        yield return new WaitForSeconds(shootRate);
        isShooting = false;
    }

    IEnumerator dmgFlash()
    {
        model.material.color = Color.red;
        yield return new WaitForSeconds(0.3f);
        model.material.color = colorOrig;
    }

}
