using UnityEngine;

public class PlaceObject : InteractionObject
{
    public override bool CanInteract()
    {
        return _player.HasPickUp;
    }

}
