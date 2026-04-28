using System.Collections;
using UnityEngine;

public class SoundLightSignalPanel : MonoBehaviour
{
    [SerializeField]
    Color[] _lightColors;
    [SerializeField]
    Color _defaultColor;

    MeshRenderer[] _lightRenderers;
    Light[] _lights;
    AudioSource _audioPlayer;
    MaterialPropertyBlock _propertyBlock;

    readonly float _playSequenceRecoverTime = 10f;

    bool _playing = false;
    bool _canPlay = true;
    float _playSequenceRecoverTimer = 5f;

    void Start()
    {
        _propertyBlock = new MaterialPropertyBlock();
        _audioPlayer = GetComponent<AudioSource>();
        _lightRenderers = new MeshRenderer[transform.childCount];
        _lights = new Light[transform.childCount];

        for (int i = 0; i < transform.childCount; i++)
        {
            Transform light = transform.GetChild(i);
            _lightRenderers[i] = light.GetComponent<MeshRenderer>();
            _lights[i] = light.GetChild(0).GetComponent<Light>();
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
            _propertyBlock.SetColor("_BaseColor", _lightColors[i]);
            _lightRenderers[i].SetPropertyBlock(_propertyBlock);
            _lights[i].gameObject.SetActive(true);
            _lights[i].color = _lightColors[i];

            yield return new WaitForSeconds(0.5f);

            _propertyBlock.SetColor("_BaseColor", _defaultColor);
            _lightRenderers[i].SetPropertyBlock(_propertyBlock);
            _lights[i].gameObject.SetActive(false);
        }
        _playing = false;
    }
}
