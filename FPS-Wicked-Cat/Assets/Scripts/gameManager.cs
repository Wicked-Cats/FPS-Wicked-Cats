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
    public GameObject enemyAimPoint;
    public int armor;

    [Header("------ Player Upgrades------")]
    public int jumpsLimit;
    public int HPLimit;
    public int damageLimit;
    public int speedLimit;
    public int rangeUpLimit;
    public int critChanceLimit;
    public float critDamageLimit;
    public int critChance;
    public float critDamageMulti;
    public float magnetRange;
    public float magnetLimit;


    [Header("------UI Components------")]
    public GameObject optionsMenu;
    public GameObject objectives;
    public GameObject activeMenu;
    public GameObject lastMenu;
    public GameObject pauseMenu;
    public GameObject winMenu;
    public GameObject loseMenu;
    public GameObject upgradesMenu;
    public GameObject nameEntry;
    public GameObject damageFlash;
    public Image playerHPBar;
    public Image playerHPBackground;
    public TextMeshProUGUI playerHPCurrent;
    public TextMeshProUGUI playerHPMax;
    public TextMeshProUGUI forwardSlash;
    public TextMeshProUGUI componentsDisplay;
    public Image reticle;
    public Image crosshair;
    public Image timerBackground;
    public TextMeshProUGUI respawnButtonText;
    public TextMeshProUGUI rangeButtonText;
    public TextMeshProUGUI damageButtonText;
    public TextMeshProUGUI HPButtonText;
    public TextMeshProUGUI speedButtonText;
    public TextMeshProUGUI critChanceButtonText;
    public TextMeshProUGUI critDamageButtonText;
    public TextMeshProUGUI magnetButtonText;
    public TextMeshProUGUI armorButtonText;
    public TextMeshProUGUI healthPackButtonText;
    public TextMeshProUGUI armorText;
    public TextMeshProUGUI armorCurrent;
    public GameObject highscoreTable;
    public TMP_InputField nameEnrtyText;



    [Header("------ Timer ------")]
    public float timeCurrent;
    public TextMeshProUGUI timerText;
    private int damageIncreaseOffset;
    public int timeDamageIncrease;
    public float timeTotal;


    [Header("------ Upgrades Stuff ------")]
    public TextMeshProUGUI upgradesComponentCurrent;
    public Button respawnButt;
    public Button jumpButton;
    public Button dmgButton;
    public Button HPButton;
    public Button speedButton;
    public Button rangeButton;
    public Button critDamageButton;
    public Button critChanceButton;
    public Button magnetButton;
    public Button armorButton;
    public Button healthPackButton;
    public int respawnCost;
    public int damageCost;
    public int HPCost;
    public int rangeCost;
    public int speedCost;
    public int magnetCost;
    public int critChanceCost;
    public int critDamageCost;
    public int armorCost;
    public int healthPackCost;

    [Header("------ Enemy Spawning ------")]
    [Range(1, 100)][SerializeField] float spawnTimer;
    public GameObject[] enemiesOptions;
    [SerializeField] GameObject miniBoss;
    [SerializeField] GameObject droneBoss;
    public NavMeshTriangulation navMeshTri;
    private GameObject enemyToSpawn;
    public float diffTickTime;
    private float spawnOffset;
    private bool waitingToTick;

    [Header("----- Main Menu -----")]
    public GameObject mainMenu;
    public bool isMain;
    public bool isOptionBtnMain = false;
  

    [Header("----- Audio -----")]
    public Slider SFXSlider;
    public Slider BGMSlider;
    private float defaultVol = 0.5f;

    [Header("----- Shop -----")]
    public buttonFunctions btnFunc;
    public GameObject interactableTextParent;
    public TextMeshProUGUI interactableText;
    public GameObject shopSpawnBroadcastParent;
    public TextMeshProUGUI shopSpawnBrodcast;





    public int scoreTotal;
    public int killcount;
    public bool isPaused;
    float timeScaleBase;
    public GameObject playerSpawnPos;
    bool isSpawning;
    public int componentsCurrent;
    public int componentsTotal;
    public bool objectivesSeen = false;
    public bool forceFieldActive;
    public GameObject forceField;
    public GameObject forceFieldMaker;
    bool miniBossSpawned;
    bool droneBossSpawned;
    public TableScores tableScores;
    private string nameHighscore;

   
    

    void Awake()
    {
        instance = this;

        // Audio Saved from previous game
        if (PlayerPrefs.HasKey("BGM") && PlayerPrefs.HasKey("SFX"))
        {
            BGMSlider.value = PlayerPrefs.GetFloat("BGM");
            SFXSlider.value = PlayerPrefs.GetFloat("SFX");
        }
        else
        {
            BGMSlider.value = defaultVol;
            SFXSlider.value = defaultVol;
        }

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

        armor = 0;

        tableScores = highscoreTable.GetComponent<TableScores>();

    }

    private void Start()
    {
        // set all menus inactive
        pauseMenu.SetActive(false);
        loseMenu.SetActive(false);
        winMenu.SetActive(false);
        objectives.SetActive(false);
        upgradesMenu.SetActive(false);
    }

    void Update()
    {
        if (!isMain)
        {
            UIDisable();
            isPaused = !isPaused;
            activeMenu = mainMenu;
            activeMenu.SetActive(isPaused);
            pause();
            isMain = true;
            NavigateMenu.instance.OnMenuOpen(0);
        }

        // displays the objectives only at start.
        //if (!objectivesSeen)
        //{
        //    isPaused = !isPaused;
        //    activeMenu = objectives;
        //    activeMenu.SetActive(isPaused);
        //    pause();
        //    objectivesSeen = true;
        //}

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
            if (!waitingToTick)
            {
                StartCoroutine(difficultyTick());
            }
        }


        //opens pause menu
        if (Input.GetButtonDown("Cancel") && activeMenu == null)
        {
            isPaused = !isPaused;
            activeMenu = pauseMenu;
            activeMenu.SetActive(isPaused);
            NavigateMenu.instance.OnMenuOpen(1);

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
            StartCoroutine(spawnEnemies(0));
        }

        if (!miniBossSpawned)
        {
            if (timeCurrent < timeTotal * 0.75f)
            {
                StartCoroutine(spawnEnemies(1));
                miniBossSpawned = true;
            }
        }

        if (!droneBossSpawned)
        {
            if(timeCurrent < (timeTotal / 2))
            {
                StartCoroutine(spawnEnemies(2));
                droneBossSpawned = true;
            }
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

    IEnumerator spawnEnemies(int spawnType)
    {
        if (spawnType == 0)
        {
            isSpawning = true;
            enemyToSpawn = enemiesOptions[Random.Range(0, (int)spawnOffset)];
        }
        else if (spawnType == 1)
        {
            enemyToSpawn = miniBoss;
        }
        else if (spawnType == 2)
        {
            enemyToSpawn = droneBoss;
        }

        //int possibleLocation = Random.Range(0, navMeshTri.vertices.Length);

        int xChange = Random.Range(-30, 30);
        int zChange = Random.Range(-30, 30);

        Vector3 targetPos = new Vector3(gameManager.instance.player.transform.position.x + xChange,
            gameManager.instance.player.transform.position.y,
            gameManager.instance.player.transform.position.z + zChange);

        NavMeshHit hit;
        if (NavMesh.SamplePosition(targetPos, out hit, 30f, -1))
        {
            Instantiate(enemyToSpawn, hit.position, this.transform.rotation);
        }

        yield return new WaitForSeconds(spawnTimer);
        if (spawnType == 0)
        {
            isSpawning = false;
        }
    }

    void timerUpdate(float displaytime)
    {
        if (displaytime < 0)
        {
            displaytime = 0;
        }
        if (displaytime < 30)
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

        spawnOffset += 1f;
        damageIncreaseOffset++;
        if (damageIncreaseOffset % 3 == 0)
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
        playerHPBackground.enabled = true;
        playerHPMax.enabled = true;
        playerHPCurrent.enabled = true;
        armorCurrent.enabled = true;
        armorText.enabled = true;
        componentsDisplay.enabled = true;
        timerText.enabled = true;
        reticle.enabled = true;
        crosshair.enabled = true;
        forwardSlash.enabled = true;
        timerBackground.enabled = true;
        
        updateComponentsDisplay();

    }
    // turning OFF UI
    public void UIDisable()
    {
        playerHPBar.enabled = false;
        playerHPBackground.enabled = false;
        playerHPMax.enabled = false;
        playerHPCurrent.enabled = false;
        armorText.enabled = false;
        armorCurrent.enabled = false;
        componentsDisplay.enabled = false;
        timerText.enabled = false;
        reticle.enabled = false;
        crosshair.enabled = false;
        forwardSlash.enabled = false;
        timerBackground.enabled = false;
       
    }

    public void SetHighScore()
    {
        scoreTotal += Mathf.FloorToInt(timeTotal - timeCurrent);

        tableScores.AddHighScoreEntry(scoreTotal, nameHighscore, killcount, timeTotal - timeCurrent);
    }

    public void SetName(string _name)
    {
        nameHighscore = _name;
    }
}
