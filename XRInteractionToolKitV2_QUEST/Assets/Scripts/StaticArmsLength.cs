using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StaticArmsLength : MonoBehaviour
{
    //public float InheritarmLenght = 0.70f;
    public static float InheritarmLenght;// = 0.70f;
    public GameObject inputField;
    public GameObject textDisplay;

    public void StoreVariable()
    {
        InheritarmLenght = float.Parse(inputField.GetComponent<Text>().text);
        textDisplay.GetComponent<Text>().text = "You Have Set Armlength to " + InheritarmLenght + " meters!";
    }
}