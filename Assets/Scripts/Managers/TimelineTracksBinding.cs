using System.Linq;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;
using Zenject;

public class TimelineTracksBinding : IInitializable
{ 
    PlayableDirector _timelineDirector;
    [Inject]
    PlayerController _player;
    [Inject]
    CameraController _camera;
    [Inject]
    UIManager _UI;

    public void Initialize()
    {
        BindTrack("PlayerTrack", _player.gameObject);
    }

    void BindTrack(string trackName, GameObject trckObj)
    {
        var timeline = _timelineDirector.playableAsset as TimelineAsset;

        if(timeline!=null)
        {
            TrackAsset track = timeline.GetOutputTracks().First(t => t.name == trackName);
            _timelineDirector.SetGenericBinding(track, trckObj);
        }
    }
}
