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

}
