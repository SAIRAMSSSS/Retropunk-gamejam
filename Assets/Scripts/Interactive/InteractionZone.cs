using UnityEngine;
using UnityEngine.Events;
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

    void Start()
    {
        _interactionUI = GameObject.Find("IneractionCanvas").GetComponent<InteractionUIManager>();
        _input = GameObject.Find("Player").GetComponent<PlayerInput>();
    }

    void OnTriggerEnter(Collider other)
    {
        InteractionObject newObj = other.gameObject.GetComponent<InteractionObject>();
        if (newObj.CanInteract() && (CurrentObject == null || CurrentObject != null && newObj.Prioroty > CurrentObject.Prioroty))
        {
            CurrentObject = newObj;
            ObjectDetected = true;
            _input.EnableInteraction(true);
            _interactionUI.Show(newObj.transform, newObj.InteractionText);
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.gameObject == CurrentObject.gameObject)
        {
            ObjectDetected = false;
            CurrentObject = null;
            _interactionUI.Hide();
            _input.EnableInteraction(false);
        }
    }
}
