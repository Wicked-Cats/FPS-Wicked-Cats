using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interaction : MonoBehaviour
{
    bool playerInSphere = false;
    bool inMenu = false;
    GameObject[] menus;
    GameObject currentMenu;
    [SerializeField] string interactionText;
    [SerializeField] string menuName;

    void Awake()
    {
        //menus = GameObject.FindGameObjectsWithTag("Menu");
        //foreach(GameObject menu in menus)
        //{
        //    if (menu.name == menuName)
        //    {
        //        currentMenu = menu;
        //    }
        //}
    }

    // Update is called once per frame
    void Update()
    {
        if(playerInSphere == true && Input.GetKeyDown(KeyCode.E) && inMenu == false)
        {
            // pull up screen
            gameManager.instance.pause();
            gameManager.instance.isPaused = !gameManager.instance.isPaused;
            gameManager.instance.activeMenu = gameManager.instance.upgradesMenu;
            gameManager.instance.activeMenu.SetActive(true);

            inMenu = true;
        }
        else if (Input.GetKeyDown(KeyCode.E) && inMenu == true)
        {
            if (gameManager.instance.isPaused)
            {
                gameManager.instance.unPause();
                gameManager.instance.isPaused = !gameManager.instance.isPaused;
            }

            inMenu = false;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInSphere = true;
            gameManager.instance.interactableText.text = interactionText;
            gameManager.instance.interactableTextParent.SetActive(true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInSphere = false;
            gameManager.instance.interactableTextParent.SetActive(false);
        }
    }
}
