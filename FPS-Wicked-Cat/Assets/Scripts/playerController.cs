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
    [Range(3, 20)] [SerializeField] public float playerSpeed;
    [Range(10, 15)] [SerializeField] int jumpHeight;
    [Range(15, 35)] [SerializeField] int gravityValue;
    [Range(1, 3)] [SerializeField] public int jumpsMax;
    public int HPOrig;
    [SerializeField] int pushBackTime;

    [Header("----- Gun Stats ----")]
    [SerializeField] List<gunObjects> gunList = new List<gunObjects>();
    [SerializeField] public int shootDamage;
    [SerializeField] float shootRate;
    [SerializeField] float shootDist;
    [SerializeField] Transform shootPos;
    [SerializeField] public int damage;
    [SerializeField] public int rangeUp;
    [SerializeField] GameObject gunModel;
    [SerializeField] GameObject[] gunPos;
    [SerializeField] GameObject hitEffect;

    [Header("----- Audio ----")]
    [SerializeField] AudioSource aud;
    [SerializeField] AudioClip gunShot;
    [Range(0, 1)] [SerializeField] float gunShotVol;
    [SerializeField] AudioClip[] audPlayerHurt;
    [Range(0, 1)] [SerializeField] float playerHurtVol;
    [SerializeField] AudioClip[] audPlayerJump;
    [Range(0, 1)] [SerializeField] float playerJumpVol;
    [SerializeField] AudioClip[] audPlayerSteps;
    [Range(0, 1)] [SerializeField] float playerStepsVol;

    bool isShooting;
    int jumpedTimes;
    int selectedGun;
    private Vector3 playerVelocity;
    Vector3 move;
    Vector3 pushBack;
    float pS;
    bool turning;
    bool stepIsPlaying;
    bool isSprinting;

    private void Start()
    {
        SetPlayerPos();
        HPOrig = HP;
        updateHPBar();
        pS = playerSpeed;
    }

    void Update()
    {
        if (!gameManager.instance.isPaused)
        {
            anim.SetFloat("Speed", move.normalized.magnitude);

            pushBack = Vector3.Lerp(pushBack, Vector3.zero, Time.deltaTime * pushBackTime);
            //might keep for future use
            //pushBack.x = Mathf.Lerp(pushBack.x, 0, Time.deltaTime * pushBackTime);
            //pushBack.y = Mathf.Lerp(pushBack.y, 0, Time.deltaTime * pushBackTime * 2f);
            //pushBack.z = Mathf.Lerp(pushBack.z, 0, Time.deltaTime * pushBackTime);

            movement();

            if (!stepIsPlaying && move.magnitude > 0.3f && controller.isGrounded)
            {
                StartCoroutine(playSteps());
            }

            if (gunList.Count > 0)
            {
                if (gunList[selectedGun].bulletModel == null)
                {
                    StartCoroutine(shoot());
                }
                else
                {
                    StartCoroutine(projectileShoot());
                }
                gunSelect();
            }

            if (!turning)
            {
                StartCoroutine(turnModel());
            }
        }
    }

    void movement()
    {
        if (Input.GetButtonDown("Sprint"))
        {
            playerSpeed = pS * 1.5f;
            isSprinting = true;

            if (playerSpeed > 25)
            {
                playerSpeed = 25;
            }
            anim.SetBool("isSprinting", true);
        }
        if (Input.GetButtonUp("Sprint"))
        {
            playerSpeed = pS;
            isSprinting = false;

            anim.SetBool("isSprinting", false);
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
            jumpedTimes++;
            playerVelocity.y = jumpHeight;
            aud.PlayOneShot(audPlayerJump[Random.Range(0, audPlayerJump.Length)], playerJumpVol);

        }


        playerVelocity.y -= gravityValue * Time.deltaTime;
        controller.Move((playerVelocity + pushBack) * Time.deltaTime);
    }

    //for later development
    IEnumerator shoot()
    {
        if (!isShooting && Input.GetButton("Shoot"))
        {
            isShooting = true;
            RaycastHit hit;

            if (Physics.Raycast(Camera.main.ViewportPointToRay(new Vector2(0.5f, 0.5f)), out hit, shootDist + rangeUp))
            {
                if (hit.collider.GetComponent<IDamage>() != null)
                {
                    hit.collider.GetComponent<IDamage>().takeDamage(shootDamage + damage);
                }

                Instantiate(hitEffect, hit.point, hitEffect.transform.rotation);
            }

            yield return new WaitForSeconds(shootRate);
            isShooting = false;
        }
    }

    IEnumerator projectileShoot()
    {
        if (!isShooting && Input.GetButton("Shoot"))
        {
            isShooting = true;
            Instantiate(gunList[selectedGun].bulletModel, shootPos.position, transform.rotation);
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
            gameManager.instance.isPaused = !gameManager.instance.isPaused;
            gameManager.instance.pause();
            gameManager.instance.activeMenu = gameManager.instance.loseMenu;
            gameManager.instance.respawnButtonText.text = "Respawn (-" + (gameManager.instance.respawnCost + gameManager.instance.timeDamageIncrease) + " Components";
            gameManager.instance.activeMenu.SetActive(true);
            if (gameManager.instance.componentsCurrent < gameManager.instance.respawnCost + gameManager.instance.timeDamageIncrease)
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

    public void SetPlayerPos()
    {
        controller.enabled = false;
        transform.position = gameManager.instance.playerSpawnPos.transform.position;
        controller.enabled = true;
    }

    public void ResetPlayerHP()
    {
        HP = HPOrig;
    }

    public void updateHPBar()
    {
        if (HP < 0)
        {
            HP = 0;
        }
        gameManager.instance.playerHPBar.fillAmount = (float)HP / (float)HPOrig;
        gameManager.instance.playerHPCurrent.text = HP.ToString("F0");
        gameManager.instance.playerHPMax.text = HPOrig.ToString("F0");
    }

    IEnumerator turnModel()
    {
        Quaternion cameraMain = Camera.main.transform.rotation;

        cameraMain.x = 0;
        cameraMain.z = 0;
        model.transform.rotation = cameraMain;
        turning = true;
        yield return new WaitForSeconds(0.1f);
        turning = false;
    }

    IEnumerator playSteps()
    {
        stepIsPlaying = true;
        aud.PlayOneShot(audPlayerSteps[Random.Range(0, audPlayerSteps.Length - 1)], playerStepsVol);
        if (isSprinting)
        {
            yield return new WaitForSeconds((float)pS / 20f);
        }
        else
        {
            yield return new WaitForSeconds((float)pS / 10f);

        }
        stepIsPlaying = false;
    }

    public void PushBackInput(Vector3 dir)
    {
        pushBack = dir;
    }

    void gunSelect()
    {
        for(int i = 0; i < gunPos.Length; i++)
        {
            gunPos[i].SetActive(false);
        }

        if (gunList.Count > 0)
        {
            if (Input.GetAxis("Mouse ScrollWheel") > 0 && selectedGun < gunList.Count - 1)
            {
                selectedGun++;
                ChangeGun();
            }
            else if (Input.GetAxis("Mouse ScrollWheel") < 0 && selectedGun > 0)
            {
                selectedGun--;
                ChangeGun();
            }
        }

        if (gunList[selectedGun].name == "BazookaGunStats")
        {
            gunPos[0].SetActive(true);
        }
        else if (gunList[selectedGun].name == "Sniper Rifle")
        {
            gunPos[1].SetActive(true);
        }
        else if (gunList[selectedGun].name == "Pistol")
        {
            gunPos[2].SetActive(true);
        }
    }

    void ChangeGun()
    {
        shootDamage = gunList[selectedGun].shootDamage;
        shootRate = gunList[selectedGun].shootRate;
        shootDist = gunList[selectedGun].shootDistance;

        gunModel.GetComponent<MeshFilter>().sharedMesh = gunList[selectedGun].gunModel.GetComponent<MeshFilter>().sharedMesh;             // transfers over the FILTER which is the MODEL
        gunModel.GetComponent<MeshRenderer>().sharedMaterial = gunList[selectedGun].gunModel.GetComponent<MeshRenderer>().sharedMaterial; // transfers over the MATERIAL which is the RENDERER
    }

    public void gunPickup(gunObjects gunObject)
    {
        // assign gunModel
        if (gunObject.name == "BazookaGunStats")
        {
            gunPos[1].SetActive(false);
            gunPos[2].SetActive(false);
            gunModel = gunPos[0];
            gunPos[0].SetActive(true);
        }
        else if (gunObject.name == "Sniper Rifle")
        {
            gunPos[0].SetActive(false);
            gunPos[2].SetActive(false);
            gunModel = gunPos[1];
            gunPos[1].SetActive(true);
        }
        else if (gunObject.name == "Pistol")
        {
            gunPos[0].SetActive(false);
            gunPos[1].SetActive(false);
            gunModel = gunPos[2];
            gunPos[2].SetActive(true);
        }

        gunList.Add(gunObject);

        if (gunList.Count > 0)
        {
            selectedGun = gunList.Count - 1;
        }

        ChangeGun();
    }
}