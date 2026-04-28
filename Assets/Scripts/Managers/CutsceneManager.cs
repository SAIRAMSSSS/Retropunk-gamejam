using DG.Tweening;
using System.Linq;
using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;
using Zenject;

public class CutsceneManager : MonoBehaviour
{
    [SerializeField]
    CinemachineCamera _cutsceneCamera;
    [SerializeField]
    TimelineAsset[] _timelines;

    Animator _animator;
    PlayableDirector _timeline;

    [Inject]
    PlayerController _player;
    [Inject]
    UIManager _UI;
    [Inject]
    LevelManager _levelManager;
    [Inject]
    CutoutShaderController _cutoutController;

    readonly int _dialogueLayerIndex = 2;

    string _nextCutsceneName;

    private void Start()
    {
        _timeline = GetComponent<PlayableDirector>();
        _animator = _player.GetComponent<Animator>();
    }
    /// <summary>
    /// Sets a timeline by name
    /// </summary>
    /// <param name="timeline"></param>
    public void SetTimeline(string timeline)
    {
        _timeline.playableAsset = _timelines.First(t => t.name == timeline);
    }
    /// <summary>
    /// Sets objects to tracks, disables the player controller
    /// </summary>
    public void StartCutscene()
    {
        if (_cutsceneCamera != null)
            _cutsceneCamera.Priority = 10;
        _player.StopMovement();
        _player.enabled = false;
        BindTrack("DialogueTrack", _UI.gameObject);
        BindTrack("ScreenTrack", _UI.gameObject);
        BindTrack("CameraTrack", Camera.main.gameObject);
        BindTrack("AudioTrack", gameObject);
        BindTrack("PlayerTrack", _player.gameObject);
        BindTrack("CutsceneTrack", gameObject);
        _timeline.Play();
    }

    public virtual void EndCutscene()
    {
        _player.enabled = true;
        _cutsceneCamera.Priority = 0;
        _cutoutController.SetValues(_player.gameObject.transform, 0.15f);
    }
    /// <summary>
    /// Sets the cutout shader for the cutscene
    /// </summary>
    public void SetCutout()
    {
        _cutoutController.SetValues(transform, 0.7f);
    }
    /// <summary>
    /// Starts a call animation, turns on the dialogue animator layer
    /// </summary>
    public void StartPhoneCall()
    {
        DOVirtual.Float(
        _animator.GetLayerWeight(_dialogueLayerIndex),
        1,
        1f,
        (weight) =>
        {
            _animator.SetLayerWeight(_dialogueLayerIndex, weight);
        });
        _animator.SetTrigger("PhoneCall");
    }
    /// <summary>
    /// Loads a new scene and continues the cutscene there
    /// </summary>
    /// <param name="levelIndex"></param>
    public void ContinueInNextLevel(int levelIndex)
    {
        _levelManager.LoadLevelWithCutscene(levelIndex, _nextCutsceneName);
    }
    /// <summary>
    /// Sets the name for the next cutscene in a row
    /// </summary>
    /// <param name="cutsceneName"></param>
    public void SetNextCutscene(string cutsceneName)
    {
        _nextCutsceneName = cutsceneName;
    }
    /// <summary>
    /// Binds an object to a timeline track
    /// </summary>
    /// <param name="trackName"></param>
    /// <param name="trckObj"></param>
    protected void BindTrack(string trackName, GameObject trckObj)
    {
        var timeline = _timeline.playableAsset as TimelineAsset;

        if (timeline != null)
        {
            TrackAsset track = timeline.GetOutputTracks().FirstOrDefault(t => t.name == trackName);
            if (track != null)
            {
                _timeline.SetGenericBinding(track, trckObj);
            }
        }
    }
}
