using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class enemyAI : MonoBehaviour
{
    [Header("-- Enemy Components --")]
    [SerializeField] int HP;
    [SerializeField] int enemySpeed;
    [SerializeField] Renderer model;        // enemy model
    [SerializeField] NavMeshAgent agent;    // can walk on surface

    [Header("Move Points")]
    public Transform[] movePoints; // this will be empty objects 
                                   // just like the respawn object
    public int HPOrig;
    public Transform player;
    bool isShooting, playerInRange;


    // Start is called before the first frame update
    void Start()
    {
        HPOrig = HP;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    IEnumerator dmgFlash()
    {
        model.material.color = Color.red;
        yield return new WaitForSeconds(0.5f);
        model.material.color = Color.white;
    }



    void AiMovement()
    {
        agent.SetDestination(player.position);

    }

    public void Patrol()
    {
            
    }

    public void takeDamage(int dmg)
    {
        HP -= dmg;

        AiMovement(); // after enemy takes damage being to move 
        
        StartCoroutine(dmgFlash());

        if (HP <= 0)
        {
            Destroy(gameObject);
        }
    }
}
