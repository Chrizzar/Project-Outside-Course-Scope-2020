using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

// Changing the XR Grab Interactable function, such that the attach point of the hand can be placed wherever the object is,
// instead of that the interactable object moves towards to the attach point on the hand.
public class OffsetGrab : XRGrabInteractable
{
    // Current positions of our interactor, i.e. the attach point of the original position and rotation.
    private Vector3 interactorPosition = Vector3.zero;
    private Quaternion interactorRotation = Quaternion.identity;

    protected override void OnSelectEnter(XRBaseInteractor interactor)
    {
        // Calling the base of this function, to make sure that we are running all the functionality for the grabbing logic.
        // If we do not call it, nothing is going to work.
        // Calling them in order they are going to be used (see below).
        base.OnSelectEnter(interactor);
        StoreInteractor(interactor);
        MatchAttachmentPoints(interactor);
    }

    // Store the interactor (i.e. the hand) that is currently being passed in 
    private void StoreInteractor(XRBaseInteractor interactor)
    {
        // Getting and storing the original local position and rotation of the attachment
        // point of the interactor
        interactorPosition = interactor.attachTransform.localPosition;
        interactorRotation = interactor.attachTransform.localRotation;
    }

    // Move the attachment point of the interactor (i.e. the hand) to that of the interactable object
    private void MatchAttachmentPoints(XRBaseInteractor interactor)
    {
        // Check if the current object has an attach point.
        // If does not have an attach point, we will use its transform (not every object in the scene have a
        // specific attach point, that the user is gonna need to grab unto).
        bool hasAttach = attachTransform != null;

        // Moving the attach transform of the hand to that of the interactable object.
        // If we have an attachment point, we will use its position and rotation. 
        // If not, we will use the transformation position and rotation of the grabbable object itself.
        interactor.attachTransform.position = hasAttach ? attachTransform.position : transform.position; 
        interactor.attachTransform.rotation = hasAttach ? attachTransform.rotation : transform.rotation;
    }

    // When the user drops the object, do the opposite
    protected override void OnSelectExit(XRBaseInteractor interactor)
    {
        // Calling the base of this function.
        base.OnSelectExit(interactor);
        ResetAttachmentPoint(interactor); // Reset the attach point the interactor (i.e. hand).
        ClearInteractor(interactor);      // Clears everything out
    }

    // Return the interactors transform back to where it was previously, for both its position and rotation.
    private void ResetAttachmentPoint(XRBaseInteractor interactor)
    {
        interactor.attachTransform.localPosition = interactorPosition;
        interactor.attachTransform.localRotation = interactorRotation;
    }

    // This handles the values, when where is no interactions happening 
    private void ClearInteractor(XRBaseInteractor interactor)
    {
        interactorPosition = Vector3.zero;
        interactorRotation = Quaternion.identity;
    }
}