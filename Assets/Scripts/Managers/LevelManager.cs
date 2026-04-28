using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using Zenject;

public enum LevelNames
{
    Apartment,
    EngineRoom,
    LifeSupportRoom,
    WaterSupplyRoom, 
    RadiationShieldingMatrix,
    MainControlRoom,
    MainDrag
}

public class LevelManager : MonoBehaviour
{
    [Inject]
    UIManager _UI;
    [Inject]
    AudioManager _audioManager;
    [Inject]
    PlayerInput _player;
    [Inject]
    DiContainer _container;

    LevelNames _currentLevel;

    //readonly string _endCutsceneName = "Ending_MainDrag";

    public void SetNewLevel(LevelNames levelName)
    {
        SceneManager.LoadScene(levelName.ToString());
        if(LevelNames.MainDrag == levelName)
        {
            _container.Resolve<PlayerSpawner>().SetPlayerSpawnPosition((int)_currentLevel);
        }
        _currentLevel = levelName;
    }
    /// <summary>
    /// Darkens the screen, loads a new scene, and removes the blackout
    /// </summary>
    /// <param name="levelName"></param>
    /// <returns></returns>
    public IEnumerator LoadLevel(LevelNames levelName)
    {
        _player.LockInput(true);
        yield return StartCoroutine(_UI.DarkenScreen(1));
        SetNewLevel(levelName);
        yield return null;
        StartCoroutine(_audioManager.StartNewLevelMusic((int)levelName));
        yield return StartCoroutine(_UI.DarkenScreen(0));
        _player.LockInput(false);
        //if(levelIndex == _mainDragIndex && _gameManager.AllPuzzlesCompleted)
        //{
        //    PlayCutscene(_endCutsceneName);
        //}
    }
    /// <summary>
    /// Loads new scene and plays the cutscene
    /// </summary>
    /// <param name="levelIndex"></param>
    /// <param name="cutsceneName"></param>
    public void LoadLevelWithCutscene(int levelIndex, string cutsceneName)
    {
        SetNewLevel((LevelNames)levelIndex);
        PlayCutscene(cutsceneName);
    }

    public T GetCurrentCutscene<T>()=> _container.Resolve<CutsceneManager>().GetComponent<T>();

    public void PlayCutscene(string cutsceneName)
    {
        var cutscene = GetCurrentCutscene<CutsceneManager>();
        cutscene.SetTimeline(cutsceneName);
        cutscene.StartCutscene();
    }
}
