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
    }


    IEnumerator spawn()
    {


        // select random spawn location
        shopSpawnLocation = Random.Range(0, shopSpawnLocations.Length - 1);


        // spawn shop
        instantiatedShopModel = Instantiate(shopModel, shopSpawnLocations[shopSpawnLocation].transform.position, shopSpawnLocations[shopSpawnLocation].transform.rotation);


        // broadcast message
        gameManager.instance.shopSpawnBroadcastParent.SetActive(true);


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
        gameManager.instance.shopSpawnBroadcastParent.SetActive(false);


        // update UI
        Interaction.playerInSphere = false;
        gameManager.instance.interactableTextParent.SetActive(false);


        isSpawning = false;
    }
}
