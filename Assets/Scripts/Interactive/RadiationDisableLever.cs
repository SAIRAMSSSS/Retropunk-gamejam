using UnityEngine;
using Zenject;

public class RadiationDisableLever : InteractionObject
{
    [SerializeField]
    RadioactivePanel[] _panels;

    [Inject]
    GameManager _gameManager;
    /// <summary>
    /// Disables radioactive panels after using the lever
    /// </summary>
    public void DisableRadiation()
    {
        foreach (var panel in _panels)
        {
            panel.enabled = false;
            _canInteract = false;
            _gameManager.CompleteRoom(3);
        }
    }
}
