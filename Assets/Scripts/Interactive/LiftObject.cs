using UnityEngine;
using UnityEngine.AI;

public class LiftObject : InteractionObject
{
    [SerializeField]
    int _objectNum;

    public int ObjectNum => _objectNum;

    public bool Picked { get; private set; }
    public string PlatformName => _placePlatform.name;

    string _dropName = "Drop";
    string _dropText = "Press E to drop an object";
    float _dropOffset = 0.2f;

    string _liftName = "Lift";
    string _liftText = "Press E to lift an object";
    float _liftOffset = 1.2f;

    Rigidbody _rb;
    NavMeshObstacle _obstacle;
    PlatformObject _placePlatform;

    Vector3 _scale;

    void Start()
    {
        _obstacle = GetComponent<NavMeshObstacle>();
        _rb = GetComponent<Rigidbody>();
        _scale = transform.lossyScale;
        _interaction.AddListener(Lift);
    }

    public override bool CanInteract()
    {
        return _canInteract && (!Picked && !_player.HasPickUp || Picked);
    }

    public void Lift()
    {
        //changes interaction text
        _interactionName = _dropName;
        _interactionText = _dropText;
        _interactonTextOffset = _dropOffset;
        //turns off physics
        Picked = true;
        _rb.isKinematic = true;
        _rb.useGravity = false;
        _obstacle.enabled = false;
        transform.SetLocalPositionAndRotation(Vector3.zero, Quaternion.identity);
        transform.localScale = _scale;
        //changes interaction event
        _interaction.RemoveListener(Lift);
        _interaction.AddListener(Drop);
    }

    public void Place()
    {
        //turns off physics
        _rb.isKinematic = true;
        _rb.useGravity = false;
        _obstacle.enabled = false;
        Picked = false;
        transform.SetLocalPositionAndRotation(Vector3.zero, Quaternion.identity);
        //changes interaction text
        _interactionName = _liftName;
        _interactionText = _liftText;
        _interactonTextOffset = _liftOffset;
        //changes interaction event
        _interaction.RemoveListener(Place);
        _interaction.AddListener(Lift);
    }

    public void SetScale(Vector3 parentScale)
    {
        transform.localScale = new Vector3(
            _scale.x / parentScale.x,
            _scale.y / parentScale.y,
            _scale.z / parentScale.z
        );
    }

    public void SetPlatform(PlatformObject platform)
    {
        _placePlatform = platform;
        SetScale(platform.transform.lossyScale);
    }

    public void Drop()
    {
        //turns on physics
        _rb.isKinematic = false;
        _rb.useGravity = true;
        _obstacle.enabled = true;
        Picked = false;
        //changes interaction text
        _interactionName = _liftName;
        _interactionText = _liftText;
        _interactonTextOffset = _liftOffset;
        //disconnects from the player
        transform.SetParent(null);
        transform.position = _player.transform.position + _player.transform.forward * 0.65f + Vector3.up * 0.06f;
        //changes interaction event
        _interaction.RemoveListener(Drop);
        _interaction.AddListener(Lift);
    }
}
