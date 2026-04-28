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

    void Start()
    {
        _interaction.AddListener(Place);
    }

    public override bool CanInteract()
    {
        return _canInteract && (_player.HasPickUp && !_placed || !_player.HasPickUp && _placed);
    }

    public void PlaceObject(LiftObject obj)
    {
        //connects a lift object to the platform
        _placed = true;
        _placedObject = obj;
        obj.PerformActions();
        obj.transform.SetParent(_placePoint);
        obj.SetPlatform(this);
        //changes interaction name
        _interactionName = _pickupName;
        _interactionText = _pickupText;
        _interactonTextOffset = _pickupOffset;
        _interactionUI.Show(transform, _interactionText, _pickupOffset);
        //changes interaction event
        _interaction.RemoveListener(Place);
        _interaction.AddListener(PickUp);
    }

    public void Place()
    {
        PlaceObject(_player.PickedUpObject as LiftObject);
    }

    public void PickUp()
    {
        //changes interaction name
        _interactionName = _placeName;
        _interactionText = _placeText;
        _interactonTextOffset = _placeOffset;
        _interactionUI.Show(transform, _interactionText,_placeOffset);
        //disconnects a lift object
        _placed = false;
        _player.PickedUpObject = _placedObject;
        //changes interaction event
        _interaction.RemoveListener(PickUp);
        _interaction.AddListener(Place);
    }
    /// <summary>
    /// Lowers the platform after the puzzle is completed
    /// </summary>
    public void LowerPlatform()
    {
        transform.position = new Vector3(transform.position.x, 0.2f, transform.position.z);
        _canInteract = false;
        _placedObject.DisableInteraction();
    }
    //if the object on the platform is correct
    public bool PlatformObjectMatch() => _platformNum == (_placedObject as LiftObject).ObjectNum;
}
