/* ************************************ GO-GO-Technique 1 (Working) ************************************ */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;

public class FirstTechnique : MonoBehaviour
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

    // Initialize gameobjects and variables
    GameObject virtualHand = null;
    GameObject controller = null;
	GameObject chestOrigin = null;

	private float armLenght = 0.0f; // Length of arm
	private float thresholdD = 0.0f; // Threshold for linear/exponentiel mapping
	private float gogoFactorK = 0.0f; // Go-Go factor coefficient.

    // Go-Go Function
    public float gogo;

    public void Start()
    {
        // Find the virtual hand, the controller and the chest interaction origin game object
        virtualHand = GameObject.Find("Hand Presence Right");
        controller = GameObject.Find ("RightHand Controller");
		chestOrigin = GameObject.Find ("ChestOrigin");

        armLenght = StaticArmsLength.InheritarmLenght;
        //Debug.Log("Arm length T1 = " + armLenght);
		thresholdD = 0.25f * this.armLenght; // default 2/3 of arm lenght 
		gogoFactorK = 21.0f; // 0 < k < 21
		
		Debug.Log("Start Go-Go Technique 1");

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

            // Apply The Go-Go Interaction Technique:

            // Distance between the coordinates of chest origin and the controller
            float distanceToOrigin = Vector3.Distance(controller.transform.position, chestOrigin.transform.position);
            //Debug.Log("distanceToOrigin = " + distanceToOrigin);

            // Distance vector between chest origin and virtual hand
		    Vector3 vectorDistance = (controller.transform.position - chestOrigin.transform.position);

            // The Go-Go-function F
            if (distanceToOrigin > this.thresholdD)
            {
                // Use Exponential mapping
                // Mapping for: Rr + k(Rr - D)^4   otherwise
                gogo = gogoFactorK * Mathf.Pow((distanceToOrigin - thresholdD), 4.0f);
            }
            else
            {
                // Use Linear mapping
                // Mapping for: Rr   if Rr < D
                gogo = 0.0f;
            }
            
            // Updating the transform of the virtual hand
            virtualHand.transform.position = controller.transform.position + (gogo * vectorDistance.normalized);
            virtualHand.transform.rotation = controller.transform.rotation;
        }
    }
}
