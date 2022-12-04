using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class enemyAI : MonoBehaviour, IDamage
{
    [Header("-- Components --")]
    [SerializeField] int HP;
    [SerializeField] Renderer model;
    [SerializeField] NavMeshAgent agent;



    [SerializeField] bool isPatrolling;
    int HPOrig;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        AiMovement();
    }

    

    IEnumerator dmgFlash()
    {
        model.material.color = Color.red;
        yield return new WaitForSeconds(0.5f);
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
        
    }
}
