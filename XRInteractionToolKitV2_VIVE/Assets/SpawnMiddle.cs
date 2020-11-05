/*
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class SpawnMiddle : MonoBehaviour
{
    public GameObject ifHided;
    //public GameObject Prefab;
    //public Transform Spawnpoint;
    public GameObject itSelf;
    
    //private bool isCreated = false;

    private void FindChildren()
    {
        Transform[] allChildren = GetComponentsInChildren<Transform>();
        
        if (ifHided.active == true)
        {
            foreach (Transform child in allChildren)
            {
                child.gameObject.SetActive(false);
            }
        }
        else
        {
            itSelf.SetActive(true);
            
            foreach (Transform child in allChildren)
            {
                child.gameObject.SetActive(true);
            }
            
        }
        
    }

    void Start()
    {
        //FindChildren();
    }

    // Update is called once per frame
    void Update()
    {
        if(!isCreated)
        {
            //Instantiate(PlusOneLifePreFab,Vector3(0,2.5,75.6),Quaternion.identity);
            isCreated = true;
        }
        if (IfDestroyed == null)
        {
           Instantiate(Prefab, Spawnpoint.position, Spawnpoint.rotation); 
        }
        if (!ifHided.active)
        {
            Transform[] allChildren = GetComponentsInChildren<Transform>();
            foreach (Transform child in allChildren)
            {
                child.gameObject.SetActive(true);
            }
        }
        
        //Transform[] allChildren = GetComponentsInChildren<Transform>();
        //FindChildren();

        
    }
}
*/


using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class SpawnMiddle : MonoBehaviour
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
