using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class SpawnFarmost : MonoBehaviour
{
    public GameObject ifHided;
    Renderer[] listOfChildren;
    
    void Start()
    {
        listOfChildren = GetComponentsInChildren<Renderer>();
        foreach(Renderer child in listOfChildren)
        {
            child.enabled = false;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (ifHided.active == false)
        {
            listOfChildren = GetComponentsInChildren<Renderer>();
            foreach(Renderer child in listOfChildren)
            {
                child.enabled = true;
            }
        }
    }
}

