using System.Linq;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;
using Zenject;

public class CutsceneManager : MonoBehaviour
{
    [SerializeField]
    TimelineAsset[] _timelines;

    Animator _animator;
    PlayableDirector _timeline;

    [Inject]
    PlayerController _player;
    [Inject]
    DialogueManager _dialogueManager;
    [Inject]
    SceneTransitionManager _transitionManager;

    private void Start()
    {
        _timeline = GetComponent<PlayableDirector>();
        _animator = _player.GetComponent<Animator>();
    }

    public void SetTimeline(string timeline)
    {
        _timeline.playableAsset = _timelines.First(t => t.name == timeline);
    }
    /// <summary>
    /// Sets objects to tracks, disables the player controller
    /// </summary>
    public void StartCutscene()
    {
        _player.enabled = false;
        BindTrack("DialogueTrack", _dialogueManager.gameObject);
        BindTrack("ScreenTrack", _dialogueManager.transform.GetChild(0).gameObject);
        _timeline.Play();
    }
    /// <summary>
    /// Enables the player controller in the end of the cutscene
    /// </summary>
    public void EndCutscene()
    {
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

    public void StartPrologueCutscene()
    {
        _player.enabled = false;
        BindTrack("DialogueTrack", _dialogueManager.gameObject);
        BindTrack("ScreenTrack", _dialogueManager.transform.GetChild(0).gameObject);
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
    public void ContinueInNextScene(string sceneName, string cutsceneName)
    {
        _transitionManager.LoadSceneWithCutscene(sceneName, cutsceneName);
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
            _timeline.SetGenericBinding(track, trckObj);
        }
    }
}
