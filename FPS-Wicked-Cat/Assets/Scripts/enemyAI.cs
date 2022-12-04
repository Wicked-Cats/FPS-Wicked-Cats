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
    [SerializeField] bool isPatrolling;
    [SerializeField] int lineOfSight;
    bool inSight;

    [Header("-- Enemy Gun Stats --")]
    [SerializeField] int shootDmg;
    [SerializeField] float shootRate;        
    [SerializeField] GameObject bullet;
    [SerializeField] Transform shootPos;
    [SerializeField] bool isShooting;

    [Header("-- Patrol Points --")]
    [SerializeField] Transform[] patrolPoints;

    // Start is called before the first frame update
    void Start()
    {
        HPOrig = HP;
    }

    // Update is called once per frame
    void Update()
    {
        AiMovement();
    }

    

    IEnumerator dmgFlash()
    {
        model.material.color = Color.red;
        yield return new WaitForSeconds(0.2f);
        model.material.color = Color.white;
    }

    void AiMovement()
    {
        if (isPatrolling)
        {

        }
        agent.SetDestination(gameManager.instance.player.transform.position);
    }

    public void takeDamage(int damage)
    {
        HP -= damage;
        StartCoroutine(dmgFlash());
        transform.LookAt(gameManager.instance.player.transform.position);
        agent.SetDestination(gameManager.instance.player.transform.position);

        if (HP <= 0)
        {
            Destroy(gameObject);
        }
        
    }



    IEnumerator shoot()
    {
        isShooting= true;
        Instantiate(bullet, shootPos.position, transform.rotation);
        yield return new WaitForSeconds(shootRate);
        isShooting= false;
    }
}
