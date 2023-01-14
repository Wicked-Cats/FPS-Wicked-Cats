using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;


public class NavigateMenu : MonoBehaviour
{
    public static NavigateMenu instance;

    public int selection;
    private Image currImage;
    Color Orig;


    [Header("--- Main Menu Button List ---")]
    [SerializeField] Button[] mainMenuArr;

    [Header("--- Pause Menu Button List ---")]
    [SerializeField] Button[] pauseMenuArr;


    // Start is called before the first frame update
    void Awake()
    {
        instance = this;
    }

    // Update is called once per frame
    void Update()
    {
        if (gameManager.instance.activeMenu == gameManager.instance.mainMenu)
        {

            if (Input.GetKeyDown(KeyCode.LeftArrow) || Input.GetKeyDown(KeyCode.A))
            {
                if (selection != 0)
                {
                    currImage.color = Orig;
                    selection--;
                    MenuSelection();
                }
                else
                {
                    currImage.color = Orig;
                    selection = mainMenuArr.Length - 1;
                    MenuSelection();
                }
            }
            else if (Input.GetKeyDown(KeyCode.RightArrow) || Input.GetKeyDown(KeyCode.D))
            {
                if (selection != mainMenuArr.Length - 1)
                {
                    currImage.color = Orig;
                    selection++;
                    MenuSelection();
                }
                else
                {
                    currImage.color = Orig;
                    selection = 0;
                    MenuSelection();
                }

            }
            if (Input.GetKeyDown(KeyCode.Return))
            {
                mainMenuArr[selection].onClick.Invoke();
            }
            

        }
        else if(gameManager.instance.activeMenu == gameManager.instance.pauseMenu)
        {

            if (Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.W))
            {
                if (selection != 0)
                {
                    currImage.color = Orig;
                    selection--;
                    PauseSelection();
                }
                else
                {
                    currImage.color = Orig;
                    selection = pauseMenuArr.Length - 1;
                    PauseSelection();

                }
            } // DOWN KEY DOE
            else if (Input.GetKeyDown(KeyCode.DownArrow) || Input.GetKeyDown(KeyCode.S))
            {
                if (selection != pauseMenuArr.Length - 1)
                {
                    currImage.color = Orig;
                    selection++;
                    PauseSelection();
                }
                else
                {
                    currImage.color = Orig;
                    selection = 0;
                    PauseSelection();
                }
            }

            if (Input.GetKeyDown(KeyCode.Return))
            {
                pauseMenuArr[selection].onClick.Invoke();
            }
        }

    }


    private void MenuSelection()
    {

        if (mainMenuArr[selection].name == "Options_Button")
        {
            currImage = mainMenuArr[selection].GetComponent<Image>();
            Orig = currImage.color;
            currImage.color = Color.gray;
        }
        else if (mainMenuArr[selection].name == "Exit_Game_Button")
        {
            currImage = mainMenuArr[selection].GetComponent<Image>();
            Orig = currImage.color;
            currImage.color = Color.grey;
        }
        else
        {
            currImage = mainMenuArr[selection].GetComponent<Image>();
            Orig = currImage.color;
            currImage.color = Color.red;
        }

    }

    private void PauseSelection()
    {

        currImage = pauseMenuArr[selection].GetComponent<Image>();
        Orig = currImage.color;
        currImage.color = Color.cyan;
    }

    public void OnMenuOpen(int choice)
    {
        selection = 0;

        if (choice == 0)
        {
            currImage = mainMenuArr[selection].GetComponent<Image>();
            Orig = currImage.color;
            MenuSelection();
        }
        else if (choice == 1)
        {
            currImage = pauseMenuArr[selection].GetComponent<Image>();
            Orig = currImage.color;
            PauseSelection();
        }
    }
}
