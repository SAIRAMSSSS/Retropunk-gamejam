using System.Collections;
using UnityEngine;
using Zenject;

public class LevelManager : MonoBehaviour
{
    [SerializeField]
    GameObject[] _levels;

    [Inject]
    GameManager _gameManager;
    [Inject]
    UIManager _UI;
    [Inject]
    PlayerInput _player;
    [Inject]
    DiContainer _container;

    GameObject _currentLevel;
    int _currentLevelIndex;

    readonly string _endCutsceneName = "Ending_MainDrag";
    readonly int _mainDragIndex = 6;

    private void Start()
    {
        _currentLevel = _container.InstantiatePrefab(_levels[1]);
    }

    public void SetNewLevel(int levelIndex)
    {
        if (_currentLevel != null)
        {
            Destroy(_currentLevel);
        }
        _currentLevel = _container.InstantiatePrefab(_levels[levelIndex]);
        if(levelIndex == _mainDragIndex)
        {
            _currentLevel.GetComponentInChildren<PlayerSpawner>().SetPlayerSpawnPosition(_currentLevelIndex);
        }
        _currentLevelIndex = levelIndex;
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
        if(levelIndex == _mainDragIndex && _gameManager.AllPuzzlesCompleted)
        {
            PlayCutscene(_endCutsceneName);
        }
    }
    /// <summary>
    /// Loads new scene and plays the cutscene
    /// </summary>
    /// <param name="levelIndex"></param>
    /// <param name="cutsceneName"></param>
    public void LoadLevelWithCutscene(int levelIndex, string cutsceneName)
    {
        SetNewLevel(levelIndex);
        PlayCutscene(cutsceneName);
    }

    public CutsceneManager GetCurrentCutscene()=> _currentLevel.GetComponentInChildren<CutsceneManager>();

    void PlayCutscene(string cutsceneName)
    {
        var cutscene = GetCurrentCutscene();
        cutscene.SetTimeline(cutsceneName);
        cutscene.StartCutscene();
    }
}
