using System.Collections;
using UnityEngine;

public class SoundLightSignalPanel : MonoBehaviour
{
    [SerializeField]
    Material[] _lightColors;
    [SerializeField]
    Material _defaultLight;

    MeshRenderer[] _lights;

    AudioSource _audioPlayer;

    readonly float _playSequenceRecoverTime = 10f;

    bool _playing = false;
    bool _canPlay = true;
    float _playSequenceRecoverTimer = 0f;

    void Start()
    {
        _audioPlayer = GetComponent<AudioSource>();
        _lights = new MeshRenderer[transform.childCount];

        for (int i = 0; i < transform.childCount; i++)
        {
            _lights[i] = transform.GetChild(i).GetComponent<MeshRenderer>();
        }
    }

    void Update()
    {
        if (_canPlay && !_playing)
        {
            _playSequenceRecoverTimer += Time.deltaTime;
            if (_playSequenceRecoverTimer >= _playSequenceRecoverTime)
            {
                _playSequenceRecoverTimer = 0f;
                StartCoroutine(PlayToneSequence());
            }
        }
    }

    public void CanPlaySequence(bool interact)
    {
        _playSequenceRecoverTimer = _playSequenceRecoverTime / 2f;
        if (!interact)
        {
            _audioPlayer.Stop();
        }
        _canPlay = interact;
    }
    
    IEnumerator PlayToneSequence()
    {
        _playing = true;
        _audioPlayer.Play();
        for (int i = 0; i < _lights.Length; i++)
        {
            _lights[i].material = _lightColors[i];

            yield return new WaitForSeconds(0.5f);

            _lights[i].material = _defaultLight;
        }
        _playing = false;
    }
}
