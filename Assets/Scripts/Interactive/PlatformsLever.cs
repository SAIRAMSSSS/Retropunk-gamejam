using System.Linq;
using UnityEngine;

public class PlatformsLever : InteractionObject
{
    [SerializeField]
    PlatformObject[] _platforms;

    public override bool CanInteract()
    {
        return _canInteract && (_platforms.All(p => p.Placed) && !_player.HasPickUp);
    }

    public void Interact()
    {
        if(_platforms.All(p=> p.PlatformObjectMatch()))
        {
            foreach (var platform in _platforms)
            {
                platform.LowerPlatform();
                _canInteract = false;
            }
        }
    }
}
