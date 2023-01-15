using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interaction : MonoBehaviour
{
    bool playerInSphere;
    [SerializeField] string interactionText;

    // Update is called once per frame
    void Update()
    {
        if(playerInSphere == true && Input.GetKeyDown(KeyCode.E)
        {

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
