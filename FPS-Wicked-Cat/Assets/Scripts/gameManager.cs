using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class gameManager : MonoBehaviour
{
    public static gameManager instance;

    [Header("------Player Components------")]
    public GameObject player;
    public playerController playerScript;


    [Header("------UI Components------")]
    public GameObject objectives;
    public GameObject activeMenu;
    public GameObject pauseMenu;
    public GameObject winMenu;
    public GameObject loseMenu;
    public GameObject damageFlash;

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
    private bool objectivesSeen;


    void Awake()
    {
        instance = this;
        player = GameObject.FindGameObjectWithTag("Player");
        playerScript = player.GetComponent<playerController>();
        timeScaleBase = Time.timeScale;
        //playerSpawnPos = GameObject.FindGameObjectWithTag("Player Spawn Pos");
        //player.transform.position = playerSpawnPos.transform.position;
    }

    void Update()
    {
        if (!objectivesSeen)
        {
            isPaused = !isPaused;
            activeMenu = objectives;
            activeMenu.SetActive(isPaused);
            pause();
            objectivesSeen = true;
        }
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
        else if (Input.GetButtonDown("Cancel") && activeMenu == pauseMenu)
        {
            isPaused = !isPaused;
            unPause();
        }

        StartCoroutine(spawnFly());

        if (componentsTotal == 30 && activeMenu == null)
        {
            isPaused = !isPaused;
            activeMenu = winMenu;
            activeMenu.SetActive(isPaused);
            pause();
            objectivesSeen = false;
            componentsTotal = 0;
            componentsCurrent = 0;
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
