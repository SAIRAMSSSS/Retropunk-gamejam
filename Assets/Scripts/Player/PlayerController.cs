using UnityEngine;
using UnityEngine.AI;

public class PlayerController : MonoBehaviour
{
    [SerializeField]
    InteractionZone _interactionZone;
    [SerializeField]
    Transform _pickupPoint;
    [SerializeField]
    LayerMask _movementLayer;

    PlayerInput _input;
    NavMeshAgent _navAgent;
    Animator _animator;

    public bool HasPickUp { get; private set; }

    void Start()
    {
        _input = GetComponent<PlayerInput>();
        _animator = GetComponent<Animator>();
        _navAgent = GetComponent<NavMeshAgent>();
    }

    void Update()
    {
        //moves player
        if (_input.Click)
        {
            Ray ray = Camera.main.ScreenPointToRay(_input.MousePosition);
            if(Physics.Raycast(ray, out RaycastHit hit, 100f, _movementLayer))
            {
                _navAgent.SetDestination(hit.point);
            }
        }
        //interaction with objects
        if(_interactionZone.ObjectDetected && _input.Interact)
        {
            switch (_interactionZone.CurrentObject.InteractionName)
            {
                case "Pickup":
                    HasPickUp = true;
                    _interactionZone.CurrentObject.transform.SetParent(_pickupPoint);
                    _interactionZone.CurrentObject.PerformActions();
                    break;
                case "Drop":
                    HasPickUp = false;
                    _interactionZone.CurrentObject.PerformActions();
                    _interactionZone.CurrentObject.transform.SetParent(null);
                    break;
                case "Lever":

                    break;
                case "Place":

                    break;
            }
        }
    }
}
