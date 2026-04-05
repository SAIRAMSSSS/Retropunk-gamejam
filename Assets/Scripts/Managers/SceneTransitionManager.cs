using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using Zenject;

public class SceneTransitionManager : MonoBehaviour
{
    [Inject]
    UIManager _UI;
    float _darkenDuration = 0.8f;

    void Start()
    {
    }

    void Update()
    {

    }
    /// <summary>
    /// Darkens the screen, loads a new scene, and removes the blackout
    /// </summary>
    /// <param name="sceneName"></param>
    /// <returns></returns>
    public IEnumerator NewScene(string sceneName)
    {
        yield return new WaitForSeconds(5);
        yield return StartCoroutine(DarkenScreen(0, 1));
        SceneManager.LoadScene(sceneName, LoadSceneMode.Single);
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
    /// <param name="sceneName"></param>
    /// <param name="cutsceneName"></param>
    public void LoadSceneWithCutscene(string sceneName, string cutsceneName)
    {
        SceneManager.LoadScene(sceneName, LoadSceneMode.Single);
        CutsceneManager cutsceneManager = GameObject.Find("Cutscene").GetComponent<CutsceneManager>();
        cutsceneManager.SetTimeline(cutsceneName);
        cutsceneManager.StartCutscene();
    }
}
