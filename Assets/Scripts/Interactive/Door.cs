using UnityEngine;
using Zenject;

public class Door : InteractionObject
{
    [SerializeField]
    LevelNames _levelName;

    [Inject]
    LevelManager _levelManager;

    /// <summary>
    /// Moves the player to the scene with a given name
    /// </summary>
    public void GoToLevel()
    {
        _levelManager.LoadLevel(_levelName);
    }
}
