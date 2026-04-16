using System.Collections;
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

    readonly int _hashSpeed = Animator.StringToHash("Speed");
    readonly int _hashIdleNum = Animator.StringToHash("IdleNum");
    readonly int _hashSwitchIdle = Animator.StringToHash("SwitchIdle");
    readonly int _hashIdleAnimationEnded = Animator.StringToHash("IdleAnimationEnded");
    readonly int _hashLift = Animator.StringToHash("Lift");
    readonly int _hashDrop = Animator.StringToHash("Drop");
    readonly int _hashPlace = Animator.StringToHash("Place");
    readonly int _hashTurn = Animator.StringToHash("Turning");
    readonly int _hashTurnDirection = Animator.StringToHash("TurnDirection");
    readonly int _hashCanMove = Animator.StringToHash("CanMove");
    readonly int _hashPickup = Animator.StringToHash("Pickup");
    readonly int _hashLever = Animator.StringToHash("Lever");
    readonly int _hashConsole = Animator.StringToHash("Console");

    PlayerInput _input;
    NavMeshAgent _navAgent;
    Animator _animator;
    SFXController _SFXPlayer;

    public bool HasPickUp { get; private set; }
    public InteractionObject PickedUpObject { get; set; }

    readonly float _runSpeed = 6;
    readonly float _walkSpeed = 2.8f;
    readonly float _liftSpeed = 2f;
    readonly float _idleAnimationTime = 5f;
    readonly int _idleAnimationsNum = 3;
    readonly float _weightChangeTime = 5f;

    float _idleAnimationTimer;
    bool _canMove = true;
    float _upperBodyLayerWeight = 0;

    void Start()
    {
        _input = GetComponent<PlayerInput>();
        _animator = GetComponent<Animator>();
        _navAgent = GetComponent<NavMeshAgent>();
        _SFXPlayer = GetComponent<SFXController>();
    }

    void Update()
    {
        _animator.SetLayerWeight(1, Mathf.Lerp(_animator.GetLayerWeight(1), _upperBodyLayerWeight, Time.deltaTime * _weightChangeTime));
        _canMove = _animator.GetBool(_hashCanMove);
        //moves player
        if (_input.Click && _canMove)
        {
            Ray ray = Camera.main.ScreenPointToRay(_input.MousePosition);
            if (Physics.Raycast(ray, out RaycastHit hit, 100f, _movementLayer))
            {
                _navAgent.speed = _walkSpeed;
                _navAgent.SetDestination(hit.point);
            }
        }
        //sets running speed
        if (_input.DoubleClick && !HasPickUp)
        {
            _navAgent.speed = _runSpeed;
        }
        else if (HasPickUp)
        {
            _navAgent.speed = _liftSpeed;
        }
        //interaction with objects
        if (_interactionZone.ObjectDetected && _input.Interact
            && _canMove && _navAgent.velocity.magnitude < 0.1)
        {
            switch (_interactionZone.CurrentObject.InteractionName)
            {
                //lifts from the floop
                case "Lift":
                    PickedUpObject = _interactionZone.CurrentObject;
                    StartCoroutine(RotateTowardsInteractiveObject(() =>
                    {
                        _animator.SetTrigger(_hashLift);
                    }));
                    break;
                //picks up from a platform
                case "Pickup":
                    StartCoroutine(RotateTowardsInteractiveObject(() =>
                    {
                        _upperBodyLayerWeight = 1;
                        _animator.SetTrigger(_hashPickup);
                        _interactionZone.CurrentObject.PerformActions();
                        ConnectLiftObject();
                    }));
                    break;
                //drops on the floor
                case "Drop":
                    _animator.SetTrigger(_hashDrop);
                    break;
                //pulls a lever
                case "Lever":
                    StartCoroutine(RotateTowardsInteractiveObject(() =>
                    {
                        _animator.SetTrigger(_hashLever);
                        _interactionZone.CurrentObject.PerformActions();
                    }));
                    break;
                //places on a platform
                case "Place":
                    StartCoroutine(RotateTowardsInteractiveObject(() =>
                    {
                        _animator.SetTrigger(_hashPlace);
                        _interactionZone.CurrentObject.PerformActions();
                        HasPickUp = false;
                        PickedUpObject = null;
                        _upperBodyLayerWeight = 0f;
                    }));
                    break;
                case "Console":
                    StartCoroutine(RotateTowardsInteractiveObject(() =>
                    {
                        _animator.SetTrigger(_hashConsole);
                        _interactionZone.CurrentObject.PerformActions();
                    }));
                    break;
                default:
                    _interactionZone.CurrentObject.PerformActions();
                    break;
            }
        }
        //stops moving when at the destination 
        if (!_navAgent.pathPending
            && _navAgent.remainingDistance <= _navAgent.stoppingDistance)
        {
            if (!_navAgent.hasPath || _navAgent.velocity.magnitude < 0.1)
            {
                _navAgent.ResetPath();
            }
        }
        //sets different idle animations
        if (_navAgent.velocity.magnitude < 0.1 && _animator.GetBool(_hashIdleAnimationEnded) &&!HasPickUp)
        {
            _idleAnimationTimer += Time.deltaTime;
            if (_idleAnimationTimer > _idleAnimationTime)
            {
                _idleAnimationTimer = 0f;
                _animator.SetInteger(_hashIdleNum,UnityEngine.Random.Range(0, _idleAnimationsNum));
                _animator.SetTrigger(_hashSwitchIdle);
            }
        }
        else
        {
            _idleAnimationTimer = 0f;
        }

        _animator.SetFloat(_hashSpeed, _navAgent.velocity.magnitude);
    }
    /// <summary>
    /// Rotates towards an interactive object if interacts
    /// </summary>
    /// <returns></returns>
    IEnumerator RotateTowardsInteractiveObject(System.Action interaction)
    {
        Vector3 direction = (_interactionZone.CurrentObject.transform.position - transform.position).normalized;
        direction.y = 0;

        float angle = Quaternion.Angle(transform.rotation, Quaternion.LookRotation(direction));
        bool smallAngle = angle < 15f;
        //if an angle is lower than 15 degrees - doesn't turn on the rotation animation
        if (smallAngle)
        {
            interaction.Invoke();
        }
        else
        {
            _animator.SetBool(_hashTurn, true);
        }
        //turning...
        while (angle >= 5f)
        {
            float turnDirection = Vector3.SignedAngle(transform.forward, direction, Vector3.up);
            _animator.SetFloat(_hashTurnDirection, Mathf.Sign(turnDirection));

            transform.rotation = Quaternion.RotateTowards(transform.rotation,
            Quaternion.LookRotation(direction),
            360f * Time.deltaTime);
            angle = Quaternion.Angle(transform.rotation, Quaternion.LookRotation(direction));
            yield return null;
        }
        //triggers the lift animation anfter the rotation animation
        if (!smallAngle)
        {
            _animator.SetBool(_hashTurn, false);
            interaction.Invoke();
        }
    }

    public void ConnectLiftObject()
    {
        HasPickUp = true;
        PickedUpObject.transform.SetParent(_pickupPoint);
        PickedUpObject.PerformActions();
    }

    public void TurnOnUpperLayer()
    {
        _upperBodyLayerWeight = 1;
        _animator.SetTrigger(_hashPickup);
    }

    public void TurnOffUpperLayer()
    {
        _upperBodyLayerWeight = 0;
    }

    public void DisconnectLiftObject()
    {
        HasPickUp = false;
        PickedUpObject.PerformActions();
        PickedUpObject = null;
        _upperBodyLayerWeight = 0f;
    }

    public void Interact()
    {
        _interactionZone.CurrentObject?.PerformActions();
    }

    public void PlayFootstep()
    {
        _SFXPlayer.PlaySound($"Metal - Option_{Random.Range(1, 3)}");
    }
}
