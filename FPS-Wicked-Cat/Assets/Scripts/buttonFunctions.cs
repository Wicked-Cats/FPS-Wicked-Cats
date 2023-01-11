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
        if (gameManager.instance.componentsCurrent >= gameManager.instance.respawnCost + gameManager.instance.timeDamageIncrease)
        {
            gameManager.instance.componentsCurrent -= gameManager.instance.respawnCost + gameManager.instance.timeDamageIncrease;
            gameManager.instance.componentsTotal -= gameManager.instance.respawnCost + gameManager.instance.timeDamageIncrease;
            gameManager.instance.respawnCost += 5;
            gameManager.instance.playerScript.ResetPlayerHP();
            gameManager.instance.isPaused = !gameManager.instance.isPaused;
            gameManager.instance.unPause();
            //gameManager.instance.playerScript.SetPlayerPos();
            gameManager.instance.updateComponentsDisplay();
            gameManager.instance.playerScript.updateHPBar();
        }
    }

    //not using may use at a later date
    //public void addJump()
    //{
    //    if (gameManager.instance.playerScript.jumpsMax < gameManager.instance.jumpsLimit)
    //    {
    //        if (gameManager.instance.componentsCurrent >= 3)
    //        {
    //            gameManager.instance.componentsCurrent -= 3;
    //            gameManager.instance.playerScript.jumpsMax++;
    //            upgradesButttonsCheck();
    //            gameManager.instance.updateComponentsDisplay();
    //        }
    //    }
    //}

    public void damageUp()
    {
        if (gameManager.instance.playerScript.damage < gameManager.instance.damageLimit)
        {
            if (gameManager.instance.componentsCurrent >= gameManager.instance.damageCost)
            {
                gameManager.instance.componentsCurrent -= gameManager.instance.damageCost;
                gameManager.instance.playerScript.damage++;
                gameManager.instance.damageCost += 2;
                upgradesButttonsCheck();
                gameManager.instance.updateComponentsDisplay();
            }
        }
    }

    public void HpUP()
    {
        if (gameManager.instance.playerScript.HPOrig < gameManager.instance.HPLimit)
        {
            if (gameManager.instance.componentsCurrent >= gameManager.instance.HPCost)
            {
                gameManager.instance.componentsCurrent -= gameManager.instance.HPCost;
                gameManager.instance.playerScript.HPOrig += 5;
                gameManager.instance.playerScript.HP += 5;
                gameManager.instance.playerScript.updateHPBar();
                gameManager.instance.HPCost += 2;
                upgradesButttonsCheck();
                gameManager.instance.updateComponentsDisplay();
            }
        }
    }

    public void speedUp()
    {
        if (gameManager.instance.playerScript.playerSpeed < gameManager.instance.speedLimit)
        {
            if (gameManager.instance.componentsCurrent >= gameManager.instance.speedCost)
            {
                gameManager.instance.componentsCurrent -= gameManager.instance.speedCost;
                gameManager.instance.playerScript.playerSpeed++;
                gameManager.instance.speedCost += 2;
                upgradesButttonsCheck();
                gameManager.instance.updateComponentsDisplay();
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
        if (gameManager.instance.componentsCurrent < gameManager.instance.damageCost)
        {
            gameManager.instance.dmgButton.interactable = false;
        }
        else
        {
            gameManager.instance.dmgButton.interactable = true;
        }
        if (gameManager.instance.componentsCurrent < gameManager.instance.rangeCost)
        {
            gameManager.instance.rangeButton.interactable = false;
        }
        else
        {
            gameManager.instance.rangeButton.interactable = true;
        }
        if (gameManager.instance.componentsCurrent < gameManager.instance.HPCost)
        {
            gameManager.instance.HPButton.interactable = false;
        }
        else
        {
            gameManager.instance.HPButton.interactable = true;
        }
        if (gameManager.instance.componentsCurrent < gameManager.instance.speedCost)
        {
            gameManager.instance.speedButton.interactable = false;
        }
        else
        {
            gameManager.instance.speedButton.interactable = true;
        }
        gameManager.instance.upgradesComponentCurrent.text = gameManager.instance.componentsCurrent.ToString("F0");
        gameManager.instance.damageButtonText.text = "Damage + 1 (-" + gameManager.instance.damageCost.ToString() + " Components)";
        gameManager.instance.HPButtonText.text = "HP + 5 (-" + gameManager.instance.HPCost.ToString() + " Components)";
        gameManager.instance.rangeButtonText.text = "Range + 1 (-" + gameManager.instance.rangeCost.ToString() + " Components)";
        gameManager.instance.speedButtonText.text = "Speed + 1 (-" + gameManager.instance.speedCost.ToString() + " Components)";
    }

    public void rangeUp()
    {
        if (gameManager.instance.playerScript.rangeUp < gameManager.instance.rangeUpLimit)
        {
            if (gameManager.instance.componentsCurrent >= gameManager.instance.rangeCost)
            {
                gameManager.instance.componentsCurrent -= gameManager.instance.rangeCost;
                gameManager.instance.playerScript.rangeUp++;
                gameManager.instance.rangeCost += 2;
                upgradesButttonsCheck();
                gameManager.instance.updateComponentsDisplay();
            }
        }
    }


    // VVV Main Menu Items VVV
    public void QuickPlay()
    {
        gameManager.instance.timeCurrent = 300;
        gameManager.instance.diffTickTime = gameManager.instance.timeCurrent / (gameManager.instance.enemiesOptions.Length - 1);
        gameManager.instance.isPaused = !gameManager.instance.isPaused;
        gameManager.instance.objectivesSeen = true;
        gameManager.instance.unPause();
        gameManager.instance.activeMenu = null;
        gameManager.instance.UIEnable();
    }

    public void SurvivalMode()
    {
        gameManager.instance.timeCurrent = 1800;
        gameManager.instance.diffTickTime = gameManager.instance.timeCurrent / (gameManager.instance.enemiesOptions.Length - 1);
        gameManager.instance.isPaused = !gameManager.instance.isPaused;
        gameManager.instance.objectivesSeen = true;
        gameManager.instance.unPause();
        gameManager.instance.activeMenu = null;
        gameManager.instance.UIEnable();
    }

    public void OptionsMenu()
    {
        gameManager.instance.activeMenu = gameManager.instance.optionsMenu;
        gameManager.instance.activeMenu.SetActive(true);
        gameManager.instance.pauseMenu.SetActive(false);
    }

    public void CloseOptions()
    {
        gameManager.instance.activeMenu = gameManager.instance.mainMenu;
        gameManager.instance.optionsMenu.SetActive(false);
        gameManager.instance.activeMenu.SetActive(true);
    }
}
