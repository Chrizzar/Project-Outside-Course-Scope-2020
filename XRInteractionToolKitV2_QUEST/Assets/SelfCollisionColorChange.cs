using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Change color of self, from colliding to a chosen gameobject
public class SelfCollisionColorChange : MonoBehaviour
{
    public GameObject ObjectToCollideWith;
    public GameObject ObjectChangeColor;
    public Material[] material;
    Renderer render;

    void Start()
    {
        render = GetComponent<Renderer>();
        render.enabled = true;
        render.sharedMaterial = material[0];
    }

    void OnCollisionEnter(Collision col)
    {
        if (col.gameObject == ObjectToCollideWith)
        {
            // If colliding with the gameobject, destroy it.
            //Destroy(col.gameObject);

            // If colliding with the ObjectToCollideWith, change the color of ObjectChangeColor.
            render.sharedMaterial = material[1];
        }
    }
}
