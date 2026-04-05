using ModestTree;
using UnityEngine;
using Zenject;

public class PlayerSpawner : MonoBehaviour
{
    [SerializeField]
    string[] _sceneNames;
    [Inject]
    PlayerInput _player;

    void Start()
    {
        SetSpawnPoint(transform);
    }
    /// <summary>
    /// Sets a player position when he exits a certain scene
    /// </summary>
    /// <param name="fromScene"></param>
    public void SetPlayerSpawnPosition(string fromScene)
    {
        Transform spawnPoint = transform.GetChild(_sceneNames.IndexOf(fromScene));
        SetSpawnPoint(spawnPoint);
    }

    void SetSpawnPoint(Transform spawn)
    {
        _player.transform.position = spawn.position;
        _player.transform.rotation = spawn.rotation;
    }
}
