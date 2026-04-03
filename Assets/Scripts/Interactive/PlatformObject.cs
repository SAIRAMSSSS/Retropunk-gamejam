using UnityEngine;
using Zenject;

public class PlatformObject : InteractionObject
{
    [SerializeField]
    Transform _placePoint;
    [SerializeField]
    int _platformNum;

    public bool Placed => _placed;

    bool _placed = false;
    InteractionObject _placedObject;

    string _placeName = "Place";
    string _placeText = "Press E to place on the platform";
    float _placeOffset = 1.2f;

    string _pickupName = "Pickup";
    string _pickupText = "Press E to pick up from the platform";
    float _pickupOffset = 1.5f;

    [Inject]
    InteractionUIManager _interactionUI;

    protected override void Start()
    {
        base.Start();
        _interactionUI = GameObject.Find("IneractionCanvas").GetComponent<InteractionUIManager>();
        _interaction = Place;
    }

    public override bool CanInteract()
    {
        return _canInteract && (_player.HasPickUp && !_placed || !_player.HasPickUp && _placed);
    }

    public void Place()
    {
        _player.PickedUpObject.PerformActions();
        _player.PickedUpObject.transform.SetParent(_placePoint);
        (_player.PickedUpObject as LiftObject).Place();
        (_player.PickedUpObject as LiftObject).SetScale(transform.lossyScale);
        _interactionName = _pickupName;
        _interactionText = _pickupText;
        _interactonTextOffset = _pickupOffset;
        _interactionUI.Show(transform, _interactionText,_pickupOffset);
        _placed = true;
        _placedObject = _player.PickedUpObject;
        _interaction = PickUp;
    }

    public void PickUp()
    {
        _interactionName = _placeName;
        _interactionText = _placeText;
        _interactonTextOffset = _placeOffset;
        _interactionUI.Show(transform, _interactionText,_placeOffset);
        _placed = false;
        _player.PickedUpObject = _placedObject;
        _interaction = Place;
    }

    public void LowerPlatform()
    {
        transform.position = new Vector3(transform.position.x, 0.2f, transform.position.z);
        _canInteract = false;
        _placedObject.DisableInteraction();
    }

    public bool PlatformObjectMatch() => _platformNum == (_placedObject as LiftObject).ObjectNum;
}
