using UnityEngine;

public class SoundLever : MonoBehaviour
{
    [SerializeField]
    AudioClip[] _tones;
    [SerializeField]
    int _rightToneIndex;

    AudioSource _audioPlayer;

    int _chosenToneIndex;

    void Start()
    {
        _audioPlayer = transform.parent.GetComponent<AudioSource>();
    }
    /// <summary>
    /// Sets a point for the lever
    /// </summary>
    /// <param name="index">snap point index</param>
    public void SnapPoint(int index)
    {
        _chosenToneIndex = index;
        PlayTone(index);
    }
    /// <summary>
    /// Plays a tone on the snap point
    /// </summary>
    /// <param name="index">snap point index</param>
    public void PlayTone(int index)
    {
        if (index != 0)
        {
            _audioPlayer.PlayOneShot(_tones[index - 1]);
        }
    }
    /// <summary>
    /// Plays the right tone for the lever
    /// </summary>
    public void PlayRightTone()
    {
        PlayTone(_rightToneIndex);
    }
    /// <summary>
    /// Checks if the chosen snap point is correct
    /// </summary>
    /// <returns></returns>
    public bool CheckChosenTone()
    {
        return _rightToneIndex == _chosenToneIndex;
    }
}
