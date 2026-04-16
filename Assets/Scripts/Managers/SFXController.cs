using System.Collections.Generic;
using UnityEngine;
[RequireComponent(typeof(AudioSource))]
public class SFXController : MonoBehaviour
{
    [Header("SFX")]
    [SerializeField]
    List<AudioClip> _sounds;

    protected AudioSource _audio;

    public bool IsPlaying => _audio.isPlaying;

    private void Start()
    {
        _audio = GetComponent<AudioSource>();
    }
    /// <summary>
    /// Plays a sound once
    /// </summary>
    /// <param name="clip"></param>
    public void PlaySoundClip(AudioClip clip)
    {
        _audio.PlayOneShot(clip);
    }
    /// <summary>
    /// Plays a sound on loop
    /// </summary>
    /// <param name="clip"></param>
    public void PlaySoundLoop(AudioClip clip)
    {
        _audio.loop = true;
        if (_audio.clip != clip)
        {
            _audio.Stop();
            _audio.clip = clip;
        }
        if (!_audio.isPlaying)
        {
            _audio.Play();
        }
    }
    /// <summary>
    /// Plays a sound on loop
    /// </summary>
    /// <param name="clip"></param>
    public void PlaySoundLoop(string sound)
    {
        _audio.loop = true;
        _audio.Stop();
        _audio.clip = _sounds.Find(s => s.name.Contains(sound));
        if (!_audio.isPlaying)
        {
            _audio.Play();
        }
    }
    /// <summary>
    /// Plays a sound by name
    /// </summary>
    /// <param name="sound"></param>
    public void PlaySound(string sound)
    {
        _audio.PlayOneShot(_sounds.Find(s => s.name.Contains(sound)));
    }
    /// <summary>
    /// Stops a sound
    /// </summary>
    public void StopSound()
    {
        _audio.loop = false;
        _audio.Stop();
    }
}
