using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnObjects : MonoBehaviour
{
    public Transform Spawnpoint1;
    public GameObject Prefab1;
    public Transform Spawnpoint2;
    public GameObject Prefab2;

    void OnTriggerEnter()
    {
        Instantiate(Prefab1, Spawnpoint1.position, Spawnpoint1.rotation);
        Instantiate(Prefab2, Spawnpoint2.position, Spawnpoint2.rotation);
    }
}
