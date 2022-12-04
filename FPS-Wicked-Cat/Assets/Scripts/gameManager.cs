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

    public bool isPaused;
    float timeScaleBase;
    public GameObject playerSpawnPos;
    GameObject flyerSpawn1;
    GameObject flyerSpawn2;
    bool isSpawningFly;
    public int componentsCurrent;
    public int componentsTotal;


    void Awake()
    {
        instance = this;
        player = GameObject.FindGameObjectWithTag("Player");
        playerScript = player.GetComponent<playerController>();
        timeScaleBase = Time.timeScale;
        //playerSpawnPos = GameObject.FindGameObjectWithTag("Player Spawn Pos");
        //player.transform.position = playerSpawnPos.transform.position;
    }

    // Update is called once per frame
    void Update()
    {
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
        if(!isSpawningFly)
        {
            isSpawningFly = true;
            float num = Random.Range(0, 99);
            if(num < 50)
            {
                Instantiate(flyer, flyerSpawn1.transform.position, flyerSpawn1.transform.rotation);
            }
            else
            {
                Instantiate(flyer, flyerSpawn2.transform.position, flyerSpawn2.transform.rotation);
            }
        }
        yield return new WaitForSeconds(30f);
        isSpawningFly = false;
    }
}
