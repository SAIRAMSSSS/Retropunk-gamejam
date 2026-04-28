using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuManager : MonoBehaviour
{
    [SerializeField]
    UIManager _UI;

    public void StartGame()
    {
        StartCoroutine(LoadScene());
    }

    IEnumerator LoadScene()
    {
        yield return StartCoroutine(_UI.DarkenScreen(1));
        SceneManager.LoadScene(1,LoadSceneMode.Single);
    }

    public void ExitGame()
    {
        Application.Quit();
    }
}
