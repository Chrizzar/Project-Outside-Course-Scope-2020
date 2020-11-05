using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// When colliding to a chosen gameobject, a mesh of another game object will change its material
public class CollisionExternalColorChange : MonoBehaviour
{
    public GameObject ObjectToCollideWith;
    //public GameObject ObjectChangeColor;
    [SerializeField] private Renderer ObjectChangeColor;
    public Material material;


    void OnCollisionEnter(Collision col)
    {
        if (col.gameObject == ObjectToCollideWith)
        {
            // If colliding with the ObjectToCollideWith, change the color of ObjectChangeColor.
            ObjectChangeColor.material = material;
        }
    }
}