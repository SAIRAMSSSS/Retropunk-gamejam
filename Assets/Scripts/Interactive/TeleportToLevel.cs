using UnityEngine;
using Zenject;

public class TeleportToLevel : InteractionObject
{
    [SerializeField]
    int _levelNumber;

    [Inject]
    LevelManager _levelManager;

    /// <summary>
    /// Moves the player to the scene with a given name
    /// </summary>
    public void GoToLevel()
    {
        _levelManager.LoadLevel(_levelNumber);
    }
}
