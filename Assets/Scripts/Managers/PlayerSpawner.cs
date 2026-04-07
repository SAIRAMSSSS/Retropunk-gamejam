using ModestTree;
using UnityEngine;
using Zenject;

public class PlayerSpawner : MonoBehaviour
{
    [Inject]
    PlayerInput _player;

    void Start()
    {
        SetSpawnPoint(transform);
    }
    /// <summary>
    /// Sets a player position when they exit a certain level
    /// </summary>
    public void SetPlayerSpawnPosition(int fromLevelNum)
    {
        Transform spawnPoint = transform.GetChild(fromLevelNum - 1);
        SetSpawnPoint(spawnPoint);
    }

    void SetSpawnPoint(Transform spawn)
    {
        _player.transform.position = spawn.position;
        _player.transform.rotation = spawn.rotation;
    }
}
