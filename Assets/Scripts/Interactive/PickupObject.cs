using UnityEngine;

public class PickupObject : InteractionObject
{
    bool _picked = false;

    string _dropName = "Drop";
    string _dropText = "Press E to drop";

    string _pickupName = "Pickup";
    string _pickupText = "Press E to pickup";

    Rigidbody _rb;

    protected override void Start()
    {
        base.Start();
        _rb = GetComponent<Rigidbody>();
    }

    public override bool CanInteract()
    {
        return !_picked && !_player.HasPickUp || _picked;
    }

    public void Pickup()
    {
        _interactionName = _dropName;
        _interactionText = _dropText;
        _picked = true;
        _interactions.RemoveListener(Pickup);
        _interactions.AddListener(Drop);
        _rb.isKinematic = true;
        _rb.useGravity = false;
        transform.localPosition = Vector3.zero;
        transform.rotation = Quaternion.identity;
    }

    public void Drop()
    {
        _rb.isKinematic = false;
        _rb.useGravity = true;
        _picked = false;
        _interactionName = _pickupName;
        _interactionText = _pickupText;
        _interactions.RemoveListener(Drop);
        _interactions.AddListener(Pickup);
    }
}
