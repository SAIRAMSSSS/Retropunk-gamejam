using Unity.Cinemachine;
using UnityEngine;
using Zenject;

public class GameOverCutscene : MonoBehaviour
{
    [Inject]
    UIManager _UI;

    CinemachineImpulseSource _impulseSource;
    CutsceneManager _cutsceneManager;

    readonly string _gameOverCutsceneName = "GameOver";

    private void Start()
    {
        _cutsceneManager = GetComponent<CutsceneManager>();
        _impulseSource = GetComponent<CinemachineImpulseSource>();
    }

    public void StartGameOver()
    {
        _cutsceneManager.SetTimeline(_gameOverCutsceneName);
        _impulseSource.GenerateImpulse();
    }

    public void GameOver()
    {
        _UI.SetGameOver();
    }
}
