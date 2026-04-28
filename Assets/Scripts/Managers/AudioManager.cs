using DG.Tweening;
using System.Collections;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    [SerializeField]
    AudioSource _initialBackgroundMusicPlayer;
    [SerializeField]
    AudioSource _remainderBackgroundMusicPlayer;
    [SerializeField]
    AudioClip[] _backgroundInitialMusic;
    [SerializeField]
    AudioClip[] _backgroundRemainderMusic;

    //AudioSource _backgroundMusicPlayer;

    readonly float _fadeDuration = 1f;

    int _musicNum = 3;

    void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }

    void Start()
    {
        SwitchMusic();
    }


    void SwitchMusic()
    {
        _initialBackgroundMusicPlayer.clip = _backgroundInitialMusic[_musicNum];
        _initialBackgroundMusicPlayer.Play();

        double nextStartTime = AudioSettings.dspTime + _backgroundInitialMusic[_musicNum].length;

        _remainderBackgroundMusicPlayer.clip = _backgroundRemainderMusic[_musicNum];
        _remainderBackgroundMusicPlayer.PlayScheduled(nextStartTime);
    }
    /// <summary>
    /// Smoothly turns on  next background music
    /// </summary>
    /// <param name="index"></param>
    /// <returns></returns>
    public IEnumerator StartNewLevelMusic(int index)
    {
        StopAllCoroutines();
        _musicNum = index + 1;
        //turns off previous music
        AudioSource audio = _initialBackgroundMusicPlayer.isPlaying ? _initialBackgroundMusicPlayer : _remainderBackgroundMusicPlayer;
        yield return audio.DOFade(0, _fadeDuration);
        //stops previous music
        audio.Stop();
        audio.volume = 1;
        SwitchMusic();
    }
}
