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
    public IEnumerator DarkenScreen(float startAlpha, float endAlpha)
    {
        float timer = 0;
        while (timer < _darkenDuration)
        {
            timer += Time.deltaTime;
            float alpha = Mathf.Lerp(startAlpha, endAlpha, timer / _darkenDuration);
            SetDarkenScreen(alpha);
            yield return null;
        }

        SetDarkenScreen(endAlpha);
    }
    /// <summary>
    /// Sets an alpha to the darken image color
    /// </summary>
    /// <param name="alpha"></param>
    public void SetDarkenScreen(float alpha)
    {
        Color newColor = _darkenScreen.color;
        newColor .a = alpha;    
        _darkenScreen.color = newColor;
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
