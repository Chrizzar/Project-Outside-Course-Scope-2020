using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Quit : MonoBehaviour
{
    public void QuitExperiment()
    {
        Debug.Log("You have quit the experiment");
        Application.Quit();
    }
}
