using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopSpawn : MonoBehaviour
{
    [SerializeField] private GameObject shopModel;
    [SerializeField] private GameObject[] shopSpawnLocations;
    [SerializeField] private int shopSpawnFrequency;
    [SerializeField] private int shopDuration;

    private int shopSpawnLocation;
    private GameObject instantiatedShopModel;
    private bool isSpawning = false;
    int tick;

    private void Start()
    {
        // determine spawn locations
        shopSpawnLocations = GameObject.FindGameObjectsWithTag("Shop Spawn Pos");
    }

    public void Update()
    {
        if (!isSpawning)
        {
            isSpawning = true;
            StartCoroutine(spawn());
        }
        if (isSpawning)
        {

        }
    }


    IEnumerator spawn()
    {


        // select random spawn location
        shopSpawnLocation = Random.Range(0, shopSpawnLocations.Length - 1);


        // spawn shop
        instantiatedShopModel = Instantiate(shopModel, shopSpawnLocations[shopSpawnLocation].transform.position, shopSpawnLocations[shopSpawnLocation].transform.rotation);


        // broadcast message
        StartCoroutine(popUpWait());


        StartCoroutine(remove());
        // wait before spawning
        yield return new WaitForSeconds(shopSpawnFrequency);
    }

    IEnumerator remove()
    {
        // wait before removing
        yield return new WaitForSeconds(shopDuration);


        // despawn
        Destroy(instantiatedShopModel);


        // remove broadcast message


        // update UI
        Interaction.playerInSphere = false;
        gameManager.instance.interactableTextParent.SetActive(false);


        isSpawning = false;
    }

    IEnumerator popUp()
    {
        gameManager.instance.shopSpawnBroadcastParent.SetActive(true);
        yield return new WaitForSeconds(15f);
        gameManager.instance.shopSpawnBroadcastParent.SetActive(false);
    }
    
    IEnumerator popUpWait()
    {
        yield return new WaitForSeconds(3f);
        StartCoroutine(popUp());
    }
}
