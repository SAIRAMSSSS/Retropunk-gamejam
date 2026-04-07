using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using Zenject;

public class LevelManager : MonoBehaviour
{
    [SerializeField]
    GameObject[] _levels;

    [Inject]
    UIManager _UI;
    [Inject]
    DiContainer _container;

    readonly float _darkenDuration = 0.8f;

    GameObject _currentLevel;

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
    }
    /// <summary>
    /// Darkens the screen, loads a new scene, and removes the blackout
    /// </summary>
    /// <param name="levelIndex"></param>
    /// <returns></returns>
    public IEnumerator LoadLevel(int levelIndex)
    {
        yield return StartCoroutine(DarkenScreen(0, 1));
        SetNewLevel(levelIndex);
        yield return null;
        yield return StartCoroutine(DarkenScreen(1, 0));
    }
    /// <summary>
    /// Darkens the screen before loading the new scene
    /// </summary>
    /// <returns></returns>
    IEnumerator DarkenScreen(float startAlpha, float endAlpha)
    {
        float timer = 0;
        while (timer < _darkenDuration)
        {
            timer += Time.deltaTime;
            float alpha = Mathf.Lerp(startAlpha, endAlpha, timer / _darkenDuration);
            _UI.SetDarkenScreen(alpha);
            yield return null;
        }

        _UI.SetDarkenScreen(endAlpha);
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
