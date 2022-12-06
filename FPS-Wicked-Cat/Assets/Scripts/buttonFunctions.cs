using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class buttonFunctions : MonoBehaviour
{
    public void resume()
    {
        gameManager.instance.isPaused = !gameManager.instance.isPaused;
        gameManager.instance.unPause();
    }

    public void restart()
    {
        gameManager.instance.unPause();
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void quit()
    {
        Application.Quit();
    }

    public void respawnPlayer()
    {
        if (gameManager.instance.componentsCurrent >= 5)
        {
            gameManager.instance.componentsCurrent -= 5;
            gameManager.instance.componentsTotal -= 5;
            gameManager.instance.playerScript.ResetPlayerHP();
            gameManager.instance.unPause();
            gameManager.instance.playerScript.SetPlayerPos();
        }
    }

    public void addJump()
    {
        if (gameManager.instance.playerScript.jumpsMax < gameManager.instance.jumpsLimit)
        {
            if (gameManager.instance.componentsCurrent >= 3)
            {
                gameManager.instance.componentsCurrent -= 3;
                gameManager.instance.playerScript.jumpsMax++;
                upgradesButttonsCheck();
            }
        }
    }

    public void damageUp()
    {
        if (gameManager.instance.playerScript.damage < gameManager.instance.damageLimit)
        {
            if (gameManager.instance.componentsCurrent >= 3)
            {
                gameManager.instance.componentsCurrent -= 3;
                gameManager.instance.playerScript.damage++;
                upgradesButttonsCheck();
            }
        }
    }

    public void HpUP()
    {
        if (gameManager.instance.playerScript.HPMax < gameManager.instance.HPLimit)
        {
            if (gameManager.instance.componentsCurrent >= 5)
            {
                gameManager.instance.componentsCurrent -= 5;
                gameManager.instance.playerScript.HPMax += 5;
                gameManager.instance.playerScript.HP += 5;
                upgradesButttonsCheck();

            }
        }
    }

    public void speedUp()
    {
        if (gameManager.instance.playerScript.playerSpeed < gameManager.instance.speedLimit)
        {
            if (gameManager.instance.componentsCurrent >= 4)
            {
                gameManager.instance.componentsCurrent -= 4;
                gameManager.instance.playerScript.playerSpeed++;
                upgradesButttonsCheck();
            }
        }
    }

    public void upgradesMenu()
    {
        gameManager.instance.activeMenu = gameManager.instance.upgradesMenu;
        gameManager.instance.activeMenu.SetActive(true);
        gameManager.instance.pauseMenu.SetActive(false);
        upgradesButttonsCheck();
    }
    public void closeUpgrades()
    {
        gameManager.instance.activeMenu = gameManager.instance.pauseMenu;
        gameManager.instance.upgradesMenu.SetActive(false);
        gameManager.instance.activeMenu.SetActive(true);
    }

    public void upgradesButttonsCheck()
    {
        if(gameManager.instance.componentsCurrent < 3)
        {
            gameManager.instance.dmgButton.interactable = false;
            gameManager.instance.speedButton.interactable = false;
            gameManager.instance.HPButton.interactable = false;
            gameManager.instance.jumpButton.interactable = false;
        }
        else if(gameManager.instance.componentsCurrent < 4)
        {
            gameManager.instance.speedButton.interactable = false;
            gameManager.instance.HPButton.interactable = false;
        }
        else if(gameManager.instance.componentsCurrent < 5)
        {
            gameManager.instance.HPButton.interactable = false;
        }
        else
        {
            gameManager.instance.dmgButton.interactable = true;
            gameManager.instance.speedButton.interactable = true;
            gameManager.instance.HPButton.interactable = true;
            gameManager.instance.jumpButton.interactable = true;
        }
    }

}
