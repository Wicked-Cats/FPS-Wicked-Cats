using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopSpawn : MonoBehaviour
{
    [SerializeField] private GameObject shopModel;
    [SerializeField] private GameObject[] shopSpawnLocations;

    private int shopSpawnLocation;

    // Start is called before the first frame update
    void Start()
    {
        shopSpawnLocations = GameObject.FindGameObjectsWithTag("Shop Spawn Pos");

        shopSpawnLocation = Random.Range(0, shopSpawnLocations.Length - 1);
        
        Instantiate(shopModel, shopSpawnLocations[shopSpawnLocation].transform.position, shopSpawnLocations[shopSpawnLocation].transform.rotation);
    }
}
