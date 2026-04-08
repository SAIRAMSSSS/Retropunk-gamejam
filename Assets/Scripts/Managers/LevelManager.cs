using System.Collections;
using UnityEngine;
using Zenject;

public class LevelManager : MonoBehaviour
{
    [SerializeField]
    GameObject[] _levels;

    [Inject]
    UIManager _UI;
    [Inject]
    PlayerInput _player;
    [Inject]
    DiContainer _container;

    GameObject _currentLevel;
    public int CurrentLevelIndex { get; private set; }

    private void Start()
    {
        _currentLevel = _container.InstantiatePrefab(_levels[0]);
    }

    public void SetNewLevel(int levelIndex)
    {
        if (_currentLevel != null)
        {
            Destroy(_currentLevel);
        }
        _currentLevel = _container.InstantiatePrefab(_levels[levelIndex]);
        if(CurrentLevelIndex == 6)
        {
            _currentLevel.GetComponentInChildren<PlayerSpawner>().SetPlayerSpawnPosition(CurrentLevelIndex);
        }
    }
    /// <summary>
    /// Darkens the screen, loads a new scene, and removes the blackout
    /// </summary>
    /// <param name="levelIndex"></param>
    /// <returns></returns>
    public IEnumerator LoadLevel(int levelIndex)
    {
        _player.LockInput(true);
        yield return StartCoroutine(_UI.DarkenScreen(0, 1));
        SetNewLevel(levelIndex);
        yield return StartCoroutine(_UI.DarkenScreen(1, 0));
        _player.LockInput(false);
    }
    /// <summary>
    /// Loads new scene and plays the cutscene
    /// </summary>
    /// <param name="levelIndex"></param>
    /// <param name="cutsceneName"></param>
    public void LoadLevelWithCutscene(int levelIndex, string cutsceneName)
    {
        SetNewLevel(levelIndex);
        var cutscene = _currentLevel.GetComponentInChildren<CutsceneManager>();
        cutscene.SetTimeline(cutsceneName);
        cutscene.StartCutscene();
    }
}
