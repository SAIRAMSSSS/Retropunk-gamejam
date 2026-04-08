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
        Debug.Log($"Place called. Listeners before: {_interaction?.GetPersistentEventCount()}");

        _interaction.RemoveListener(Place);
        Debug.Log($"After RemoveListener(Place): {_interaction?.GetPersistentEventCount()}");

        _interaction.AddListener(PickUp);
        Debug.Log($"After AddListener(PickUp): {_interaction?.GetPersistentEventCount()}");
    }

    public void PickUp()
    {
        _interactionName = _placeName;
        _interactionText = _placeText;
        _interactonTextOffset = _placeOffset;
        _interactionUI.Show(transform, _interactionText,_placeOffset);
        _placed = false;
        _player.PickedUpObject = _placedObject;
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
