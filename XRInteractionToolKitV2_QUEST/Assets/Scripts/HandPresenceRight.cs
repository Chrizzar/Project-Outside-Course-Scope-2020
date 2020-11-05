using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;

/// <summary>
/// 
/// </summary>
public class HandPresenceRight : MonoBehaviour
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