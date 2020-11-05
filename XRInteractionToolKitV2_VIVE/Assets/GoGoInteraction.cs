/*using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;

using System.IO;
using System;

public class GoGoInteraction : MonoBehaviour
{
    // ************************** Go-Go Interaction Algorithm ******************************* //
    // Rr = length of vector                                                                  //
    // (Rr pointing from origin (at the chest) to the user's hand).                           //
    //                                                                                        //
    // Virtual hand position = {Rv, Φ, θ}                                                     //
    // (Rr pointing from origin (at the chest) to the user's hand, and Φ. θ is its rotation). //
    //                                                                                        //
    // Rv = length of the virtual arm                                                         //
    // (Rv = vector from origin (at the chest) to the virtual hand).                          //
    // ************************************************************************************** //

    
    public GameObject chestOrigin;
    public GameObject virtualHand;

    // Go-Go interaction technique variables
    float k = 1 + 0.99f; // Go-Go factor coefficient: 0 < k < 1. Adding 1.00 since we want to measure in meters [m]
    float d; // Set threshold

    Vector3 chestOriginCoordinates;
    Vector3 vectorDistance;

    float distanceToOrigin; // Distance between chest origin and the virtual hand
    

    // Start is called before the first frame update
    void Start()
    {
        chestOrigin = GameObject.Find("ChestOrigin");
        virtualHand = GameObject.Find("RightHand Controller");
    }

    // Update is called once per frame
    void Update()
    {
        distanceToOrigin = Vector3.Distance(virtualHand.transform.position, chestOrigin.transform.position);
        Debug.Log("Distance To Origin from virtual hand = " + distanceToOrigin);
    }
}
*/