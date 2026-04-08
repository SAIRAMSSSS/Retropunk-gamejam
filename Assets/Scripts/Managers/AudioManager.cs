using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    [SerializeField]
    AudioClip[] _roomsBackgroundMusic;

    AudioSource _backgroundMusicPlayer;
    AudioSource _fadeAudioPlayer;

    readonly float _fadeDuration = 1f;

    void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }

    void Start()
    {
        _backgroundMusicPlayer = GetComponent<AudioSource>();
        _fadeAudioPlayer = transform.AddComponent<AudioSource>();
    }

    void Update()
    {

    }
    /// <summary>
    /// Smoothly turns on the next background music
    /// </summary>
    /// <param name="index"></param>
    /// <returns></returns>
    public IEnumerator CrossFade(int index)
    {
        float transitionTime = 0;

        _fadeAudioPlayer.clip = _roomsBackgroundMusic[index];
        _fadeAudioPlayer.volume = 0;
        _fadeAudioPlayer.Play();

        while (transitionTime < _fadeDuration)
        {
            transitionTime += Time.deltaTime;
            float delta = transitionTime / _fadeDuration;
            _backgroundMusicPlayer.volume = Mathf.Lerp(1, 0, delta);
            _fadeAudioPlayer.volume = Mathf.Lerp(0, 1, delta);
            yield return null;
        }
        //stops the previous music
        _backgroundMusicPlayer.Stop();
        _backgroundMusicPlayer.volume = 1;
        //swaps players
        (_fadeAudioPlayer, _backgroundMusicPlayer) = (_backgroundMusicPlayer, _fadeAudioPlayer);
    }
}
