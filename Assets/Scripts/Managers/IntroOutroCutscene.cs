using UnityEngine;
using UnityEngine.Playables;
using Zenject;

public class IntroOutroCutscene : MonoBehaviour
{
    [Inject]
    PlayerController _player;
    [Inject]
    GameManager _gameManager;
    [Inject]
    UIManager _UI;

    Animator _animator;
    PlayerInput _input;
    PlayableDirector _timeline;

    bool _resume;

    private void Start()
    {
        _animator = _player.GetComponent<Animator>();
    }
    /// <summary>
    /// Enables sit animation
    /// </summary>
    public void EndPrologueCutscene()
    {
        _animator.SetBool("Sit", false);
    }

    public void StartGameplay()
    {
        _gameManager.StartTimer();
    }

    public void EndGame()
    {
        _UI.ReloadGame();
    }

    public void StopTimeline()
    {
        _resume = true;
        _input = _player.GetComponent<PlayerInput>();
        _input.LockInput(false);
        _timeline = GetComponent<PlayableDirector>();
        _timeline.Pause();
    }

    private void Update()
    {
        if(_resume && _input.Click)
        {
            _resume = false;
            _timeline.Play();
        }
    }
}
