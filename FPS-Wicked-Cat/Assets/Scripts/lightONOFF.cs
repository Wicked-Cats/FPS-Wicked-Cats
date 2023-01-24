using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class lightONOFF : MonoBehaviour
{
    private Light light;
    // Start is called before the first frame update
    void Start()
    {
        light = GetComponent<Light>();
    }

    // Update is called once per frame
    void Update()
    {
        if((gameManager.instance.player.transform.position - this.transform.position).magnitude > 40)
        {
            light.enabled = false;
        }
        else
        {
            light.enabled = true;
        }
    }
}
