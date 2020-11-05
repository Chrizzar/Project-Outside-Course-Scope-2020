using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PressButtonVertical : MonoBehaviour
{
    public GameObject hideObjects;

    [SerializeField] private Renderer ObjectChangeColor;
    public Material material;

    private AudioSource source;
    public AudioClip buttonSound;

    [System.Serializable]
    public class ButtonEvent : UnityEvent { }

    public float pressLength;
    public bool pressed;
    public ButtonEvent downEvent;
    public ButtonEvent ReleaseEvent;

    Vector3 startPos;
    Rigidbody rb;

    IEnumerator ActivateHide()
    {        
        //Wait for 3 secs.
        yield return new WaitForSeconds(3);

        //Game object will turn off
        hideObjects.SetActive(false);
    }

    void Start()
    {
        source = gameObject.AddComponent<AudioSource>();

        startPos = transform.position;
        rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        // If our distance is greater than what we specified as a press
        // set it to our max distance and register a press if we haven't already
        float distance = Mathf.Abs(transform.position.y - startPos.y);
        if (distance >= pressLength)
        {
            // Prevent the button from going past the pressLength
            transform.position = new Vector3(transform.position.x, startPos.y - pressLength, transform.position.z);
            if (!pressed)
            {
                source.PlayOneShot(buttonSound);

                pressed = true;
                // If we have an event, invoke it
                downEvent?.Invoke();

                ObjectChangeColor.material = material;

                
                //Destroy(destroyObjects, 3f);
                StartCoroutine(ActivateHide());
            }
        } 
        else
        {
            // If we aren't all the way down, reset our press
            pressed = false;
            // If we have an event, invoke it
            ReleaseEvent?.Invoke();
        }
        // Prevent button from springing back up past its original position
        if (transform.position.y > startPos.y)
        {
            transform.position = new Vector3(transform.position.x, startPos.y, transform.position.z);
        }
    }
}