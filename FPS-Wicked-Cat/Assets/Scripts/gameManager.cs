using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.AI;
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

    [Header("-- Enemy Spawning --")]
    private NavMeshTriangulation navMeshTri;
    [SerializeField] GameObject[] enemiesOptions;
    private GameObject enemyToSpawn;

    public bool isPaused;
    float timeScaleBase;
    public GameObject playerSpawnPos;
    bool isSpawning;
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

        //Do nav mesh triangulation for spawning
        navMeshTri = NavMesh.CalculateTriangulation();
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

        if(!isSpawning)
        {
            StartCoroutine(spawnEnemies());
        }

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

    IEnumerator spawnEnemies()
    {
        isSpawning = true;

        enemyToSpawn = enemiesOptions[Random.Range(0, enemiesOptions.Length)];

        int possibleLocation = Random.Range(0, navMeshTri.vertices.Length);

        NavMeshHit hit;
        if(NavMesh.SamplePosition(navMeshTri.vertices[possibleLocation], out hit, 2f, 1))
        {
            Instantiate(enemyToSpawn, hit.position, this.transform.rotation);
        }

        yield return new WaitForSeconds(1f);
        isSpawning = false;
    }
    
}
