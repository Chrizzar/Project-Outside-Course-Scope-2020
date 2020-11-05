
/*
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;

/// <summary>
/// 
/// </summary>
public class HandPresence : MonoBehaviour
{
	// Show controller or/and hands
    public bool showController = false;
    public InputDeviceCharacteristics controllerCharacteristics;

    // List of controller prefabs
    public List<GameObject> controllerPrefabs;
    public GameObject handModelPrefab;

    private InputDevice targetDevice;
    private GameObject spawnedController;
    private GameObject spawnedHandModel;
    private Animator handAnimator;

    // Start is called before the first frame update
    void Start()
    {
        TryInitialize();
    }

    void TryInitialize()
    {
        List<InputDevice> devices = new List<InputDevice>();
        InputDevices.GetDevicesWithCharacteristics(controllerCharacteristics, devices);

        foreach (var item in devices)
        {
            Debug.Log(item.name + item.characteristics);
        }

        // Log devices if the user has higher than 0 devices (e.g. only a right controller, but no left)
        if (devices.Count > 0)
        {
            targetDevice = devices[0];

            // Search for a prefab that has the same property name as the device names
            GameObject prefab = controllerPrefabs.Find(controller => controller.name == targetDevice.name);
            
            // If we find a matching prefab name, spawn the prefab
            if (prefab)
            {
                spawnedController = Instantiate(prefab, transform);
            }
            // If the prefab is not among the list of prefab controller models, write an error message
            else
            {
                Debug.LogError("Did not find corresponding controller model");
                spawnedController = Instantiate(controllerPrefabs[0], transform);
            }

            // Spawn hand model
            spawnedHandModel = Instantiate(handModelPrefab, transform);
            handAnimator = spawnedHandModel.GetComponent<Animator>();
        }
    }

    // Get trigger value of the controller
    void UpdateHandAnimation()
    {
        // Running the fist animation if trigger is triggered
        if (targetDevice.TryGetFeatureValue(CommonUsages.trigger, out float triggerValue))
        {
            handAnimator.SetFloat("Trigger", triggerValue);
        }
        // When release the trigger, run the animation back to default hand
        else
        {
            handAnimator.SetFloat("Trigger", 0);
        }

        if (targetDevice.TryGetFeatureValue(CommonUsages.grip, out float gripValue))
        {
            handAnimator.SetFloat("Grip", gripValue);
        }
        else
        {
            handAnimator.SetFloat("Grip", 0);
        }
    }

    // Update is called once per frame
    void Update()
    {
        // Check if the devices are not on
        if (!targetDevice.isValid)
        {
            TryInitialize();
        }
        // If the devices are on, show the hand/controller
        else
        {
            if (showController)
            {
                spawnedHandModel.SetActive(false);
                spawnedController.SetActive(true);
            }
            else
            {
                spawnedHandModel.SetActive(true);
                spawnedController.SetActive(false);
                UpdateHandAnimation();
            }
        }
    }
}
*/


using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;

/// <summary>
/// 
/// </summary>
public class HandPresence : MonoBehaviour
{
	// Show controller or/and hands
    public bool showController = false;
    public InputDeviceCharacteristics controllerCharacteristics;

    // List of controller prefabs
    public List<GameObject> controllerPrefabs;
    public GameObject handModelPrefab;

    private InputDevice targetDevice;
	private GameObject spawnedController;
	private GameObject spawnedHandModel;
	private Animator handAnimator;


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

    GameObject chestOrigin = null;
	GameObject virtualHand = null;
	
	
    // Go-Go interaction technique variables
    public float armlength; // Length of arm = 0.6cm * 20
    float k = 1 + 0.99f; // Go-Go factor coefficient: 0 < k < 1. Adding 1.00 since we want to measure in meters [m]
    float D; // Set threshold
    public float gogo; 
	
    Vector3 chestOriginCoordinates;
    Vector3 vectorDistance;

    // Distance between chest origin and the virtual hand
    float distanceToOrigin;

    /// <summary>
    ///
    /// </summary>
    public void Start()
    {   
        // Find Chest interaction origin game object and the virtual hand
        chestOrigin = GameObject.Find("ChestOrigin");
		virtualHand = GameObject.Find("RightHand Controller");

		
        // Define threshold for linear/exponentiel mapping
        D = 0.66f * this.armlength; // use 2/3 of armlength

        TryInitialize();
    }

    void TryInitialize()
    {
        List<InputDevice> devices = new List<InputDevice>();
        InputDevices.GetDevicesWithCharacteristics(controllerCharacteristics, devices);

        foreach (var item in devices)
        {
            Debug.Log(item.name + item.characteristics);
        }

        // Log devices if the user has higher than 0 devices (e.g. only a right controller, but no left)
        if (devices.Count > 0)
        {
            targetDevice = devices[0];

            // Search for a prefab that has the same property name as the device names
            GameObject prefab = controllerPrefabs.Find(controller => controller.name == targetDevice.name);
            
            // If we find a matching prefab name, spawn the prefab
            if (prefab)
            {
                spawnedController = Instantiate(prefab, transform);                
            }
            // If the prefab is not among the list of prefab controller models, write an error message
            else
            {
                Debug.LogError("No corresponding controller model was found. Default controller is therefore added.");
                spawnedController = Instantiate(controllerPrefabs[0], transform);
            }

            // Spawn hand model
            spawnedHandModel = Instantiate(handModelPrefab, transform);
            handAnimator = spawnedHandModel.GetComponent<Animator>();
        }
    }

    // Get trigger value of the controller
    void UpdateHandAnimation()
    {
        // Running the fist animation if trigger is triggered
        if (targetDevice.TryGetFeatureValue(CommonUsages.trigger, out float triggerValue))
        {
            handAnimator.SetFloat("Trigger", triggerValue);
        }
        // When release the trigger, run the animation back to default hand
        else
        {
            handAnimator.SetFloat("Trigger", 0);
        }

        if (targetDevice.TryGetFeatureValue(CommonUsages.grip, out float gripValue))
        {
            handAnimator.SetFloat("Grip", gripValue);
        }
        else
        {
            handAnimator.SetFloat("Grip", 0);
        }
    }

    // Update is called once per frame
    void Update()
    {
        // Check if the devices are not on
        if (!targetDevice.isValid)
        {
            TryInitialize();
        }
        // If the devices are on, show the hand/controller
        else
        {
            if (showController)
            {
                spawnedHandModel.SetActive(false);
                spawnedController.SetActive(true);
            }
            else
            {
                spawnedHandModel.SetActive(true);
                spawnedController.SetActive(false);
                UpdateHandAnimation();
            }

            // ------------- Interaction technique implementation ------- //

            // Current interaction origin (the chest) coordinates 
            chestOriginCoordinates = chestOrigin.transform.position;
            //Debug.Log("Coordinates of the chest = " + chestOriginCoordinates);

            // Distance from chest origin to vitual hand, physical position
            distanceToOrigin = Vector3.Distance(virtualHand.transform.position, chestOriginCoordinates);
            Debug.Log("Distance To Origin from virtual hand = " + distanceToOrigin);

            // Distance vector between chest origin and virtual hand, physical position in WC
            vectorDistance = (virtualHand.transform.position - chestOriginCoordinates);

            // The Go-Go function
            if (distanceToOrigin > D)
            {
                // Use Exponential mapping
                // Mapping for: Rr + k(Rr - D)^2   otherwise
                gogo = k * Mathf.Pow((distanceToOrigin - D), 4.0f);
            }
            else
            {
                // Use Linear mapping
                // Mapping for: Rr   if Rr < D
                gogo = 0.0f;
            }
        
            // Update the transform for the virtual hand
            this.transform.position = virtualHand.transform.position + gogo * vectorDistance.normalized;
            this.transform.rotation = virtualHand.transform.rotation;
        }
    }
}
