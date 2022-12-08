using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class playerController : MonoBehaviour
{
    [Header("----- Components ----")]
    [SerializeField] CharacterController controller;
    [SerializeField] GameObject model;

    [Header("----- Player Stats ----")]
    public int damage;
    [Range(1, 10)] public int HPMax;
    [Range(1, 10)] public int HP;
    [Range(3, 20)] public int playerSpeed;
    [Range(10, 15)] [SerializeField] int jumpHeight;
    [Range(15, 35)] [SerializeField] int gravityValue;
    [Range(1, 3)] public int jumpsMax;

    [Header("----- Gun Stats ----")]
    public int shootDamage;
    [SerializeField] float shootRate;
    [SerializeField] float shootDist;
    [SerializeField] GameObject bullet;
    [SerializeField] Transform shootPos;

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
        if(HPMax < HP)
        {
            HP = HPMax;
        }
        HPOrig = HP;
        pS = playerSpeed;
        updateHPBar();
    }

    void Update()
    {
        if (!gameManager.instance.isPaused)
        {
            movement();
            StartCoroutine(projectileShoot());
            //StartCoroutine(shoot());   us for later 
            if(!turning)
            {
                StartCoroutine(turnModel());
            }
            

        }

    }

    void movement()
    {
        //Sprint functionality
        if(Input.GetButtonDown("Sprint"))
        {
            playerSpeed = pS * 2;

            if(playerSpeed > 25)
            {
                playerSpeed = 25;
            }
        }
        if(Input.GetButtonUp("Sprint"))
        {
            playerSpeed = pS;
        }

        //reset player gravity movement and jumps while on ground
        if (controller.isGrounded && playerVelocity.y < 0)
        {
            jumpedTimes = 0;
            playerVelocity.y = 0f;
        }

        //set move variable with key movements
        move = transform.right * Input.GetAxis("Horizontal") +  
               transform.forward * Input.GetAxis("Vertical");  

        //applies movement to controller independent of framerate
        controller.Move(move * Time.deltaTime * playerSpeed);

        //jump functionality
        if (Input.GetButtonDown("Jump") && jumpedTimes < jumpsMax)
        {
            //for testing purposes
            //gameManager.instance.componentsTotal += 10;
            //gameManager.instance.componentsCurrent += 10;

            jumpedTimes++;
            playerVelocity.y = jumpHeight;
        }

        
        //applies gravity to player
        playerVelocity.y -= gravityValue * Time.deltaTime;
        controller.Move(playerVelocity * Time.deltaTime);
    }

    //for later development
    //IEnumerator shoot()
    //{
    //    if (!isShooting && Input.GetButton("Shoot"))
    //    {
    //        isShooting = true;

    //        RaycastHit hit;

    //        if (Physics.Raycast(Camera.main.ViewportPointToRay(new Vector2(0.5f, 0.5f)), out hit, shootDist))
    //        {
    //            if (hit.collider.GetComponent<IDamage>() != null)
    //            {
    //                hit.collider.GetComponent<IDamage>().takeDamage(shootDamage);
    //            }
    //        }

    //        yield return new WaitForSeconds(shootRate);
    //        isShooting = false;
    //    }
    //}

    IEnumerator projectileShoot()
    {
        if (!isShooting && Input.GetButton("Shoot"))
        {
            isShooting = true;
            Instantiate(bullet, shootPos.position, transform.rotation);
            yield return new WaitForSeconds(shootRate);
            isShooting = false;
        }
    }
   
    public void takeDamage(int dmg)
    {
        //damage player and start feedback to player
        HP -= dmg;
        updateHPBar();
        StartCoroutine(playerDmgFlash());

        //check for player death
        if (HP <= 0)
        {
            gameManager.instance.pause();
            gameManager.instance.activeMenu = gameManager.instance.loseMenu;
            gameManager.instance.activeMenu.SetActive(true);

            //checks if player can afford a respawn
            if(gameManager.instance.componentsCurrent < 5)
            {
                gameManager.instance.respawnButt.interactable = false;
            }
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

    public void SetPlayerPos()
    {
        controller.enabled = false;
        transform.position = gameManager.instance.playerSpawnPos.transform.position;
        controller.enabled = true;
    }

    public void ResetPlayerHP()
    {
        HP = HPOrig;
        updateHPBar();
    }

    IEnumerator turnModel()
    {
        // keeps player model looking in same direction as camera so it can't be seen when turning
        Quaternion cameraMain = Camera.main.transform.rotation;

        cameraMain.x = 0;
        cameraMain.z = 0;
        model.transform.rotation = cameraMain;
        turning = true;
        yield return new WaitForSeconds(0.1f);
        turning = false;
    }

    public void updateHPBar()
    {
        gameManager.instance.playerHPBar.fillAmount = (float)HP / (float)HPMax;
        gameManager.instance.playerHPCurrent.text = HP.ToString("F0");
        gameManager.instance.playerHPMax.text = HPMax.ToString("F0");
    }
}