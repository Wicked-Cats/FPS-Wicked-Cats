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
    [SerializeField] public GameObject enemyAimPoint;

    [Header("------ Player Upgrades------")]
    public int jumpsLimit;
    public int HPLimit;
    public int damageLimit;
    public int speedLimit;
    public int rangeUpLimit;


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
    public TextMeshProUGUI forwardSlash;
    [SerializeField] public TextMeshProUGUI componentsDisplay;
    public Image reticle;
    public Image crosshair;
    public TextMeshProUGUI respawnButtonText;
    public TextMeshProUGUI rangeButtonText;
    public TextMeshProUGUI damageButtonText;
    public TextMeshProUGUI HPButtonText;
    public TextMeshProUGUI speedButtonText;

    [Header("------ Timer ------")]
    [SerializeField] public float timeCurrent;
    [SerializeField] public TextMeshProUGUI timerText;
    private int damageIncreaseOffset;
    public int timeDamageIncrease;


    [Header("------ Upgrades Stuff ------")]
    public TextMeshProUGUI upgradesComponentCurrent;
    public Button respawnButt;
    public Button jumpButton;
    public Button dmgButton;
    public Button HPButton;
    public Button speedButton;
    public Button rangeButton;
    public int respawnCost;
    public int damageCost;
    public int HPCost;
    public int rangeCost;
    public int speedCost;

    [Header("------ Enemy Spawning ------")]
    [Range(1, 100)][SerializeField] float spawnTimer;
    [SerializeField] GameObject[] enemiesOptions;
    private NavMeshTriangulation navMeshTri;
    private GameObject enemyToSpawn;
    private float diffTickTime;
    private int spawnOffset;
    private bool waitingToTick;

    [Header("----- Main Menu -----")]
    public GameObject mainMenu;
    public bool isMain;

    public bool isPaused;
    float timeScaleBase;
    public GameObject playerSpawnPos;
    bool isSpawning;
    public int componentsCurrent;
    public int componentsTotal;
    public bool objectivesSeen =false;
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

        diffTickTime = timeCurrent / (enemiesOptions.Length - 1);
        spawnOffset = 0;

        //set and move player to spawn
        playerSpawnPos = GameObject.FindGameObjectWithTag("Player Spawn Pos");
        player.transform.position = playerSpawnPos.transform.position;

        //set enemy aim point 
        enemyAimPoint = GameObject.FindGameObjectWithTag("Enemy Aim Point");

        //Do nav mesh triangulation for spawning
        navMeshTri = NavMesh.CalculateTriangulation();

        //Set up UI
        updateComponentsDisplay();
    }


    void Update()
    {
        if (!isMain) //DONT DELETE
        {
            // turning off UI elements they are turn on when user clicks a mode
            UIDisable();

            isPaused = !isPaused;
            activeMenu = mainMenu;
            activeMenu.SetActive(isPaused);
            pause();
            isMain = true;
        }

        // displays the objectives only at start.
        if (objectivesSeen)
        {
            isPaused = !isPaused;
            activeMenu = objectives;
            activeMenu.SetActive(isPaused);
            pause();
            objectivesSeen = false;
        }

        //timer ticking
        if (!isPaused)
        {
            if (timeCurrent > 0)
            {
                timeCurrent -= Time.deltaTime;
            }
            else
            {
                //win condition
                timeCurrent = 0;
                isPaused = !isPaused;
                activeMenu = winMenu;
                activeMenu.SetActive(isPaused);
                pause();
                componentsTotal = 0;
                componentsCurrent = 0;
            }

            timerUpdate(timeCurrent);
        }

        if(!waitingToTick)
        {
            StartCoroutine(difficultyTick());
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

        if (!isSpawning)
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

        enemyToSpawn = enemiesOptions[Random.Range(0, spawnOffset)];

        int possibleLocation = Random.Range(0, navMeshTri.vertices.Length);

        NavMeshHit hit;
        if (NavMesh.SamplePosition(navMeshTri.vertices[possibleLocation], out hit, 2f, 1))
        {
            Instantiate(enemyToSpawn, hit.position, this.transform.rotation);
        }

        yield return new WaitForSeconds(spawnTimer);
        isSpawning = false;
    }

    void timerUpdate(float displaytime)
    {
        if (displaytime < 0)
        {
            displaytime = 0;
        }
        if(displaytime < 30)
        {
            spawnTimer = 1;
            timerText.color = Color.red;
        }

        float minutes = Mathf.FloorToInt(displaytime / 60);
        float seconds = Mathf.FloorToInt(displaytime % 60);

        timerText.text = string.Format("{0:00}:{1:00}", minutes, seconds);

    }

    IEnumerator difficultyTick()
    {
        waitingToTick = true;

        spawnOffset++;
        damageIncreaseOffset++;
        if(damageIncreaseOffset % 3 == 0)
        {
            timeDamageIncrease++;
        }
        yield return new WaitForSeconds(diffTickTime);
        waitingToTick = false;
    }

    public void updateComponentsDisplay()
    {
        componentsDisplay.text = "Components: " + componentsCurrent.ToString();
    }

    // turning ON UI 
    public void UIEnable()
    {
        playerHPBar.enabled = true;
        playerHPMax.enabled = true;
        playerHPCurrent.enabled = true;
        componentsDisplay.enabled = true;
        timerText.enabled = true;
        reticle.enabled = true;
        crosshair.enabled = true;
        forwardSlash.enabled = true;
        updateComponentsDisplay();

    }
    // turning OFF UI
    public void UIDisable()
    {
        playerHPBar.enabled = false;
        playerHPMax.enabled = false;
        playerHPCurrent.enabled = false;
        componentsDisplay.enabled = false;
        timerText.enabled = false;
        reticle.enabled = false;
        crosshair.enabled = false;
        forwardSlash.enabled = false;

    }
}
