using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowCamera : MonoBehaviour
{
    public Transform target;
    public Vector3 offset;

    // Update is called once per frame
    /*
    void FixedUpdate()
    {
        // Follow the camera with set offset position
        transform.position = target.position + offset;

        // Rotate the origin on the y-axis corresponding to camera rotation
        transform.eulerAngles = new Vector3(0, target.eulerAngles.y, 0);
    }
    */
    void FixedUpdate()
    {
        // Follow the camera with set offset position
        transform.position = target.position + Vector3.up * offset.y
            + Vector3.ProjectOnPlane(target.right,Vector3.up).normalized * offset.x
            + Vector3.ProjectOnPlane(target.forward, Vector3.up).normalized * offset.z;

        // Rotate the origin on the y-axis corresponding to camera rotation
        transform.eulerAngles = new Vector3(0, target.eulerAngles.y, 0);
    }
}

