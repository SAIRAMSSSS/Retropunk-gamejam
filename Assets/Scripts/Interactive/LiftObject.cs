using UnityEngine;
using UnityEngine.AI;

public class LiftObject : InteractionObject
{
    [SerializeField]
    int _objectNum;

    public int ObjectNum => _objectNum;

    bool _picked = false;

    string _dropName = "Drop";
    string _dropText = "Press E to drop an object";
    float _dropOffset = 0.2f;

    string _liftName = "Lift";
    string _liftText = "Press E to lift an object";
    float _liftOffset = 1.2f;

    Rigidbody _rb;
    NavMeshObstacle _obstacle;

    Vector3 _scale;

    protected override void Start()
    {
        base.Start();
        _obstacle = GetComponent<NavMeshObstacle>();
        _rb = GetComponent<Rigidbody>();
        _interaction = Lift;
        _scale = transform.lossyScale;
    }

    public override bool CanInteract()
    {
        return _canInteract && (!_picked && !_player.HasPickUp || _picked);
    }

    public void Lift()
    {
        _interactionName = _dropName;
        _interactionText = _dropText;
        _interactonTextOffset = _dropOffset;
        _picked = true;
        _interaction = Drop;
        _rb.isKinematic = true;
        _rb.useGravity = false;
        _obstacle.enabled = false;
        transform.SetLocalPositionAndRotation(Vector3.zero, Quaternion.identity);
        transform.localScale = _scale;
    }

    public void Place()
    {
        _rb.isKinematic = true;
        _rb.useGravity = false;
        _obstacle.enabled = false;
        _picked = false;
        _interactionName = _liftName;
        _interactionText = _liftText;
        _interactonTextOffset = _liftOffset;
        transform.SetLocalPositionAndRotation(Vector3.zero, Quaternion.identity);
        _interaction = Lift;
    }

    public void SetScale(Vector3 parentScale)
    {
        transform.localScale = new Vector3(
            _scale.x / parentScale.x,
            _scale.y / parentScale.y,
            _scale.z / parentScale.z
        );
    }

    public void Drop()
    {
        _rb.isKinematic = false;
        _rb.useGravity = true;
        _obstacle.enabled = true;
        _picked = false;
        _interactionName = _liftName;
        _interactionText = _liftText;
        _interactonTextOffset = _liftOffset;
        _interaction = Lift;
        transform.SetParent(null);
        transform.position = _player.transform.position + _player.transform.forward * 0.65f + Vector3.up * 0.06f;
    }
}
