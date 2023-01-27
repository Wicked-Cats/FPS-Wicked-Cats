using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;


public class NavigateMenu : MonoBehaviour
{
    public static NavigateMenu instance;

    public int selection;
    public Image currImage;
    public Color Orig;
    messageAppear message;

    [Header("--- Main Menu Button List ---")]
    [SerializeField] Button[] mainMenuArr;

    [Header("--- Pause Menu Button List ---")]
    [SerializeField] Button[] pauseMenuArr;

    [Header("--- Options Menu Button List ---")]
    [SerializeField] Selectable[] selectableArr;
    [SerializeField] Slider[] sliderArr;
    [SerializeField] Button[] highlightBtnArr;
    [SerializeField] Button[] optionBtnArr;

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
                    if (message != null)
                    {
                        message.OnPointerExitManual();
                    }
                    selection--;
                    MenuSelection();
                }
                else
                {
                    currImage.color = Orig;
                    if (message != null)
                    {
                        message.OnPointerExitManual();
                    }
                    selection = mainMenuArr.Length - 1;
                    MenuSelection();
                }
            }
            else if (Input.GetKeyDown(KeyCode.RightArrow) || Input.GetKeyDown(KeyCode.D))
            {
                if (selection != mainMenuArr.Length - 1)
                {
                    currImage.color = Orig;
                    if (message != null)
                    {
                        message.OnPointerExitManual();
                    }
                    selection++;
                    MenuSelection();
                }
                else
                {
                    currImage.color = Orig;
                    if (message != null)
                    {
                        message.OnPointerExitManual();
                    }
                    selection = 0;
                    MenuSelection();
                }

            }

            if (Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.KeypadEnter))
            {
                currImage.color = Orig;
                mainMenuArr[selection].onClick.Invoke();
            }


        }
        else if (gameManager.instance.activeMenu == gameManager.instance.pauseMenu)
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
            }
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

            if (Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.KeypadEnter))
            {
                currImage.color = Orig;
                pauseMenuArr[selection].onClick.Invoke();
            }
        }
        else if (gameManager.instance.activeMenu == gameManager.instance.optionsMenu)
        {
            if (Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.W))
            {
                if (selection != 0)
                {
                    currImage.color = Orig;
                    selection--;
                    OptionSelection();
                }
                else
                {
                    currImage.color = Orig;
                    selection = 0;
                    OptionSelection();
                }

            }
            else if (Input.GetKeyDown(KeyCode.DownArrow) || Input.GetKeyDown(KeyCode.S))
            {
                if (selection != selectableArr.Length - 1)
                {
                    currImage.color = Orig;
                    selection++;
                    OptionSelection();
                }
                else
                {
                    currImage.color = Orig;
                    selection = 0;
                    OptionSelection();
                }
            }

            if (Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.KeypadEnter))
            {
                currImage.color = Orig;
                if (selectableArr[selection].name== "CloseOptions Menu")
                {
                   optionBtnArr[0].onClick.Invoke();
                }
                else if (selectableArr[selection].name == "Apply_Changes")
                {
                    optionBtnArr[1].onClick.Invoke();
                }
                //else if (selectableArr[selection].name == "BGM Slider")
                //{
                //    SoundControls();
                //}
                //else if (selectableArr[selection].name == "SFX Slider")
                //{
                //    SoundControls();
                //}
            }

        }

    }


    private void MenuSelection()
    {
        message = mainMenuArr[selection].GetComponent<messageAppear>();
        if (message != null)
        {
            message.OnPointerEnterManual();
        }

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
            currImage.color = Color.gray;
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
        if (choice == 0)
        {
            selection = 0;
            currImage = mainMenuArr[selection].GetComponent<Image>();
            Orig = currImage.color;
            MenuSelection();
        }
        else if (choice == 1)
        {
            selection = 0;
            currImage = pauseMenuArr[selection].GetComponent<Image>();
            Orig = currImage.color;
            PauseSelection();
        }
        else if (choice == 3)
        {
            selection = 1;
            currImage = highlightBtnArr[selection].GetComponent<Image>();
            Orig = currImage.color;
            OptionSelection();    
        }
    }

    private void OptionSelection()
    {
        if (selectableArr[selection].name == "CloseOptions Menu")
        {
            currImage = highlightBtnArr[selection].GetComponent<Image>();
            Orig = currImage.color;
            currImage.color = Color.gray;
        }
        //else if (selectableArr[selection].name == "SFX Slider")
        //{
        //    currImage = highlightBtnArr[selection].gameObject.GetComponent<Image>();
        //    Orig = currImage.color;
        //    currImage.color = Color.gray;        
        //}
        //else if (selectableArr[selection].name == "BGM Slider")
        //{
        //    currImage = highlightBtnArr[selection].gameObject.GetComponent<Image>();
        //    Orig = currImage.color;
        //    currImage.color = Color.gray;
        //}
        else if (selectableArr[selection].name == "Apply_Changes")
        {
            currImage = highlightBtnArr[selection].GetComponent<Image>();
            Orig = currImage.color;
            currImage.color = Color.gray;

        }
    }

    private void SoundControls()
    {
        // Increase Vol
        if (Input.GetKeyDown(KeyCode.RightArrow) || Input.GetKeyDown(KeyCode.D))
        {
            if (sliderArr[selection].value > 1)
            {
                sliderArr[selection].value = 1f;
            }
            else
            {
                sliderArr[selection].value += 0.1f;
            }
        }// Decrease Vol
        else if (Input.GetKeyDown(KeyCode.LeftArrow) || Input.GetKeyDown(KeyCode.A))
        {
            if (sliderArr[selection].value < 0)
            {
                sliderArr[selection].value = 0.0001f;
            }
            else
            {
                sliderArr[selection].value -= 0.1f;
            }
        }
    }
}
