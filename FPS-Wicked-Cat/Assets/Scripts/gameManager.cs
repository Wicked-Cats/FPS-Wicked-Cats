using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class gameManager : MonoBehaviour
{
    public static gameManager instance;

    [Header("----- Player Stuff ----")]
    public GameObject player;
    public playerController playerScript;

    [Header("----- UI Stuff ----")]
    public GameObject pauseMenu;
    public GameObject activeMenu;
    public GameObject winMenu;
    public GameObject loseMenu;
    public GameObject playerFlashDmg;

    [Header("----- Collectables ----")]
    public int jumpCost;
    public int coins;
    public int enemyCount;

    public bool isPaused;
    float timeScaleOrig;
    public GameObject playerSpawnPos;

    void Awake()
    {
        instance = this;
        player = GameObject.FindGameObjectWithTag("pLayer");
        playerScript = player.GetComponent<playerController>();
        timeScaleOrig = Time.timeScale;
        playerSpawnPos = GameObject.FindGameObjectWithTag("Player Spawn Pos");
        player.transform.position = playerSpawnPos.transform.position;
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
    }

    public void pause()
    {
        Time.timeScale = 0;
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.Confined;
    }

    public void unPause()
    {
        Time.timeScale = timeScaleOrig;
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        activeMenu.SetActive(false);
        activeMenu = null;
    }

    public void AddCoins(int amount)
    {
        coins += amount;
    }

    public void UpdateEnemyCount(int amount)
    {
        enemyCount += amount;
        if (enemyCount <= 0)
        {
            // end game
            // bring up end game screen
            winMenu.SetActive(true);
            pause();
            activeMenu = winMenu;

        }
    }
}