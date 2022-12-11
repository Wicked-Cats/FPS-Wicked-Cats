using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class playerController : MonoBehaviour
{
    [Header("----- Components ----")]
    [SerializeField] CharacterController controller;
    [SerializeField] GameObject model;
    [SerializeField] Animator anim;

    [Header("----- Player Stats ----")]
    [Range(1, 10)] [SerializeField] public int HP;
    [Range(3, 20)] [SerializeField] public int playerSpeed;
    [Range(10, 15)] [SerializeField] int jumpHeight;
    [Range(15, 35)] [SerializeField] int gravityValue;
    [Range(1, 3)] [SerializeField] public int jumpsMax;
    [Range(1, 3)][SerializeField] public int HPMax;


    [Header("----- Gun Stats ----")]
    [SerializeField] public int shootDamage;
    [SerializeField] float shootRate;
    [SerializeField] float shootDist;
    [SerializeField] GameObject bullet;
    [SerializeField] Transform shootPos;
    [SerializeField] public int damage;

    bool isShooting;
    int jumpedTimes;
    private Vector3 playerVelocity;
    Vector3 move;
    public int HPOrig;
    int pS;
    bool turning;

    private void Start()
    {
        //SetPlayerPos();
        HPOrig = HP;
        updateHPBar();
        pS = playerSpeed;
    }

    void Update()
    {
        if (!gameManager.instance.isPaused)
        {
            anim.SetFloat("Speed", move.normalized.magnitude);

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
            //for testing purposes
            //gameManager.instance.componentsTotal += 10;
            jumpedTimes++;
            playerVelocity.y = jumpHeight;
        }

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
        HP -= dmg;
        updateHPBar();
        StartCoroutine(playerDmgFlash());

        if (HP <= 0)
        {
            gameManager.instance.pause();
            gameManager.instance.activeMenu = gameManager.instance.loseMenu;
            gameManager.instance.activeMenu.SetActive(true);
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

    public void updateHPBar()
    {
        gameManager.instance.playerHPBar.fillAmount = (float)HP/ (float)HPOrig;
        gameManager.instance.playerHPCurrent.text = HP.ToString("F0");
        gameManager.instance.playerHPMax.text = HPOrig.ToString("F0");
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

    //public void ComponentsPickUp()
    //{
    //    shootDamage = gunStat.shootDamage;
    //    shootRate = gunStat.shootRate;
    //    shootDist = gunStat.shootDist;

    //    gunModel.GetComponent<MeshFilter>().sharedMesh = gunStat.gunModel.GetComponent<MeshFilter>().sharedMesh;             
    //    gunModel.GetComponent<MeshRenderer>().sharedMaterial = gunStat.gunModel.GetComponent<MeshRenderer>().sharedMaterial; 

    //    gunList.Add(gunStat);

    //    selectedGun = gunList.Count - 1;
    //}

    //void gunSelect()
    //{
    //    if (Input.GetAxis("Mouse ScrollWheel") > 0 && selectedGun < gunList.Count - 1)
    //    {
    //        selectedGun++;
    //        ChangeGun();
    //    }
    //    else if (Input.GetAxis("Mouse ScrollWheel") < 0 && selectedGun > 0)
    //    {
    //        selectedGun--;
    //        ChangeGun();
    //    }
    //}

    //IEnumerator playSteps()
    //{
    //    stepIsPlaying = true;
    //    aud.PlayOneShot(audPlayerSteps[Random.Range(0, audPlayerSteps.Length)], playerStepsVol);
    //    if (isSprinting)
    //    {
    //        yield return new WaitForSeconds(0.3f);
    //    }
    //    else
    //    {
    //        yield return new WaitForSeconds(0.5f);

    //    }
    //    stepIsPlaying = false;
    //}

}