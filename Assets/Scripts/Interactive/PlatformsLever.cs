using System.Linq;
using UnityEngine;
using Zenject;

public class PlatformsLever : InteractionObject
{
    [SerializeField]
    PlatformObject[] _platforms;

    [Inject]
    GameManager _gameManager;

    public override bool CanInteract()
    {
        return _canInteract && (_platforms.All(p => p.Placed) && !_player.HasPickUp);
    }
    /// <summary>
    /// Checks if all objects on platforms are correct
    /// </summary>
    public void Interact()
    {
        if(_platforms.All(p=> p.PlatformObjectMatch()))
        {
            foreach (var platform in _platforms)
            {
                platform.LowerPlatform();
                _canInteract = false;
                _gameManager.CompleteRoom(0);
            }
        }
    }
}
