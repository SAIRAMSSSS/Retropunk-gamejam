using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    [SerializeField]
    AudioClip[] _backgroundInitialMusic;
    [SerializeField]
    AudioClip[] _backgroundRemainderMusic;

    AudioSource _backgroundMusicPlayer;

    readonly float _fadeDuration = 1f;

    int _musicNum = 1;

    void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }

    void Start()
    {
        _backgroundMusicPlayer = GetComponent<AudioSource>();

        StartCoroutine(SwitchMusic());
    }


    IEnumerator SwitchMusic()
    {
        _backgroundMusicPlayer.clip = _backgroundInitialMusic[_musicNum];
        _backgroundMusicPlayer.loop = false;
        _backgroundMusicPlayer.Play();

        yield return new WaitForSeconds(_backgroundInitialMusic[_musicNum].length);

        _backgroundMusicPlayer.clip = _backgroundRemainderMusic[_musicNum];
        _backgroundMusicPlayer.loop = true;
        _backgroundMusicPlayer.Play();
    }
    /// <summary>
    /// Smoothly turns on the next background music
    /// </summary>
    /// <param name="index"></param>
    /// <returns></returns>
    public IEnumerator StartNewLevelMusic(int index)
    {
        StopAllCoroutines();
        _musicNum = index + 1;
        float fadeOutTime = 0;

        while (fadeOutTime < _fadeDuration)
        {
            fadeOutTime += Time.deltaTime;
            float delta = fadeOutTime / _fadeDuration;
            _backgroundMusicPlayer.volume = Mathf.Lerp(1, 0, delta);
            yield return null;
        }
        //stops the previous music
        _backgroundMusicPlayer.Stop();
        _backgroundMusicPlayer.volume = 1;
        StartCoroutine(SwitchMusic());
    }
}
