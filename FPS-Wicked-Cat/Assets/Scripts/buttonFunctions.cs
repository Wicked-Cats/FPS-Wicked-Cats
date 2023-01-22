using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class buttonFunctions : MonoBehaviour
{
    bool openedFromMenu;

    public void resume()
    {
        SFXBtnClick();
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
        SFXBtnClick();
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
        SFXBtnClick();
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
        SFXBtnClick();
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
        SFXBtnClick();
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
        openedFromMenu = true;
        SFXBtnClick();
        gameManager.instance.activeMenu = gameManager.instance.upgradesMenu;
        gameManager.instance.activeMenu.SetActive(true);
        gameManager.instance.pauseMenu.SetActive(false);
        upgradesButttonsCheck();
    }
    public void closeUpgrades()
    {
        SFXBtnClick();
        if (openedFromMenu)
        {
            gameManager.instance.activeMenu = gameManager.instance.pauseMenu;
            gameManager.instance.upgradesMenu.SetActive(false);
            gameManager.instance.activeMenu.SetActive(true);
        }
        else
        {
            gameManager.instance.isPaused = !gameManager.instance.isPaused;
            gameManager.instance.unPause();
            gameManager.instance.activeMenu=null;
        }
        openedFromMenu = false;
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
        if (gameManager.instance.componentsCurrent < gameManager.instance.critDamageCost)
        {
            gameManager.instance.critDamageButton.interactable = false;
        }
        else
        {
            gameManager.instance.critDamageButton.interactable = true;
        }
        if (gameManager.instance.componentsCurrent < gameManager.instance.critChanceCost)
        {
            gameManager.instance.critChanceButton.interactable = false;
        }
        else
        {
            gameManager.instance.critChanceButton.interactable = true;
        }
        if (gameManager.instance.componentsCurrent < gameManager.instance.magnetCost)
        {
            gameManager.instance.magnetButton.interactable = false;
        }
        else
        {
            gameManager.instance.magnetButton.interactable = true;
        }
        if (gameManager.instance.componentsCurrent < gameManager.instance.armorCost || gameManager.instance.armor >= gameManager.instance.playerScript.HPOrig)
        {
            gameManager.instance.armorButton.interactable = false;
        }
        else
        {
            gameManager.instance.armorButton.interactable = true;
        }
        if (gameManager.instance.componentsCurrent < gameManager.instance.healthPackCost || gameManager.instance.playerScript.HP >= gameManager.instance.playerScript.HPOrig)
        {
            gameManager.instance.healthPackButton.interactable = false;
        }
        else
        {
            gameManager.instance.healthPackButton.interactable = true;
        }

        gameManager.instance.upgradesComponentCurrent.text = gameManager.instance.componentsCurrent.ToString("F0");
        gameManager.instance.damageButtonText.text = "Damage + 1 (-" + gameManager.instance.damageCost.ToString("F0") + " Components)";
        gameManager.instance.HPButtonText.text = "HP + 5 (-" + gameManager.instance.HPCost.ToString("F0") + " Components)";
        gameManager.instance.rangeButtonText.text = "Range + 1 (-" + gameManager.instance.rangeCost.ToString("F0") + " Components)";
        gameManager.instance.speedButtonText.text = "Speed + 1 (-" + gameManager.instance.speedCost.ToString("F0") + " Components)";
        gameManager.instance.critDamageButtonText.text = "Crit Damage + 25% (-" + gameManager.instance.critDamageCost.ToString("F0") + " Components)";
        gameManager.instance.critChanceButtonText.text = "Crit Chance + 5% (-" + gameManager.instance.critChanceCost.ToString("F0") + " Components)";
        gameManager.instance.magnetButtonText.text = "Pickup Range + 10% (-" + gameManager.instance.magnetCost.ToString("F0") + " Components)";
        gameManager.instance.armorButtonText.text = "Armor + 10 (-" + gameManager.instance.armorCost.ToString("F0") + " Componemts)";
        gameManager.instance.healthPackButtonText.text = "Heal 10% of HP (-" + gameManager.instance.healthPackCost.ToString("F0") + " Components)";
    }

    public void rangeUp()
    {
        SFXBtnClick();
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
        SFXBtnClick();
        gameManager.instance.timeCurrent = 300;
        gameManager.instance.diffTickTime = gameManager.instance.timeCurrent / (gameManager.instance.enemiesOptions.Length - 1);
        gameManager.instance.isPaused = !gameManager.instance.isPaused;
        gameManager.instance.objectivesSeen = true;
        gameManager.instance.mainMenu.SetActive(false);
        gameManager.instance.unPause();
        gameManager.instance.activeMenu = null;
        gameManager.instance.UIEnable();
    }

    public void SurvivalMode()
    {
        SFXBtnClick();
        gameManager.instance.timeCurrent = 1800;
        gameManager.instance.diffTickTime = gameManager.instance.timeCurrent / (gameManager.instance.enemiesOptions.Length - 1);
        gameManager.instance.isPaused = !gameManager.instance.isPaused;
        gameManager.instance.objectivesSeen = true;
        gameManager.instance.mainMenu.SetActive(false);
        gameManager.instance.unPause();
        gameManager.instance.activeMenu = null;
        gameManager.instance.UIEnable();
    }

    public void OptionsMenu()
    {
        SFXBtnClick();
        gameManager.instance.lastMenu = gameManager.instance.activeMenu;
        gameManager.instance.activeMenu = gameManager.instance.optionsMenu;
        gameManager.instance.activeMenu.SetActive(true);
        gameManager.instance.pauseMenu.SetActive(false);
        NavigateMenu.instance.OnMenuOpen(3); 
    }

    public void GoBackToMain()
    {
        SFXBtnClick();
        gameManager.instance.activeMenu = gameManager.instance.mainMenu;
        gameManager.instance.optionsMenu.SetActive(false);
        gameManager.instance.activeMenu.SetActive(true);
    }

    public void GoBackToPause()
    {
        SFXBtnClick();
        gameManager.instance.activeMenu = gameManager.instance.pauseMenu;
        gameManager.instance.optionsMenu.SetActive(false);
        gameManager.instance.activeMenu.SetActive(true);
    }

    public void CloseOptionsBtn()
    {
        SFXBtnClick();
        if (gameManager.instance.lastMenu == gameManager.instance.mainMenu)
        {
            GoBackToMain();
        }
        else if(gameManager.instance.lastMenu == gameManager.instance.pauseMenu) 
        {
            GoBackToPause();
        }
    }

    public void SFXBtnClick()
    {
        gameManager.instance.playerScript.aud.PlayOneShot(gameManager.instance.playerScript.SFXBtn, 1f);
    }

    public void MagnetUpgrade()
    {
        if (gameManager.instance.magnetRange < gameManager.instance.magnetLimit)
        {
            if (gameManager.instance.componentsCurrent >= gameManager.instance.magnetCost)
            {
                gameManager.instance.componentsCurrent -= gameManager.instance.magnetCost;
                gameManager.instance.magnetRange += (gameManager.instance.magnetRange / 10);
                gameManager.instance.magnetCost += 10;
                gameManager.instance.playerScript.magnetRangeSet(gameManager.instance.magnetRange);
                upgradesButttonsCheck();
                gameManager.instance.updateComponentsDisplay();
            }
        }
    }

    public void CritChanceUpgrade()
    {
        if (gameManager.instance.critChance < gameManager.instance.critChanceLimit)
        {
            if (gameManager.instance.componentsCurrent >= gameManager.instance.critChanceCost)
            {
                gameManager.instance.componentsCurrent -= gameManager.instance.critChanceCost;
                gameManager.instance.critChance += 5;
                gameManager.instance.critChanceCost += 5;
                upgradesButttonsCheck();
                gameManager.instance.updateComponentsDisplay();
            }
        }
    }

    public void CritDamageUpgrade()
    {
        if (gameManager.instance.critDamageMulti < gameManager.instance.critDamageLimit)
        {
            if (gameManager.instance.componentsCurrent >= gameManager.instance.critDamageCost)
            {
                gameManager.instance.componentsCurrent -= gameManager.instance.critDamageCost;
                gameManager.instance.critDamageMulti += 0.25f;
                gameManager.instance.critDamageCost += 5;
                upgradesButttonsCheck();
                gameManager.instance.updateComponentsDisplay();
            }
        }
    }

    public void ArmorUpgrade()
    {
        if (gameManager.instance.armor < gameManager.instance.playerScript.HPOrig)
        {
            if (gameManager.instance.componentsCurrent >= gameManager.instance.armorCost)
            {
                gameManager.instance.componentsCurrent -= gameManager.instance.armorCost;
                gameManager.instance.armor += 10;
                gameManager.instance.armorCost += 4;
                gameManager.instance.playerScript.updateHPBar();
                upgradesButttonsCheck();
                gameManager.instance.updateComponentsDisplay();
            }
        }
    }

    public void HealthPackUpgrade()
    {
        if (gameManager.instance.playerScript.HP < gameManager.instance.playerScript.HPOrig)
        {
            if (gameManager.instance.componentsCurrent >= gameManager.instance.healthPackCost)
            {
                gameManager.instance.componentsCurrent -= gameManager.instance.healthPackCost;
                gameManager.instance.playerScript.HP += (gameManager.instance.playerScript.HPOrig / 10);

                if (gameManager.instance.playerScript.HP > gameManager.instance.playerScript.HPOrig)
                {
                    gameManager.instance.playerScript.HP = gameManager.instance.playerScript.HPOrig;
                }

                gameManager.instance.playerScript.updateHPBar();
                gameManager.instance.healthPackCost += 2;
                upgradesButttonsCheck();
                gameManager.instance.updateComponentsDisplay();
            }
        }
    }
}
