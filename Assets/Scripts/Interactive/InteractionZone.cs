using UnityEngine;
using Zenject;

[RequireComponent(typeof(Collider))]
public class InteractionZone : MonoBehaviour
{
    public bool ObjectDetected { get; private set; }
    public InteractionObject CurrentObject { get; private set; }

    [Inject]
    InteractionUIManager _interactionUI;
    [Inject]
    PlayerInput _input;

    void OnTriggerEnter(Collider other)
    {
        InteractionObject newObj = other.gameObject.GetComponent<InteractionObject>();
        if (newObj.CanInteract() && (CurrentObject == null || CurrentObject != null && newObj.Prioroty > CurrentObject.Prioroty))
        {
            CurrentObject = newObj;
            ObjectDetected = true;
            _input.EnableInteraction(true);
            _interactionUI.Show(newObj.transform, newObj.InteractionText,newObj.InteractionOffset);
        }
    }

    private void OnTriggerStay(Collider other)
    {
        InteractionObject newObj = other.gameObject.GetComponent<InteractionObject>();
        if (newObj.CanInteract() && CurrentObject == null)
        {
            CurrentObject = newObj;
            ObjectDetected = true;
            _input.EnableInteraction(true);
            _interactionUI.Show(newObj.transform, newObj.InteractionText, newObj.InteractionOffset);
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (CurrentObject != null && other.gameObject == CurrentObject.gameObject)
        {
            ObjectDetected = false;
            CurrentObject = null;
            _interactionUI.Hide();
            _input.EnableInteraction(false);
        }
    }
}
