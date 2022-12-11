using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class gameManager : MonoBehaviour
{
    public static gameManager instance;

    [Header("------Player Components------")]
    public GameObject player;
    public playerController playerScript;

    [Header("------ Player Upgrades------")]
    public int jumpsLimit;
    public int HPLimit;
    public int damageLimit;
    public int speedLimit;


    [Header("------UI Components------")]
    public GameObject objectives;
    public GameObject activeMenu;
    public GameObject pauseMenu;
    public GameObject winMenu;
    public GameObject loseMenu;
    public GameObject upgradesMenu;
    public GameObject damageFlash;
    public Image playerHPBar;
    public TextMeshProUGUI playerHPCurrent;
    public TextMeshProUGUI playerHPMax;

    [Header("------ Upgrades Stuff ------")]
    public TextMeshProUGUI upgradesComponentCurrent;
    public Button respawnButt;
    public Button jumpButton;
    public Button dmgButton;
    public Button HPButton;
    public Button speedButton;

    [Header("------Enemies------")]
    [SerializeField] GameObject flyer;
    [SerializeField] GameObject tank;
    [SerializeField] GameObject speedy;

    [Header("-- Spwaners --")]
    [SerializeField] GameObject flyerSpawn1;
    [SerializeField] GameObject flyerSpawn2;

    public bool isPaused;
    float timeScaleBase;
    public GameObject playerSpawnPos;
    bool isSpawningFly;
    public int componentsCurrent;
    public int componentsTotal;
    public bool objectivesSeen;
    public bool forceFieldActive;
    public GameObject forceField;
    public GameObject forceFieldMaker;


    void Awake()
    {
        instance = this;

        // set player character info
        player = GameObject.FindGameObjectWithTag("Player");
        playerScript = player.GetComponent<playerController>();

        timeScaleBase = Time.timeScale;

        //set and move player to spawn
        playerSpawnPos = GameObject.FindGameObjectWithTag("Player Spawn Pos");
        player.transform.position = playerSpawnPos.transform.position;
    }

    void Update()
    {
        //moved to a different location left for testing purposes.
        //if (componentsTotal >= 30 && activeMenu == null)
        //{
        //    isPaused = !isPaused;
        //    activeMenu = winMenu;
        //    activeMenu.SetActive(isPaused);
        //    pause();
        //    objectivesSeen = false;
        //    componentsTotal = 0;
        //    componentsCurrent = 0;
        //}

        // displays the objectives only at start.
        if (!objectivesSeen)
        {
            isPaused = !isPaused;
            activeMenu = objectives;
            activeMenu.SetActive(isPaused);
            pause();
            objectivesSeen = true;
        }

        //opens pause menu
        if (Input.GetButtonDown("Cancel") && activeMenu == null)
        {
            isPaused = !isPaused;
            activeMenu = pauseMenu;
            activeMenu.SetActive(isPaused);

            if (isPaused)
            {
                pause();
            }
            else
            {
                unPause();
            }
        }
        //closes pause menu only when it is open
        else if (Input.GetButtonDown("Cancel") && activeMenu == pauseMenu)
        {
            isPaused = !isPaused;
            unPause();
        }

        //Work in progress or will be completely overhauled in future
        //StartCoroutine(spawnFly());

    }

    public void pause()
    {
        Time.timeScale = 0;
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.Confined;
    }

    public void unPause()
    {
        Time.timeScale = timeScaleBase;
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        activeMenu.SetActive(false);
        activeMenu = null;
    }

    //creates flying enemies at a designated spawner
    //may be changed later or completely phased out
    IEnumerator spawnFly()
    {
        if (!isSpawningFly)
        {
            isSpawningFly = true;
            float num = Random.Range(0, 99);
            if (num < 50)
            {
                Instantiate(flyer, flyerSpawn1.transform.position, flyerSpawn1.transform.rotation);
            }
            else
            {
                Instantiate(flyer, flyerSpawn2.transform.position, flyerSpawn2.transform.rotation);
            }
            yield return new WaitForSeconds(10f);
            isSpawningFly = false;
        }
    }
}
