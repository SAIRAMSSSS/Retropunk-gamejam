using System.Collections;
using UnityEngine;

public class SoundLevers : MonoBehaviour
{
    [SerializeField]
    SoundLightSignalPanel _soundLightSignalPanel;
    [SerializeField]
    AudioClip _wrongSound;
    [SerializeField]
    ConsoleObject _console;

    SoundLever[] _levers;
    AudioSource _audioPlayer;
    GameObject _leversLayout;

    private void Start()
    {
        _audioPlayer = GetComponent<AudioSource>();
        _leversLayout = transform.GetChild(0).gameObject;
        _levers = new SoundLever[_leversLayout.transform.childCount];

        for (int i = 0; i < _leversLayout.transform.childCount; i++)
        {
            _levers[i] = _leversLayout.transform.GetChild(i).GetComponent<SoundLever>();
        }
    }

    public void ExitConsole()
    {
        _leversLayout.SetActive(false);
        _soundLightSignalPanel.CanPlaySequence(true);
    }

    public void Submit()
    {
        foreach (var lever in _levers)
        {
            if (!lever.CheckChosenTone())
            {
                _audioPlayer.PlayOneShot(_wrongSound);
                return;
            }
        }

        StartCoroutine(PlayRightSequence());
    }
    /// <summary>
    /// Plays the right tone sequence if the puzzle is completed
    /// </summary>
    /// <returns></returns>
    IEnumerator PlayRightSequence()
    {
        foreach (var lever in _levers)
        {
            lever.PlayRightTone();
            yield return new WaitForSeconds(0.5f);
        }
        _console.CompletePuzzle(1);
        ExitConsole();
    }
}
