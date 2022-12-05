using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerController : MonoBehaviour
{
    [Header("----- Components ----")]
    [SerializeField] CharacterController controller;
    [SerializeField] GameObject model;

    [Header("----- Player Stats ----")]
    [Range(1, 10)] [SerializeField] int HP;
    [Range(3, 20)] [SerializeField] int playerSpeed;
    [Range(10, 15)] [SerializeField] int jumpHeight;
    [Range(15, 35)] [SerializeField] int gravityValue;
    [Range(1, 3)] [SerializeField] int jumpsMax;

    [Header("----- Gun Stats ----")]
    [SerializeField] int shootDamage;
    [SerializeField] float shootRate;
    [SerializeField] float shootDist;

    bool isShooting;
    int jumpedTimes;
    private Vector3 playerVelocity;
    Vector3 move;
    int HPOrig;
    int pS;
    bool turning;
    

    private void Start()
    {
        //SetPlayerPos();
        HPOrig = HP;
        pS = playerSpeed;
    }

    void Update()
    {
        if (!gameManager.instance.isPaused)
        {
            movement();
            StartCoroutine(shoot());
            if(!turning)
            {
                StartCoroutine(turnModel());
            }
            

        }
       
    }

    void movement()
    {
        if(Input.GetButton("Sprint"))
        {
            playerSpeed = pS * 2;

            if(playerSpeed > 25)
            {
                playerSpeed = 25;
            }
        }
        else
        {
            playerSpeed = pS;
        }
            

        


        if (controller.isGrounded && playerVelocity.y < 0)
        {
            jumpedTimes = 0;
            playerVelocity.y = 0f;
        }

        move = transform.right * Input.GetAxis("Horizontal") +  
               transform.forward * Input.GetAxis("Vertical");  

        controller.Move(move * Time.deltaTime * playerSpeed);

        if (Input.GetButtonDown("Jump") && jumpedTimes < jumpsMax)
        {
            //gameManager.instance.componentsTotal += 10;
            jumpedTimes++;
            playerVelocity.y = jumpHeight;
        }

        

        playerVelocity.y -= gravityValue * Time.deltaTime;
        controller.Move(playerVelocity * Time.deltaTime);
    }

    IEnumerator shoot()
    {
        if (!isShooting && Input.GetButton("Shoot"))
        {
            isShooting = true;

            RaycastHit hit;

            if (Physics.Raycast(Camera.main.ViewportPointToRay(new Vector2(0.5f, 0.5f)), out hit, shootDist))
            {
                if (hit.collider.GetComponent<IDamage>() != null)
                {
                    hit.collider.GetComponent<IDamage>().takeDamage(shootDamage);
                }
            }

            yield return new WaitForSeconds(shootRate);
            isShooting = false;
        }
    }
   
    public void takeDamage(int dmg)
    {
        HP -= dmg;
        StartCoroutine(playerDmgFlash());

        if (HP <= 0)
        {
            gameManager.instance.pause();
            gameManager.instance.loseMenu.SetActive(true);
            gameManager.instance.activeMenu = gameManager.instance.loseMenu;
        }
    }

    IEnumerator playerDmgFlash()
    {
        gameManager.instance.damageFlash.SetActive(true);
        yield return new WaitForSeconds(0.1f);
        gameManager.instance.damageFlash.SetActive(false);

    }

    //public void AddJump(int amount)
    //{
    //    jumpsMax += amount;
    //    gameManager.instance.coins -= gameManager.instance.jumpCost;
    //}

    //public void SetPlayerPos()
    //{
    //    controller.enabled = false;
    //    transform.position = gameManager.instance.playerSpawnPos.transform.position;
    //    controller.enabled = true;
    //}

    public void ResetPlayerHP()
    {
        HP = HPOrig;
    }

    IEnumerator turnModel()
    {
        //model.transform.LookAt(Camera.main.transform.position);
        Quaternion cameraMain = Camera.main.transform.rotation;

        cameraMain.x = 0;
        cameraMain.z = 0;
        model.transform.rotation = cameraMain;
        turning = true;
        yield return new WaitForSeconds(0.1f);
        turning = false;
    }
}