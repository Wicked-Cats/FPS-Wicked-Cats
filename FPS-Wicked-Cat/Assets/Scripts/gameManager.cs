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
    public GameObject activeMenu;
    public GameObject pauseMenu;
    public GameObject winMenu;
    public GameObject loseMenu;
    public GameObject damageFlash;

    public bool isPuased;
    float timeScaleBase;


    void Awake()
    {
        instance = this;
        //player = GameObject.FindGameObjectWithTag("Player");
        //playerScript = player.GetComponent<playerController>();
        timeScaleBase = Time.timeScale;
    }

    
    void Update()
    {
        

        if (Input.GetButtonDown("Cancel") && activeMenu == null)
        {
            isPuased = !isPuased;
            activeMenu = pauseMenu;
            activeMenu.SetActive(isPuased);

            if(isPuased)
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
            isPuased = !isPuased;
            unPause();
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
}
