using DG.Tweening;
using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [SerializeField]
    GameObject _gameOverLayout;

    Image _darkenScreen;

    readonly float _darkenDuration = 0.8f;

    private void Start()
    {
        _darkenScreen = transform.GetChild(0).GetComponent<Image>();
    }

    /// <summary>
    /// Darkens the screen 
    /// </summary>
    /// <returns></returns>
    public IEnumerator DarkenScreen(float endAlpha)
    {
        yield return _darkenScreen.DOFade(endAlpha, _darkenDuration);
    }

    public void SetGameOver()
    {
        _gameOverLayout.SetActive(true);
    }

    public void ReloadGame()
    {
        SceneManager.LoadScene(0, LoadSceneMode.Single);
    }
}
