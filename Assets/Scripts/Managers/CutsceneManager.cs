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
    DialogueManager _dialogueManager;
    [Inject]
    LevelManager _levelManager;

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
        _cutsceneCamera.Priority = 10;
        _player.enabled = false;
        BindTrack("DialogueTrack", _dialogueManager.gameObject);
        BindTrack("ScreenTrack", _dialogueManager.transform.GetChild(0).gameObject);
        BindTrack("CameraTrack", Camera.main.gameObject);
        BindTrack("PlayerTrack", _player.gameObject);
        _timeline.Play();
    }
    /// <summary>
    /// Enables the player controller in the end of the cutscene
    /// </summary>
    public void EndCutscene()
    {
        _cutsceneCamera.Priority = 0;
        _cutsceneCamera.Follow = _player.transform;
        _player.enabled = true;
    }
    /// <summary>
    /// Starts a call animation
    /// </summary>
    public void StartPhoneCall()
    {
        _animator.SetLayerWeight(2, 1);
        _animator.SetTrigger("PhoneCall");
    }

    public void EndPrologueCutscene()
    {
        _animator.SetBool("Sit", false);
    }
    /// <summary>
    /// Loads a new scene and continues the cutscene there
    /// </summary>
    /// <param name="sceneName"></param>
    /// <param name="cutsceneName"></param>
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
    void BindTrack(string trackName, GameObject trckObj)
    {
        var timeline = _timeline.playableAsset as TimelineAsset;

        if (timeline != null)
        {
            TrackAsset track = timeline.GetOutputTracks().First(t => t.name == trackName);
            if (track != null)
            {
                _timeline.SetGenericBinding(track, trckObj);
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        StartCutscene();
    }
}
