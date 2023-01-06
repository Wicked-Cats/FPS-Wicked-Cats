using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class MenuMessage : MonoBehaviour
{
    // this is the mouse
    EventSystem _mouse;

    // Start is called before the first frame update
    void Start()
    {
       _mouse= GameObject.Find("EventSystem").GetComponent<EventSystem>();
    }

    public string MouseHover()
    {
        string quickPlay = "\tThe rules to the game are simple: \n" +
                            "Rule 1: Survive the Onslaught of Robots during the Lockdown.\n" +
                            "Rule 2: Find Mr. Computo or Mr. Scrapper to purchase items.\n" +
                            "Rule 3: Use the (esc) to pause the game if needed.\n" +
                            "Rule 4: DO YOUR BEST NOT TO END UP AS SCRAPS....";
        if (_mouse.IsPointerOverGameObject()){
            return quickPlay;
        }else
        {
            return null;
        }
    }


}
